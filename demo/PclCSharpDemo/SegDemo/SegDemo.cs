using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Kitware.VTK;
using PointCloudSharp;
using PclCSharp;

/*
 * Copyright (c) 2022, 舒登登
 * All rights reserved.
 * Auther:舒登登(ShuDengdeng)
 * Email:2237380450@qq.com
 * Segmentation模块demo
 * time:2022.12.09
 */

namespace SegDemo
{
    public partial class SegDemo : Form
    {
        //创建VTK控件renderWindowControl1
        private Kitware.VTK.RenderWindowControl renderWindowControl1;
        private Kitware.VTK.RenderWindowControl renderWindowControl2;
        vtkPoints Points = vtkPoints.New();
        vtkPolyData polyData = vtkPolyData.New();
        vtkPolyDataMapper polyDataMapper = vtkPolyDataMapper.New();
        vtkActor actor = vtkActor.New();
        //圆形点云的参数
        const int currentTheta = 0;
        const double intervalPerPoint = 4;
        const int numPoints = 100;
        public SegDemo()
        {
            InitializeComponent();
            //将VTK控件renderWindowControl添加到父控件panel里
            //必须使用panel控件充当容器装载vtk控件，直接在界面中拖曳vtk控件会报错
            renderWindowControl1 = new Kitware.VTK.RenderWindowControl();
            //将renderWindowControl1边框填充到其父组件里
            renderWindowControl1.Dock = DockStyle.Fill;
            //将指定的renderWindowControl1控件添加到panel1控件集合中
            panel1.Controls.Add(renderWindowControl1);

            renderWindowControl2 = new Kitware.VTK.RenderWindowControl();
            //将renderWindowControl1边框填充到其父组件里
            renderWindowControl2.Dock = DockStyle.Fill;
            //将指定的renderWindowControl1控件添加到panel1控件集合中
            panel2.Controls.Add(renderWindowControl2);
        }

        /*按照指定的轴设定点云颜色属性
       极小值处设置为蓝色（0，0，255），并且蓝色通道的数值由255向中间位置按坐标变化递减至0，
       绿色通道的数值由0按坐标变化递增至255；中间位置设置为绿色（0，255，0），
       并且绿色通道的数值由255按坐标变化递减至0，红色通道数值由0按坐标变化递增至255；
       极大值位置设置为（255，0，0）*/
        vtkUnsignedCharArray setColorBaseAxis(char axis, PointCloudXYZ in_pc)
        {
            vtkUnsignedCharArray colors_rgb = vtkUnsignedCharArray.New();
            //点云的极值,第一第二个元素分别是x的最小最大值，yz依次类推
            double[] minmax = new double[6];
            in_pc.GetMinMaxXYZ(minmax);
            double z = minmax[5] - minmax[4];//z轴的差值
            double y = minmax[3] - minmax[2];//y轴的差值
            double x = minmax[1] - minmax[0];//x轴的差值
            double z_median = z / 2;
            double y_median = y / 2;
            double x_median = x / 2;
            colors_rgb.SetNumberOfComponents(3);//设置颜色的组分，因为是rgb，所以组分为3
            double r = 0, g = 0, b = 0;
            if (axis == 'x')
            {
                for (int i = 0; i < in_pc.Size; i++)
                {
                    //中间值为界，x值大于中间值的b组分为0，r组分逐渐变大
                    if ((in_pc.GetX(i) - minmax[0]) > x_median)
                    {
                        //x值要先归一化再乘以255，不然数值将会超出255

                        r = (255 * ((in_pc.GetX(i) - minmax[0] - x_median) / x_median)); ;
                        g = (255 * (1 - ((in_pc.GetX(i) - minmax[0] - x_median) / x_median)));
                        b = 0;
                        colors_rgb.InsertNextTuple3(r, g, b);
                    }
                    //中间值为界，x值小于中间值的r组分为0，b组分逐渐变大
                    else
                    {
                        //x值要先归一化再乘以255，不然数值将会超出255
                        r = 0;
                        g = (255 * ((in_pc.GetX(i) - minmax[0]) / x_median));
                        b = (255 * (1 - ((in_pc.GetX(i) - minmax[0]) / x_median))); ;
                        colors_rgb.InsertNextTuple3(r, g, b);
                    }
                }
            }
            else if (axis == 'y')
            {
                for (int i = 0; i < in_pc.Size; i++)
                {
                    //中间值为界，y值大于中间值的b组分为0，r组分逐渐变大
                    if ((in_pc.GetY(i) - minmax[2]) > y_median)
                    {
                        //y值要先归一化再乘以255，不然数值将会超出255
                        r = (255 * ((in_pc.GetY(i) - minmax[2] - y_median) / y_median)); ;
                        g = (255 * (1 - ((in_pc.GetY(i) - minmax[2] - y_median) / y_median)));
                        b = 0;
                        colors_rgb.InsertNextTuple3(r, g, b);
                    }
                    //中间值为界，y值小于中间值的r组分为0，b组分逐渐变大
                    else
                    {
                        r = 0;
                        g = (255 * ((in_pc.GetY(i) - minmax[2]) / y_median));
                        b = (255 * (1 - ((in_pc.GetY(i) - minmax[2]) / y_median))); ;
                        colors_rgb.InsertNextTuple3(r, g, b);
                    }
                }
            }
            else if (axis == 'z')
            {

                for (int i = 0; i < in_pc.Size; i++)
                {
                    //中间值为界，z值大于中间值的b组分为0，r组分逐渐变大
                    if ((in_pc.GetZ(i) - minmax[4]) > z_median)
                    {
                        //z值要先归一化再乘以255，不然数值将会超出255
                        r = (255 * ((in_pc.GetZ(i) - minmax[4] - z_median) / z_median)); ;
                        g = (255 * (1 - ((in_pc.GetZ(i) - minmax[4] - z_median) / z_median)));
                        b = 0;
                        colors_rgb.InsertNextTuple3(r, g, b);
                    }
                    //中间值为界，z值小于中间值的r组分为0，b组分逐渐变大
                    else
                    {
                        r = 0;
                        g = (255 * ((in_pc.GetZ(i) - minmax[4]) / z_median));
                        b = (255 * (1 - ((in_pc.GetZ(i) - minmax[4]) / z_median)));
                        colors_rgb.InsertNextTuple3(r, g, b);
                    }
                }
            }

            return colors_rgb;
        }

       

