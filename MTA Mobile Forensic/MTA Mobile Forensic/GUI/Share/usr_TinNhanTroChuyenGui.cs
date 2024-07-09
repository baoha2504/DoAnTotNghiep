using System;
using System.Drawing;
using System.Windows.Forms;

namespace MTA_Mobile_Forensic.GUI.Share
{
    public partial class usr_TinNhanTroChuyenGui : UserControl
    {
        public usr_TinNhanTroChuyenGui()
        {
            InitializeComponent();
        }

        public usr_TinNhanTroChuyenGui(string noidung, string thoigian, int chieurong)
        {
            InitializeComponent();
            if (noidung.Length < 30)
            {
                noidung = noidung.PadLeft(30);
            }
            lblNoiDung.Text = noidung;
            lblThoiGian.Text = thoigian;
            lblNoiDung.MaximumSize = new Size(chieurong, 0);
        }

        private void usr_TinNhanTroChuyenGui_Resize(object sender, EventArgs e)
        {

        }
    }
}
