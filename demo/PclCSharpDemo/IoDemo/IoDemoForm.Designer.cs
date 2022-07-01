namespace IoDemo
{
    partial class IoDemoForm
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
            this.loadPlyButton = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.loadPcdButton = new System.Windows.Forms.Button();
            this.loadObjButton = new System.Windows.Forms.Button();
            this.loadStlButton = new System.Windows.Forms.Button();
            this.savePcdButton = new System.Windows.Forms.Button();
            this.savePlyButton = new System.Windows.Forms.Button();
            this.loadTxtButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // loadPlyButton
            // 
            this.loadPlyButton.Location = new System.Drawing.Point(36, 505);
            this.loadPlyButton.Name = "loadPlyButton";
            this.loadPlyButton.Size = new System.Drawing.Size(107, 45);
            this.loadPlyButton.TabIndex = 0;
            this.loadPlyButton.Text = "加载ply文件";
            this.loadPlyButton.UseVisualStyleBackColor = true;
            this.loadPlyButton.Click += new System.EventHandler(this.loadPlyButton_Click);
            // 
            // panel1
            // 
            this.panel1.Location = new System.Drawing.Point(36, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(958, 467);
            this.panel1.TabIndex = 1;
            // 
            // loadPcdButton
            // 
            this.loadPcdButton.Location = new System.Drawing.Point(186, 505);
            this.loadPcdButton.Name = "loadPcdButton";
            this.loadPcdButton.Size = new System.Drawing.Size(107, 45);
            this.loadPcdButton.TabIndex = 0;
            this.loadPcdButton.Text = "加载pcd文件";
            this.loadPcdButton.UseVisualStyleBackColor = true;
            this.loadPcdButton.Click += new System.EventHandler(this.loadPcdButton_Click);
            // 
            // loadObjButton
            // 
            this.loadObjButton.Location = new System.Drawing.Point(348, 505);
            this.loadObjButton.Name = "loadObjButton";
            this.loadObjButton.Size = new System.Drawing.Size(107, 45);
            this.loadObjButton.TabIndex = 0;
            this.loadObjButton.Text = "加载obj文件";
            this.loadObjButton.UseVisualStyleBackColor = true;
            this.loadObjButton.Click += new System.EventHandler(this.loadObjButton_Click);
            // 
            // loadStlButton
            // 
            this.loadStlButton.Location = new System.Drawing.Point(492, 505);
            this.loadStlButton.Name = "loadStlButton";
            this.loadStlButton.Size = new System.Drawing.Size(108, 45);
            this.loadStlButton.TabIndex = 2;
            this.loadStlButton.Text = "加载stl文件";
            this.loadStlButton.UseVisualStyleBackColor = true;
            this.loadStlButton.Click += new System.EventHandler(this.loadStlButton_Click);
            // 
            // savePcdButton
            // 
            this.savePcdButton.Location = new System.Drawing.Point(752, 505);
            this.savePcdButton.Name = "savePcdButton";
            this.savePcdButton.Size = new System.Drawing.Size(100, 45);
            this.savePcdButton.TabIndex = 3;
            this.savePcdButton.Text = "存为pcd文件";
            this.savePcdButton.UseVisualStyleBackColor = true;
            this.savePcdButton.Click += new System.EventHandler(this.savePcdButton_Click);
            // 
            // savePlyButton
            // 
            this.savePlyButton.Location = new System.Drawing.Point(883, 505);
            this.savePlyButton.Name = "savePlyButton";
            this.savePlyButton.Size = new System.Drawing.Size(111, 45);
            this.savePlyButton.TabIndex = 4;
            this.savePlyButton.Text = "存为ply文件";
            this.savePlyButton.UseVisualStyleBackColor = true;
            this.savePlyButton.Click += new System.EventHandler(this.savePlyButton_Click);
            // 
            // loadTxtButton
            // 
            this.loadTxtButton.Location = new System.Drawing.Point(631, 505);
            this.loadTxtButton.Name = "loadTxtButton";
            this.loadTxtButton.Size = new System.Drawing.Size(100, 45);
            this.loadTxtButton.TabIndex = 5;
            this.loadTxtButton.Text = "加载txt文件";
            this.loadTxtButton.UseVisualStyleBackColor = true;
            this.loadTxtButton.Click += new System.EventHandler(this.loadTxtButton_Click);
            // 
            // IoDemoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1020, 562);
            this.Controls.Add(this.loadTxtButton);
            this.Controls.Add(this.savePlyButton);
            this.Controls.Add(this.savePcdButton);
            this.Controls.Add(this.loadStlButton);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.loadObjButton);
            this.Controls.Add(this.loadPcdButton);
            this.Controls.Add(this.loadPlyButton);
            this.Name = "IoDemoForm";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button loadPlyButton;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button loadPcdButton;
        private System.Windows.Forms.Button loadObjButton;
        private System.Windows.Forms.Button loadStlButton;
        private System.Windows.Forms.Button savePcdButton;
        private System.Windows.Forms.Button savePlyButton;
        private System.Windows.Forms.Button loadTxtButton;
    }
}

