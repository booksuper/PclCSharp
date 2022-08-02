using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PclCSharp
{
    public class Segmentation
    {
        /// @brief 封装后的区域生长，可去除无关部分的点云，直接返回点数最多的聚类
        /// @param in_pc 输入的点云对象指针，使用PointCloudXYZ类的PointCloudXYZPointer属性
        /// @param neighbor_num 指定一个点的邻居数目
        /// @param smooth_thresh 两法向量夹角阈值
        /// @param curva_thresh  曲率阈值
        /// @param MinClusterSize 成为一个点云簇的最小点数
        /// @param MaxClusterSize 成为一个点云簇的最大点数
        /// @param out_pc 结果点云对象指针，使用PointCloudSharp类的PointCloudPointer属性
        /// @note 在DD马达端面跳动测量中，neighbor_num建议取50，smooth_thresh和curva_thresh建议取1
        /// MinClusterSize建议取100，MaxClusterSize建议取5000000
        [DllImport("PclDll.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "modifiedGrowRegion", CharSet = CharSet.Auto)]
        public static extern void modifiedGrowRegion(IntPtr in_pc, int neighbor_num, float smooth_thresh, float curva_thresh,
                                             int MinClusterSize, int MaxClusterSize, IntPtr out_pc);

        /// @brief 原始区域生长
        /// @param in_pc 输入的点云对象指针，使用PointCloudXYZ类的PointCloudXYZPointer属性
        /// @param neighbor_num 指定一个点的邻居数目
        /// @param smooth_thresh 两法向量夹角阈值
        /// @param curva_thresh  曲率阈值
        /// @param MinClusterSize 成为一个点云簇的最小点数
        /// @param MaxClusterSize 成为一个点云簇的最大点数
        /// @param out_pc 点云簇向量指针，存储了各个点云簇的索引。使用PointIndices类的PointIndicesPointer属性
        /// @note 获得了out_pc指针之后，里面只是存储了点云簇的索引，并不是真实的点云，所以还需要使用Util类中的
        /// copyPcBaseOnIndice函数将对应的点云簇复制出来
        [DllImport("PclDll.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "oriGrowRegion", CharSet = CharSet.Auto)]
        public static extern void oriGrowRegion(IntPtr in_pc, int neighbor_num, float smooth_thresh, float curva_thresh,
                                                int MinClusterSize, int MaxClusterSize, IntPtr out_Indices);

        /// @brief 欧式聚类
        /// @param in_pc 输入的点云对象指针，使用PointCloudXYZ类的PointCloudXYZPointer属性
        /// @param distance_thresh 欧式聚类的距离阈值，欧式聚类小于这个距离的点被归为一类
        /// @param MinClusterSize 成为一个点云簇的最小点数
        /// @param MaxClusterSize 成为一个点云簇的最大点数
        /// @param out_pc 点云簇向量指针，存储了各个点云簇的索引。使用PointIndices类的PointIndicesPointer属性
        /// @note 获得了out_pc指针之后，里面只是存储了点云簇的索引，并不是真实的点云，所以还需要使用Util类中的
        /// copyPcBaseOnIndice函数将对应的点云簇复制出来。
        /// @attention 该算法时间复杂度有点高
        [DllImport("PclDll.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "euclideanCluster", CharSet = CharSet.Auto)]
        public static extern void euclideanCluster(IntPtr in_pc, double distance_thresh, int MinClusterSize,
                                                   int MaxClusterSize, IntPtr out_Indices);

        /// @brief 平面分割
        /// @param in_pc 输入的点云对象指针，使用PointCloudXYZ类的PointCloudXYZPointer属性
        /// @param distance_thresh 平面分割的距离阈值，也可以等同于平面厚度阈值，这个值越大，离平面更远的点也被算进平面来
        /// @param negative 为true，提取除平面以外的物体，为false提取平面
        /// @param out_pc 点云簇向量指针，存储了各个点云簇的索引。使用PointIndices类的PointIndicesPointer属性
        /// @param max_iterations 最大迭代次数 默认最大迭代250次
        [DllImport("PclDll.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "planeModelSegment", CharSet = CharSet.Auto)]
        public static extern void planeModelSegment(IntPtr in_pc, double distance_thresh, bool negative, IntPtr out_pc, int max_iterations = 250);

        /// @brief 圆柱分割
        /// @param in_pc 输入的点云对象指针，使用PointCloudXYZ类的PointCloudXYZPointer属性
        /// @param distance_thresh 内点到模型的距离允许最大值
        /// @param min_radius 半径下限
        /// @param max_radius 半径上限
        /// @param negative 为true，提取除圆柱以外的物体，为false提取圆柱
        /// @param out_pc 点云簇向量指针，存储了各个点云簇的索引。使用PointIndices类的PointIndicesPointer属性
        /// @param neighbor_num 指定一个点的邻居数目 
        /// @param normal_distance_weight 表面法线权重系数 默认0.1
        /// @param max_iterations 最大迭代次数 默认10000(迭代次数取大一点，因为圆柱不好分割)
        /// @attention 该算法鲁棒性不是很高，参数中的distance_thresh要设小一些，且半径范围应该设置的尽量准确
        [DllImport("PclDll.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "cylinderModelSegment", CharSet = CharSet.Auto)]
        public static extern void cylinderModelSegment(IntPtr in_pc, double distance_thresh, double min_radius, double max_radius,
            bool negative, IntPtr out_pc, int neighbor_num = 50, double normal_distance_weight = 0.1, int max_iterations = 10000);
    }
}
