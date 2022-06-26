#include "filter.h"

/*
 * Copyright (c) 2022, 舒登登
 * All rights reserved.
 * Auther:舒登登(ShuDengdeng)
 * Email:2237380450@qq.com
 * 2022626,点云滤波功能
 */

//对点云进行体素下采样
HEAD void CallingConvention voxelDownSample(pcl::PointCloud<pcl::PointXYZ> * in_pc, double leaf_size,
	                                        pcl::PointCloud<pcl::PointXYZ> * out_pc)
{
	pcl::VoxelGrid<pcl::PointXYZ> voxel;
	voxel.setInputCloud(in_pc->makeShared());
	voxel.setLeafSize(leaf_size, leaf_size, leaf_size);
	voxel.filter(*out_pc);

}
//对点云进行近似体素下采样
HEAD void CallingConvention approximateVoxelDownSample(pcl::PointCloud<pcl::PointXYZ> * in_pc, double leaf_size,
	                                                   pcl::PointCloud<pcl::PointXYZ> * out_pc)
{
	pcl::ApproximateVoxelGrid<pcl::PointXYZ> avoxel;
	avoxel.setInputCloud(in_pc->makeShared());
	avoxel.setLeafSize(leaf_size, leaf_size, leaf_size);
	avoxel.filter(*out_pc);
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