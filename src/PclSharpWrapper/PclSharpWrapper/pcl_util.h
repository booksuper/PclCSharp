#pragma once
#include <iostream>
#include <vector>
#include <pcl/common/transforms.h>
#include <pcl/common/common.h>
#include <pcl/common/common_headers.h>

/*
 * Copyright (c) 2022, 舒登登
 * All rights reserved.
 * Auther:舒登登(ShuDengdeng)
 * Email:2237380450@qq.com
 * 点云处理的一些工具函数，不提供对外接口
 */

using namespace std;

//用于区域生长，将点数最多的点簇的索引返回
int getMaxPointCluster(vector<pcl::PointIndices> cluster);
//用于平面拟合中获得拟合后的平面与xy参考面的倾斜角
double getAngle(double line1[3], double line2[3]);
//用于获取两向量的变旋转换矩阵
Eigen::Matrix4f getRotationMatrix(Eigen::Vector3f vector_before, Eigen::Vector3f vector_after);
//将向量点云连接为点云对象
void connect_pc(vector<pcl::PointCloud<pcl::PointXYZ>>& input_pc1, pcl::PointCloud<pcl::PointXYZ> & out_cloud);

//函数重载，3西格玛法则剔除异常值，输入是点云，输出是点云向量
void stdDev_filter(vector<pcl::PointCloud<pcl::PointXYZ>>& input_cloud, vector<pcl::PointCloud<pcl::PointXYZ>>& out_cloud);