using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.Xpo.DB.Helpers;
using MTA_Mobile_Forensic.GUI.Share;
using MTA_Mobile_Forensic.Support;

namespace MTA_Mobile_Forensic.GUI.Android
{
    public partial class usr_Anh : UserControl
    {
        public usr_Anh()
        {
            InitializeComponent();
        }

        string query = "";
        function function = new function();
        exiftool exiftool = new exiftool();

        private async void LoadWeb(string link)
        {
            await webView21.EnsureCoreWebView2Async(null);
            webView21.CoreWebView2.Navigate(link);
        }

        private void CheckInfoImage(string pathImage)
        {
            query = pathImage;
            string str = exiftool.exiftoolCommand(query);
            txtThongTinAnh.Text = str;

            string link = function.GetGPSPositionToLink(str);
            LoadWeb(link);
        }

        private void GetImageInFolder(string folderPath)
        {
            // Các đuôi file dạng ảnh
            string[] imageExtensions = new string[] { ".jpg", ".jpeg", ".png", ".bmp", ".gif", ".tiff", ".webp" };

            // Lấy tất cả các tệp dạng ảnh trong thư mục
            var imageFiles = Directory.GetFiles(folderPath)
                                      .Where(file => imageExtensions.Contains(Path.GetExtension(file).ToLower()))
                                      .ToList();

            foreach (var file in imageFiles)
            {
                usr_AnhMini usr_AnhMini = new usr_AnhMini(file, Path.GetFileName(file), function.GetLastModified(file));
                usr_AnhMini.ControlClicked += flpDSAnh_Click;
                flpDSAnh.Controls.Add(usr_AnhMini);
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
            GetImageInFolder(txtTimKiem.Text);
        }

        private void flpDSAnh_Click(object sender, EventArgs e)
        {
            if (sender is usr_AnhMini clickedControl)
            {
                pbAnhDaChon.SizeMode = PictureBoxSizeMode.Zoom;
                pbAnhDaChon.Load(clickedControl.linkanh);
                CheckInfoImage(clickedControl.linkanh);
            }
        }

        private void flpDSAnh_Resize(object sender, EventArgs e)
        {
            foreach (UserControl control in flpDSAnh.Controls)
            {
                control.Width = flpDSAnh.Width;
            }
        }
    }
}
