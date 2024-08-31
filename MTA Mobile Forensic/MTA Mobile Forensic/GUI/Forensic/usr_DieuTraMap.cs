using MTA_Mobile_Forensic.GUI.Share;
using MTA_Mobile_Forensic.Support;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace MTA_Mobile_Forensic.GUI.Forensic
{
    public partial class usr_DieuTraMap : UserControl
    {
        string query = "";
        string fullFilePath = "";
        function function = new function();
        exiftool exiftool = new exiftool();
        string[] imageExtensions = { ".jpg", ".jpeg", ".png", ".bmp", ".gif" };
        string[] videoExtensions = { ".mp4", ".avi", ".mov", ".wmv", ".flv" };
        private List<string> image_videoFiles;

        public usr_DieuTraMap()
        {
            InitializeComponent();

            flpChonAnh.DragEnter += Flp_DragEnter;
            flpAnhDaChon.DragEnter += Flp_DragEnter;

            flpChonAnh.DragDrop += Flp_DragDrop;
            flpAnhDaChon.DragDrop += Flp_DragDrop;
        }

        private void panelEx1_Resize(object sender, EventArgs e)
        {
            try
            {
                splitContainer1.SplitterDistance = (int)(3.25 * panelEx1.Width / 10);
                splitContainer2.SplitterDistance = (int)(1 * panelEx1.Width / 10);
            }
            catch { }
        }

        private void pictureBox1_MouseEnter(object sender, EventArgs e)
        {
            if (sender is PictureBox picture)
            {
                picture.BackColor = Color.Gold;
            }
        }

        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
            if (sender is PictureBox picture)
            {
                picture.BackColor = Color.Transparent;
            }
        }

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

        private void btnChonFile_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
            {
                DialogResult result = folderBrowserDialog.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(folderBrowserDialog.SelectedPath))
                {
                    string selectedFolder = folderBrowserDialog.SelectedPath;
                    txtPathChoiseFile.Text = selectedFolder;
                }
            }
        }

        private void GetImage_VideoInFolder(string folderPath)
        {
            if (folderPath != string.Empty)
            {
                try
                {
                    // Các đuôi file dạng ảnh
                    string[] image_videoExtensions = new string[] { ".jpg", ".jpeg", ".png", ".bmp", ".gif", ".tiff", ".webp", ".mp4", ".avi", ".mov", ".wmv", ".flv", ".mkv", ".webm", ".m4v" };

                    // Lấy tất cả các tệp dạng ảnh trong thư mục
                    image_videoFiles = Directory.GetFiles(folderPath)
                                             .Where(file => image_videoExtensions.Contains(Path.GetExtension(file).ToLower()))
                                             .ToList();
                    Add_To_flpChonAnh();
                }
                catch (Exception ex)
                {
                    frm_Notification frm_Notification = new frm_Notification("error", ex.ToString());
                    frm_Notification.ShowDialog();
                }
            }
        }

        private void Add_To_flpChonAnh()
        {
            flpChonAnh.Controls.Clear();

            foreach (var item in image_videoFiles)
            {
                if (imageExtensions.Contains(Path.GetExtension(item)))
                {
                    usr_AnhMini usr_AnhMini = new usr_AnhMini(item, Path.GetFileName(item), function.GetLastModified(item));
                    usr_AnhMini.ControlClicked += flpAnhDaChon_Click;
                    //usr_AnhMini.checkBox.Visible = false;
                    flpChonAnh.Controls.Add(usr_AnhMini);
                }
                else if (videoExtensions.Contains(Path.GetExtension(item)))
                {
                    usr_VideoMini usr_VideoMini = new usr_VideoMini(item, Path.GetFileName(item), function.GetLastModified(item));
                    usr_VideoMini.ControlClicked += flpAnhDaChon_Click;
                    //usr_VideoMini.checkBox.Visible = false;
                    flpChonAnh.Controls.Add(usr_VideoMini);
                }
            }
        }

        private void txtPathChoiseFile_TextChanged(object sender, EventArgs e)
        {
            GetImage_VideoInFolder(txtPathChoiseFile.Text);
        }

        private void Flp_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(usr_AnhMini)) || e.Data.GetDataPresent(typeof(usr_VideoMini)))
            {
                e.Effect = DragDropEffects.Move;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void Flp_DragDrop(object sender, DragEventArgs e)
        {
            FlowLayoutPanel targetPanel = (FlowLayoutPanel)sender;
            Control draggedControl = null;

            if (e.Data.GetDataPresent(typeof(usr_AnhMini)))
            {
                draggedControl = (usr_AnhMini)e.Data.GetData(typeof(usr_AnhMini));
            }
            else if (e.Data.GetDataPresent(typeof(usr_VideoMini)))
            {
                draggedControl = (usr_VideoMini)e.Data.GetData(typeof(usr_VideoMini));
            }

            if (draggedControl != null)
            {
                // Remove from old panel
                FlowLayoutPanel oldPanel = (FlowLayoutPanel)draggedControl.Parent;
                oldPanel.Controls.Remove(draggedControl);

                // Add to new panel
                targetPanel.Controls.Add(draggedControl);
            }
        }

        private void flpAnhDaChon_Click(object sender, EventArgs e)
        {
            if (sender is usr_AnhMini anhMini)
            {
                frm_XemAnh frm_XemAnh = new frm_XemAnh(anhMini.linkanh);
                frm_XemAnh.Show();
            }
            else if (sender is usr_VideoMini videoMini)
            {
                frm_XemVideo frm_XemVideo = new frm_XemVideo(videoMini.linkvideo);
                frm_XemVideo.Show();
            }
        }

        private void btnChonTatCa_Click(object sender, EventArgs e)
        {
            foreach (UserControl control in flpAnhDaChon.Controls)
            {
                if (control is usr_AnhMini anhMini)
                {
                    anhMini.checkBox.Checked = true;
                }
                else if (control is usr_VideoMini videoMini)
                {
                    videoMini.checkBox.Checked = true;
                }
            }
        }

        private void btnBoChonTatCa_Click(object sender, EventArgs e)
        {
            foreach (UserControl control in flpAnhDaChon.Controls)
            {
                if (control is usr_AnhMini anhMini)
                {
                    anhMini.checkBox.Checked = false;
                }
                else if (control is usr_VideoMini videoMini)
                {
                    videoMini.checkBox.Checked = false;
                }
            }
        }

        private void flpAnhDaChon_ControlAdded(object sender, ControlEventArgs e)
        {
            lblLink.Visible = false;
            lblLink.Text = string.Empty;
        }

        private void flpAnhDaChon_ControlRemoved(object sender, ControlEventArgs e)
        {
            lblLink.Visible = false;
            lblLink.Text = string.Empty;
        }

        private string CheckInfoImage_Video(string pathImage)
        {
            query = pathImage;
            string str = exiftool.exiftoolCommand(query);

            string location = function.GetGPSPositionNoLinkToText(str);
            if (location != "Error")
            {
                return location;
            }
            else
            {
                return string.Empty;
            }
        }

        public void CreateKmlFile(string filePath, string content)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    writer.Write(content);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Có lỗi xảy ra khi tạo file .kml: " + ex.Message);
            }
        }

        private void btnXuatFile_Click(object sender, EventArgs e)
        {
            int index_checkBox = 0;
            string kmlContent = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n<kml xmlns=\"http://www.opengis.net/kml/2.2\">\r\n  <Document>\r\n    <name>My Locations</name>\r\n    <description>Danh sách các địa điểm</description>\r\n    <Style id=\"iconStyle\">\r\n      <IconStyle>\r\n        <Icon>\r\n          <href>https://maps.google.com/mapfiles/kml/paddle/blu-circle.png</href>\r\n        </Icon>\r\n      </IconStyle>\r\n    </Style>\r\n";
            foreach (UserControl control in flpAnhDaChon.Controls)
            {
                if (control is usr_AnhMini anhMini)
                {
                    if (anhMini.checkBox.Checked == true)
                    {
                        string tenfile = Path.GetFileName(anhMini.linkanh);
                        string location = CheckInfoImage_Video(anhMini.linkanh);
                        if (location != string.Empty)
                        {
                            kmlContent += $"<!-- {tenfile} -->\r\n    <Placemark>\r\n      <name>{tenfile}</name>\r\n      <styleUrl>#iconStyle</styleUrl>\r\n      <Point>\r\n        <coordinates>{location}</coordinates>\r\n      </Point>\r\n    </Placemark>";
                            index_checkBox++;
                        }
                    }
                }
                else if (control is usr_VideoMini videoMini)
                {
                    if (videoMini.checkBox.Checked == true)
                    {
                        string tenfile = Path.GetFileName(videoMini.linkvideo);
                        string location = CheckInfoImage_Video(videoMini.linkvideo);
                        if (location != string.Empty)
                        {
                            kmlContent += $"<!-- {tenfile} -->\r\n    <Placemark>\r\n      <name>{tenfile}</name>\r\n      <styleUrl>#iconStyle</styleUrl>\r\n      <Point>\r\n        <coordinates>{location}</coordinates>\r\n      </Point>\r\n    </Placemark>";
                            index_checkBox++;
                        }
                    }
                }
            }
            kmlContent += "\r\n</Document>\r\n</kml>";
            if (index_checkBox == 0)
            {
                MessageBox.Show("Chưa chọn dữ liệu để xuất file", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                string sourceDirectory = AppDomain.CurrentDomain.BaseDirectory;
                string projectDirectory = Directory.GetParent(sourceDirectory).Parent.Parent.FullName;
                string filePath = Path.Combine(projectDirectory, "Cache");
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                DateTime dateTime = DateTime.Now;
                string fileName = "Location_export_" + dateTime.ToString("HH.mm.ss_dd.MM.yyyy") + ".kml";
                fullFilePath = Path.Combine(filePath, fileName);
                CreateKmlFile(fullFilePath, kmlContent);
                lblLink.Text = fileName;
                lblLink.Visible = true;

                using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
                {
                    folderBrowserDialog.Description = "Chọn thư mục lưu dữ liệu thu được";
                    DialogResult result = folderBrowserDialog.ShowDialog();
                    if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(folderBrowserDialog.SelectedPath))
                    {
                        string outputPath = folderBrowserDialog.SelectedPath;
                        CopyFile(fullFilePath, outputPath);
                        MessageBox.Show($"Lưu thành công file!");
                    }
                }
            }
        }

        private void lblLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                string filePath = fullFilePath;

                if (File.Exists(filePath))
                {
                    // Mở thư mục và trỏ vào file
                    string argument = "/select, \"" + filePath + "\"";
                    Process.Start("explorer.exe", argument);
                }
                else
                {
                    MessageBox.Show("File không tồn tại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Có lỗi xảy ra: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CopyFile(string sourceFilePath, string destinationDirectory)
        {
            try
            {
                // Kiểm tra thư mục đích và tạo thư mục nếu chưa tồn tại
                if (!Directory.Exists(destinationDirectory))
                {
                    Directory.CreateDirectory(destinationDirectory);
                }

                // Lấy tên file từ đường dẫn nguồn
                string fileName = Path.GetFileName(sourceFilePath);
                string destinationFilePath = Path.Combine(destinationDirectory, fileName);

                // Sao chép file từ nguồn đến đích
                File.Copy(sourceFilePath, destinationFilePath, true); // Tham số 'true' để ghi đè nếu file đã tồn tại
            }
            catch (Exception ex)
            {
                MessageBox.Show("Có lỗi xảy ra khi sao chép file: " + ex.Message);
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            LoadWeb("https://earth.google.com/web/");
        }

        private void btnOpenBigForm_Click(object sender, EventArgs e)
        {
            try
            {
                if (webView21.Source.ToString() != "about:blank" || webView21.Source.ToString() != string.Empty)
                {
                    frm_OpenWeb frm_OpenWeb = new frm_OpenWeb(webView21.Source.ToString());
                    frm_OpenWeb.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Trang web hiện tại trống", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch { }
        }
    }
}