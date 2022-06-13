#include "PclDll.h"


void Pc2Array(pcl::PointCloud<pcl::PointXYZ>::Ptr cloud_in, double * out_x, double * out_y, double * out_z)
{
	for (size_t i = 0; i < cloud_in->points.size(); i++) {

		out_x[i] = cloud_in->points[i].x;
		out_y[i] = cloud_in->points[i].y;
		out_z[i] = cloud_in->points[i].z;

	}
}
//用于区域生长，将点数最多的点簇的索引返回
int getMaxPointCluster(vector<pcl::PointIndices> cluster)
{
	int i = 0;
	int pos = 0;
	int max = 0;
	//迭代器代表向量组中的元素，比如本句中的迭代器it就代表一个pcl::pointindices
	for (vector<pcl::PointIndices>::iterator it = cluster.begin(); it != cluster.end(); ++it)
	{
		if (it->indices.size() > max)
		{
			pos = i;
			max = it->indices.size();
		}
		i = i + 1;
	}
	return pos;
}
//用于平面拟合中获得拟合后的平面与xy参考面的倾斜角
double getAngle(double line1[3], double line2[3])
{
	double cos = abs(line1[0] * line2[0] + line1[1] * line2[1] + line1[2] * line2[2]) /
		sqrt(line1[0] * line1[0] + line1[1] * line1[1] + line1[2] * line1[2]) * sqrt(line2[0] * line2[0] + line2[1] * line2[1] + line2[2] * line2[2]);
	double deg = std::acos(cos);

	return deg * 180 / 3.1415926;
}
//用于获取两向量的变旋转换矩阵
Eigen::Matrix4f getRotationMatrix(Eigen::Vector3f vector_before, Eigen::Vector3f vector_after)
{
	vector_before.normalize();
	vector_after.normalize();
	float angle = acos(vector_before.dot(vector_after));//点积，得到两向量的夹角
	Eigen::Vector3f p_rotate = vector_before.cross(vector_after);//叉积，得到的还是向量
	p_rotate.normalize();//该向量的单位向量，即旋转轴的单位向量
	Eigen::Matrix4f rotationMatrix = Eigen::Matrix4f::Identity();
	rotationMatrix(0, 0) = cos(angle) + p_rotate[0] * p_rotate[0] * (1 - cos(angle));
	rotationMatrix(0, 1) = p_rotate[0] * p_rotate[1] * (1 - cos(angle) - p_rotate[2] * sin(angle));//这里跟公式比多了一个括号，但是看实验结果它是对的。
	rotationMatrix(0, 2) = p_rotate[1] * sin(angle) + p_rotate[0] * p_rotate[2] * (1 - cos(angle));

	rotationMatrix(1, 0) = p_rotate[2] * sin(angle) + p_rotate[0] * p_rotate[1] * (1 - cos(angle));
	rotationMatrix(1, 1) = cos(angle) + p_rotate[1] * p_rotate[1] * (1 - cos(angle));
	rotationMatrix(1, 2) = -p_rotate[0] * sin(angle) + p_rotate[1] * p_rotate[2] * (1 - cos(angle));

	rotationMatrix(2, 0) = -p_rotate[1] * sin(angle) + p_rotate[0] * p_rotate[2] * (1 - cos(angle));
	rotationMatrix(2, 1) = p_rotate[0] * sin(angle) + p_rotate[1] * p_rotate[2] * (1 - cos(angle));
	rotationMatrix(2, 2) = cos(angle) + p_rotate[2] * p_rotate[2] * (1 - cos(angle));
	return rotationMatrix;
}
//将向量点云连接为点云对象
void connect_pc(vector<pcl::PointCloud<pcl::PointXYZ>>& input_pc1, pcl::PointCloud<pcl::PointXYZ> & out_cloud)
{
	for (int i = 0; i < input_pc1.size(); i++)
	{
		out_cloud = out_cloud + input_pc1[i];
	}
}

//函数重载，3西格玛法则剔除异常值，输入是点云，输出是点云向量
void stdDev_filter(vector<pcl::PointCloud<pcl::PointXYZ>>& input_cloud, vector<pcl::PointCloud<pcl::PointXYZ>>& out_cloud)
{
	double mean = 0;
	double stdDev = 0;
	vector<float> vec_z;
	out_cloud.resize(input_cloud.size());
	//计算点云的均值和标准差
	for (int i = 0; i < input_cloud.size(); i++)
	{
		for (int k = 0; k < input_cloud[i].points.size(); k++)
		{
			vec_z.push_back(input_cloud[i].points[k].z);
		}

	}
	pcl::getMeanStdDev(vec_z, mean, stdDev);
	//以2西格玛准则剔除点
	for (int j = 0; j < input_cloud.size(); j++)
	{
		for (int d = 0; d < input_cloud[j].points.size(); d++)
		{
			//有可能会出现某条线的点全部被剔除的情况，这时候该点云没有点会不会报错
			if (fabs(input_cloud[j].points[d].z - mean) < 2 * stdDev)
			{
				out_cloud[j].push_back(input_cloud[j].points[d]);
			}
		}

	}
}

