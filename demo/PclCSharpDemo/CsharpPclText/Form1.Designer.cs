namespace CsharpPclText
{
    partial class Form1
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
            this.button1 = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.button2 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.radiusbutton = new System.Windows.Forms.Button();
            this.euclideanbutton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(250, 436);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(91, 41);
            this.button1.TabIndex = 0;
            this.button1.Text = "打开点云";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // panel1
            // 
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(631, 396);
            this.panel1.TabIndex = 1;
            // 
            // panel2
            // 
            this.panel2.Location = new System.Drawing.Point(692, 12);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(694, 396);
            this.panel2.TabIndex = 2;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(726, 433);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(97, 44);
            this.button2.TabIndex = 5;
            this.button2.Text = "区域生长";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(880, 437);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(137, 40);
            this.button4.TabIndex = 7;
            this.button4.Text = "保存结果点云";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // radiusbutton
            // 
            this.radiusbutton.Location = new System.Drawing.Point(415, 435);
            this.radiusbutton.Name = "radiusbutton";
            this.radiusbutton.Size = new System.Drawing.Size(94, 42);
            this.radiusbutton.TabIndex = 8;
            this.radiusbutton.Text = "半径滤波";
            this.radiusbutton.UseVisualStyleBackColor = true;
            this.radiusbutton.Click += new System.EventHandler(this.radiusbutton_Click);
            // 
            // euclideanbutton
            // 
            this.euclideanbutton.Location = new System.Drawing.Point(568, 435);
            this.euclideanbutton.Name = "euclideanbutton";
            this.euclideanbutton.Size = new System.Drawing.Size(93, 40);
            this.euclideanbutton.TabIndex = 9;
            this.euclideanbutton.Text = "欧式分割";
            this.euclideanbutton.UseVisualStyleBackColor = true;
            this.euclideanbutton.Click += new System.EventHandler(this.euclideanbutton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1416, 489);
            this.Controls.Add(this.euclideanbutton);
            this.Controls.Add(this.radiusbutton);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button radiusbutton;
        private System.Windows.Forms.Button euclideanbutton;
    }
}

