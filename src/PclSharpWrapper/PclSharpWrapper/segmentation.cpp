#include "segmentation.h"

/*
 * Copyright (c) 2022, ��ǵ�
 * All rights reserved.
 * Auther:��ǵ�(ShuDengdeng)
 * Email:2237380450@qq.com
 * ���Ʒָ��
 */

//ԭʼ����������
HEAD void CallingConvention oriGrowRegion(pcl::PointCloud<pcl::PointXYZ> * in_pc,
	int neighbor_num, float smooth_thresh, float curva_thresh,
	int MinClusterSize, int MaxClusterSize,
	vector<pcl::PointIndices> * out_indice)
{
	//������Ƶķ�����
	pcl::PointCloud<pcl::Normal>::Ptr normals(new pcl::PointCloud<pcl::Normal>());
	pcl::search::KdTree<pcl::PointXYZ>::Ptr tree(new pcl::search::KdTree<pcl::PointXYZ>());

	pcl::NormalEstimation<pcl::PointXYZ, pcl::Normal> ne;
	ne.setInputCloud(in_pc->makeShared());
	ne.setSearchMethod(tree);
	ne.setKSearch(neighbor_num);
	ne.compute(*normals);

	//��������
	pcl::RegionGrowing<pcl::PointXYZ, pcl::Normal> rg;
	rg.setSearchMethod(tree);
	rg.setInputCloud(in_pc->makeShared());
	rg.setInputNormals(normals);
	rg.setMinClusterSize(MinClusterSize);
	rg.setMaxClusterSize(MaxClusterSize);
	rg.setCurvatureThreshold(curva_thresh);
	rg.setNumberOfNeighbours(neighbor_num);
	rg.setSmoothnessThreshold(smooth_thresh / 180.0 * M_PI);//Ҫ���ǻ��ȣ�������ǽǶȣ�����ת��һ��

	//��ȡ�����ĵ��
	//vector<pcl::PointIndices> cluster;
	rg.extract(*out_indice);

}

//ŷʽ����
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

//��װ�������������ֱ�ӷ��ص���������ƽ��
HEAD void CallingConvention modifiedGrowRegion(pcl::PointCloud<pcl::PointXYZ> * in_pc,
	int neighbor_num, float smooth_thresh, float curva_thresh,
	int MinClusterSize, int MaxClusterSize,
	pcl::PointCloud<pcl::PointXYZ> * out_pc)
{
	//������Ƶķ�����
	pcl::PointCloud<pcl::Normal>::Ptr normals(new pcl::PointCloud<pcl::Normal>());
	pcl::search::KdTree<pcl::PointXYZ>::Ptr tree(new pcl::search::KdTree<pcl::PointXYZ>());

	pcl::NormalEstimation<pcl::PointXYZ, pcl::Normal> ne;
	ne.setInputCloud(in_pc->makeShared());
	ne.setSearchMethod(tree);
	ne.setKSearch(neighbor_num);
	ne.compute(*normals);

	//��������
	pcl::RegionGrowing<pcl::PointXYZ, pcl::Normal> rg;
	rg.setSearchMethod(tree);
	rg.setInputCloud(in_pc->makeShared());
	rg.setInputNormals(normals);
	rg.setMinClusterSize(MinClusterSize);
	rg.setMaxClusterSize(MaxClusterSize);
	rg.setCurvatureThreshold(curva_thresh);
	rg.setNumberOfNeighbours(neighbor_num);
	rg.setSmoothnessThreshold(smooth_thresh / 180.0 * M_PI);//Ҫ���ǻ��ȣ�������ǽǶȣ�����ת��һ��

	//��ȡ�����ĵ��
	vector<pcl::PointIndices> cluster;
	rg.extract(cluster);

	//�ҵ���������ƽ��
	int pos = getMaxPointCluster(cluster);
	pcl::copyPointCloud(*in_pc, cluster[pos], *out_pc);


}

//ƽ��ָ�
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

	// ��ȡ����
	pcl::ExtractIndices<pcl::PointXYZ> extract;
	extract.setInputCloud(in_pc->makeShared());
	extract.setIndices(inliers);
	extract.setNegative(negative);
	extract.filter(*out_pc);


}

//Բ���ָ�
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

	// ������Ʒ�����
	ne.setInputCloud(in_pc->makeShared());
	ne.setSearchMethod(tree);
	ne.setKSearch(neighbor_num);
	ne.compute(*normals);

	// ����seg����
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

	// ��ȡ����
	pcl::ExtractIndices<pcl::PointXYZ> extract;
	extract.setInputCloud(in_pc->makeShared());
	extract.setIndices(inliers);
	extract.setNegative(negative);
	extract.filter(*out_pc);


}