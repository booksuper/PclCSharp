using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PointCloudSharp
{
    public class PointIndices
    {
        //返回一个空的点云索引指针
        [DllImport("PointCloudDll.dll", EntryPoint = "CreatePointIndices", CharSet = CharSet.Auto)]
        public static extern IntPtr CreatePointIndices();

        //删除指针
        [DllImport("PointCloudDll.dll", EntryPoint = "DeletePointIndices", CharSet = CharSet.Auto)]
        public static extern void DeletePointIndices(IntPtr PointIndicesPointer);
        //返回点云索引总共有几个
        [DllImport("PointCloudDll.dll", EntryPoint = "CountPointIndices", CharSet = CharSet.Auto)]
        public static extern int CountPointIndices(IntPtr PointIndicesPointer);


        //根据指定的序号返回对应的点云索引指针
        [DllImport("PointCloudDll.dll", EntryPoint = "getPointIndice", CharSet = CharSet.Auto)]
        public static extern IntPtr getPointIndice(IntPtr PointIndicesPointer, int pos);
        //根据指定的序号，返回这个点云索引总共有多少个点
        [DllImport("PointCloudDll.dll", EntryPoint = "getSizeOfIndice", CharSet = CharSet.Auto)]
        public static extern int getSizeOfIndice(IntPtr PointIndicesPointer, int pos);



        //字段
        private IntPtr _PointIndicesPointer;
        private int _Size;
        //不用布尔值bool是因为C#中bool占4个字节，C++中占1个字节，直接传递会因为字节不一样出错

        // 声明 size 属性
        public int Size
        {
            get
            {
                _Size = CountPointIndices(_PointIndicesPointer);
                return _Size;
            }

        }
        //析构之后可能出错
        public IntPtr PointIndicesPointer
        {
            get
            {
                return _PointIndicesPointer;
            }

        }

        ///@brief 无参构造函数
        ///@details 初始化一个空的点云对象
        public PointIndices()
        {
            _PointIndicesPointer = CreatePointIndices();
        }


        ///@brief 无参析构函数
        ///@details 内存管理。当对象的生存期结束时，释放掉点云对象内存。这里容易出现野指针，万分注意！
        ~PointIndices()
        {
            DeletePointIndices(_PointIndicesPointer);
        }

        ///@brief 根据索引获得对应的点索引指针
        ///@param index 索引号
        public IntPtr GetPointIndice(int index)
        {
            return getPointIndice(_PointIndicesPointer, index);
        }

        ///@brief 根据索引获得对应点云簇索引的总大小
        ///@param index 索引号
        public int GetSizeOfIndice(int index)
        {
            return getSizeOfIndice(_PointIndicesPointer, index);
        }


    }
}
