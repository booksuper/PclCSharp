#pragma once
#include <pcl/point_cloud.h>
#include <pcl/point_types.h>
#include <iostream>
#include <string>
#include <pcl/io/ply_io.h>
#include <pcl/io/pcd_io.h>
#include <pcl/common/common.h>

/*
 * Copyright (c) 2022, 舒登登
 * All rights reserved.
 * Auther:舒登登(ShuDengdeng)
 * Email:2237380450@qq.com
 * 给C#层提供Pcl数据结构的接口
 */

using namespace std;

//定义导出方式：以C语言的方式导出，因为C语言方式函数名保持不变
#define EXTERNC extern "C"
//定义dll导出方式，此处是导出，如果是导入则为dllinport
#define HEAD EXTERNC __declspec(dllexport)
//定义调用约定，此处选择标准调用约定，也可以用c调用约定
#define CallingConvention __stdcall

//返回该类的指针
HEAD pcl::PointCloud<pcl::PointXYZ> * CallingConvention CreatePointCloud()
{
	return new pcl::PointCloud<pcl::PointXYZ>();
}
//从点云文件中加载点云
HEAD pcl::PointCloud<pcl::PointXYZ> * CallingConvention loadPcFile(char * path)
{
	pcl::PointCloud<pcl::PointXYZ> * cloud = new pcl::PointCloud<pcl::PointXYZ>();

	if (pcl::io::loadPLYFile(path, *cloud) == -1)
	{
		pcl::PointXYZ p(0, 0, 0);
		cloud->points.push_back(p);
		return cloud;
	}
	else
	{
		return cloud;
	}

}
//删除点云指针
HEAD void CallingConvention DeletePointCloud(pcl::PointCloud<pcl::PointXYZ> * pc)
{
	delete  pc;
}
//返回点云大小
HEAD int CallingConvention CountPointCloud(pcl::PointCloud<pcl::PointXYZ> * pc)
{
	return  pc->size();
}

//返回点云高度
HEAD int CallingConvention getPointCloudH(pcl::PointCloud<pcl::PointXYZ> * pc)
{
	return  pc->height;
}

//返回点云宽度
HEAD int CallingConvention getPointCloudW(pcl::PointCloud<pcl::PointXYZ> * pc)
{
	//直接使用pc->width的话，则这个值只有一开始的时候会变，假如点云大小改变，这个值是不会变的
	//因为它只是一个属性，并不会随着点云大小改变而自适应改变值
	//鉴于大部分点云都是无序点云，width值就是size值，所以这里直接用size方法

	return  pc->size();
}

//返回点云xyz的极值
HEAD void CallingConvention getMinMaxXYZ(pcl::PointCloud<pcl::PointXYZ> * pc,double * out_res)
{
	pcl::PointXYZ minp(0, 0, 0);
	pcl::PointXYZ maxp(0, 0, 0);
	pcl::getMinMax3D(*pc, minp, maxp);
	out_res[0] = minp.x;//最小x
	out_res[1] = maxp.x;//最大x
	out_res[2] = minp.y;//最小y
	out_res[3] = maxp.y;//最大y
	out_res[4] = minp.z;//最小z
	out_res[5] = maxp.z;//最大z


}

//返回索引index的X值
HEAD double CallingConvention getX(pcl::PointCloud<pcl::PointXYZ> * pc, int index)
{
	return  pc->points[index].x;
}
//返回索引index的Y值
HEAD double CallingConvention getY(pcl::PointCloud<pcl::PointXYZ> * pc, int index)
{
	return  pc->points[index].y;
}
//返回索引index的Z值
HEAD double CallingConvention getZ(pcl::PointCloud<pcl::PointXYZ> * pc, int index)
{
	return  pc->points[index].z;
}
//改变点云的大小
HEAD void CallingConvention reSize(pcl::PointCloud<pcl::PointXYZ> * pc, int size)
{
	pc->points.resize(size);
}
//压入一个点
HEAD void CallingConvention push(pcl::PointCloud<pcl::PointXYZ> * pc, double x, double y, double z)
{
	pcl::PointXYZ p(x, y, z);
	pc->points.push_back(p);
}
//弹出一个点
HEAD void CallingConvention pop(pcl::PointCloud<pcl::PointXYZ> * pc)
{
	pc->points.pop_back();
}

//清空点云对象中所有点
HEAD void CallingConvention clear(pcl::PointCloud<pcl::PointXYZ> * pc)
{
	pc->clear();
}



//点云索引的相关函数
//返回点云索引向量的指针
HEAD vector<pcl::PointIndices> * CallingConvention CreatePointIndices()
{
	return new vector<pcl::PointIndices>();
}
//删除指针
HEAD void CallingConvention DeletePointIndices(vector<pcl::PointIndices> * in_indice)
{
	delete in_indice;
}
//返回点云索引的大小
HEAD int CallingConvention CountPointIndices(vector<pcl::PointIndices> * in_indice)
{
	return in_indice->size();
}

HEAD pcl::PointIndices * CallingConvention getPointIndice(vector<pcl::PointIndices> * in_indice, int pos)
{
	pcl::PointIndices * indices = new pcl::PointIndices();
	*indices = (*in_indice)[pos];
	return indices;
}

HEAD int CallingConvention getSizeOfIndice(vector<pcl::PointIndices> * in_indice, int pos)
{
	return (*in_indice)[pos].indices.size();
}


