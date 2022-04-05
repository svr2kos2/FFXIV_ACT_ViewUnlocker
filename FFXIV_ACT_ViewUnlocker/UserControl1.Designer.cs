namespace FFXIV_ACT_ViewUnlocker
{
    partial class ViewUnlocker 
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
            this.zoom = new System.Windows.Forms.TextBox();
            this.Zoom_Lable = new System.Windows.Forms.Label();
            this.FOV_Lable = new System.Windows.Forms.Label();
            this.fov = new System.Windows.Forms.TextBox();
            this.SetToDefault = new System.Windows.Forms.Button();
            this.offset = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.PullOffsetFromeNetwork = new System.Windows.Forms.Button();
            this.ScanOffsetLocally = new System.Windows.Forms.Button();
            this.RequestInfo = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // zoom
            // 
            this.zoom.Location = new System.Drawing.Point(74, 25);
            this.zoom.Name = "zoom";
            this.zoom.Size = new System.Drawing.Size(100, 21);
            this.zoom.TabIndex = 0;
            this.zoom.Text = "100";
            this.zoom.TextChanged += new System.EventHandler(this.zoom_TextChanged);
            this.zoom.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textKeyPress);
            // 
            // Zoom_Lable
            // 
            this.Zoom_Lable.AutoSize = true;
            this.Zoom_Lable.Location = new System.Drawing.Point(23, 28);
            this.Zoom_Lable.Name = "Zoom_Lable";
            this.Zoom_Lable.Size = new System.Drawing.Size(29, 12);
            this.Zoom_Lable.TabIndex = 1;
            this.Zoom_Lable.Text = "缩放";
            // 
            // FOV_Lable
            // 
            this.FOV_Lable.AutoSize = true;
            this.FOV_Lable.Location = new System.Drawing.Point(23, 58);
            this.FOV_Lable.Name = "FOV_Lable";
            this.FOV_Lable.Size = new System.Drawing.Size(23, 12);
            this.FOV_Lable.TabIndex = 2;
            this.FOV_Lable.Text = "FOV";
            // 
            // fov
            // 
            this.fov.Location = new System.Drawing.Point(74, 52);
            this.fov.Name = "fov";
            this.fov.Size = new System.Drawing.Size(100, 21);
            this.fov.TabIndex = 3;
            this.fov.Text = "1.3";
            this.fov.TextChanged += new System.EventHandler(this.fov_TextChanged);
            this.fov.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textKeyPress);
            // 
            // SetToDefault
            // 
            this.SetToDefault.Location = new System.Drawing.Point(25, 88);
            this.SetToDefault.Name = "SetToDefault";
            this.SetToDefault.Size = new System.Drawing.Size(75, 23);
            this.SetToDefault.TabIndex = 4;
            this.SetToDefault.Text = "恢复默认";
            this.SetToDefault.UseVisualStyleBackColor = true;
            this.SetToDefault.Click += new System.EventHandler(this.button_SetToDefault_Click);
            // 
            // offset
            // 
            this.offset.Location = new System.Drawing.Point(74, 167);
            this.offset.Name = "offset";
            this.offset.Size = new System.Drawing.Size(100, 21);
            this.offset.TabIndex = 5;
            this.offset.Text = "0";
            this.offset.TextChanged += new System.EventHandler(this.offset_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 170);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 6;
            this.label1.Text = "Offset";
            // 
            // PullOffsetFromeNetwork
            // 
            this.PullOffsetFromeNetwork.Location = new System.Drawing.Point(18, 207);
            this.PullOffsetFromeNetwork.Name = "PullOffsetFromeNetwork";
            this.PullOffsetFromeNetwork.Size = new System.Drawing.Size(75, 23);
            this.PullOffsetFromeNetwork.TabIndex = 7;
            this.PullOffsetFromeNetwork.Text = "从网络获取";
            this.PullOffsetFromeNetwork.UseVisualStyleBackColor = true;
            this.PullOffsetFromeNetwork.Click += new System.EventHandler(this.PullOffsetFromeNetwork_Click);
            // 
            // ScanOffsetLocally
            // 
            this.ScanOffsetLocally.Location = new System.Drawing.Point(99, 207);
            this.ScanOffsetLocally.Name = "ScanOffsetLocally";
            this.ScanOffsetLocally.Size = new System.Drawing.Size(75, 23);
            this.ScanOffsetLocally.TabIndex = 8;
            this.ScanOffsetLocally.Text = "内存搜索";
            this.ScanOffsetLocally.UseVisualStyleBackColor = true;
            this.ScanOffsetLocally.Click += new System.EventHandler(this.ScanOffsetLocally_Click);
            // 
            // RequestInfo
            // 
            this.RequestInfo.AutoSize = true;
            this.RequestInfo.Location = new System.Drawing.Point(23, 253);
            this.RequestInfo.Name = "RequestInfo";
            this.RequestInfo.Size = new System.Drawing.Size(0, 12);
            this.RequestInfo.TabIndex = 9;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(196, 167);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(185, 24);
            this.label2.TabIndex = 10;
            this.label2.Text = "使用内存搜索前请将视角拉到最远\r\n内存搜索会耗费一定时间";
            // 
            // ViewUnlocker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label2);
            this.Controls.Add(this.RequestInfo);
            this.Controls.Add(this.ScanOffsetLocally);
            this.Controls.Add(this.PullOffsetFromeNetwork);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.offset);
            this.Controls.Add(this.SetToDefault);
            this.Controls.Add(this.fov);
            this.Controls.Add(this.FOV_Lable);
            this.Controls.Add(this.Zoom_Lable);
            this.Controls.Add(this.zoom);
            this.Name = "ViewUnlocker";
            this.Size = new System.Drawing.Size(800, 450);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox zoom;
        private System.Windows.Forms.Label Zoom_Lable;
        private System.Windows.Forms.Label FOV_Lable;
        private System.Windows.Forms.TextBox fov;
        private System.Windows.Forms.Button SetToDefault;
		private System.Windows.Forms.TextBox offset;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button PullOffsetFromeNetwork;
		private System.Windows.Forms.Button ScanOffsetLocally;
		private System.Windows.Forms.Label RequestInfo;
		private System.Windows.Forms.Label label2;
	}
}
