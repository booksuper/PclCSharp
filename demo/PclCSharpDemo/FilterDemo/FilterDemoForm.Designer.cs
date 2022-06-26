namespace FilterDemo
{
    partial class FilterDemoForm
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
            this.Voxelbutton = new System.Windows.Forms.Button();
            this.appVoxelbutton = new System.Windows.Forms.Button();
            this.RadiusButton = new System.Windows.Forms.Button();
            this.StaButton = new System.Windows.Forms.Button();
            this.PassButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(666, 444);
            this.panel1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Location = new System.Drawing.Point(696, 12);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(661, 444);
            this.panel2.TabIndex = 1;
            // 
            // loadPcdButton
            // 
            this.loadPcdButton.Location = new System.Drawing.Point(49, 476);
            this.loadPcdButton.Name = "loadPcdButton";
            this.loadPcdButton.Size = new System.Drawing.Size(103, 50);
            this.loadPcdButton.TabIndex = 2;
            this.loadPcdButton.Text = "加载pcd点云";
            this.loadPcdButton.UseVisualStyleBackColor = true;
            this.loadPcdButton.Click += new System.EventHandler(this.loadPcdButton_Click);
            // 
            // Voxelbutton
            // 
            this.Voxelbutton.Location = new System.Drawing.Point(181, 476);
            this.Voxelbutton.Name = "Voxelbutton";
            this.Voxelbutton.Size = new System.Drawing.Size(101, 50);
            this.Voxelbutton.TabIndex = 3;
            this.Voxelbutton.Text = "体素下采样";
            this.Voxelbutton.UseVisualStyleBackColor = true;
            this.Voxelbutton.Click += new System.EventHandler(this.Voxelbutton_Click);
            // 
            // appVoxelbutton
            // 
            this.appVoxelbutton.Location = new System.Drawing.Point(320, 476);
            this.appVoxelbutton.Name = "appVoxelbutton";
            this.appVoxelbutton.Size = new System.Drawing.Size(127, 50);
            this.appVoxelbutton.TabIndex = 4;
            this.appVoxelbutton.Text = "近似体素下采样";
            this.appVoxelbutton.UseVisualStyleBackColor = true;
            this.appVoxelbutton.Click += new System.EventHandler(this.appVoxelbutton_Click);
            // 
            // RadiusButton
            // 
            this.RadiusButton.Location = new System.Drawing.Point(489, 476);
            this.RadiusButton.Name = "RadiusButton";
            this.RadiusButton.Size = new System.Drawing.Size(114, 50);
            this.RadiusButton.TabIndex = 5;
            this.RadiusButton.Text = "半径滤波";
            this.RadiusButton.UseVisualStyleBackColor = true;
            this.RadiusButton.Click += new System.EventHandler(this.RadiusButton_Click);
            // 
            // StaButton
            // 
            this.StaButton.Location = new System.Drawing.Point(656, 476);
            this.StaButton.Name = "StaButton";
            this.StaButton.Size = new System.Drawing.Size(91, 50);
            this.StaButton.TabIndex = 6;
            this.StaButton.Text = "统计滤波";
            this.StaButton.UseVisualStyleBackColor = true;
            this.StaButton.Click += new System.EventHandler(this.StaButton_Click);
            // 
            // PassButton
            // 
            this.PassButton.Location = new System.Drawing.Point(793, 476);
            this.PassButton.Name = "PassButton";
            this.PassButton.Size = new System.Drawing.Size(99, 50);
            this.PassButton.TabIndex = 7;
            this.PassButton.Text = "直通滤波";
            this.PassButton.UseVisualStyleBackColor = true;
            this.PassButton.Click += new System.EventHandler(this.PassButton_Click);
            // 
            // FilterDemoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1385, 538);
            this.Controls.Add(this.PassButton);
            this.Controls.Add(this.StaButton);
            this.Controls.Add(this.RadiusButton);
            this.Controls.Add(this.appVoxelbutton);
            this.Controls.Add(this.Voxelbutton);
            this.Controls.Add(this.loadPcdButton);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "FilterDemoForm";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button loadPcdButton;
        private System.Windows.Forms.Button Voxelbutton;
        private System.Windows.Forms.Button appVoxelbutton;
        private System.Windows.Forms.Button RadiusButton;
        private System.Windows.Forms.Button StaButton;
        private System.Windows.Forms.Button PassButton;
    }
}

