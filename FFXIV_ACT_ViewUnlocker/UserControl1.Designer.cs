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
            this.SuspendLayout();
            // 
            // zoom
            // 
            this.zoom.Location = new System.Drawing.Point(74, 25);
            this.zoom.Name = "zoom";
            this.zoom.Size = new System.Drawing.Size(100, 21);
            this.zoom.TabIndex = 0;
            this.zoom.Text = "100";
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
            this.fov.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textKeyPress);
            // 
            // ViewUnlocker 
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.fov);
            this.Controls.Add(this.FOV_Lable);
            this.Controls.Add(this.Zoom_Lable);
            this.Controls.Add(this.zoom);
            this.Name = "ViewUnlocker ";
            this.Size = new System.Drawing.Size(800, 450);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox zoom;
        private System.Windows.Forms.Label Zoom_Lable;
        private System.Windows.Forms.Label FOV_Lable;
        private System.Windows.Forms.TextBox fov;
    }
}
