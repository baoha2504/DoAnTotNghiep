using MTA_Mobile_Forensic.GUI.Share;
using MTA_Mobile_Forensic.Support;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;

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
        string linkanhdachon = "";

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
            txtThongTinAnh.Text = str;

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

        private void GetImageInFolder(string folderPath)
        {
            try
            {
                // Các đuôi file dạng ảnh
                string[] imageExtensions = new string[] { ".jpg", ".jpeg", ".png", ".bmp", ".gif", ".tiff", ".webp" };

                // Lấy tất cả các tệp dạng ảnh trong thư mục
                var imageFiles = Directory.GetFiles(folderPath)
                                          .Where(file => imageExtensions.Contains(Path.GetExtension(file).ToLower()))
                                          .ToList();

                // Nếu cbbTuyChon.Text không phải là "Trong thư mục", lấy thêm các tệp ảnh trong các thư mục con
                if (cbbTuyChon.Text != "Trong thư mục")
                {
                    var subDirectories = Directory.GetDirectories(folderPath);
                    foreach (var subDir in subDirectories)
                    {
                        var subDirFiles = Directory.GetFiles(subDir)
                                                   .Where(file => imageExtensions.Contains(Path.GetExtension(file).ToLower()))
                                                   .ToList();
                        imageFiles.AddRange(subDirFiles);
                    }
                }

                // Clear và add các file ảnh vào flpDSAnh
                flpDSAnh.Controls.Clear();
                foreach (var file in imageFiles)
                {
                    usr_AnhMini usr_AnhMini = new usr_AnhMini(file, Path.GetFileName(file), function.GetLastModified(file));
                    usr_AnhMini.ControlClicked += flpDSAnh_Click;
                    flpDSAnh.Controls.Add(usr_AnhMini);
                }
            }
            catch (Exception ex)
            {
                frm_Notification frm_Notification = new frm_Notification("success", ex.ToString());
                frm_Notification.ShowDialog();
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
                linkanhdachon = clickedControl.linkanh;

            }
        }

        private void flpDSAnh_Resize(object sender, EventArgs e)
        {
            foreach (UserControl control in flpDSAnh.Controls)
            {
                control.Width = flpDSAnh.Width;
            }
        }

        private void pbAnhDaChon_DoubleClick(object sender, EventArgs e)
        {
            frm_XemAnh frm_XemAnh = new frm_XemAnh(linkanhdachon);
            frm_XemAnh.Show();
        }

        private void btnXuat_Click(object sender, EventArgs e)
        {

        }

        private void btnNhap_Click(object sender, EventArgs e)
        {

        }

        private void btnChonTatCa_Click(object sender, EventArgs e)
        {
            foreach (usr_AnhMini control in flpDSAnh.Controls)
            {
                control.checkBox.Checked = true;
            }
        }

        private void btnBoChonTatCa_Click(object sender, EventArgs e)
        {
            foreach (usr_AnhMini control in flpDSAnh.Controls)
            {
                control.checkBox.Checked = false;
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            txtThongTinAnh.Text = string.Empty;
            ClearWebView();
            GetImageInFolder(txtTimKiem.Text);
            pbAnhDaChon.Image = null;

        }

        private void cbbTuyChon_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetImageInFolder(txtTimKiem.Text);
        }
    }
}
