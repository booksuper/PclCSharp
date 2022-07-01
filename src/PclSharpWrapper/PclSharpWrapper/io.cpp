#include "io.h"

void Pc2Array(pcl::PointCloud<pcl::PointXYZ>::Ptr cloud_in, double * out_x, double * out_y, double * out_z)
{
	for (size_t i = 0; i < cloud_in->points.size(); i++) {

		out_x[i] = cloud_in->points[i].x;
		out_y[i] = cloud_in->points[i].y;
		out_z[i] = cloud_in->points[i].z;

	}
}
//加载点云文件，函数弃用
HEAD int CallingConvention loadFile(char * path, double * out_x, double * out_y, double * out_z)
{
	pcl::PointCloud<pcl::PointXYZ>::Ptr cloud(new pcl::PointCloud<pcl::PointXYZ>());
	if (pcl::io::loadPLYFile(path, *cloud) == -1)
	{
		return 0;
	}
	else
	{
		Pc2Array(cloud, out_x, out_y, out_z);
		return 1;
	}
}
//加载ply文件
HEAD int CallingConvention loadPlyFile(char * path, pcl::PointCloud<pcl::PointXYZ>* pc)
{
	if (pcl::io::loadPLYFile(path, *pc) == -1)
	{
		return 0;
	}
	else
	{
		return 1;
	}
}
//加载pcd文件
HEAD int CallingConvention loadPcdFile(char* path, pcl::PointCloud<pcl::PointXYZ> * pc)
{
	if (pcl::io::loadPCDFile(path, *pc) == -1)
	{
		return 0;
	}
	else
	{
		return 1;
	}

}
//加载obj
HEAD int CallingConvention loadObjFile(char* path, pcl::PointCloud<pcl::PointXYZ> * pc)
{
	//不能直接使用pcl::io::loadOBJFile这个函数，应该先用loadPolygonFile吧文件加载进PolygonMesh
	//再用fromPCLPointCloud2进行转换。直接使用pcl::io::loadOBJFile会报错
	pcl::PolygonMesh mesh;

	if (pcl::io::loadPolygonFile(path, mesh) < 0)
	{
		cout << "load failed" << endl;
		return 0;
	}
	else
	{
		pcl::fromPCLPointCloud2(mesh.cloud, *pc);
		return 1;
	}

}
//加载txt文件
HEAD int CallingConvention loadTxtFile(char * path, pcl::PointCloud<pcl::PointXYZ>* pc)
{
	
	ifstream Points_in(path);
	pcl::PointXYZ tmpoint;
	if (Points_in.is_open())
	{
		while (!Points_in.eof())   //尚未到达文件结尾
		{
			//>>操作符会忽略空白符和换行符
			Points_in >> tmpoint.x >> tmpoint.y >> tmpoint.z;
			pc->points.push_back(tmpoint);
		}
		pc->width = (int)pc->points.size();
		pc->height = 1;
		return 1;
	}
	else
	{
		return 0;
	}
	
}

//保存pcd文件
HEAD void CallingConvention savePcdFile(char* path, pcl::PointCloud<pcl::PointXYZ> * pc, int binaryMode)
{
	if (binaryMode >= 1)
	{
		pcl::io::savePCDFile(path, *pc, true);
	}
	else
	{
		pcl::io::savePCDFile(path, *pc, false);
	}

}
//保存ply文件
HEAD void CallingConvention savePlyFile(char* path, pcl::PointCloud<pcl::PointXYZ> * pc, int binaryMode)
{
	if (binaryMode >= 1)
	{
		pcl::io::savePLYFile(path, *pc, true);
	}
	else
	{
		pcl::io::savePLYFile(path, *pc, false);
	}

}
//保存obj文件

HEAD void CallingConvention saveObjFile(char* path, pcl::PointCloud<pcl::PointXYZ> * pc)
{
	//TODO 添加将点云对象保存为obj格式的代码

}

//stl转为点云对象
HEAD void CallingConvention stl2PointCloud(char * path, pcl::PointCloud<pcl::PointXYZ>* pc)
{
	vtkSmartPointer<vtkSTLReader> stlreader = vtkSmartPointer<vtkSTLReader>::New();
	stlreader->SetFileName(path);
	stlreader->Update();
	vtkSmartPointer<vtkPolyData> poly = vtkSmartPointer<vtkPolyData>::New();
	poly = stlreader->GetOutput();
	pcl::io::vtkPolyDataToPointCloud(poly, *pc);
}