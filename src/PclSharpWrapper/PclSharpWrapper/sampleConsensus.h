#pragma once

#include "pcl_util.h"
#include <pcl/sample_consensus/model_types.h>
#include <pcl/sample_consensus/method_types.h>
#include <pcl/segmentation/sac_segmentation.h>
#include <pcl/segmentation/extract_clusters.h>//欧式聚类
#include <pcl/ModelCoefficients.h>
#include <pcl/common/transforms.h>
#include <pcl/common/common.h>
#include <pcl/common/common_headers.h>
#include <set>

/*
 * Copyright (c) 2022, 舒登登
 * All rights reserved.
 * Auther:舒登登(ShuDengdeng)
 * Email:2237380450@qq.com
 * 随机抽样一致性模块
 */

using namespace std;
//定义导出方式：以C语言的方式导出，因为C语言方式函数名保持不变
#define EXTERNC extern "C"
//定义dll导出方式，此处是导出，如果是导入则为dllinport
#define HEAD EXTERNC __declspec(dllexport)
//定义调用约定，此处选择标准调用约定，也可以用c调用约定
#define CallingConvention __stdcall





/*
功能：使用Ransac算法拟合分割后点云的平面，并返回平面倾斜角度
param[in] in_pc 目标点云对象指针
param[in] distance_thresh Ransac算法距离阈值
param[in] max_itera Ransac算法最大迭代次数
param[out] normal 拟合平面的方程系数，依次包含a、b、c、d四个值。方程形式为ax+by+cz+d=0
*/
HEAD float CallingConvention fitPlane(pcl::PointCloud<pcl::PointXYZ> * in_pc,
	                                  float distance_thresh, int max_itera, float * normal);