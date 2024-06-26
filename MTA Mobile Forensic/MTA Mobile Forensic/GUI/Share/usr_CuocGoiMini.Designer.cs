namespace MTA_Mobile_Forensic.GUI.Share
{
    partial class usr_CuocGoiMini
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(usr_CuocGoiMini));
            this.panel1 = new System.Windows.Forms.Panel();
            this.checkBox = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.txtThoiGian = new System.Windows.Forms.Label();
            this.txtTinNhan = new System.Windows.Forms.Label();
            this.txtAddress = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.checkBox);
            this.panel1.Controls.Add(this.txtThoiGian);
            this.panel1.Controls.Add(this.txtTinNhan);
            this.panel1.Controls.Add(this.txtAddress);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(460, 60);
            this.panel1.TabIndex = 1;
            // 
            // checkBox
            // 
            // 
            // 
            // 
            this.checkBox.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.checkBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox.Location = new System.Drawing.Point(9, 14);
            this.checkBox.Name = "checkBox";
            this.checkBox.Size = new System.Drawing.Size(25, 25);
            this.checkBox.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.checkBox.TabIndex = 4;
            // 
            // txtThoiGian
            // 
            this.txtThoiGian.AutoSize = true;
            this.txtThoiGian.Dock = System.Windows.Forms.DockStyle.Right;
            this.txtThoiGian.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtThoiGian.ForeColor = System.Drawing.Color.Gray;
            this.txtThoiGian.Location = new System.Drawing.Point(397, 0);
            this.txtThoiGian.Name = "txtThoiGian";
            this.txtThoiGian.Size = new System.Drawing.Size(63, 16);
            this.txtThoiGian.TabIndex = 3;
            this.txtThoiGian.Text = "Thời gian";
            // 
            // txtTinNhan
            // 
            this.txtTinNhan.AutoSize = true;
            this.txtTinNhan.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTinNhan.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.txtTinNhan.Location = new System.Drawing.Point(84, 34);
            this.txtTinNhan.Name = "txtTinNhan";
            this.txtTinNhan.Size = new System.Drawing.Size(58, 16);
            this.txtTinNhan.TabIndex = 2;
            this.txtTinNhan.Text = "Tin nhắn";
            // 
            // txtAddress
            // 
            this.txtAddress.AutoSize = true;
            this.txtAddress.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAddress.Location = new System.Drawing.Point(83, 9);
            this.txtAddress.Name = "txtAddress";
            this.txtAddress.Size = new System.Drawing.Size(107, 18);
            this.txtAddress.TabIndex = 1;
            this.txtAddress.Text = "Số điện thoại";
            // 
            // panel2
            // 
            this.panel2.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panel2.BackgroundImage")));
            this.panel2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel2.Location = new System.Drawing.Point(38, 12);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(35, 35);
            this.panel2.TabIndex = 0;
            // 
            // usr_CuocGoiMini
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Name = "usr_CuocGoiMini";
            this.Size = new System.Drawing.Size(460, 60);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        public DevComponents.DotNetBar.Controls.CheckBoxX checkBox;
        private System.Windows.Forms.Label txtThoiGian;
        private System.Windows.Forms.Label txtTinNhan;
        private System.Windows.Forms.Label txtAddress;
        private System.Windows.Forms.Panel panel2;
    }
}
