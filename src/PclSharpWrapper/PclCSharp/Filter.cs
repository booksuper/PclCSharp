using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

/*
 * Copyright (c) 2022, 舒登登
 * All rights reserved.
 * Auther:舒登登(ShuDengdeng)
 * Email:2237380450@qq.com
 * 2022626,点云滤波功能
 */

namespace PclCSharp
{
    public class Filter
    {
        /// @brief 对点云进行均匀下采样，可有效减少点云数量
        /// @param in_pc 输入的点云对象指针，使用PointCloudSharp类的PointCloudPointer属性
        /// @param radius 下采样半径，该值越大，采样后点云越稀疏
        /// @param out_pc 采样后的点云对象指针，使用PointCloudSharp类的PointCloudPointer属性
        /// @note 在DD马达端面跳动测量中，radius建议取100
        [DllImport("PclDll.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "uniformDownSample", CharSet = CharSet.Auto)]
        public static extern void uniformDownSample(IntPtr in_pc, double radius, IntPtr out_pc);

        /// @brief 对点云进行体素下采样，可有效减少点云数量
        /// @param in_pc 输入的点云对象指针，使用PointCloudSharp类的PointCloudPointer属性
        /// @param leaf_size 下采样叶子尺寸，该值越大，采样后点云越稀疏
        /// @param out_pc 采样后的点云对象指针，使用PointCloudSharp类的PointCloudPointer属性
        /// @note 采用体素中心点替代体素内的所有点云，这种方法比直接使用中心点要慢，
        /// 但是更加精确。该采样同时保存点云的形状特征，在提高配准，曲面重建，形状识别等算法速度中非常实用。
        /// @attention 体素下采样的点并不是点云中的原始点，而是通过计算的中心点，这和均匀下采样不一样
        /// 均匀下采样的点是点云原始点，所以在高精度测量领域，不应该用体素下采样！！！
        [DllImport("PclDll.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "voxelDownSample", CharSet = CharSet.Auto)]
        public static extern void voxelDownSample(IntPtr in_pc, double leaf_size, IntPtr out_pc);

        /// @brief 对点云进行近似体素下采样，可有效减少点云数量
        /// @param in_pc 输入的点云对象指针，使用PointCloudSharp类的PointCloudPointer属性
        /// @param leaf_size 下采样叶子尺寸，该值越大，采样后点云越稀疏
        /// @param out_pc 采样后的点云对象指针，使用PointCloudSharp类的PointCloudPointer属性
        /// @note 近似体素下采样比体素下采样速度更快，同样参数的情况下，采样后的点数也更多。它逼近的是体素质心
       
        [DllImport("PclDll.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "approximateVoxelDownSample", CharSet = CharSet.Auto)]
        public static extern void approximateVoxelDownSample(IntPtr in_pc, double leaf_size, IntPtr out_pc);

        /// @brief 对点云进行直通滤波处理
        /// @details 直通滤波是一种依赖于坐标的滤波方法。通过指定要滤波的轴，并设置要过滤的大小区间来进行滤波
        /// @param in_pc 输入的点云对象指针，使用PointCloudSharp类的PointCloudPointer属性
        /// @param axis_name 指定对哪个轴过滤，只有三个参数供选择，分别是'x','y'和'z'。字母应小写
        /// @param limit_min 过滤区间的最小值
        /// @param limit_max 过滤区间的最大值
        /// @param negative 指定是否反向过滤。negative大于0时，说明是反向过滤，即剔除[limit_min,limit_max]区间之内的点，反之亦然
        /// @param out_pc 结果点云对象指针，使用PointCloudSharp类的PointCloudPointer属性
        /// @note 一般来说，建议将negative设置为0，不执行反向过滤，即只剔除[limit_min,limit_max]区间之外的点
        [DllImport("PclDll.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "passThroughFilter", CharSet = CharSet.Auto)]
        public static extern void passThroughFilter(IntPtr in_pc, [MarshalAs(UnmanagedType.LPStr)] string axis_name,
                                                    float limit_min, float limit_max, int negative, IntPtr out_pc);

        /// @brief 对点云进行统计滤波，可去除离群点
        /// @param in_pc 输入的点云对象指针，使用PointCloudSharp类的PointCloudPointer属性
        /// @param neighbor_num 指定一个点的邻居数目
        /// @param thresh 统计滤波阈值
        /// @param out_pc 结果点云对象指针，使用PointCloudSharp类的PointCloudPointer属性
        /// @note 点的平均距离在[μ-α×σ,μ＋α×σ]之外的点被剔除，thresh即是α，默认为1。
        /// μ和σ分别是整个点云距离的均值和标准差。在DD马达端面跳动测量中，neighbor_num建议取50，thresh建议取1
        [DllImport("PclDll.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "staFilter", CharSet = CharSet.Auto)]
        public static extern void staFilter(IntPtr in_pc, int neighbor_num, float thresh, IntPtr out_pc);

        /// @brief 半径滤波，可去除离群点
        /// @param in_pc 输入的点云对象指针，使用PointCloudXYZ类的PointCloudXYZPointer属性
        /// @param radius 指定半径范围
        /// @param num_thresh 在指定半径范围内的个数阈值。
        /// @param out_pc 结果点云对象指针，使用PointCloudXYZ类的PointCloudXYZPointer属性
        /// @note 半径滤波思路很简单，首先指定某点的半径范围，然后计算在该半径范围内的近邻点数目
        /// 若该数目小于指定的num_thresh，则剔除该点。该滤波剔除稀疏的点云效果好
        [DllImport("PclDll.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "radiusFilter", CharSet = CharSet.Auto)]
        public static extern void radiusFilter(IntPtr in_pc,  double radius, int num_thresh, IntPtr out_pc);

        /// @brief sigam法则剔除异常值
        /// @param in_pc 输入的点云对象指针，使用PointCloudSharp类的PointCloudPointer属性
        /// @param sigam_thresh sigam系数值，一般为3
        /// @param out_pc 结果点云对象指针，使用PointCloudSharp类的PointCloudPointer属性
        /// @note 在DD马达端面跳动测量中，sigam_thresh建议取2
        [DllImport("PclDll.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "sigamFilter", CharSet = CharSet.Auto)]
        public static extern void sigamFilter(IntPtr in_pc, int sigam_thresh, IntPtr out_pc);
    }
}
