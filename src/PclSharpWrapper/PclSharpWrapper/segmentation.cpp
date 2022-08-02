#include "segmentation.h"

/*
 * Copyright (c) 2022, 舒登登
 * All rights reserved.
 * Auther:舒登登(ShuDengdeng)
 * Email:2237380450@qq.com
 * 点云分割功能
 */

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

//平面分割
HEAD void CallingConvention planeModelSegment(pcl::PointCloud<pcl::PointXYZ> * in_pc,
	double distance_thresh, bool negative, pcl::PointCloud<pcl::PointXYZ> * out_pc, int max_interaions)
{
	pcl::ModelCoefficients::Ptr coefficients(new pcl::ModelCoefficients);
	pcl::PointIndices::Ptr inliers(new pcl::PointIndices);
	// Create the segmentation object
	pcl::SACSegmentation<pcl::PointXYZ> seg;
	// Optional
	seg.setOptimizeCoefficients(true);
	// Mandatory
	seg.setModelType(pcl::SACMODEL_PLANE);
	seg.setMethodType(pcl::SAC_RANSAC);
	seg.setMaxIterations(max_interaions);
	seg.setDistanceThreshold(distance_thresh);

	seg.setInputCloud(in_pc->makeShared());
	seg.segment(*inliers, *coefficients);

	// 提取点云
	pcl::ExtractIndices<pcl::PointXYZ> extract;
	extract.setInputCloud(in_pc->makeShared());
	extract.setIndices(inliers);
	extract.setNegative(negative);
	extract.filter(*out_pc);


}

//圆柱分割
HEAD void CallingConvention cylinderModelSegment(pcl::PointCloud<pcl::PointXYZ> * in_pc,
	double distance_thresh, double min_radius, double max_radius,
	bool negative, pcl::PointCloud<pcl::PointXYZ> * out_pc, int neighbor_num, double normal_distance_weight, int max_iterations)
{
	pcl::PointCloud<pcl::Normal>::Ptr normals(new pcl::PointCloud<pcl::Normal>());
	pcl::SACSegmentationFromNormals<pcl::PointXYZ, pcl::Normal> seg;
	pcl::NormalEstimation<pcl::PointXYZ, pcl::Normal> ne;
	pcl::search::KdTree<pcl::PointXYZ>::Ptr tree(new pcl::search::KdTree<pcl::PointXYZ>());

	pcl::ModelCoefficients::Ptr coefficients(new pcl::ModelCoefficients);
	pcl::PointIndices::Ptr inliers(new pcl::PointIndices);

	// 计算点云法向量
	ne.setInputCloud(in_pc->makeShared());
	ne.setSearchMethod(tree);
	ne.setKSearch(neighbor_num);
	ne.compute(*normals);

	// 设置seg参数
	seg.setOptimizeCoefficients(true);
	seg.setModelType(pcl::SACMODEL_CYLINDER);
	seg.setMethodType(pcl::SAC_RANSAC);
	seg.setNormalDistanceWeight(normal_distance_weight);
	seg.setMaxIterations(max_iterations);
	seg.setDistanceThreshold(distance_thresh);
	seg.setRadiusLimits(min_radius, max_radius);
	seg.setInputCloud(in_pc->makeShared());
	seg.setInputNormals(normals);

	seg.segment(*inliers, *coefficients);

	// 提取点云
	pcl::ExtractIndices<pcl::PointXYZ> extract;
	extract.setInputCloud(in_pc->makeShared());
	extract.setIndices(inliers);
	extract.setNegative(negative);
	extract.filter(*out_pc);


}