using DevExpress.XtraEditors.Filtering.Templates;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
namespace MTA_Mobile_Forensic.GUI.Share
{
    public partial class usr_TinNhanMini : UserControl
    {
        public usr_TinNhanMini()
        {
            InitializeComponent();
        }

        public string diachi = "";
        public string tinnhan = "";
        public string thoigian = "";
        public string dateSent = "";
        public string read = "";
        public string status = "";
        public string serviceCenter = "";
        public string simId = "";
        public event EventHandler ControlClicked;

        public usr_TinNhanMini(string diachi, string tinnhan, string thoigian, string dateSent, string read,string status, string serviceCenter, string simId)
        {
            InitializeComponent();

            this.diachi = diachi;
            this.tinnhan = tinnhan;
            this.thoigian = thoigian;
            this.dateSent = dateSent;
            this.read = read;
            this.status = status;
            this.serviceCenter = serviceCenter;
            this.simId = simId;

            txtAddress.Text = diachi;
            txtTinNhan.Text = tinnhan;
            txtThoiGian.Text = thoigian;
        }

        private void panel1_MouseEnter(object sender, EventArgs e)
        {
            // Cast sender back to Panel to access its properties
            if (sender is Panel panel)
            {
                // Change the Panel's BackColor when mouse enters
                panel.BackColor = Color.LightBlue;
            }
        }

        private void panel1_MouseLeave(object sender, EventArgs e)
        {
            // Cast sender back to Panel to access its properties
            if (sender is Panel panel)
            {
                // Change the Panel's BackColor back to its original color when mouse leaves
                panel.BackColor = Color.White;
            }
        }

        private void panel1_Click(object sender, EventArgs e)
        {
            ControlClicked?.Invoke(this, EventArgs.Empty);
        }

        private void panel1_DoubleClick(object sender, EventArgs e)
        {
            checkBox.Checked = !checkBox.Checked;
        }
    }
}
