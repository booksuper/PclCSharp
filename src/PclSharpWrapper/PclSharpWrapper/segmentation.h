#pragma once

#include "pcl_util.h"
#include <iostream>
#include <vector>
#include <pcl/segmentation/region_growing.h>
#include <pcl/features/normal_3d.h>
#include <pcl/filters/extract_indices.h>
#include <pcl/sample_consensus/model_types.h>
#include <pcl/sample_consensus/method_types.h>
#include <pcl/segmentation/sac_segmentation.h>
#include <pcl/segmentation/extract_clusters.h>//欧式聚类
#include <pcl/ModelCoefficients.h>
#include <set>

/*
 * Copyright (c) 2022, 舒登登
 * All rights reserved.
 * Auther:舒登登(ShuDengdeng)
 * Email:2237380450@qq.com
 * 点云分割功能
 */


//定义导出方式：以C语言的方式导出，因为C语言方式函数名保持不变
#define EXTERNC extern "C"
//定义dll导出方式，此处是导出，如果是导入则为dllinport
#define HEAD EXTERNC __declspec(dllexport)
//定义调用约定，此处选择标准调用约定，也可以用c调用约定
#define CallingConvention __stdcall

/*
功能：封装后的区域生长，直接返回点数最多的结果
param[in] in_pc 目标点云对象指针
param[in] neighbor_num 近邻数，根据点的数目选取合适的邻居数
param[in] smooth_thresh 法向量阈值
param[in] curva_thresh 曲率阈值
param[in] MinClusterSize 成为一个类的最小点数
param[in] MaxClusterSize 成为一个类的最大点数，
这意味着分割后的点云数目必须在[MinClusterSize,MaxClusterSize]之间才能被输出
param[out] out_pc 结果点云的对象指针
*/
HEAD void CallingConvention modifiedGrowRegion(pcl::PointCloud<pcl::PointXYZ> * in_pc,
	int neighbor_num, float smooth_thresh, float curva_thresh,
	int MinClusterSize, int MaxClusterSize,
	pcl::PointCloud<pcl::PointXYZ> * out_pc);

//原始的区域生长
HEAD void CallingConvention oriGrowRegion(pcl::PointCloud<pcl::PointXYZ> * in_pc,
	int neighbor_num, float smooth_thresh, float curva_thresh,
	int MinClusterSize, int MaxClusterSize,
	vector<pcl::PointIndices> * out_indice);

//欧式聚类
HEAD void CallingConvention euclideanCluster(pcl::PointCloud<pcl::PointXYZ> * in_pc,
	double distance_thresh, int MinClusterSize, int MaxClusterSize,
	vector<pcl::PointIndices> * out_indice);