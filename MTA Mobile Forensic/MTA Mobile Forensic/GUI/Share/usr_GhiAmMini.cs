using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace MTA_Mobile_Forensic.GUI.Share
{
    public partial class usr_GhiAmMini : UserControl
    {
        public string tenghiam = "";
        public string thoigian = "";
        public string path = "";
        public string pathRecord = "";
        public event EventHandler ControlClicked;

        public usr_GhiAmMini()
        {
            InitializeComponent();
        }

        public usr_GhiAmMini(string path, string tenghiam, string thoigian)
        {
            InitializeComponent();
            this.tenghiam = tenghiam;
            this.thoigian = thoigian;
            this.path = path;

            txtTenGhiAm.Text = tenghiam;
            txtThoiGian.Text = thoigian;

            string sourceDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string projectDirectory = Directory.GetParent(sourceDirectory).Parent.Parent.FullName;
            string imagePath = Path.Combine(projectDirectory, "Data", "Image");
            string imageName = "";

            string extension = Path.GetExtension(tenghiam).ToLower();

            if (extension == ".mp3")
            {
                imageName = "mp3.png";
                pathRecord = Path.Combine(imagePath, imageName);
                pbAnh.Image = Image.FromFile(pathRecord);
            }
            else if (extension == ".wav")
            {
                imageName = "wav.png";
                pathRecord = Path.Combine(imagePath, imageName);
                pbAnh.Image = Image.FromFile(pathRecord);
            }
            else if (extension == ".aac")
            {
                imageName = "aac.png";
                pathRecord = Path.Combine(imagePath, imageName);
                pbAnh.Image = Image.FromFile(pathRecord);
            }
        }

        private void pbAnh_Click(object sender, EventArgs e)
        {
            ControlClicked?.Invoke(this, EventArgs.Empty);
        }

        private void txtTenGhiAm_Click(object sender, EventArgs e)
        {
            ControlClicked?.Invoke(this, EventArgs.Empty);
        }

        private void txtThoiGian_Click(object sender, EventArgs e)
        {
            ControlClicked?.Invoke(this, EventArgs.Empty);
        }
    }
}
