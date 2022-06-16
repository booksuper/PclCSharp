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

namespace CsharpPclText
{

    public partial class Form1 : Form
    {
        //创建VTK控件renderWindowControl1
        private Kitware.VTK.RenderWindowControl renderWindowControl1;
        //创建VTK控件renderWindowControl2
        private Kitware.VTK.RenderWindowControl renderWindowControl2;
        //点云文件路径
        string url;
        //输入的点云对象
        PointCloudXYZ cloud = new PointCloudXYZ();
        //存处理后的点云
        PointCloudXYZ cloud_res = new PointCloudXYZ();
        public Form1()
        {
            InitializeComponent();
            //将VTK控件renderWindowControl添加到父控件panel里
            //必须使用panel控件充当容器装载vtk控件，直接在界面中拖曳vtk控件会报错
            renderWindowControl1 = new Kitware.VTK.RenderWindowControl();
            //将renderWindowControl1边框填充到其父组件里
            renderWindowControl1.Dock = DockStyle.Fill;
            //将指定的renderWindowControl1控件添加到panel1控件集合中
            panel1.Controls.Add(renderWindowControl1);

            //将VTK控件renderWindowContro2添加到父控件panel里
            renderWindowControl2 = new Kitware.VTK.RenderWindowControl();
            renderWindowControl2.Dock = DockStyle.Fill;
            panel2.Controls.Add(renderWindowControl2);
        }

