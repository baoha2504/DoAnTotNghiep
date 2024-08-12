﻿using System;
using System.IO;
using System.Windows.Forms;

namespace MTA_Mobile_Forensic.GUI.Share
{
    public partial class usr_ThietBiMini : UserControl
    {
        public event EventHandler ControlClicked;
        public string idthietbi = "";
        public string pathImage = "";
        public string tenthietbi = "";

        public usr_ThietBiMini()
        {
            InitializeComponent();
        }

        public usr_ThietBiMini(string idthietbi, string tenthietbi, string imei, string phienban, string capnhatlancuoi)
        {
            InitializeComponent();
            this.idthietbi = idthietbi;
            this.tenthietbi = tenthietbi;
            lblTenThietBi.Text = tenthietbi;
            lblIMEI.Text = imei;
            lblPhienBan.Text = phienban;
            lblCapNhatLanCuoi.Text = capnhatlancuoi;
            pathImage = checkNameDevice();
        }

        private void btnKetNoi_Click(object sender, EventArgs e)
        {
            ControlClicked?.Invoke(this, EventArgs.Empty);
        }

        private string checkNameDevice()
        {
            string sourceDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string projectDirectory = Directory.GetParent(sourceDirectory).Parent.Parent.FullName;
            string imagePath = Path.Combine(projectDirectory, "Data", "Device");
            string imageName = "";
            if (lblTenThietBi.Text == "XIAOMI MI 9T")
            {
                imageName = "xiaomi-mi9t.png";

            }
            else if (lblTenThietBi.Text == "SAMSUNG SM-A600G")
            {
                imageName = "samsung-galaxy-a6.png";
            }
            else if (lblTenThietBi.Text == "OPPO CPH1803")
            {
                imageName = "oppo-a3s.png";
            }
            else
            {
                imageName = "phone.png";
            }
            string fullImagePath = Path.Combine(imagePath, imageName);
            return fullImagePath;
        }
    }
}
