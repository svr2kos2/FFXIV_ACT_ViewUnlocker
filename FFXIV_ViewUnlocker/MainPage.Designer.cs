namespace FFXIV_ACT_ViewUnlocker
{
    partial class MainPage
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

        #region 组件设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.ZoomInputField = new System.Windows.Forms.TextBox();
            this.Zoom_Lable = new System.Windows.Forms.Label();
            this.FOV_Lable = new System.Windows.Forms.Label();
            this.FovInputfield = new System.Windows.Forms.TextBox();
            this.SetToDefault = new System.Windows.Forms.Button();
            this.StatusLable = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // ZoomInputField
            // 
            this.ZoomInputField.Location = new System.Drawing.Point(38, 8);
            this.ZoomInputField.Name = "ZoomInputField";
            this.ZoomInputField.Size = new System.Drawing.Size(48, 21);
            this.ZoomInputField.TabIndex = 0;
            this.ZoomInputField.Text = "20";
            this.ZoomInputField.TextChanged += new System.EventHandler(this.zoom_TextChanged);
            // 
            // Zoom_Lable
            // 
            this.Zoom_Lable.AutoSize = true;
            this.Zoom_Lable.Location = new System.Drawing.Point(3, 9);
            this.Zoom_Lable.Name = "Zoom_Lable";
            this.Zoom_Lable.Size = new System.Drawing.Size(29, 12);
            this.Zoom_Lable.TabIndex = 1;
            this.Zoom_Lable.Text = "缩放";
            // 
            // FOV_Lable
            // 
            this.FOV_Lable.AutoSize = true;
            this.FOV_Lable.Location = new System.Drawing.Point(92, 11);
            this.FOV_Lable.Name = "FOV_Lable";
            this.FOV_Lable.Size = new System.Drawing.Size(23, 12);
            this.FOV_Lable.TabIndex = 2;
            this.FOV_Lable.Text = "FOV";
            // 
            // FovInputfield
            // 
            this.FovInputfield.Location = new System.Drawing.Point(127, 8);
            this.FovInputfield.Name = "FovInputfield";
            this.FovInputfield.Size = new System.Drawing.Size(48, 21);
            this.FovInputfield.TabIndex = 3;
            this.FovInputfield.Text = "0.78";
            this.FovInputfield.TextChanged += new System.EventHandler(this.fov_TextChanged);
            // 
            // SetToDefault
            // 
            this.SetToDefault.Location = new System.Drawing.Point(181, 6);
            this.SetToDefault.Name = "SetToDefault";
            this.SetToDefault.Size = new System.Drawing.Size(75, 23);
            this.SetToDefault.TabIndex = 4;
            this.SetToDefault.Text = "恢复默认";
            this.SetToDefault.UseVisualStyleBackColor = true;
            this.SetToDefault.Click += new System.EventHandler(this.SetToDefault_Click);
            // 
            // StatusLable
            // 
            this.StatusLable.AutoSize = true;
            this.StatusLable.BackColor = System.Drawing.SystemColors.Control;
            this.StatusLable.Location = new System.Drawing.Point(3, 32);
            this.StatusLable.Name = "StatusLable";
            this.StatusLable.Size = new System.Drawing.Size(77, 12);
            this.StatusLable.TabIndex = 11;
            this.StatusLable.Text = "等待游戏启动";
            // 
            // MainPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.StatusLable);
            this.Controls.Add(this.SetToDefault);
            this.Controls.Add(this.FovInputfield);
            this.Controls.Add(this.FOV_Lable);
            this.Controls.Add(this.Zoom_Lable);
            this.Controls.Add(this.ZoomInputField);
            this.Name = "MainPage";
            this.Size = new System.Drawing.Size(264, 55);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox ZoomInputField;
        private System.Windows.Forms.Label Zoom_Lable;
        private System.Windows.Forms.Label FOV_Lable;
        private System.Windows.Forms.TextBox FovInputfield;
        private System.Windows.Forms.Button SetToDefault;
		private System.Windows.Forms.Label StatusLable;
	}
}
