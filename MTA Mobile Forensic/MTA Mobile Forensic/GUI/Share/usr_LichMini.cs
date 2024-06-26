using System;
using System.Drawing;
using System.Windows.Forms;

namespace MTA_Mobile_Forensic.GUI.Share
{
    public partial class usr_LichMini : UserControl
    {
        public usr_LichMini()
        {
            InitializeComponent();
        }

        public string account_type = "";
        public string account_name = "";
        public string lastDate = "";
        public string dtstart = "";
        public string dtend = "";
        public string title = "";
        public string calendar_displayName = "";
        public string eventTimezone = "";
        public string calendar_timezone = "";
        public event EventHandler ControlClicked;

        public usr_LichMini(string account_type, string account_name, string lastDate, string dtstart, string dtend, string title, string calendar_displayName, string eventTimezone, string calendar_timezone, string ngay)
        {
            InitializeComponent();
            this.account_type = account_type;
            this.account_name = account_name;
            this.lastDate = lastDate;
            this.dtstart = dtstart;
            this.dtend = dtend;
            this.title = title;
            if (calendar_displayName.Contains("@gmail.com"))
            {
                this.calendar_displayName = "Không có chủ đề";
            }
            else
            {
                this.calendar_displayName = calendar_displayName;
            }
            this.eventTimezone = eventTimezone;
            this.calendar_timezone = calendar_timezone;

            txtSuKien.Text = title;
            //txtThoiGian.Text = lastDate.Replace("00:00:00 ", "");
            txtThoiGian.Text = lastDate;
            txtNgay.Text = ngay;
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
    }
}
