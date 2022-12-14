#pragma once
#include <pcl/point_cloud.h>
#include <pcl/point_types.h>
#include <iostream>
#include <string>
#include <pcl/io/ply_io.h>
#include <pcl/io/pcd_io.h>
#include <pcl/common/common.h>

/*
 * Copyright (c) 2022, ��ǵ�
 * All rights reserved.
 * Auther:��ǵ�(ShuDengdeng)
 * Email:2237380450@qq.com
 * ��C#���ṩPcl���ݽṹ�Ľӿ�
 */

using namespace std;

//���嵼����ʽ����C���Եķ�ʽ��������ΪC���Է�ʽ���������ֲ���
#define EXTERNC extern "C"
//����dll������ʽ���˴��ǵ���������ǵ�����Ϊdllinport
#define HEAD EXTERNC __declspec(dllexport)
//�������Լ�����˴�ѡ���׼����Լ����Ҳ������c����Լ��
#define CallingConvention __stdcall

//��ͬ���ݽṹ�ĺ�
#define POINTXYZ 1
#define POINTINDICES 1
#define POINTXYZRGB 0

/*PointCloudXYZ����غ����ӿ�*/
#if POINTXYZ
//���ظ����ָ��
HEAD pcl::PointCloud<pcl::PointXYZ> * CallingConvention CreatePointCloud()
{
	return new pcl::PointCloud<pcl::PointXYZ>();
}
//�ӵ����ļ��м��ص���
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
//ɾ������ָ��
HEAD void CallingConvention DeletePointCloud(pcl::PointCloud<pcl::PointXYZ> * pc)
{
	delete  pc;
}
//���ص��ƴ�С
HEAD int CallingConvention CountPointCloud(pcl::PointCloud<pcl::PointXYZ> * pc)
{
	return  pc->size();
}

//���ص��Ƹ߶�
HEAD int CallingConvention getPointCloudH(pcl::PointCloud<pcl::PointXYZ> * pc)
{
	return  pc->height;
}

//���ص��ƿ��
HEAD int CallingConvention getPointCloudW(pcl::PointCloud<pcl::PointXYZ> * pc)
{
	//ֱ��ʹ��pc->width�Ļ��������ֵֻ��һ��ʼ��ʱ���䣬������ƴ�С�ı䣬���ֵ�ǲ�����
	//��Ϊ��ֻ��һ�����ԣ����������ŵ��ƴ�С�ı������Ӧ�ı�ֵ
	//���ڴ󲿷ֵ��ƶ���������ƣ�widthֵ����sizeֵ����������ֱ����size����

	return  pc->size();
}

//���ص���xyz�ļ�ֵ
HEAD void CallingConvention getMinMaxXYZ(pcl::PointCloud<pcl::PointXYZ> * pc,double * out_res)
{
	pcl::PointXYZ minp(0, 0, 0);
	pcl::PointXYZ maxp(0, 0, 0);
	pcl::getMinMax3D(*pc, minp, maxp);
	out_res[0] = minp.x;//��Сx
	out_res[1] = maxp.x;//���x
	out_res[2] = minp.y;//��Сy
	out_res[3] = maxp.y;//���y
	out_res[4] = minp.z;//��Сz
	out_res[5] = maxp.z;//���z


}

//��������index��Xֵ
HEAD double CallingConvention getX(pcl::PointCloud<pcl::PointXYZ> * pc, int index)
{
	return  pc->points[index].x;
}
//��������index��Yֵ
HEAD double CallingConvention getY(pcl::PointCloud<pcl::PointXYZ> * pc, int index)
{
	return  pc->points[index].y;
}
//��������index��Zֵ
HEAD double CallingConvention getZ(pcl::PointCloud<pcl::PointXYZ> * pc, int index)
{
	return  pc->points[index].z;
}
//�ı�����index��Xֵ
HEAD void CallingConvention setX(pcl::PointCloud<pcl::PointXYZ> * pc, int index,double x)
{
	pc->points[index].x = x;
}
//�ı�����index��Xֵ
HEAD void CallingConvention setY(pcl::PointCloud<pcl::PointXYZ> * pc, int index, double y)
{
	pc->points[index].y = y;
}
//�ı�����index��Xֵ
HEAD void CallingConvention setZ(pcl::PointCloud<pcl::PointXYZ> * pc, int index, double z)
{
	pc->points[index].z = z;
}

//�ı���ƵĴ�С
HEAD void CallingConvention reSize(pcl::PointCloud<pcl::PointXYZ> * pc, int size)
{
	pc->points.resize(size);
}
//ѹ��һ����
HEAD void CallingConvention push(pcl::PointCloud<pcl::PointXYZ> * pc, double x, double y, double z)
{
	pcl::PointXYZ p(x, y, z);
	pc->points.push_back(p);
}
//����һ����
HEAD void CallingConvention pop(pcl::PointCloud<pcl::PointXYZ> * pc)
{
	pc->points.pop_back();
}

//��յ��ƶ��������е�
HEAD void CallingConvention clear(pcl::PointCloud<pcl::PointXYZ> * pc)
{
	pc->clear();
}

#endif


/*PointIndices����غ����ӿ�*/
#if POINTINDICES
//���ص�������������ָ��
HEAD vector<pcl::PointIndices> * CallingConvention CreatePointIndices()
{
	return new vector<pcl::PointIndices>();
}
//ɾ��ָ��
HEAD void CallingConvention DeletePointIndices(vector<pcl::PointIndices> * in_indice)
{
	delete in_indice;
}
//���ص��������Ĵ�С
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
#endif


/*PointXYZRGB����غ����ӿ�,����ʹ��ģ���̣����ӷ�����*/
#if POINTXYZRGB
//���ظ����ָ��

HEAD pcl::PointCloud<pcl::PointXYZ> * CallingConvention CreatePointCloud()
{
	return new pcl::PointCloud<pcl::PointXYZ>();
}
//�ӵ����ļ��м��ص���
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

#endif