        //生成圆形点云
        void generatePointByAngel(double theta)
        {
            double radius = 4;
            //每个循环生成一个点，循环结束就生成了一根线点云
            for (int i = 0; i < numPoints; i++)
            {
               
                double x = Math.Cos(theta * (Math.PI) / 180) * radius;
                double y = Math.Sin(theta * (Math.PI) / 180) * radius;
                System.Random random = new Random();
                double z = random.NextDouble() * 10 - 5;//z为-5到5
                if (theta > 160 && theta < 210)
                {
                    z = random.NextDouble() * 10 - 30;
                } 
                if (theta > 40 && theta < 90)
                {
                    z = random.NextDouble() * 10 + 30;
                }
                Points.InsertNextPoint(x, y, z);
                radius += intervalPerPoint;
            }
        }

        //Delay function
        void delay(int milliSecond)
        {
            int start = Environment.TickCount;
            while (Math.Abs(Environment.TickCount - start) < milliSecond)
            {
                Application.DoEvents();
            }
        }


        //将点云对象可视化
        vtkRenderer showPointCloud(PointCloudXYZ in_pc)
        {
            //显示点云
            vtkPoints points = vtkPoints.New();
            //把点云指针中的点依次放进points
            for (int i = 0; i < in_pc.Size; i++)
            {
                points.InsertNextPoint(in_pc.GetX(i), in_pc.GetY(i), in_pc.GetZ(i));

            }
            //创建每个点的属性数据，这里代表颜色
            vtkUnsignedCharArray colors_rgb = setColorBaseAxis('z', in_pc);
            vtkPolyData polydata = vtkPolyData.New();
            //将points数据传进polydata
            polydata.SetPoints(points);
            //将点数据的颜色属性传进polydata
            polydata.GetPointData().SetScalars(colors_rgb);

            vtkVertexGlyphFilter glyphFilter = vtkVertexGlyphFilter.New();
            glyphFilter.SetInputConnection(polydata.GetProducerPort());
            // 新建制图器
            vtkPolyDataMapper mapper = vtkPolyDataMapper.New();
            //设置颜色模式，这个是默认模式，不加也行
            //即把unsigned char类型的标量属性数据当作颜色值，不执行隐式。对于其他类型的标量数据，将通过查询表映射
            mapper.SetColorModeToDefault();

            mapper.SetScalarVisibility(1);
            mapper.SetInputConnection(glyphFilter.GetOutputPort());// 连接管道

            vtkActor actor = vtkActor.New(); // 新建角色
            actor.SetMapper(mapper); // 传递制图器

            //给图中添加颜色刻度表,刻度值和颜色表的还没对应
            vtkScalarBarActor scalarBar = vtkScalarBarActor.New();
            scalarBar.SetLookupTable(mapper.GetLookupTable());
            scalarBar.SetTitle("Point Cloud");
            scalarBar.SetHeight(0.7);
            scalarBar.SetWidth(0.1);
            scalarBar.SetNumberOfLabels(10);
            scalarBar.GetLabelTextProperty().SetFontSize(4);
            vtkRenderer out_render = vtkRenderer.New();

            out_render.AddActor(actor);
            //添加颜色刻度表
            out_render.AddActor(scalarBar);
            // 设置Viewport窗口
            out_render.SetViewport(0.0, 0.0, 1.0, 1.0);
            // 打开渐变色背景开关
            out_render.GradientBackgroundOn();
            out_render.SetBackground(0.2, 0.3, 0.3);
            out_render.SetBackground2(0.8, 0.8, 0.8);
            return out_render;
        }

