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
using MediaToolkit;
using MediaToolkit.Model;
using MediaToolkit.Options;
using MTA_Mobile_Forensic.GUI.Share;
using MTA_Mobile_Forensic.Support;

namespace MTA_Mobile_Forensic.GUI.Android
{
    public partial class usr_Video : UserControl
    {
        public usr_Video()
        {
            InitializeComponent();
        }

        string query = "";
        function function = new function();
        exiftool exiftool = new exiftool();
        string linkvideodachon = "";
        int itemsPerPage = 20;
        int currentPage = 0;
        private List<string> videoFiles;

        private async void LoadWeb(string link)
        {
            await webView21.EnsureCoreWebView2Async(null);
            webView21.CoreWebView2.Navigate(link);
        }

        private async void ClearWebView()
        {
            await webView21.EnsureCoreWebView2Async(null);
            webView21.CoreWebView2.Navigate("about:blank");
        }

        private void CheckInfoImage(string pathImage)
        {
            query = pathImage;
            string str = exiftool.exiftoolCommand(query);
            txtThongTinVideo.Text = str;

            string link = function.GetGPSPositionToText(str);
            if (link != "Error")
            {
                LoadWeb(link);
            }
            else
            {
                ClearWebView();
            }
        }

        private void GetVideoInFolder(string folderPath)
        {
            if (folderPath != string.Empty)
            {
                try
                {
                    string[] videoExtensions = new string[] { ".mp4", ".avi", ".mov", ".wmv", ".flv", ".mkv", ".webm", ".m4v", ".mpeg", ".mpg" };

                    videoFiles = Directory.GetFiles(folderPath)
                                             .Where(file => videoExtensions.Contains(Path.GetExtension(file).ToLower()))
                                             .ToList();

                    if (cbbTuyChon.Text != "Trong thư mục")
                    {
                        var subDirectories = Directory.GetDirectories(folderPath);
                        foreach (var subDir in subDirectories)
                        {
                            var subDirFiles = Directory.GetFiles(subDir)
                                                       .Where(file => videoExtensions.Contains(Path.GetExtension(file).ToLower()))
                                                       .ToList();
                            videoFiles.AddRange(subDirFiles);
                        }
                    }
                    Add_usr_VideoMini(currentPage);
                }
                catch (Exception ex)
                {
                    frm_Notification frm_Notification = new frm_Notification("error", ex.ToString());
                    frm_Notification.ShowDialog();
                }
            }
        }

        private void Add_usr_VideoMini(int pageNumber)
        {
            flpDSVideo.Controls.Clear();

            int start = pageNumber * itemsPerPage;
            int end = Math.Min(start + itemsPerPage, videoFiles.Count);

            txtTrangHienTai.Text = $"Trang {pageNumber + 1} / {Math.Ceiling((double)videoFiles.Count / itemsPerPage)}";

            for (int i = start; i < end; i++)
            {
                usr_VideoMini usr_VideoMini = new usr_VideoMini(videoFiles[i], Path.GetFileName(videoFiles[i]), function.GetLastModified(videoFiles[i]));
                usr_VideoMini.ControlClicked += flpDSVideo_Click;
                flpDSVideo.Controls.Add(usr_VideoMini);
            }
        }

        private void btnChonFolder_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
            {
                DialogResult result = folderBrowserDialog.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(folderBrowserDialog.SelectedPath))
                {
                    string selectedFolder = folderBrowserDialog.SelectedPath;
                    txtTimKiem.Text = selectedFolder;
                }
            }
        }

        private void txtTimKiem_TextChanged(object sender, EventArgs e)
        {
            GetVideoInFolder(txtTimKiem.Text);
        }

        private void flpDSVideo_Click(object sender, EventArgs e)
        {
            if (sender is usr_VideoMini clickedControl)
            {
                pbVideoDaChon.SizeMode = PictureBoxSizeMode.Zoom;

                string thumbnailPath = Path.Combine(Path.GetTempPath(), Path.GetFileName(clickedControl.linkvideo) + "_thumbnail.jpg");
                if (!File.Exists(thumbnailPath))
                {
                    ExtractThumbnail(clickedControl.linkvideo, thumbnailPath);
                }

                if (File.Exists(thumbnailPath))
                {
                    pbVideoDaChon.Load(thumbnailPath);
                }

                CheckInfoImage(clickedControl.linkvideo);
                linkvideodachon = clickedControl.linkvideo;
            }
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

        private void flpDSVideo_Resize(object sender, EventArgs e)
        {
            foreach (UserControl control in flpDSVideo.Controls)
            {
                control.Width = flpDSVideo.Width;
            }
        }

        private void pbVideoDaChon_DoubleClick(object sender, EventArgs e)
        {
            if(linkvideodachon != String.Empty)
            {
                frm_XemVideo frm_XemVideo = new frm_XemVideo(linkvideodachon);
                frm_XemVideo.Show();
            }
        }

        private void btnXemVideo_Click(object sender, EventArgs e)
        {
            pbVideoDaChon_DoubleClick(sender, e);
        }

        private void btnXuat_Click(object sender, EventArgs e)
        {

        }

        private void btnNhap_Click(object sender, EventArgs e)
        {

        }

        private void btnChonTatCa_Click(object sender, EventArgs e)
        {
            foreach (usr_VideoMini control in flpDSVideo.Controls)
            {
                control.checkBox.Checked = true;
            }
        }

        private void btnBoChonTatCa_Click(object sender, EventArgs e)
        {
            foreach (usr_VideoMini control in flpDSVideo.Controls)
            {
                control.checkBox.Checked = false;
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            txtThongTinVideo.Text = string.Empty;
            ClearWebView();
            GetVideoInFolder(txtTimKiem.Text);
            pbVideoDaChon.Image = null;
        }

        private void cbbTuyChon_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetVideoInFolder(txtTimKiem.Text);
        }

        private void btnTrangTruoc_Click(object sender, EventArgs e)
        {
            btnTrangTiep.Enabled = true;
            if (currentPage > 0)
            {
                currentPage--;
                Add_usr_VideoMini(currentPage);
            }
            else
            {
                btnTrangTruoc.Enabled = false;
            }
        }

        private void btnTrangTiep_Click(object sender, EventArgs e)
        {
            btnTrangTruoc.Enabled = true;
            if (currentPage < (videoFiles.Count - 1) / itemsPerPage)
            {
                currentPage++;
                Add_usr_VideoMini(currentPage);
            }
            else
            {
                btnTrangTiep.Enabled = false;
            }
        }

        
    }
}
