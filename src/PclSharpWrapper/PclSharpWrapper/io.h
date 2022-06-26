#pragma once
#include <pcl/io/ply_io.h>
#include <iostream>
#include <pcl/io/pcd_io.h>
#include <vtkSmartPointer.h>
#include <vtkSTLReader.h>
#include <vtkPolyData.h>
#include <pcl/io/vtk_io.h>
#include <pcl/io/vtk_lib_io.h>//包含vtkpolydata转点云函数
#include <pcl/io/obj_io.h>
#include <pcl/common/transforms.h>
#include <pcl/common/common.h>
#include <pcl/common/common_headers.h>


/*
 * Copyright (c) 2022, 舒登登
 * All rights reserved.
 * Auther:舒登登(ShuDengdeng)
 * Email:2237380450@qq.com
 * 2022626,点云输入输出功能
 */

using namespace std;
//定义导出方式：以C语言的方式导出，因为C语言方式函数名保持不变
#define EXTERNC extern "C"
//定义dll导出方式，此处是导出，如果是导入则为dllinport
#define HEAD EXTERNC __declspec(dllexport)
//定义调用约定，此处选择标准调用约定，也可以用c调用约定
#define CallingConvention __stdcall


/*
功能：将pointcloud结构用三个数组存储,这是最原始的方式，后面将点云对象封装了，这方法弃用了
*/
void Pc2Array(pcl::PointCloud<pcl::PointXYZ>::Ptr cloud_in, double * out_x, double * out_y, double * out_z);

/*
功能：加载ply、pcd格式点云文件，并将点云中的X、Y和Z坐标存储在三个double数组中,函数弃用
param[in] path 点云文件路径
param[out] out_x 所有点的X坐标
param[out] out_y 所有点的Y坐标
param[out] out_z 所有点的Z坐标
成功加载文件返回1，失败则返回0
*/
HEAD int CallingConvention loadFile(char* path, double * out_x, double * out_y, double * out_z);

/*
功能：加载ply格式点云文件，并将点云存在PointCloud对象中
param[in] path 点云文件路径
param[out] pc 点云对象
成功加载文件返回1，失败则返回0
*/
HEAD int CallingConvention loadPlyFile(char* path, pcl::PointCloud<pcl::PointXYZ> * pc);

/*
功能：加载pcd格式点云文件，并将点云存在PointCloud对象中
param[in] path 点云文件路径
param[out] pc 点云对象
成功加载文件返回1，失败则返回0
*/
HEAD int CallingConvention loadPcdFile(char* path, pcl::PointCloud<pcl::PointXYZ> * pc);

/*
功能：加载obj文件
param[in] path 文件路径
param[out] pc 点云对象
成功加载文件返回1，失败则返回0
*/
HEAD int CallingConvention loadObjFile(char* path, pcl::PointCloud<pcl::PointXYZ> * pc);


/*
功能：保存pcd格式点云文件
param[in] path 点云文件路径
param[out] pc 点云对象
成功加载文件返回1，失败则返回0
*/
HEAD void CallingConvention savePcdFile(char* path, pcl::PointCloud<pcl::PointXYZ> * pc, int binaryMode);

/*
功能：保存ply格式点云文件
param[in] path 点云文件路径
param[out] pc 点云对象
成功加载文件返回1，失败则返回0
*/
HEAD void CallingConvention savePlyFile(char* path, pcl::PointCloud<pcl::PointXYZ> * pc, int binaryMode);

/*
功能：将stl格式文件转为点云对象
param[in] path stl格式文件路径
param[out] pc 点云对象
*/
HEAD void CallingConvention stl2PointCloud(char* path, pcl::PointCloud<pcl::PointXYZ> * pc);

/*
//TODO 暂未实现该功能 2022620
功能：点云存为obj格式
param[in] path 存储路径
param[out] pc 待存点云对象
*/
HEAD void CallingConvention saveObjFile(char* path, pcl::PointCloud<pcl::PointXYZ> * pc);