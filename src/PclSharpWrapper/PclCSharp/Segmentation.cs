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
    }
}
