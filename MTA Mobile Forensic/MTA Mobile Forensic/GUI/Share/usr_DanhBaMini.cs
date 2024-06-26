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
    public partial class usr_DanhBaMini : UserControl
    {
        public usr_DanhBaMini()
        {
            InitializeComponent();
        }

        public string display_name = "";
        public string contact_last_updated_timestamp = "";
        public string company = "";
        public string account_name = "";
        public string account_type = "";
        public string hash_id = "";
        public string nickname = "";
        public string data1 = "";
        public event EventHandler ControlClicked;

        public usr_DanhBaMini(string display_name, string contact_last_updated_timestamp, string company, string account_type, string account_name, string nickname, string data1, string hash_id)
        {
            InitializeComponent();

            this.display_name = display_name;
            this.contact_last_updated_timestamp = contact_last_updated_timestamp;
            this.company = company;
            this.account_name = account_name;
            this.account_type = account_type;
            this.hash_id = hash_id;
            this.nickname = nickname;
            this.data1 = data1;

            txtTenDanhBa.Text = display_name;
            txtSoDienThoai.Text = data1;
        }

        private void panel1_Click(object sender, EventArgs e)
        {
            ControlClicked?.Invoke(this, EventArgs.Empty);
        }

        private void panel1_DoubleClick(object sender, EventArgs e)
        {
            checkBox.Checked = !checkBox.Checked;
        }

        private void panel1_MouseEnter(object sender, EventArgs e)
        {
            if (sender is Panel panel)
            {
                // Change the Panel's BackColor when mouse enters
                panel.BackColor = Color.LightBlue;
            }
        }

        private void panel1_MouseLeave(object sender, EventArgs e)
        {
            if (sender is Panel panel)
            {
                // Change the Panel's BackColor back to its original color when mouse leaves
                panel.BackColor = Color.White;
            }
        }
    }
}
