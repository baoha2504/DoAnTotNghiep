using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MTA_Mobile_Forensic.GUI.Share
{
    public partial class usr_AnhMini : UserControl
    {
        public usr_AnhMini()
        {
            InitializeComponent();
        }

        public string linkanh = "";
        public string tenfile = "";
        public string thoigian = "";
        public event EventHandler ControlClicked;

        public usr_AnhMini(string linkanh, string tenfile, string thoigian)
        {
            InitializeComponent();

            this.linkanh = linkanh;
            this.tenfile = tenfile;
            this.thoigian = thoigian;

            pbAnh.SizeMode = PictureBoxSizeMode.Zoom;
            pbAnh.Load(linkanh);
            txtTenAnh.Text = tenfile;
            txtThoiGian.Text = thoigian;

        }

        private void pbAnh_Click(object sender, EventArgs e)
        {
            ControlClicked?.Invoke(this, EventArgs.Empty);
        }

        private void pbAnh_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.DoDragDrop(this, DragDropEffects.Move);
            }
        }
    }
}