        /*按照指定的轴设定点云颜色属性
         极小值处设置为蓝色（0，0，255），并且蓝色通道的数值由255向中间位置按坐标变化递减至0，
         绿色通道的数值由0按坐标变化递增至255；中间位置设置为绿色（0，255，0），
         并且绿色通道的数值由255按坐标变化递减至0，红色通道数值由0按坐标变化递增至255；
         极大值位置设置为（255，0，0）*/
        vtkUnsignedCharArray setColorBaseAxis(char axis,PointCloudXYZ in_pc)
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
                        r =  0;
                        g = (255 * ((in_pc.GetX(i) - minmax[0]) / x_median));
                        b = (255 * (1 - ((in_pc.GetX(i) - minmax[0]) / x_median))); ;
                        colors_rgb.InsertNextTuple3(r, g, b);
                    }
                }
            }
            else if(axis == 'y')
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
                        r =  0;
                        g = (255 * ((in_pc.GetY(i) - minmax[2]) / y_median));
                        b = (255 * (1 - ((in_pc.GetY(i) - minmax[2]) / y_median))); ;
                        colors_rgb.InsertNextTuple3(r, g, b);
                    }
                }
            }
            else if(axis == 'z')
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

  

        //打开点云文件
        private void button1_Click(object sender, EventArgs e)
        {
            //清空cloud中的点，如果不清空的话，在多次打开新的点云文件时，点数会叠加
            cloud.Clear();
            //打开点云文件
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "请选择点云文件";
            ofd.InitialDirectory = @"C:\Users\17116\Desktop";
            ofd.Filter = "点云文件|*.pcd";
            ofd.ShowDialog();
            url = ofd.FileName;
            //加载ply文件，并将点云对象存储到cloud中的PointCloudPointer指针中
            PclCSharp.Io.loadPcdFile(url, cloud.PointCloudXYZPointer);
      
            //显示点云
            vtkPoints points = vtkPoints.New();
            //把点云指针中的点依次放进points
            for (int i = 0; i < cloud.Size; i++)
            {
                points.InsertNextPoint(cloud.GetX(i), cloud.GetY(i), cloud.GetZ(i));
                
            }
            //创建每个点的属性数据，这里代表颜色
            vtkUnsignedCharArray colors_rgb = setColorBaseAxis('z', cloud);
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
            
            // 新建渲染器
            vtkRenderer render1 = vtkRenderer.New();
            vtkRenderWindow renWin = renderWindowControl1.RenderWindow;
            // 将“角色Actor”添加到“渲染器Renderer”并渲染
            renWin.AddRenderer(render1);
            render1.AddActor(actor);
            //添加颜色刻度表
            render1.AddActor(scalarBar);
            // 设置Viewport窗口
            render1.SetViewport(0.0, 0.0, 1.0, 1.0);
            // 打开渐变色背景开关
            render1.GradientBackgroundOn();
            render1.SetBackground(0.2, 0.3, 0.3);
            render1.SetBackground2(0.8, 0.8, 0.8);
            //刷新panel，这样就不需要点击一下屏幕才会显示点云
            panel1.Refresh();
        }



        //区域生长
        private void button2_Click(object sender, EventArgs e)
        {
            //清空cloud_res中的点，如果不清空的话，在多次打开新的点云文件时，点数会叠加
            cloud_res.Clear();
            //下采样
            //PclCSharp.Filter.uniformDownSample(cloud.PointCloudXYZPointer, 100, cloud.PointCloudXYZPointer);

            //使用原生的区域生长
            //初始化一个新的点云索引
            PointIndices pointIndice = new PointIndices();
            //不同的文件需要设置不同的参数
            Segmentation.oriGrowRegion(cloud.PointCloudXYZPointer, 50, 1, 1, 10, 5000000, pointIndice.PointIndicesPointer);
            //找到点数最多的聚类
            int pos = 0;
            int max = 0;
            //有十个聚类，第一个最大
            for (int i = 0; i < pointIndice.Size; i++)
            {
                if (pointIndice.GetSizeOfIndice(i) > max)
                {
                    pos = i;
                    max = pointIndice.GetSizeOfIndice(i);
                }
                i = i + 1;
            }
            //将点数最多的点簇转为点云
            Util.copyPcBaseOnIndice(cloud.PointCloudXYZPointer, pointIndice.GetPointIndice(pos), cloud_res.PointCloudXYZPointer);
            //显示点云
            vtkPoints points = vtkPoints.New();
            for (int i = 0; i < cloud_res.Size; i++)
            {
                points.InsertNextPoint(cloud_res.GetX(i), cloud_res.GetY(i), cloud_res.GetZ(i));
            }
            //创建每个点的属性数据，这里代表颜色
            vtkUnsignedCharArray colors_rgb = setColorBaseAxis('z', cloud_res);
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

            //给图中添加颜色刻度表
            vtkScalarBarActor scalarBar = vtkScalarBarActor.New();
            scalarBar.SetLookupTable(mapper.GetLookupTable());
            scalarBar.SetTitle("Point Cloud");
            scalarBar.SetHeight(0.7);
            scalarBar.SetWidth(0.1);
            scalarBar.SetNumberOfLabels(10);
            scalarBar.GetLabelTextProperty().SetFontSize(4);

            // 新建渲染器
            vtkRenderer render2 = vtkRenderer.New();
            vtkRenderWindow renWin2 = renderWindowControl2.RenderWindow;
            // 将“角色Actor”添加到“渲染器Renderer”并渲染
            renWin2.AddRenderer(render2);
            render2.AddActor(actor);
            render2.AddActor(scalarBar);
            // 设置Viewport窗口
            render2.SetViewport(0.0, 0.0, 1.0, 1.0);
            // 设置背景色
            // 打开渐变色背景开关
            render2.GradientBackgroundOn();
            render2.SetBackground(0.2, 0.3, 0.3);
            render2.SetBackground2(0.8, 0.8, 0.8);
            panel2.Refresh();
        }

        //保存结果点云
        private void button4_Click(object sender, EventArgs e)
        {
            PclCSharp.Io.savePcdFile("result.pcd", cloud_res.PointCloudXYZPointer, 0);
        }
        //半径滤波
        private void radiusbutton_Click(object sender, EventArgs e)
        {
            //不同的点云文件需要调整半径滤波参数
            Filter.radiusFilter(cloud.PointCloudXYZPointer, 0.2, 25, cloud_res.PointCloudXYZPointer);

            //显示点云
            vtkPoints points = vtkPoints.New();
            for (int i = 0; i < cloud_res.Size; i++)
            {
                points.InsertNextPoint(cloud_res.GetX(i), cloud_res.GetY(i), cloud_res.GetZ(i));
            }
            //创建每个点的属性数据，这里代表颜色
            vtkUnsignedCharArray colors_rgb = setColorBaseAxis('z', cloud_res);
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

            //给图中添加颜色刻度表
            vtkScalarBarActor scalarBar = vtkScalarBarActor.New();
            scalarBar.SetLookupTable(mapper.GetLookupTable());
            scalarBar.SetTitle("Point Cloud");
            scalarBar.SetHeight(0.7);
            scalarBar.SetWidth(0.1);
            scalarBar.SetNumberOfLabels(10);
            scalarBar.GetLabelTextProperty().SetFontSize(4);

            // 新建渲染器
            vtkRenderer render2 = vtkRenderer.New();
            vtkRenderWindow renWin2 = renderWindowControl2.RenderWindow;
            // 将“角色Actor”添加到“渲染器Renderer”并渲染
            renWin2.AddRenderer(render2);
            render2.AddActor(actor);
            render2.AddActor(scalarBar);
            // 设置Viewport窗口
            render2.SetViewport(0.0, 0.0, 1.0, 1.0);
            // 设置背景色
            // 打开渐变色背景开关
            render2.GradientBackgroundOn();
            render2.SetBackground(0.2, 0.3, 0.3);
            render2.SetBackground2(0.8, 0.8, 0.8);
            panel2.Refresh();

        }
        //时间复杂度比较高
        private void euclideanbutton_Click(object sender, EventArgs e)
        {
            PointIndices euclidean_pointIndice = new PointIndices();
            //不同的文件需要调整参数
            Segmentation.euclideanCluster(cloud.PointCloudXYZPointer, 0.3, 10, 3000000, euclidean_pointIndice.PointIndicesPointer);
            //第0个点簇
            Util.copyPcBaseOnIndice(cloud.PointCloudXYZPointer, euclidean_pointIndice.GetPointIndice(0), cloud_res.PointCloudXYZPointer);
            //显示点云
            vtkPoints points = vtkPoints.New();
            for (int i = 0; i < cloud_res.Size; i++)
            {
                points.InsertNextPoint(cloud_res.GetX(i), cloud_res.GetY(i), cloud_res.GetZ(i));
            }
            vtkPolyData polydata = vtkPolyData.New();
            polydata.SetPoints(points);

            vtkVertexGlyphFilter glyphFilter = vtkVertexGlyphFilter.New();
            glyphFilter.SetInputConnection(polydata.GetProducerPort());
            // 新建制图器
            vtkPolyDataMapper mapper = vtkPolyDataMapper.New();
            mapper.SetInputConnection(glyphFilter.GetOutputPort());// 连接管道
            vtkActor actor = vtkActor.New(); // 新建角色
            actor.SetMapper(mapper); // 传递制图器
            actor.GetProperty().SetColor(255, 255, 255); // 设置“角色”颜色
            vtkRenderer ren2 = renderWindowControl2.RenderWindow.GetRenderers().GetFirstRenderer();
            // 新建渲染器
            vtkRenderer render2 = vtkRenderer.New();
            vtkRenderWindow renWin2 = renderWindowControl2.RenderWindow;
            // 将“角色Actor”添加到“渲染器Renderer”并渲染
            renWin2.AddRenderer(render2);
            render2.AddActor(actor);
            // 设置Viewport窗口
            render2.SetViewport(0.0, 0.0, 1.0, 1.0);
            // 设置背景色
            render2.SetBackground(0.0, 0.0, 0.0);
            panel2.Refresh();
        }
    }
}
