﻿using System;
using System.Windows.Forms;
using Kitware.VTK;
using PclCSharp;
using PointCloudSharp;

namespace SegmentationDemo
{
    public partial class SegmentationDemoForm : Form
    {
        //创建VTK控件renderWindowControl1
        private RenderWindowControl renderWindowControl1;
        private RenderWindowControl renderWindowControl2;

        private vtkRenderer renderer1;
        private vtkRenderer renderer2;


        //点云文件路径
        string url;
        //输入的点云对象
        PointCloudXYZ cloud = new PointCloudXYZ();
        //滤波后的点云对象
        PointCloudXYZ cloud_res = new PointCloudXYZ();
        public SegmentationDemoForm()
        {
            InitializeComponent();
            //将VTK控件renderWindowControl添加到父控件panel里
            vtkOutputWindow.SetGlobalWarningDisplay(0);
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

        // 渲染点云
        private vtkRenderer render(PointCloudXYZ cloud, RenderWindowControl renWinControl, Panel panel, vtkRenderer renderer)
        {
            vtkRenderWindow renWin = renWinControl.RenderWindow;

            // 需要清除renderer，否则画面会越来越卡
            renWin.RemoveRenderer(renderer);
            renderer = showPointCloud(cloud);
           
            // 将“角色Actor”添加到“渲染器Renderer”并渲染
            renWin.AddRenderer(renderer);
            //刷新panel，这样就不需要点击一下屏幕才会显示点云
            panel.Refresh();
            return renderer;
        }

        //加载pcd文件
        private void loadPcdButton_Click(object sender, EventArgs e)
        {
            //清空cloud中的点，如果不清空的话，在多次打开新的点云文件时，点数会叠加
            cloud.Clear();
            //打开点云文件
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "请选择点云文件";
            ofd.InitialDirectory = @"..\..\..\..\..\..\source";
            ofd.Filter = "点云文件|*.pcd";
            ofd.ShowDialog();
            url = ofd.FileName;
            //加载pcd文件，并将点云对象存储到cloud中的PointCloudPointer指针中
            Io.loadPcdFile(url, cloud.PointCloudXYZPointer);

            renderer1 = render(cloud, renderWindowControl1, panel1, renderer1);
        }

        // 原始点云
        private void OriginButton_Click(object sender, EventArgs e)
        {
            renderer2 = render(cloud, renderWindowControl2, panel2, renderer2);
        }

        // 平面分割
        private void PlaneSegButton_Click(object sender, EventArgs e)
        {
            Segmentation.planeModelSegment(cloud.PointCloudXYZPointer, 50, true, cloud_res.PointCloudXYZPointer);

            renderer2 = render(cloud_res, renderWindowControl2, panel2, renderer2);
        }

        // 圆柱分割
        private void CylinderSegButton_Click(object sender, EventArgs e)
        {
            Segmentation.cylinderModelSegment(cloud.PointCloudXYZPointer, 10, 550, 560, false, cloud_res.PointCloudXYZPointer);

            renderer2 = render(cloud_res, renderWindowControl2, panel2, renderer2);
        }

        // 欧式聚类
        private void EucButton_Click(object sender, EventArgs e)
        {
            PointIndices indices = new PointIndices();
            Segmentation.euclideanCluster(cloud.PointCloudXYZPointer, 200, 1000, 200000, indices.PointIndicesPointer);
            PointCloudXYZ cloud_res = new PointCloudXYZ();
            Util.copyPcBaseOnIndice(cloud.PointCloudXYZPointer, indices.GetPointIndice(0), cloud_res.PointCloudXYZPointer);

            renderer2 = render(cloud_res, renderWindowControl2, panel2, renderer2);
        }

        // 区域生长
        private void GrowRegionButton_Click(object sender, EventArgs e)
        {
            Segmentation.modifiedGrowRegion(cloud.PointCloudXYZPointer, 50, 1, 1, 100, 5000000, cloud_res.PointCloudXYZPointer);

            renderer2 = render(cloud_res, renderWindowControl2, panel2, renderer2);
        }
    }
}