        private void loadButton_Click(object sender, EventArgs e)
        {
            //生成圆形点云
            for (int i = currentTheta; i < 360; i = i + 4)
            {
                //生成一根线点云
                generatePointByAngel(i);
                //显示刚生成的点云
                polyData.SetPoints(Points);
               
                //不使用这个的话就得手动为每个点添加顶点id（拓扑结构），不然无法显示
                vtkVertexGlyphFilter glyphFilter = vtkVertexGlyphFilter.New();
                glyphFilter.SetInputConnection(polyData.GetProducerPort());               
                polyDataMapper.SetInputConnection(glyphFilter.GetOutputPort());
                actor.SetMapper(polyDataMapper);
                actor.GetProperty().SetColor(1, 0, 0);
                actor.GetProperty().SetPointSize(4);
                // 新建渲染器
                vtkRenderer render1 = vtkRenderer.New();
                vtkRenderWindow renWin = renderWindowControl1.RenderWindow;
                // 将“角色Actor”添加到“渲染器Renderer”并渲染
                renWin.AddRenderer(render1);
                render1.AddActor(actor);
                // 打开渐变色背景开关
                render1.GradientBackgroundOn();
                render1.SetBackground(0.2, 0.3, 0.3);
                render1.SetBackground2(0.8, 0.8, 0.8);

                //刷新panel，这样就不需要点击一下屏幕才会显示点云
                panel1.Refresh();
                //延时
                delay(50);

            }
        }
        //欧式聚类
        private void eucClusterButton_Click(object sender, EventArgs e)
        {
            //存输入的点云对象
            PointCloudXYZ cloud = new PointCloudXYZ();
            PointCloudXYZ cloud_res = new PointCloudXYZ();
            long size = Points.GetNumberOfPoints();
            for (int i = 0; i < size; i++)
            {
                cloud.Push(Points.GetPoint(i)[0], Points.GetPoint(i)[1], Points.GetPoint(i)[2]);
            }
            //初始化一个新的点云索引
            PointIndices pointIndice = new PointIndices();
            double dis = Convert.ToDouble(disThreshTextBox.Text);
            //分割之后的点云簇都保存在PointIndices
            Segmentation.euclideanCluster(cloud.PointCloudXYZPointer, dis, 100, 1000000, pointIndice.PointIndicesPointer);
            //找到点数最多的聚类
            int pos = 0;
            int max = 0;
            //找点数最多的那个
            for (int i = 0; i < pointIndice.Size; i++)
            {
                if (pointIndice.GetSizeOfIndice(i) > max)
                {
                    pos = i;
                    max = pointIndice.GetSizeOfIndice(i);
                }
                i = i + 1;
            }
            Util.copyPcBaseOnIndice(cloud.PointCloudXYZPointer, pointIndice.GetPointIndice(pos), cloud_res.PointCloudXYZPointer);

            vtkRenderer renderer2 = showPointCloud(cloud_res);
            vtkRenderWindow renWin2 = renderWindowControl2.RenderWindow;
            // 将“角色Actor”添加到“渲染器Renderer”并渲染
            renWin2.AddRenderer(renderer2);
            //刷新panel，这样就不需要点击一下屏幕才会显示点云
            panel2.Refresh();
        }
        //区域生长
        private void regionGrowButton_Click(object sender, EventArgs e)
        {
            //存输入的点云对象
            PointCloudXYZ cloud = new PointCloudXYZ();
            PointCloudXYZ cloud_res = new PointCloudXYZ();
            long size = Points.GetNumberOfPoints();
            for (int i = 0; i < size; i++)
            {
                cloud.Push(Points.GetPoint(i)[0], Points.GetPoint(i)[1], Points.GetPoint(i)[2]);
            }
            //初始化一个新的点云索引
            PointIndices pointIndice = new PointIndices();
            int neighbor = Convert.ToInt32(neighborTextBox.Text);
            double curve = Convert.ToDouble(curveTextBox.Text);
            double smooth = Convert.ToDouble(smoothTextBox.Text);
            
            Segmentation.oriGrowRegion(cloud.PointCloudXYZPointer, neighbor, (float)smooth, (float)curve, 100, 5000000, pointIndice.PointIndicesPointer);
            //找到点数最多的聚类
            int pos = 0;
            int max = 0;
            //找点数最多的那个
            for (int i = 0; i < pointIndice.Size; i++)
            {
                if (pointIndice.GetSizeOfIndice(i) > max)
                {
                    pos = i;
                    max = pointIndice.GetSizeOfIndice(i);
                }
                i = i + 1;
            }
            Util.copyPcBaseOnIndice(cloud.PointCloudXYZPointer, pointIndice.GetPointIndice(pos), cloud_res.PointCloudXYZPointer);

            vtkRenderer renderer2 = showPointCloud(cloud_res);
            vtkRenderWindow renWin2 = renderWindowControl2.RenderWindow;
            // 将“角色Actor”添加到“渲染器Renderer”并渲染
            renWin2.AddRenderer(renderer2);
            //刷新panel，这样就不需要点击一下屏幕才会显示点云
            panel2.Refresh();
        }
    }
}
