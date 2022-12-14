﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PointCloudSharpDll
{
    //测试泛型编程，暂不提供对外接口
    class PointCloudXYZRGB
    {
        //返回一个空的点云对象指针
        [DllImport("PointCloudDll.dll", EntryPoint = "CreatePointCloud", CharSet = CharSet.Auto)]
        public static extern IntPtr CreatePointCloud();
        //从文件中加载点云，返回对应的点云对象指针
        [DllImport("PointCloudDll.dll", EntryPoint = "loadPcFile", CharSet = CharSet.Auto)]
        public static extern IntPtr loadPcFile([MarshalAs(UnmanagedType.LPStr)]string path);
        //删除点云指针
        [DllImport("PointCloudDll.dll", EntryPoint = "DeletePointCloud", CharSet = CharSet.Auto)]
        public static extern void DeletePointCloud(IntPtr PointCloudPointer);
        //返回点云大小
        [DllImport("PointCloudDll.dll", EntryPoint = "CountPointCloud", CharSet = CharSet.Auto)]
        public static extern int CountPointCloud(IntPtr PointCloudPointer);
        //返回点云的宽
        [DllImport("PointCloudDll.dll", EntryPoint = "getPointCloudW", CharSet = CharSet.Auto)]
        public static extern int getPointCloudW(IntPtr PointCloudPointer);
        //返回点云的高
        [DllImport("PointCloudDll.dll", EntryPoint = "getPointCloudH", CharSet = CharSet.Auto)]
        public static extern int getPointCloudH(IntPtr PointCloudPointer);
        //返回点云xyz的极值
        [DllImport("PointCloudDll.dll", EntryPoint = "getMinMaxXYZ", CharSet = CharSet.Auto)]
        public static extern void getMinMaxXYZ(IntPtr PointCloudPointer, double[] out_res);


        //返回点云对应索引的X
        [DllImport("PointCloudDll.dll", EntryPoint = "getX", CharSet = CharSet.Auto)]
        public static extern double getX(IntPtr PointCloudPointer, int index);
        //返回点云对应索引的Y
        [DllImport("PointCloudDll.dll", EntryPoint = "getY", CharSet = CharSet.Auto)]
        public static extern double getY(IntPtr PointCloudPointer, int index);
        //返回点云对应索引的Z
        [DllImport("PointCloudDll.dll", EntryPoint = "getZ", CharSet = CharSet.Auto)]
        public static extern double getZ(IntPtr PointCloudPointer, int index);

        //改变点云对应索引的X
        [DllImport("PointCloudDll.dll", EntryPoint = "setX", CharSet = CharSet.Auto)]
        public static extern void setX(IntPtr PointCloudPointer, int index, double x);
        //改变点云对应索引的y
        [DllImport("PointCloudDll.dll", EntryPoint = "setY", CharSet = CharSet.Auto)]
        public static extern void setY(IntPtr PointCloudPointer, int index, double y);
        //改变点云对应索引的z
        [DllImport("PointCloudDll.dll", EntryPoint = "setZ", CharSet = CharSet.Auto)]
        public static extern void setZ(IntPtr PointCloudPointer, int index, double z);

        //改变点云大小
        [DllImport("PointCloudDll.dll", EntryPoint = "reSize", CharSet = CharSet.Auto)]
        public static extern void reSize(IntPtr PointCloudPointer, int size);
        //压入一个点
        [DllImport("PointCloudDll.dll", EntryPoint = "push", CharSet = CharSet.Auto)]
        public static extern void push(IntPtr PointCloudPointer, double x, double y, double z);
        //弹出一个点
        [DllImport("PointCloudDll.dll", EntryPoint = "pop", CharSet = CharSet.Auto)]
        public static extern void pop(IntPtr PointCloudPointer);
        //清空所有点
        [DllImport("PointCloudDll.dll", EntryPoint = "clear", CharSet = CharSet.Auto)]
        public static extern void clear(IntPtr PointCloudPointer);


        //字段
        private IntPtr _PointCloudPointer;
        private int _Width;
        private int _Height;
        private int _Size;
        //private double[] _MinMaxXYZ;
        //private double _MinX;
        //private double _MinY;
        //private double _MinZ;
        //private double _MaxX;
        //private double _MaxY;
        //private double _MaxZ;


        //不用布尔值bool是因为C#中bool占4个字节，C++中占1个字节，直接传递会因为字节不一样出错
        // 声明width属性
        public int Width
        {
            get
            {
                _Width = getPointCloudW(_PointCloudPointer);
                return _Width;
            }

        }
        // 声明 Height 属性
        public int Height
        {
            get
            {
                _Height = getPointCloudH(_PointCloudPointer);
                return _Height;
            }

        }
        // 声明 size 属性
        public int Size
        {
            get
            {
                _Size = CountPointCloud(_PointCloudPointer);
                return _Size;
            }

        }

        //// 声明 MinX 属性
        //public double MinX
        //{
        //    get
        //    {
        //TODO
        //2022626
        //        //备忘录技巧，如果_MinMaxXYZ的值不为0，认为前面已经计算过极值了，就不需要再重复计算
        //        //有一个问题，点云对象改变了，_MinMaxXYZ值也不为0，但是此时需要重新计算，需要解决这个问题
        //        //if (_MinMaxXYZ[0] != 0)
        //        //{
        //        //    return _MinMaxXYZ[0];
        //        //}

        //        getMinMaxXYZ(_PointCloudPointer, _MinMaxXYZ);
        //        return _MinMaxXYZ[0];
        //    }

        //}

        //析构之后可能出错,目前都运行良好。他日若有问题，优先检查指针问题
        public IntPtr PointCloudXYZPointer
        {
            get
            {
                return _PointCloudPointer;
            }

        }

        ///@brief 无参构造函数
        ///@details 初始化一个空的点云对象
        public PointCloudXYZRGB()
        {
            //_MinMaxXYZ = new double[6] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0,};

            _PointCloudPointer = CreatePointCloud();
        }

        ///@brief 有参构造函数
        ///@details 传入点云文件地址，加载对应的点云对象到_PointCloudPointer中
        ///@param path 点云文件路径
        public PointCloudXYZRGB(string path)
        {
            //_MinMaxXYZ = new double[6] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, };
            _PointCloudPointer = loadPcFile(path);

        }
        ///@brief 无参析构函数
        ///@details 内存管理。当对象的生存期结束时，释放掉点云对象内存。这里容易出现野指针，万分注意！
        ~PointCloudXYZRGB()
        {
            DeletePointCloud(_PointCloudPointer);
        }

        ///@brief 获得某点的X值
        ///@param index 点的索引号
        public double GetX(int index)
        {
            return getX(_PointCloudPointer, index);
        }

        ///@brief 获得某点的Y值
        ///@param index 点的索引号
        public double GetY(int index)
        {
            return getY(_PointCloudPointer, index);
        }

        ///@brief 获得某点的Z值
        ///@param index 点的索引号
        public double GetZ(int index)
        {
            return getZ(_PointCloudPointer, index);
        }

        ///@brief 根据索引改变某点的X值
        ///@param index 点的索引号
        ///@param x 想要改变的值
        public void SetX(int index, double x)
        {
            setX(_PointCloudPointer, index, x);
        }

        ///@brief 根据索引改变某点的Y值
        ///@param index 点的索引号
        ///@param y 想要改变的值
        public void SetY(int index, double y)
        {
            setY(_PointCloudPointer, index, y);
        }

        ///@brief 根据索引改变某点的Z值
        ///@param index 点的索引号
        ///@param z 想要改变的值
        public void SetZ(int index, double z)
        {
            setZ(_PointCloudPointer, index, z);
        }

        ///@brief 重置点云大小
        ///@param size 点云的大小
        public void ReSize(int size)
        {
            reSize(_PointCloudPointer, size);
        }

        ///@brief 压入一个点，压入的位置在元素最后面
        ///@param x 压入点的x值
        ///@param y 压入点的y值
        ///@param z 压入点的z值
        public void Push(double x, double y, double z)
        {
            push(_PointCloudPointer, x, y, z);
        }

        ///@brief 弹出一个点，类似于出栈，弹出的是元素中最后一个点
        public void Pop()
        {
            pop(_PointCloudPointer);
        }

        ///@brief 清空点云对象中所有点，对象的size属性变为0
        public void Clear()
        {
            clear(_PointCloudPointer);
        }

        ///@brief 获得点云中的极值
        ///@param out_res 极值结果
        ///@details 使用一个double数组保存结果，大小为6.其中，
        ///第一第二个元素依次是x的最小值、最大值，第三第四个元素是y的最小最大值，后面依次类推
        public void GetMinMaxXYZ(double[] out_res)
        {
            getMinMaxXYZ(_PointCloudPointer, out_res);
        }

    }
}

