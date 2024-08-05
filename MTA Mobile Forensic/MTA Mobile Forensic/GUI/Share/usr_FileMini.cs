using DevExpress.XtraWaitForm;
using MTA_Mobile_Forensic.GUI.Android;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using static DevExpress.Skins.SolidColorHelper;

namespace MTA_Mobile_Forensic.GUI.Share
{
    public partial class usr_FileMini : UserControl
    {
        public usr_FileMini()
        {
            InitializeComponent();
        }

        public string loai = "";
        public string tenfile = "";
        public string fullpath = "";
        public event EventHandler ControlClicked;

        public usr_FileMini(string loai, string tenfile, string fullpath)
        {
            InitializeComponent();

            this.loai = loai;
            this.tenfile = tenfile;
            this.fullpath = fullpath;


            string sourceDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string projectDirectory = Directory.GetParent(sourceDirectory).Parent.Parent.FullName;
            string imagePath = Path.Combine(projectDirectory, "Data", "Image");
            string fullImagePath = "";

            if (loai == "folder")
            {
                fullImagePath = Path.Combine(imagePath, "folder.png");
                pictureBox1.Image = Image.FromFile(fullImagePath);
            }
            else if (loai == "image")
            {
                fullImagePath = Path.Combine(imagePath, "image.png");
                pictureBox1.Image = Image.FromFile(fullImagePath);
            }
            else if (loai == "video")
            {
                fullImagePath = Path.Combine(imagePath, "mp4.png");
                pictureBox1.Image = Image.FromFile(fullImagePath);
            }
            else if (loai == "audio")
            {
                fullImagePath = Path.Combine(imagePath, "mp3.png");
                pictureBox1.Image = Image.FromFile(fullImagePath);
            }
            else if (loai == "word")
            {
                fullImagePath = Path.Combine(imagePath, "doc.png");
                pictureBox1.Image = Image.FromFile(fullImagePath);
            }
            else if (loai == "excel")
            {
                fullImagePath = Path.Combine(imagePath, "xls.png");
                pictureBox1.Image = Image.FromFile(fullImagePath);
            }
            else if (loai == "powerpoint")
            {
                fullImagePath = Path.Combine(imagePath, "ppt.png");
                pictureBox1.Image = Image.FromFile(fullImagePath);
            }
            else if (loai == "pdf")
            {
                fullImagePath = Path.Combine(imagePath, "pdf.png");
                pictureBox1.Image = Image.FromFile(fullImagePath);
            }
            else if (loai == "txt")
            {
                fullImagePath = Path.Combine(imagePath, "txt.png");
                pictureBox1.Image = Image.FromFile(fullImagePath);
            }
            else
            {
                fullImagePath = Path.Combine(imagePath, "file.png");
                pictureBox1.Image = Image.FromFile(fullImagePath);
            }
            pnTen.Text = tenfile;
        }

        private void pictureBox1_DoubleClick(object sender, EventArgs e)
        {
            ControlClicked?.Invoke(this, EventArgs.Empty);
            if (loai == "folder")
            {

            }
            else if (loai == "image")
            {

            }
            else if (loai == "video")
            {

            }
            else if (loai == "audio")
            {

            }
            else if (loai == "word")
            {

            }
            else if (loai == "excel")
            {

            }
            else if (loai == "powerpoint")
            {

            }
            else if (loai == "pdf")
            {

            }
            else
            {

            }
        }

        private void pnTen_DoubleClick(object sender, EventArgs e)
        {
            ControlClicked?.Invoke(this, EventArgs.Empty);
        }

        private void pnTen_Click(object sender, EventArgs e)
        {
            ControlClicked?.Invoke(this, EventArgs.Empty);
        }
    }
}
