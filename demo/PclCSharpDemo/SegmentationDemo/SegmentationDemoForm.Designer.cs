namespace SegmentationDemo
{
    partial class SegmentationDemoForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.loadPcdButton = new System.Windows.Forms.Button();
            this.PlaneSegButton = new System.Windows.Forms.Button();
            this.CylinderSegButton = new System.Windows.Forms.Button();
            this.EucButton = new System.Windows.Forms.Button();
            this.GrowRegionButton = new System.Windows.Forms.Button();
            this.OriginButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Location = new System.Drawing.Point(9, 10);
            this.panel1.Margin = new System.Windows.Forms.Padding(2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(500, 355);
            this.panel1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Location = new System.Drawing.Point(522, 10);
            this.panel2.Margin = new System.Windows.Forms.Padding(2);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(496, 355);
            this.panel2.TabIndex = 1;
            // 
            // loadPcdButton
            // 
            this.loadPcdButton.Location = new System.Drawing.Point(37, 381);
            this.loadPcdButton.Margin = new System.Windows.Forms.Padding(2);
            this.loadPcdButton.Name = "loadPcdButton";
            this.loadPcdButton.Size = new System.Drawing.Size(82, 40);
            this.loadPcdButton.TabIndex = 2;
            this.loadPcdButton.Text = "加载pcd点云";
            this.loadPcdButton.UseVisualStyleBackColor = true;
            this.loadPcdButton.Click += new System.EventHandler(this.loadPcdButton_Click);
            // 
            // PlaneSegButton
            // 
            this.PlaneSegButton.Location = new System.Drawing.Point(267, 381);
            this.PlaneSegButton.Margin = new System.Windows.Forms.Padding(2);
            this.PlaneSegButton.Name = "PlaneSegButton";
            this.PlaneSegButton.Size = new System.Drawing.Size(76, 40);
            this.PlaneSegButton.TabIndex = 3;
            this.PlaneSegButton.Text = "平面分割";
            this.PlaneSegButton.UseVisualStyleBackColor = true;
            this.PlaneSegButton.Click += new System.EventHandler(this.PlaneSegButton_Click);
            // 
            // CylinderSegButton
            // 
            this.CylinderSegButton.Location = new System.Drawing.Point(390, 381);
            this.CylinderSegButton.Margin = new System.Windows.Forms.Padding(2);
            this.CylinderSegButton.Name = "CylinderSegButton";
            this.CylinderSegButton.Size = new System.Drawing.Size(95, 40);
            this.CylinderSegButton.TabIndex = 4;
            this.CylinderSegButton.Text = "圆柱分割";
            this.CylinderSegButton.UseVisualStyleBackColor = true;
            this.CylinderSegButton.Click += new System.EventHandler(this.CylinderSegButton_Click);
            // 
            // EucButton
            // 
            this.EucButton.Location = new System.Drawing.Point(522, 381);
            this.EucButton.Margin = new System.Windows.Forms.Padding(2);
            this.EucButton.Name = "EucButton";
            this.EucButton.Size = new System.Drawing.Size(86, 40);
            this.EucButton.TabIndex = 5;
            this.EucButton.Text = "欧式聚类";
            this.EucButton.UseVisualStyleBackColor = true;
            this.EucButton.Click += new System.EventHandler(this.EucButton_Click);
            // 
            // GrowRegionButton
            // 
            this.GrowRegionButton.Location = new System.Drawing.Point(656, 381);
            this.GrowRegionButton.Margin = new System.Windows.Forms.Padding(2);
            this.GrowRegionButton.Name = "GrowRegionButton";
            this.GrowRegionButton.Size = new System.Drawing.Size(86, 40);
            this.GrowRegionButton.TabIndex = 6;
            this.GrowRegionButton.Text = "区域生长";
            this.GrowRegionButton.UseVisualStyleBackColor = true;
            this.GrowRegionButton.Click += new System.EventHandler(this.GrowRegionButton_Click);
            // 
            // OriginButton
            // 
            this.OriginButton.Location = new System.Drawing.Point(156, 381);
            this.OriginButton.Margin = new System.Windows.Forms.Padding(2);
            this.OriginButton.Name = "OriginButton";
            this.OriginButton.Size = new System.Drawing.Size(76, 40);
            this.OriginButton.TabIndex = 7;
            this.OriginButton.Text = "原始点云";
            this.OriginButton.UseVisualStyleBackColor = true;
            this.OriginButton.Click += new System.EventHandler(this.OriginButton_Click);
            // 
            // SegmentationDemoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1039, 430);
            this.Controls.Add(this.OriginButton);
            this.Controls.Add(this.GrowRegionButton);
            this.Controls.Add(this.EucButton);
            this.Controls.Add(this.CylinderSegButton);
            this.Controls.Add(this.PlaneSegButton);
            this.Controls.Add(this.loadPcdButton);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "SegmentationDemoForm";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button loadPcdButton;
        private System.Windows.Forms.Button PlaneSegButton;
        private System.Windows.Forms.Button CylinderSegButton;
        private System.Windows.Forms.Button EucButton;
        private System.Windows.Forms.Button GrowRegionButton;
        private System.Windows.Forms.Button OriginButton;
    }
}

