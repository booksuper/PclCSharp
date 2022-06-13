using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PclCSharp
{
    public class SampleConsensus
    {
        /// @brief 使用Ransac算法拟合点云平面
        /// @param in_pc 输入的点云对象指针，使用PointCloudSharp类的PointCloudPointer属性
        /// @param distance_thresh 距离阈值
        /// @param max_itera 最大迭代次数
        /// @param normal 返回拟合平面的一般方程系数，依次包含a、b、c、d四个值。方程形式为ax+by+cz+d=0
        /// @return 返回的是拟合的点云平面与法向量是（0,0,1)平面的夹角
        /// @note 在DD马达端面跳动测量中，distance_thresh建议取10，max_itera建议取100
        [DllImport("PclDll.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "fitPlane", CharSet = CharSet.Auto)]
        public static extern float fitPlane(IntPtr in_pc, float distance_thresh, int max_itera, float[] normal);
    }
}
