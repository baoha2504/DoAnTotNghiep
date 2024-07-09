using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MTA_Mobile_Forensic.GUI.Share
{
    public partial class usr_TinNhanTroChuyenNhan : UserControl
    {
        public usr_TinNhanTroChuyenNhan()
        {
            InitializeComponent();
        }

        public usr_TinNhanTroChuyenNhan(string noidung, string thoigian, int chieurong)
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
    }
}
