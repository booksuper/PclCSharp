#include "util_module.h"

/*
 * Copyright (c) 2022, 舒登登
 * All rights reserved.
 * Auther:舒登登(ShuDengdeng)
 * Email:2237380450@qq.com
 * util模块，包含很多点云处理工具函数
 */

//校正平面
HEAD void CallingConvention correctPlane(pcl::PointCloud<pcl::PointXYZ> * in_pc, float * normal,
	pcl::PointCloud<pcl::PointXYZ> * out_pc)
{
	Eigen::Vector3f vector;
	vector << normal[0], normal[1], normal[2];
	Eigen::Vector3f vector_after(0, 0, 1);
	//获得倾斜变换的矩阵
	Eigen::Matrix4f rotion_matrix = getRotationMatrix(vector, vector_after);
	//对倾斜的点云平面作变换，校正回法向量为（0,0,1）的平面
	pcl::transformPointCloud(*in_pc, *out_pc, rotion_matrix);

}

//sigam法则剔除阈值
HEAD void CallingConvention sigamFilter(pcl::PointCloud<pcl::PointXYZ> * in_pc, int sigam_thresh,
	pcl::PointCloud<pcl::PointXYZ> * out_pc)
{
	double mean = 0;
	double stdDev = 0;
	vector<float> vec_z;
	for (int i = 0; i < in_pc->points.size(); i++)
	{
		vec_z.push_back(in_pc->points[i].z);
	}
	pcl::getMeanStdDev(vec_z, mean, stdDev);
	for (int j = 0; j < in_pc->points.size(); j++)
	{
		if (fabs(in_pc->points[j].z - mean) < sigam_thresh * stdDev)
		{
			out_pc->push_back(in_pc->points[j]);
		}
	}

}