HEAD int CallingConvention loadFile(char * path, double * out_x, double * out_y, double * out_z)
{
	pcl::PointCloud<pcl::PointXYZ>::Ptr cloud(new pcl::PointCloud<pcl::PointXYZ>());
	if (pcl::io::loadPLYFile(path, *cloud) == -1)
	{
		return 0;
	}
	else
	{
		Pc2Array(cloud, out_x, out_y, out_z);
		return 1;
	}
}
//加载ply文件
HEAD int CallingConvention loadPlyFile(char * path, pcl::PointCloud<pcl::PointXYZ>* pc)
{
	if (pcl::io::loadPLYFile(path, *pc) == -1)
	{
		return 0;
	}
	else
	{
		return 1;
	}
}
//加载pcd文件
HEAD int CallingConvention loadPcdFile(char* path, pcl::PointCloud<pcl::PointXYZ> * pc)
{
	if (pcl::io::loadPCDFile(path, *pc) == -1)
	{
		return 0;
	}
	else
	{
		return 1;
	}

}
//保存pcd文件
HEAD void CallingConvention savePcdFile(char* path, pcl::PointCloud<pcl::PointXYZ> * pc, int binaryMode)
{
	if (binaryMode >= 1)
	{
		pcl::io::savePCDFile(path, *pc, true);
	}
	else
	{
		pcl::io::savePCDFile(path, *pc, false);
	}

}
//保存ply文件
HEAD void CallingConvention savePlyFile(char* path, pcl::PointCloud<pcl::PointXYZ> * pc, int binaryMode)
{
	if (binaryMode >= 1)
	{
		pcl::io::savePLYFile(path, *pc, true);
	}
	else
	{
		pcl::io::savePLYFile(path, *pc, false);
	}

}


//均匀下采样
HEAD void CallingConvention uniformDownSample(pcl::PointCloud<pcl::PointXYZ> * in_pc, double radius,
	pcl::PointCloud<pcl::PointXYZ> * out_pc)
{

	pcl::UniformSampling<pcl::PointXYZ> uniform;
	uniform.setInputCloud(in_pc->makeShared());
	uniform.setRadiusSearch(radius);
	uniform.filter(*out_pc);

}
//直通滤波
HEAD void CallingConvention passThroughFilter(pcl::PointCloud<pcl::PointXYZ> * in_pc, char * axis_name,
	float limit_min, float limit_max, int negative,
	pcl::PointCloud<pcl::PointXYZ> * out_pc)
{
	pcl::PassThrough<pcl::PointXYZ> pass;
	pass.setInputCloud(in_pc->makeShared());
	if (negative <= 0)
	{
		pass.setNegative(false);
		pass.setFilterFieldName(axis_name);
		pass.setFilterLimits(limit_min, limit_max);
		pass.filter(*out_pc);
	}
	else
	{
		pass.setNegative(true);
		pass.setFilterFieldName(axis_name);
		pass.setFilterLimits(limit_min, limit_max);
		pass.filter(*out_pc);
	}
}
//统计滤波
HEAD void CallingConvention staFilter(pcl::PointCloud<pcl::PointXYZ> * in_pc,
	int neighbor_num, float thresh,
	pcl::PointCloud<pcl::PointXYZ> * out_pc)
{
	pcl::StatisticalOutlierRemoval<pcl::PointXYZ> sta;
	sta.setInputCloud(in_pc->makeShared());
	sta.setMeanK(neighbor_num);
	sta.setStddevMulThresh(thresh);
	sta.filter(*out_pc);

}

/*
功能：半径滤波，将离群点去除
*/
HEAD void CallingConvention radiusFilter(pcl::PointCloud<pcl::PointXYZ> * in_pc,
	                                     double radius, int num_thresh,
	                                     pcl::PointCloud<pcl::PointXYZ> * out_pc)
{
	pcl::RadiusOutlierRemoval<pcl::PointXYZ> ror;
	ror.setInputCloud(in_pc->makeShared());
	ror.setRadiusSearch(radius);
	ror.setMinNeighborsInRadius(num_thresh);
	ror.filter(*out_pc);
}

