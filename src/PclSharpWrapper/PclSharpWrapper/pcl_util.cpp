#include "pcl_util.h"


/*
 * Copyright (c) 2022, 舒登登
 * All rights reserved.
 * Auther:舒登登(ShuDengdeng)
 * Email:2237380450@qq.com
 * 点云处理的一些工具函数，不提供对外接口
 */

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
