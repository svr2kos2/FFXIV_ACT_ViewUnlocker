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
            this.offsetInputField = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ScanOffsetLocally = new System.Windows.Forms.Button();
            this.StatusLable = new System.Windows.Forms.Label();
            this.MemoryScanProgress = new System.Windows.Forms.ProgressBar();
            this.GameVersionLable = new System.Windows.Forms.Label();
            this.RemoteInfoLable = new System.Windows.Forms.Label();
            this.cbUpload = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.Version = new System.Windows.Forms.Label();
            this.cbAutoCheck = new System.Windows.Forms.CheckBox();
            this.PullData = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
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
            this.FOV_Lable.Location = new System.Drawing.Point(3, 39);
            this.FOV_Lable.Name = "FOV_Lable";
            this.FOV_Lable.Size = new System.Drawing.Size(23, 12);
            this.FOV_Lable.TabIndex = 2;
            this.FOV_Lable.Text = "FOV";
            // 
            // FovInputfield
            // 
            this.FovInputfield.Location = new System.Drawing.Point(38, 36);
            this.FovInputfield.Name = "FovInputfield";
            this.FovInputfield.Size = new System.Drawing.Size(48, 21);
            this.FovInputfield.TabIndex = 3;
            this.FovInputfield.Text = "0.78";
            this.FovInputfield.TextChanged += new System.EventHandler(this.fov_TextChanged);
            // 
            // SetToDefault
            // 
            this.SetToDefault.Location = new System.Drawing.Point(3, 63);
            this.SetToDefault.Name = "SetToDefault";
            this.SetToDefault.Size = new System.Drawing.Size(75, 23);
            this.SetToDefault.TabIndex = 4;
            this.SetToDefault.Text = "恢复默认";
            this.SetToDefault.UseVisualStyleBackColor = true;
            this.SetToDefault.Click += new System.EventHandler(this.SetToDefault_Click);
            // 
            // offsetInputField
            // 
            this.offsetInputField.Location = new System.Drawing.Point(5, 107);
            this.offsetInputField.Name = "offsetInputField";
            this.offsetInputField.Size = new System.Drawing.Size(81, 21);
            this.offsetInputField.TabIndex = 5;
            this.offsetInputField.Text = "0";
            this.offsetInputField.TextChanged += new System.EventHandler(this.offsetInputField_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 89);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 6;
            this.label1.Text = "Offset";
            // 
            // ScanOffsetLocally
            // 
            this.ScanOffsetLocally.Location = new System.Drawing.Point(91, 113);
            this.ScanOffsetLocally.Name = "ScanOffsetLocally";
            this.ScanOffsetLocally.Size = new System.Drawing.Size(72, 23);
            this.ScanOffsetLocally.TabIndex = 8;
            this.ScanOffsetLocally.Text = "本地搜索";
            this.ScanOffsetLocally.UseVisualStyleBackColor = true;
            this.ScanOffsetLocally.Click += new System.EventHandler(this.ScanOffsetLocally_Click);
            // 
            // StatusLable
            // 
            this.StatusLable.AutoSize = true;
            this.StatusLable.BackColor = System.Drawing.SystemColors.Control;
            this.StatusLable.Location = new System.Drawing.Point(3, 164);
            this.StatusLable.Name = "StatusLable";
            this.StatusLable.Size = new System.Drawing.Size(77, 12);
            this.StatusLable.TabIndex = 11;
            this.StatusLable.Text = "等待游戏启动";
            // 
            // MemoryScanProgress
            // 
            this.MemoryScanProgress.BackColor = System.Drawing.SystemColors.Control;
            this.MemoryScanProgress.Location = new System.Drawing.Point(91, 139);
            this.MemoryScanProgress.Name = "MemoryScanProgress";
            this.MemoryScanProgress.Size = new System.Drawing.Size(179, 10);
            this.MemoryScanProgress.TabIndex = 12;
            // 
            // GameVersionLable
            // 
            this.GameVersionLable.AutoSize = true;
            this.GameVersionLable.Location = new System.Drawing.Point(3, 139);
            this.GameVersionLable.Name = "GameVersionLable";
            this.GameVersionLable.Size = new System.Drawing.Size(83, 24);
            this.GameVersionLable.TabIndex = 13;
            this.GameVersionLable.Text = "本地游戏版本:\r\nUnknow";
            // 
            // RemoteInfoLable
            // 
            this.RemoteInfoLable.AutoSize = true;
            this.RemoteInfoLable.Location = new System.Drawing.Point(6, 17);
            this.RemoteInfoLable.Name = "RemoteInfoLable";
            this.RemoteInfoLable.Size = new System.Drawing.Size(161, 60);
            this.RemoteInfoLable.TabIndex = 14;
            this.RemoteInfoLable.Text = "版本:\r\nOffset:\r\n游戏版本:\r\n看起来远程数据还没更新哦, \r\n尝试一下本地搜索吧";
            // 
            // cbUpload
            // 
            this.cbUpload.AutoSize = true;
            this.cbUpload.Location = new System.Drawing.Point(169, 117);
            this.cbUpload.Name = "cbUpload";
            this.cbUpload.Size = new System.Drawing.Size(108, 16);
            this.cbUpload.TabIndex = 15;
            this.cbUpload.Text = "上传分享Offset";
            this.cbUpload.UseVisualStyleBackColor = true;
            this.cbUpload.CheckStateChanged += new System.EventHandler(this.cbUpload_CheckStateChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.RemoteInfoLable);
            this.groupBox1.Location = new System.Drawing.Point(92, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(178, 83);
            this.groupBox1.TabIndex = 16;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "服务器信息";
            // 
            // Version
            // 
            this.Version.AutoSize = true;
            this.Version.Location = new System.Drawing.Point(211, 164);
            this.Version.Name = "Version";
            this.Version.Size = new System.Drawing.Size(59, 12);
            this.Version.TabIndex = 17;
            this.Version.Text = "版本:v1.1";
            // 
            // cbAutoCheck
            // 
            this.cbAutoCheck.AutoSize = true;
            this.cbAutoCheck.Location = new System.Drawing.Point(169, 95);
            this.cbAutoCheck.Name = "cbAutoCheck";
            this.cbAutoCheck.Size = new System.Drawing.Size(72, 16);
            this.cbAutoCheck.TabIndex = 18;
            this.cbAutoCheck.Text = "自动更新";
            this.cbAutoCheck.UseVisualStyleBackColor = true;
            this.cbAutoCheck.CheckStateChanged += new System.EventHandler(this.cbAutoCheck_CheckStateChanged);
            // 
            // PullData
            // 
            this.PullData.Location = new System.Drawing.Point(91, 89);
            this.PullData.Name = "PullData";
            this.PullData.Size = new System.Drawing.Size(72, 23);
            this.PullData.TabIndex = 19;
            this.PullData.Text = "手动更新";
            this.PullData.UseVisualStyleBackColor = true;
            this.PullData.Click += new System.EventHandler(this.PullData_Click);
            // 
            // MainPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.MemoryScanProgress);
            this.Controls.Add(this.cbAutoCheck);
            this.Controls.Add(this.Version);
            this.Controls.Add(this.PullData);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.cbUpload);
            this.Controls.Add(this.GameVersionLable);
            this.Controls.Add(this.StatusLable);
            this.Controls.Add(this.ScanOffsetLocally);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.offsetInputField);
            this.Controls.Add(this.SetToDefault);
            this.Controls.Add(this.FovInputfield);
            this.Controls.Add(this.FOV_Lable);
            this.Controls.Add(this.Zoom_Lable);
            this.Controls.Add(this.ZoomInputField);
            this.Name = "MainPage";
            this.Size = new System.Drawing.Size(282, 183);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox ZoomInputField;
        private System.Windows.Forms.Label Zoom_Lable;
        private System.Windows.Forms.Label FOV_Lable;
        private System.Windows.Forms.TextBox FovInputfield;
        private System.Windows.Forms.Button SetToDefault;
		private System.Windows.Forms.TextBox offsetInputField;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button ScanOffsetLocally;
		private System.Windows.Forms.Label StatusLable;
		private System.Windows.Forms.ProgressBar MemoryScanProgress;
		private System.Windows.Forms.Label GameVersionLable;
		private System.Windows.Forms.Label RemoteInfoLable;
		private System.Windows.Forms.CheckBox cbUpload;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label Version;
		private System.Windows.Forms.CheckBox cbAutoCheck;
		private System.Windows.Forms.Button PullData;
	}
}
