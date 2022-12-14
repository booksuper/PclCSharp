namespace SegDemo
{
    partial class SegDemo
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
            this.loadButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.disThreshTextBox = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.neighborTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.curveTextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.smoothTextBox = new System.Windows.Forms.TextBox();
            this.eucClusterButton = new System.Windows.Forms.Button();
            this.regionGrowButton = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Location = new System.Drawing.Point(12, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(553, 482);
            this.panel1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Location = new System.Drawing.Point(575, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(549, 482);
            this.panel2.TabIndex = 0;
            // 
            // loadButton
            // 
            this.loadButton.Location = new System.Drawing.Point(192, 539);
            this.loadButton.Name = "loadButton";
            this.loadButton.Size = new System.Drawing.Size(90, 48);
            this.loadButton.TabIndex = 0;
            this.loadButton.Text = "生成点云";
            this.loadButton.UseVisualStyleBackColor = true;
            this.loadButton.Click += new System.EventHandler(this.loadButton_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.eucClusterButton);
            this.groupBox1.Controls.Add(this.disThreshTextBox);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(434, 488);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(194, 144);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "groupBox1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 95);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "距离阈值：";
            // 
            // disThreshTextBox
            // 
            this.disThreshTextBox.Location = new System.Drawing.Point(89, 92);
            this.disThreshTextBox.Name = "disThreshTextBox";
            this.disThreshTextBox.Size = new System.Drawing.Size(77, 25);
            this.disThreshTextBox.TabIndex = 2;
            this.disThreshTextBox.Text = "5";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.regionGrowButton);
            this.groupBox2.Controls.Add(this.smoothTextBox);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.curveTextBox);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.neighborTextBox);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Location = new System.Drawing.Point(634, 488);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(326, 144);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "groupBox1";
            // 
            // neighborTextBox
            // 
            this.neighborTextBox.Location = new System.Drawing.Point(79, 92);
            this.neighborTextBox.Name = "neighborTextBox";
            this.neighborTextBox.Size = new System.Drawing.Size(77, 25);
            this.neighborTextBox.TabIndex = 2;
            this.neighborTextBox.Text = "67";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 95);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 15);
            this.label2.TabIndex = 1;
            this.label2.Text = "近邻数：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(166, 95);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(82, 15);
            this.label3.TabIndex = 1;
            this.label3.Text = "曲率阈值：";
            // 
            // curveTextBox
            // 
            this.curveTextBox.Location = new System.Drawing.Point(239, 92);
            this.curveTextBox.Name = "curveTextBox";
            this.curveTextBox.Size = new System.Drawing.Size(77, 25);
            this.curveTextBox.TabIndex = 2;
            this.curveTextBox.Text = "3";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(131, 41);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(97, 15);
            this.label4.TabIndex = 1;
            this.label4.Text = "平滑度阈值：";
            // 
            // smoothTextBox
            // 
            this.smoothTextBox.Location = new System.Drawing.Point(219, 37);
            this.smoothTextBox.Name = "smoothTextBox";
            this.smoothTextBox.Size = new System.Drawing.Size(77, 25);
            this.smoothTextBox.TabIndex = 2;
            this.smoothTextBox.Text = "4";
            // 
            // eucClusterButton
            // 
            this.eucClusterButton.Location = new System.Drawing.Point(19, 27);
            this.eucClusterButton.Name = "eucClusterButton";
            this.eucClusterButton.Size = new System.Drawing.Size(79, 45);
            this.eucClusterButton.TabIndex = 3;
            this.eucClusterButton.Text = "欧式聚类";
            this.eucClusterButton.UseVisualStyleBackColor = true;
            this.eucClusterButton.Click += new System.EventHandler(this.eucClusterButton_Click);
            // 
            // regionGrowButton
            // 
            this.regionGrowButton.Location = new System.Drawing.Point(8, 27);
            this.regionGrowButton.Name = "regionGrowButton";
            this.regionGrowButton.Size = new System.Drawing.Size(89, 45);
            this.regionGrowButton.TabIndex = 3;
            this.regionGrowButton.Text = "区域生长";
            this.regionGrowButton.UseVisualStyleBackColor = true;
            this.regionGrowButton.Click += new System.EventHandler(this.regionGrowButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1127, 644);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.loadButton);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button loadButton;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox disThreshTextBox;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox neighborTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox smoothTextBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox curveTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button eucClusterButton;
        private System.Windows.Forms.Button regionGrowButton;
    }
}

