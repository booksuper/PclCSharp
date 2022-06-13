using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PclCSharp
{
    public class Io
    {
        /// @brief 加载ply文件
        /// @param path 文件路径
        /// @param pc 点云对象指针，此处使用PointCloudSharp类的PointCloudPointer属性
        [DllImport("PclDll.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "loadPlyFile", CharSet = CharSet.Auto)]
        public static extern int loadPlyFile([MarshalAs(UnmanagedType.LPStr)] string path, IntPtr pc);

        /// @brief 加载pcd文件
        /// @param path 文件路径
        /// @param pc 点云对象指针，此处使用PointCloudSharp类的PointCloudPointer属性
        [DllImport("PclDll.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "loadPcdFile", CharSet = CharSet.Auto)]
        public static extern int loadPcdFile([MarshalAs(UnmanagedType.LPStr)] string path, IntPtr pc);

        /// @brief 保存pcd文件
        /// @param path 保存的路径
        /// @param pc 点云对象指针，此处使用PointCloudSharp类的PointCloudPointer属性
        /// @param binaryMode 是否保存为二进制文件，若参数大于0，则将点云文件保存为二进制文件，反之亦然
        /// @note 如果需要查看保存的点云文件数据，则binaryMode参数应该小于或等于0；如果需要提高文件读取速度，binaryMode应大于0
        [DllImport("PclDll.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "savePcdFile", CharSet = CharSet.Auto)]
        public static extern void savePcdFile([MarshalAs(UnmanagedType.LPStr)] string path, IntPtr pc, int binaryMode);

        /// @brief 保存ply文件
        /// @param path 保存的路径
        /// @param pc 点云对象指针，此处使用PointCloudSharp类的PointCloudPointer属性
        /// @param binaryMode 是否保存为二进制文件，若参数大于0，则将点云文件保存为二进制文件，反之亦然
        /// @note 如果需要查看保存的点云文件数据，则binaryMode参数应该小于或等于0；如果需要提高文件读取速度，binaryMode应大于0
        [DllImport("PclDll.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "savePlyFile", CharSet = CharSet.Auto)]
        public static extern void savePlyFile([MarshalAs(UnmanagedType.LPStr)] string path, IntPtr pc, int binaryMode);
    }
}
