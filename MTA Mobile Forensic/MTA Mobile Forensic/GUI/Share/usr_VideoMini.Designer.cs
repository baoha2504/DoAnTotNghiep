namespace MTA_Mobile_Forensic.GUI.Share
{
    partial class usr_VideoMini
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(usr_VideoMini));
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnPlay = new System.Windows.Forms.Panel();
            this.checkBox = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.pbAnh = new System.Windows.Forms.PictureBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.txtThoiGian = new DevComponents.DotNetBar.PanelEx();
            this.txtTenVideo = new DevComponents.DotNetBar.PanelEx();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbAnh)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.btnPlay);
            this.panel1.Controls.Add(this.checkBox);
            this.panel1.Controls.Add(this.pbAnh);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(100, 135);
            this.panel1.TabIndex = 1;
            // 
            // btnPlay
            // 
            this.btnPlay.BackColor = System.Drawing.Color.Transparent;
            this.btnPlay.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnPlay.BackgroundImage")));
            this.btnPlay.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnPlay.Location = new System.Drawing.Point(40, 57);
            this.btnPlay.Name = "btnPlay";
            this.btnPlay.Size = new System.Drawing.Size(20, 20);
            this.btnPlay.TabIndex = 13;
            this.btnPlay.Click += new System.EventHandler(this.btnPlay_Click);
            // 
            // checkBox
            // 
            // 
            // 
            // 
            this.checkBox.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.checkBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox.Location = new System.Drawing.Point(83, 0);
            this.checkBox.Name = "checkBox";
            this.checkBox.Size = new System.Drawing.Size(17, 23);
            this.checkBox.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.checkBox.TabIndex = 11;
            // 
            // pbAnh
            // 
            this.pbAnh.BackColor = System.Drawing.Color.White;
            this.pbAnh.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pbAnh.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbAnh.Image = ((System.Drawing.Image)(resources.GetObject("pbAnh.Image")));
            this.pbAnh.Location = new System.Drawing.Point(0, 0);
            this.pbAnh.Name = "pbAnh";
            this.pbAnh.Size = new System.Drawing.Size(100, 135);
            this.pbAnh.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbAnh.TabIndex = 0;
            this.pbAnh.TabStop = false;
            this.pbAnh.Click += new System.EventHandler(this.pbAnh_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.txtThoiGian);
            this.panel2.Controls.Add(this.txtTenVideo);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 135);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(100, 30);
            this.panel2.TabIndex = 2;
            // 
            // txtThoiGian
            // 
            this.txtThoiGian.CanvasColor = System.Drawing.SystemColors.Control;
            this.txtThoiGian.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.txtThoiGian.DisabledBackColor = System.Drawing.Color.Empty;
            this.txtThoiGian.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtThoiGian.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtThoiGian.Location = new System.Drawing.Point(0, 15);
            this.txtThoiGian.Name = "txtThoiGian";
            this.txtThoiGian.Size = new System.Drawing.Size(100, 15);
            this.txtThoiGian.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.txtThoiGian.Style.BackColor1.Color = System.Drawing.Color.White;
            this.txtThoiGian.Style.BackColor2.Color = System.Drawing.Color.White;
            this.txtThoiGian.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.txtThoiGian.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.txtThoiGian.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.txtThoiGian.Style.GradientAngle = 90;
            this.txtThoiGian.TabIndex = 4;
            this.txtThoiGian.Text = "Thời gian";
            // 
            // txtTenVideo
            // 
            this.txtTenVideo.CanvasColor = System.Drawing.SystemColors.Control;
            this.txtTenVideo.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.txtTenVideo.DisabledBackColor = System.Drawing.Color.Empty;
            this.txtTenVideo.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtTenVideo.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTenVideo.Location = new System.Drawing.Point(0, 0);
            this.txtTenVideo.Name = "txtTenVideo";
            this.txtTenVideo.Size = new System.Drawing.Size(100, 15);
            this.txtTenVideo.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.txtTenVideo.Style.BackColor1.Color = System.Drawing.Color.White;
            this.txtTenVideo.Style.BackColor2.Color = System.Drawing.Color.White;
            this.txtTenVideo.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.txtTenVideo.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.txtTenVideo.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.txtTenVideo.Style.GradientAngle = 90;
            this.txtTenVideo.TabIndex = 0;
            this.txtTenVideo.Text = "Tên ảnh";
            // 
            // usr_VideoMini
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "usr_VideoMini";
            this.Size = new System.Drawing.Size(100, 165);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbAnh)).EndInit();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        public DevComponents.DotNetBar.Controls.CheckBoxX checkBox;
        private System.Windows.Forms.PictureBox pbAnh;
        private System.Windows.Forms.Panel panel2;
        private DevComponents.DotNetBar.PanelEx txtThoiGian;
        private DevComponents.DotNetBar.PanelEx txtTenVideo;
        private System.Windows.Forms.Panel btnPlay;
    }
}