//功能：将点云按照列方向存储，每一列代表一个测量圆周，并输出用户指定的n列测量圆周点云
HEAD void CallingConvention getRunoutPoints(pcl::PointCloud<pcl::PointXYZ> * in_pc, int num,
	pcl::PointCloud<pcl::PointXYZ> * out_pc)
{
	/*将原始点云按照行的方式存储，其中向量中的每一个元素都是一个点云对象。一个点云对象存储的是一根激光线的点云
	  */
	vector<pcl::PointCloud<pcl::PointXYZ>> out_res;//将点云按行存储
	vector<pcl::PointCloud<pcl::PointXYZ>> out_std;//剔除无效行点云之后的行组织点云
	vector<pcl::PointCloud<pcl::PointXYZ>> out_pc1;//将点云按列存储
	vector<pcl::PointCloud<pcl::PointXYZ>> out_pc2;//剔除异常值后的按列存储点云

	int y_interval = 100;//每一根激光线的y间隔是100,这个参数是相机的参数，可以调整
	int y_pc = 0;
	pcl::PointXYZ minPt(0, 0, 0), maxPt(0, 0, 0);
	pcl::getMinMax3D(*in_pc, minPt, maxPt);//找到y的极值
	int y_min = round(minPt.y);
	int y_max = round(maxPt.y);
	//除以100是因为每一根激光线的y间隔是100，
	//y的最大最小值之差再除以间隔得到的是激光线数。但这里有个问题，如果分割的时候有点问题
	//导致并不是每隔一百一定有一条激光线，也有可能隔了好几百才有一条激光线。这样求出的结果就会偏大
	//但目前测试的时候都没有碰到这种情况，暂时是安全的。2022.5.24
	int line_nums = (y_max - y_min) / y_interval;
	line_nums = line_nums + 1;
	int pc_num = in_pc->points.size();//点云总数
	//前面的工作都是为了确定有多少条激光线，好初始化vector
	//resize之后注意内存顺序问题,这一句没有将保错
	out_res.resize(line_nums);
	//开始将点云按照行存储
	int k = 0;
	int flag = 0;
	for (int i = y_min; i <= y_max; i = i + 100)//总共有多少条激光线
	{
		for (int j = 0; j < pc_num; j++)//在所有点集中找y值一样的点，y值一样的点被认为是一根激光线的点云
		{
			y_pc = round(in_pc->points[j].y);
			if (y_pc == i)
			{

				out_res[k].push_back(in_pc->points[j]);
				flag = flag + 1;//统计一个线上有多少个点
			}
		}
		//假如第K个线没有点，则用一个零点填充，防止报错，
		//这里是预防万一激光数目偏大导致有些元素为空，是上面提到问题的一个解决方式.个人感觉不是很好，后续看能不能改进
		if (flag == 0)
		{
			pcl::PointXYZ p(0, 0, 0);
			out_res[k].push_back(p);
		}
		k++;
		flag = 0;
	}
	//剔除一些废弃激光线。点云经过行存储之后，肯定会有一些激光线点数很少，这些点数很少的点云应该被剔除
	for (int i = 0; i < out_res.size(); i++)
	{
		//小于70点的线数被认为是无效激光线，实际上反映的是马达的边缘毛刺，直接舍弃
		if (70 <= out_res[i].points.size()) //70到240点数认为是有效激光线，但是该激光线包含构成孔洞
		{
			out_std.push_back(out_res[i]);
		}

	}

	//目的：我们最终计算的是端面跳动，行存储点云还不能解决问题。下面还需要把行存储点云变为列存储点云，
	//因为每一列点云才代表一个端面圆周。

	/*点数最多的点云并不是最佳的，有些点云点数最多，但中间可能空了一块，而另外的点云这一块并没有空
	应该将所有x值放在一个集合中，鉴于此，使用集合set。set有两个重要特性，一个是
	没有重复元素，另一个是会自动按照升序排列*/

	//求得点云中有多少列
	set<int, greater<int>> x_set;//降序排列
	for (int i = 0; i < out_std.size(); i++)
	{
		for (int j = 0; j < out_std[i].points.size(); j++)
		{
			//点云数据中因为精度关系，可能会有小数，而且经过前面的校正平面之后，点的数据中也会有小数
			//所以这里需要使用round对数据进行圆整
			x_set.insert(round(out_std[i].points[j].x));
		}
	}
	//为了方便操作，把x_set集合转为vector
	vector<int> pc_x;
	for (set<int>::iterator it = x_set.begin(); it != x_set.end(); it++)
	{
		pc_x.push_back(*it);
	}


	out_pc1.resize(pc_x.size());

	//以x的值为索引，x值一样的为同一列
	//下面的算法复杂度为n3，需要改进
	for (int i = 0; i < pc_x.size(); i++)
	{
		for (int j = 0; j < out_std.size(); j++)
		{
			for (int k = 0; k < out_std[j].points.size(); k++)
			{
				//x值一样的认为是同一列的
				if (round(pc_x[i]) == round(out_std[j].points[k].x))
				{
					out_pc1[i].push_back(out_std[j].points[k]);
					//每一行只有一个一样的，一旦找到就跳出这个循环，节省时间
					break;
				}
			}
		}
	}
	//把每一列中点数过少的点剔除
	for (int i = 0; i < out_pc1.size(); i++)
	{
		if (out_pc1[i].points.size() > 120)
		{
			out_pc2.push_back(out_pc1[i]);
		}
	}


	//随机选择n个端跳圆周，并将其输出
	vector<pcl::PointCloud<pcl::PointXYZ>> out_resVec;//保存指定的端跳圆周
	int column_size = out_pc2.size();
	//求余数，因为总列数不一定能被10整除，所以可以先求出余数
	int surplus = column_size % 10;
	//将多余的数减掉，剩下的数就是10的倍数。除以指定的圆周数
	//就可以得到每一个圆周的间隔。但这样只能整除10
	//还是不够鲁棒
	int gap = (column_size - surplus) / num;
	for (int i = surplus; i < out_pc2.size(); i = i + gap)
	{
		out_resVec.push_back(out_pc2[i]);
	}
	connect_pc(out_resVec, *out_pc);

}

