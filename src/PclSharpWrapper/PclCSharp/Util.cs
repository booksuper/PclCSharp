using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PclCSharp
{
    public class Util
    {
        /// @brief 将点云校正到和法向量为（0,0,1)平面平行
        /// @param in_pc 输入的点云对象指针，使用PointCloudSharp类的PointCloudPointer属性
        /// @param normal 待校正平面的方程系数
        /// @param out_pc 结果点云对象指针，使用PointCloudSharp类的PointCloudPointer属性
        [DllImport("PclDll.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "correctPlane", CharSet = CharSet.Auto)]
        public static extern void correctPlane(IntPtr in_pc, float[] normal, IntPtr out_pc);

        /// @brief 将点云按照列方向存储，每一列代表一个测量圆周，并输出用户指定的n列测量圆周点云
        /// @param in_pc 输入的点云对象指针，使用PointCloudSharp类的PointCloudPointer属性
        /// @param num 指定输出几列测量圆周点云
        /// @param out_pc 结果点云对象指针，使用PointCloudSharp类的PointCloudPointer属性
        /// @note 一般来说，n越大，越逼近端面全跳动，当n为0,代表输出整个点云，测量的结果实际上是端面全跳动。
        /// 在DD马达端面跳动测量中，n建议取20
        [DllImport("PclDll.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "getRunoutPoints", CharSet = CharSet.Auto)]
        public static extern void getRunoutPoints(IntPtr in_pc, int num, IntPtr out_pc);

        /// @brief 将点云按照列方向存储，每一列代表一个测量圆周，并输出用户指定的n列测量圆周点云
        /// 除此之外，算法里面还会使用sigam法则剔除异常值并返回端面跳动值。
        /// @param in_pc 输入的点云对象指针，使用PointCloudSharp类的PointCloudPointer属性
        /// @param num 指定输出几列测量圆周点云
        /// @param out_pc 结果点云对象指针，使用PointCloudSharp类的PointCloudPointer属性
        /// @attention 该函数只供测试使用。使用者应调用getRunoutPoints函数
        [DllImport("PclDll.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "getRunoutPointsWithResult", CharSet = CharSet.Auto)]
        public static extern float getRunoutPointsWithResult(IntPtr in_pc, int num, IntPtr out_pc);


        /// @brief 计算端面跳动,并返回该点云中最小最大点的索引
        /// @param in_pc 输入的点云对象指针，使用PointCloudSharp类的PointCloudPointer属性
        /// @param MinMaxPointsIndices 返回该点云中Z值最小点和最大点的索引，方便可视化
        /// @return 返回端面跳动值
        [DllImport("PclDll.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "calculateRunout", CharSet = CharSet.Auto)]
        public static extern double calculateRunout(IntPtr in_pc, int[] MinMaxPointsIndices);

        /// @brief 根据点云的索引复制点云
        /// @param in_pc 输入的目标点云
        /// @param in_indice 输入的点云索引指针，这指定了你要从in_pc中复制的点云索引
        /// @param out_pc 返回复制好的点云
        [DllImport("PclDll.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "copyPcBaseOnIndice", CharSet = CharSet.Auto)]
        public static extern void copyPcBaseOnIndice(IntPtr in_pc, IntPtr in_indice, IntPtr out_pc);
    }
}
