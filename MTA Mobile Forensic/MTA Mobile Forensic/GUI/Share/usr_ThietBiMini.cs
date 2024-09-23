using System;
using System.Drawing;
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
        public string loai = "";

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
            string avatar = "";
            if (lblTenThietBi.Text.Contains("IPHONE"))
            {
                avatar = "device-ios-bg.png";
                loai = "IPHONE";
                if (lblTenThietBi.Text.Contains("IPHONE 5") || lblTenThietBi.Text.Contains("IPHONE 5s"))
                {
                    imageName = "iphone-5s-sliver.png";

                }
                else
                {
                    imageName = "phone.png";
                }
            }
            else
            {
                avatar = "device-android-bg.png";
                loai = "ANDROID";
                if (lblTenThietBi.Text == "XIAOMI MI 9T")
                {
                    imageName = "xiaomi-mi9t.png";

                }
                else if (lblTenThietBi.Text == "SAMSUNG SM-A600G")
                {
                    imageName = "samsung-galaxy-a6.png";
                }
                else if (lblTenThietBi.Text == "OPPO CPH1803" || lblTenThietBi.Text == "OPPO CPH1805+")
                {
                    imageName = "oppo-a3s.png";
                }
                else if (lblTenThietBi.Text == "XIAOMI REDMI NOTE 11")
                {
                    imageName = "xiaomi-redmi-note-11.png";
                }
                else if (lblTenThietBi.Text == "OPPO CPH1801")
                {
                    imageName = "oppo-a71-cph-1801.png";
                }
                else
                {
                    imageName = "phone.png";
                }
            }
            string fullImagePath = Path.Combine(imagePath, imageName);
            pictureBox1.Image = Image.FromFile(Path.Combine(imagePath, avatar));
            return fullImagePath;
        }
    }
}