//原始的区域生长
HEAD void CallingConvention oriGrowRegion(pcl::PointCloud<pcl::PointXYZ> * in_pc,
	                                      int neighbor_num, float smooth_thresh, float curva_thresh,
	                                      int MinClusterSize, int MaxClusterSize,
	                                      vector<pcl::PointIndices> * out_indice)
{
	//计算点云的法向量
	pcl::PointCloud<pcl::Normal>::Ptr normals(new pcl::PointCloud<pcl::Normal>());
	pcl::search::KdTree<pcl::PointXYZ>::Ptr tree(new pcl::search::KdTree<pcl::PointXYZ>());

	pcl::NormalEstimation<pcl::PointXYZ, pcl::Normal> ne;
	ne.setInputCloud(in_pc->makeShared());
	ne.setSearchMethod(tree);
	ne.setKSearch(neighbor_num);
	ne.compute(*normals);

	//区域生长
	pcl::RegionGrowing<pcl::PointXYZ, pcl::Normal> rg;
	rg.setSearchMethod(tree);
	rg.setInputCloud(in_pc->makeShared());
	rg.setInputNormals(normals);
	rg.setMinClusterSize(MinClusterSize);
	rg.setMaxClusterSize(MaxClusterSize);
	rg.setCurvatureThreshold(curva_thresh);
	rg.setNumberOfNeighbours(neighbor_num);
	rg.setSmoothnessThreshold(smooth_thresh / 180.0 * M_PI);//要求是弧度，输入的是角度，所以转换一下

	//提取聚类后的点簇
	//vector<pcl::PointIndices> cluster;
	rg.extract(*out_indice);

}

//欧式聚类
HEAD void CallingConvention euclideanCluster(pcl::PointCloud<pcl::PointXYZ> * in_pc,
	                                      double distance_thresh, int MinClusterSize, int MaxClusterSize,
	                                      vector<pcl::PointIndices> * out_indice)
{
	pcl::search::KdTree<pcl::PointXYZ>::Ptr tree(new pcl::search::KdTree<pcl::PointXYZ>());
	tree->setInputCloud(in_pc->makeShared());

	pcl::EuclideanClusterExtraction<pcl::PointXYZ> ec;
	ec.setClusterTolerance(distance_thresh);
	ec.setMinClusterSize(MinClusterSize);
	ec.setMaxClusterSize(MaxClusterSize);
	ec.setSearchMethod(tree);
	ec.setInputCloud(in_pc->makeShared());
	ec.extract(*out_indice);

}

//封装后的区域生长，直接返回点数的最多的平面
HEAD void CallingConvention modifiedGrowRegion(pcl::PointCloud<pcl::PointXYZ> * in_pc,
	int neighbor_num, float smooth_thresh, float curva_thresh,
	int MinClusterSize, int MaxClusterSize,
	pcl::PointCloud<pcl::PointXYZ> * out_pc)
{
	//计算点云的法向量
	pcl::PointCloud<pcl::Normal>::Ptr normals(new pcl::PointCloud<pcl::Normal>());
	pcl::search::KdTree<pcl::PointXYZ>::Ptr tree(new pcl::search::KdTree<pcl::PointXYZ>());

	pcl::NormalEstimation<pcl::PointXYZ, pcl::Normal> ne;
	ne.setInputCloud(in_pc->makeShared());
	ne.setSearchMethod(tree);
	ne.setKSearch(neighbor_num);
	ne.compute(*normals);

	//区域生长
	pcl::RegionGrowing<pcl::PointXYZ, pcl::Normal> rg;
	rg.setSearchMethod(tree);
	rg.setInputCloud(in_pc->makeShared());
	rg.setInputNormals(normals);
	rg.setMinClusterSize(MinClusterSize);
	rg.setMaxClusterSize(MaxClusterSize);
	rg.setCurvatureThreshold(curva_thresh);
	rg.setNumberOfNeighbours(neighbor_num);
	rg.setSmoothnessThreshold(smooth_thresh / 180.0 * M_PI);//要求是弧度，输入的是角度，所以转换一下

	//提取聚类后的点簇
	vector<pcl::PointIndices> cluster;
	rg.extract(cluster);
	
	//找到点数最多的平面
	int pos = getMaxPointCluster(cluster);
	pcl::copyPointCloud(*in_pc, cluster[pos], *out_pc);


}
//拟合平面
HEAD float CallingConvention fitPlane(pcl::PointCloud<pcl::PointXYZ> * in_pc,
	float distance_thresh, int max_itera, float * normal)
{
	float angel = 0.0;
	pcl::ModelCoefficients::Ptr modelCoeff(new pcl::ModelCoefficients());
	pcl::PointIndices::Ptr pointIndices(new pcl::PointIndices());
	pcl::SACSegmentation<pcl::PointXYZ> seg;
	seg.setModelType(pcl::SACMODEL_PLANE);
	seg.setMethodType(pcl::SAC_RANSAC);
	seg.setInputCloud(in_pc->makeShared());
	seg.setDistanceThreshold(distance_thresh);
	seg.setMaxIterations(max_itera);
	seg.setOptimizeCoefficients(true);
	seg.segment(*pointIndices, *modelCoeff);

	double line1[3] = { 0,0,1 };//参考平面向量
	double line2[3] = { modelCoeff->values[0],modelCoeff->values[1],modelCoeff->values[2] };//拟合平面向量
	angel = getAngle(line1, line2);//获取两向量间的夹角
	normal[0] = modelCoeff->values[0];
	normal[1] = modelCoeff->values[1];
	normal[2] = modelCoeff->values[2];
	normal[3] = modelCoeff->values[3];
	return angel;

}

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