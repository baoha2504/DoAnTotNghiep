using MediaToolkit.Model;
using MediaToolkit.Options;
using MediaToolkit;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MTA_Mobile_Forensic.GUI.Share
{
    public partial class usr_VideoMini : UserControl
    {
        public usr_VideoMini()
        {
            InitializeComponent();
        }

        public string linkvideo = "";
        public string tenfile = "";
        public string thoigian = "";
        public event EventHandler ControlClicked;

        public usr_VideoMini(string linkvideo, string tenfile, string thoigian)
        {
            InitializeComponent();

            this.linkvideo = linkvideo;
            this.tenfile = tenfile;
            this.thoigian = thoigian;

            pbAnh.SizeMode = PictureBoxSizeMode.Zoom;

            string thumbnailPath = Path.Combine(Path.GetTempPath(), tenfile + "_thumbnail.jpg");

            if (!File.Exists(thumbnailPath))
            {
                ExtractThumbnail(linkvideo, thumbnailPath);
            }
            
            if (File.Exists(thumbnailPath))
            {
                pbAnh.Load(thumbnailPath);
            }

            txtTenVideo.Text = tenfile;
            txtThoiGian.Text = thoigian;

        }

        private void ExtractThumbnail(string videoPath, string thumbnailPath)
        {
            var inputFile = new MediaFile { Filename = videoPath };
            var outputFile = new MediaFile { Filename = thumbnailPath };

            using (var engine = new Engine())
            {
                engine.GetMetadata(inputFile);

                var options = new ConversionOptions { Seek = TimeSpan.FromSeconds(inputFile.Metadata.Duration.TotalSeconds / 2) };
                engine.GetThumbnail(inputFile, outputFile, options);
            }
        }

        private void pbAnh_Click(object sender, EventArgs e)
        {
            ControlClicked?.Invoke(this, EventArgs.Empty);
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            ControlClicked?.Invoke(this, EventArgs.Empty);
        }
    }
}