//用于自己测试，不对外提供接口
HEAD float CallingConvention getRunoutPointsWithResult(pcl::PointCloud<pcl::PointXYZ> * in_pc, int num,
	pcl::PointCloud<pcl::PointXYZ> * out_pc)
{
	/*将原始点云按照行的方式存储，其中向量中的每一个元素都是一个点云对象。一个点云对象存储的是一根激光线的点云
	  */
	vector<pcl::PointCloud<pcl::PointXYZ>> out_res;//将点云按行存储
	vector<pcl::PointCloud<pcl::PointXYZ>> out_std1;//剔除无效行点云之后的行组织点云
	vector<pcl::PointCloud<pcl::PointXYZ>> out_std;//sigam法则剔除异常值后的向量点云
	vector<pcl::PointCloud<pcl::PointXYZ>> out_pc1;//将点云按列存储
	vector<pcl::PointCloud<pcl::PointXYZ>> out_pc2;//剔除异常值后的按列存储点云

	int y_interval = 100;//每一根激光线的y间隔是100,这个参数是相机的参数，可以调整
	int y_pc = 0;
	pcl::PointXYZ minPt(0, 0, 0), maxPt(0, 0, 0);
	pcl::getMinMax3D(*in_pc, minPt, maxPt);//找到y的极值
	int y_min = round(minPt.y);
	int y_max = round(maxPt.y);
	//除以100是因为每一根激光线的y间隔是100，
	//y的最大最小值之差再除以间隔得到的是激光线数。但这里有个问题，如果分割的时候有点问题
	//导致并不是每隔一百一定有一条激光线，也有可能隔了好几百才有一条激光线。这样求出的结果就会偏大
	//但目前测试的时候都没有碰到这种情况，暂时是安全的。2022.5.24
	int line_nums = (y_max - y_min) / y_interval;
	line_nums = line_nums + 1;
	int pc_num = in_pc->points.size();//点云总数
	//前面的工作都是为了确定有多少条激光线，好初始化vector
	//resize之后注意内存顺序问题,这一句没有将保错
	out_res.resize(line_nums);
	//开始将点云按照行存储
	int k = 0;
	int flag = 0;
	for (int i = y_min; i <= y_max; i = i + 100)//总共有多少条激光线
	{
		for (int j = 0; j < pc_num; j++)//在所有点集中找y值一样的点，y值一样的点被认为是一根激光线的点云
		{
			y_pc = round(in_pc->points[j].y);
			if (y_pc == i)
			{

				out_res[k].push_back(in_pc->points[j]);
				flag = flag + 1;//统计一个线上有多少个点
			}
		}
		//假如第K个线没有点，则用一个零点填充，防止报错，
		//这里是预防万一激光数目偏大导致有些元素为空，是上面提到问题的一个解决方式.个人感觉不是很好，后续看能不能改进
		if (flag == 0)
		{
			pcl::PointXYZ p(0, 0, 0);
			out_res[k].push_back(p);
		}
		k++;
		flag = 0;
	}
	//剔除一些废弃激光线。点云经过行存储之后，肯定会有一些激光线点数很少，这些点数很少的点云应该被剔除
	for (int i = 0; i < out_res.size(); i++)
	{
		//小于70点的线数被认为是无效激光线，实际上反映的是马达的边缘毛刺，直接舍弃
		if (70 <= out_res[i].points.size()) //70到240点数认为是有效激光线，但是该激光线包含构成孔洞
		{
			out_std1.push_back(out_res[i]);
		}

	}
	//剔除异常值，用于实验
	stdDev_filter(out_std1, out_std);

	//目的：我们最终计算的是端面跳动，行存储点云还不能解决问题。下面还需要把行存储点云变为列存储点云，
	//因为每一列点云才代表一个端面圆周。

	/*点数最多的点云并不是最佳的，有些点云点数最多，但中间可能空了一块，而另外的点云这一块并没有空
	应该将所有x值放在一个集合中，鉴于此，使用集合set。set有两个重要特性，一个是
	没有重复元素，另一个是会自动按照升序排列*/

	//求得点云中有多少列
	set<int, greater<int>> x_set;//降序排列
	for (int i = 0; i < out_std.size(); i++)
	{
		for (int j = 0; j < out_std[i].points.size(); j++)
		{
			//点云数据中因为精度关系，可能会有小数，而且经过前面的校正平面之后，点的数据中也会有小数
			//所以这里需要使用round对数据进行圆整
			x_set.insert(round(out_std[i].points[j].x));
		}
	}
	//为了方便操作，把x_set集合转为vector
	vector<int> pc_x;
	for (set<int>::iterator it = x_set.begin(); it != x_set.end(); it++)
	{
		pc_x.push_back(*it);
	}


	out_pc1.resize(pc_x.size());

	//以x的值为索引，x值一样的为同一列
	//下面的算法复杂度为n3，需要改进
	for (int i = 0; i < pc_x.size(); i++)
	{
		for (int j = 0; j < out_std.size(); j++)
		{
			for (int k = 0; k < out_std[j].points.size(); k++)
			{
				//x值一样的认为是同一列的
				if (round(pc_x[i]) == round(out_std[j].points[k].x))
				{
					out_pc1[i].push_back(out_std[j].points[k]);
					//每一行只有一个一样的，一旦找到就跳出这个循环，节省时间
					break;
				}
			}
		}
	}
	//把每一列中点数过少的点剔除
	for (int i = 0; i < out_pc1.size(); i++)
	{
		if (out_pc1[i].points.size() > 120)
		{
			out_pc2.push_back(out_pc1[i]);
		}
	}


	//随机选择n个端跳圆周，并将其输出
	vector<pcl::PointCloud<pcl::PointXYZ>> out_resVec;//保存指定的端跳圆周
	float runout = 0;
	float res = 0;
	int column_size = out_pc2.size();
	//求余数，因为总列数不一定能被10整除，所以可以先求出余数
	int surplus = column_size % 10;
	//将多余的数减掉，剩下的数就是10的倍数。除以指定的圆周数
	//就可以得到每一个圆周的间隔。但这样只能整除10
	//还是不够鲁棒
	int gap = (column_size - surplus) / num;
	vector<float> runouts;
	pcl::PointXYZ min_pt(0, 0, 0);
	pcl::PointXYZ max_pt(0, 0, 0);
	for (int i = surplus; i < out_pc2.size(); i = i + gap)
	{
		out_resVec.push_back(out_pc2[i]);
		pcl::getMinMax3D(out_pc2[i], min_pt, max_pt);
		runout = max_pt.z - min_pt.z;
		runouts.push_back(runout);
	}
	connect_pc(out_resVec, *out_pc);
	//res = *max_element(runouts.begin(), runouts.end());
	for (int g = 0; g < runouts.size(); g++)
	{
		if (res <= runouts[g])
		{
			res = runouts[g];
		}
	}
	return res;
}


HEAD double CallingConvention calculateRunout(pcl::PointCloud<pcl::PointXYZ> * in_pc, int * indices)
{
	int min_indice = 0;
	int max_indice = 0;
	pcl::PointXYZ min_point(0, 0, 0);
	pcl::PointXYZ max_point(0, 0, 0);
	pcl::getMinMax3D(*in_pc, min_point, max_point);
	//找到Z值最小最大的两点索引

	int len1 = in_pc->points.size();

	for (int i = 0; i < len1; i++)
	{
		double z = in_pc->points[i].z;
		if (z == min_point.z)
		{
			min_indice = i;
		}
		else if (z == max_point.z)
		{
			max_indice = i;
		}
	}
	indices[0] = min_indice;
	indices[1] = max_indice;
	double detect_result = max_point.z - min_point.z;//端跳结果
	return detect_result;
}

HEAD void CallingConvention copyPcBaseOnIndice(pcl::PointCloud<pcl::PointXYZ> * in_pc,
	pcl::PointIndices * in_indice,
	pcl::PointCloud<pcl::PointXYZ> * out_pc)
{
	pcl::copyPointCloud(*in_pc, *in_indice, *out_pc);
}