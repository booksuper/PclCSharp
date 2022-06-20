#pragma once
#pragma once
#include <pcl/io/ply_io.h>
#include <iostream>
#include <vector>
#include <pcl/io/pcd_io.h>
#include <vtkSmartPointer.h>
#include <vtkSTLReader.h>
#include <vtkPolyData.h>
#include <pcl/io/vtk_io.h>
#include <pcl/io/vtk_lib_io.h>//包含vtkpolydata转点云函数
#include <pcl/io/obj_io.h>
#include <pcl/filters/uniform_sampling.h>
#include <pcl/filters/radius_outlier_removal.h>
#include <pcl/segmentation/region_growing.h>
#include <pcl/features/normal_3d.h>
#include <pcl/filters/extract_indices.h>
#include <pcl/filters/uniform_sampling.h>
#include <pcl/filters/passthrough.h>
#include <pcl/filters/statistical_outlier_removal.h>
#include <pcl/sample_consensus/model_types.h>
#include <pcl/sample_consensus/method_types.h>
#include <pcl/segmentation/sac_segmentation.h>
#include <pcl/segmentation/extract_clusters.h>//欧式聚类
#include <pcl/ModelCoefficients.h>
#include <pcl/common/transforms.h>
#include <pcl/common/common.h>
#include <pcl/common/common_headers.h>
#include <set>

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

/*
功能：对点云进行均匀下采样
param[in] in_pc 目标点云对象的指针
param[in] radius 均匀下采样分辨率参数，该值越大，则采样后点云越稀疏
param[out] out_pc 结果点云对象的指针
*/
HEAD void CallingConvention uniformDownSample(pcl::PointCloud<pcl::PointXYZ> * in_pc, double radius,
	pcl::PointCloud<pcl::PointXYZ> * out_pc);

/*
功能：对点云进行直通滤波
param[in] in_pc 目标点云对象的指针
param[in] axis_name 选哪个轴进行过滤
param[in] limit_min 过滤区间的最小值
param[in] limit_max 过滤区间的最大值
param[in] negative 输出是否反向，若为true，则输出[limit_min,limit_max]之外的点，一般为false
param[out] out_pc 结果点云对象的指针
*/
HEAD void CallingConvention passThroughFilter(pcl::PointCloud<pcl::PointXYZ> * in_pc, char * axis_name,
	                                          float limit_min, float limit_max, int negative,
	                                          pcl::PointCloud<pcl::PointXYZ> * out_pc);

/*
功能：使用统计滤波对点云去噪，将离群点去除
param[in] in_x 待采样点云x坐标
param[in] in_y 待采样点云y坐标
param[in] in_z 待采样点云z坐标
param[in] neighbor_num 近邻数，根据点的数目选取合适的邻居数,默认50
param[in] thresh 点的平均距离在[μ-α×σ,μ＋α×σ]之外的点被剔除，thresh即是α
param[out] out_x 采样后所有点的X坐标
param[out] out_y 采样后所有点的Y坐标
param[out] out_z 采样后所有点的Z坐标
*/
HEAD void CallingConvention staFilter(pcl::PointCloud<pcl::PointXYZ> * in_pc,
	                                  int neighbor_num, float thresh,
	                                  pcl::PointCloud<pcl::PointXYZ> * out_pc);

/*
功能：半径滤波，将离群点去除
*/
HEAD void CallingConvention radiusFilter(pcl::PointCloud<pcl::PointXYZ> * in_pc,
	                                     double radius, int num_thresh,
	                                     pcl::PointCloud<pcl::PointXYZ> * out_pc);


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
	                                     double distance_thresh,int MinClusterSize, int MaxClusterSize,
	                                     vector<pcl::PointIndices> * out_indice);


/*
功能：使用Ransac算法拟合分割后点云的平面，并返回平面倾斜角度
param[in] in_pc 目标点云对象指针
param[in] distance_thresh Ransac算法距离阈值
param[in] max_itera Ransac算法最大迭代次数
param[out] normal 拟合平面的方程系数，依次包含a、b、c、d四个值。方程形式为ax+by+cz+d=0
*/
HEAD float CallingConvention fitPlane(pcl::PointCloud<pcl::PointXYZ> * in_pc,
	                                  float distance_thresh, int max_itera, float * normal);


/*
功能：将倾斜的点云平面校正，也就是将点云平面变换至和水平面平行
param[in] in_x 目标点云对象指针
param[in] normal 输入点云的法向量
param[out] out_x 结果点云对象的指针
*/
HEAD void CallingConvention correctPlane(pcl::PointCloud<pcl::PointXYZ> * in_pc, float * normal,
	                                     pcl::PointCloud<pcl::PointXYZ> * out_pc);


/*
功能：sigam法则剔除异常值
param[in] in_pc 目标点云对象指针
param[in] sigam_thresh   sigam法则阈值，一般为3
param[out] out_x 结果点云对象指针
*/
HEAD void CallingConvention sigamFilter(pcl::PointCloud<pcl::PointXYZ> * in_pc, int sigam_thresh,
	                                    pcl::PointCloud<pcl::PointXYZ> * out_pc);

/*
功能：将点云按照列方向存储，每一列代表一个测量圆周，并输出用户指定的n列测量圆周点云
param[in] in_pc 目标点云对象指针
param[in] num   指定输出多少列测量圆周点云，一般来说，n越大，越逼近端面全跳动
当n为整个点云的列数目，输出的就是整个点云，测量的结果实际上是端面全跳动。n一般默认为20
param[out] out_x 结果点云对象指针
*/
HEAD void CallingConvention getRunoutPoints(pcl::PointCloud<pcl::PointXYZ> * in_pc, int num,
	                                        pcl::PointCloud<pcl::PointXYZ> * out_pc);

/*
功能：用于自己测试，直接返回端跳结果，不对外提供接口
将点云按照列方向存储，每一列代表一个测量圆周，并输出用户指定的n列测量圆周点云
//而且还会用西格玛法则剔除异常值，并返回最终的端面跳动
param[in] in_x 目标点云x坐标
param[in] in_y 目标点云y坐标
param[in] in_z 目标点云z坐标
param[in] num   指定输出多少列测量圆周点云，一般来说，n越大，越逼近端面全跳动
当n为整个点云的列数目，输出的就是整个点云，测量的结果实际上是端面全跳动。n一般默认为20
param[out] out_x 结果点云的X坐标
param[out] out_y 结果点云的Y坐标
param[out] out_z 结果点云的Z坐标
*/
HEAD float CallingConvention getRunoutPointsWithResult(pcl::PointCloud<pcl::PointXYZ> * in_pc, int num,
	                                                   pcl::PointCloud<pcl::PointXYZ> * out_pc);

/*
功能： 计算端面跳动,并返回该点云中最小最大点的索引
param[in] in_pc 目标点云对象指针
*/
HEAD double CallingConvention calculateRunout(pcl::PointCloud<pcl::PointXYZ> * in_pc, int * indices);

/*
功能： 根据点云索引，从一个点云对象中拷贝出来对应的点
*/
HEAD void CallingConvention copyPcBaseOnIndice(pcl::PointCloud<pcl::PointXYZ> * in_pc, 
	                                           pcl::PointIndices * in_indice,
	                                           pcl::PointCloud<pcl::PointXYZ> * out_pc);



