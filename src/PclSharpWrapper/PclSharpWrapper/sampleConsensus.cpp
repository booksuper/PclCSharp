#include "sampleConsensus.h"

/*
 * Copyright (c) 2022, 舒登登
 * All rights reserved.
 * Auther:舒登登(ShuDengdeng)
 * Email:2237380450@qq.com
 * 随机抽样一致性模块
 */

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
