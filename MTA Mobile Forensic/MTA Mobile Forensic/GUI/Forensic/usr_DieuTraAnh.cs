using MTA_Mobile_Forensic.GUI.Share;
using MTA_Mobile_Forensic.Support;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MTA_Mobile_Forensic.GUI.Forensic
{
    public partial class usr_DieuTraAnh : UserControl
    {
        public usr_DieuTraAnh()
        {
            InitializeComponent();
        }

        int setup1 = 0;
        int setup2 = 0;
        int setup3 = 0;
        api api = new api();
        function function = new function();
        private List<string> imageFiles;
        private List<string> imageFiles_TamThoi;
        private List<string> imageFiles_NhieuKhuonMat;
        private List<string> imageFiles_NhieuKhuonMat_TamThoi;
        private List<string> pathsTimKiem;
        private List<string> pathsTimKiemNhieuKhuonMat;
        private List<string> pathsTrichXuat;

        private void tabControl1_SelectedTabChanged(object sender, DevComponents.DotNetBar.TabStripTabChangedEventArgs e)
        {
            try
            {
                int selectedIndex = tabControl1.SelectedTabIndex;
                if (selectedIndex == 0 && setup1 == 0)
                {
                    setup1 = 1;
                    splitContainer1.SplitterDistance = (int)(panelEx1.Width / 2.5);
                }
                else if (selectedIndex == 1 && setup2 == 0)
                {
                    setup2 = 1;
                    splitContainer3.SplitterDistance = (int)(panelEx1.Width / 2.5);
                    splitContainer4.SplitterDistance = (int)((panel18.Height - 35) / 2);
                }
                else if (selectedIndex == 2 && setup3 == 0)
                {
                    setup3 = 1;
                    splitContainer2.SplitterDistance = (int)(panelEx1.Width / 2.5);
                }
            }
            catch { }
        }

        private void btnChonAnhMau_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image files (*.jpg;*.jpeg;*.png;*.bmp)|*.jpg;*.jpeg;*.png;*.bmp|All files (*.*)|*.*";
                openFileDialog.Title = "Chọn file ảnh";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    txtPathAnhMau.Text = openFileDialog.FileName;
                    pictureBox_AnhMau.SizeMode = PictureBoxSizeMode.Zoom;
                    pictureBox_AnhMau.Image = Image.FromFile(openFileDialog.FileName);
                }
            }
        }

        private void btnChonFolderTimKiem_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
            {
                DialogResult result = folderBrowserDialog.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(folderBrowserDialog.SelectedPath))
                {
                    string selectedFolder = folderBrowserDialog.SelectedPath;
                    txtFolderDaChon.Text = selectedFolder;
                    GetImageInFolder(selectedFolder);
                }
            }
        }

        private void GetImageInFolder(string folderPath)
        {
            if (imageFiles != null)
            {
                imageFiles.Clear();
            }

            if (folderPath != string.Empty)
            {
                try
                {
                    // Các đuôi file dạng ảnh
                    string[] imageExtensions = new string[] { ".jpg", ".jpeg", ".png", ".bmp", ".gif", ".tiff", ".webp" };

                    // Lấy tất cả các tệp dạng ảnh trong thư mục
                    imageFiles = Directory.GetFiles(folderPath)
                                             .Where(file => imageExtensions.Contains(Path.GetExtension(file).ToLower()))
                                             .ToList();


                    var subDirectories = Directory.GetDirectories(folderPath);
                    foreach (var subDir in subDirectories)
                    {
                        var subDirFiles = Directory.GetFiles(subDir)
                                                   .Where(file => imageExtensions.Contains(Path.GetExtension(file).ToLower()))
                                                   .ToList();
                        imageFiles.AddRange(subDirFiles);
                    }

                    imageFiles_TamThoi = imageFiles;
                }
                catch (Exception ex)
                {
                    frm_Notification frm_Notification = new frm_Notification("error", ex.ToString());
                    frm_Notification.ShowDialog();
                }
            }
        }

        private async Task<List<string>> LayPathKhuonMatTimKiem(string path, List<string> listPath)
        {
            string[] listPathArray = listPath.ToArray();

            var response = await api.TimKiemKhuonMatAnh(path, listPathArray);

            if (response != null)
            {
                return response;
            }
            else
            {
                MessageBox.Show("Failed to get a response from the API.");
                return null;
            }
        }

        private async void btnTimKiemKhuonMat_Click(object sender, EventArgs e)
        {
            if (txtPathAnhMau.Text == string.Empty)
            {
                MessageBox.Show("Chưa chọn ảnh mẫu", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (txtFolderDaChon.Text == string.Empty)
            {
                MessageBox.Show("Chưa chọn thư mục tìm kiếm", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                flpAnhDaTimThay.Controls.Clear();
                pathsTimKiem = await LayPathKhuonMatTimKiem(txtPathAnhMau.Text, imageFiles);
                if (pathsTimKiem != null)
                {
                    for (int i = 0; i < pathsTimKiem.Count; i++)
                    {
                        if (pathsTimKiem[i] == "true")
                        {
                            usr_AnhMini usr_AnhMini = new usr_AnhMini(imageFiles[i], Path.GetFileName(imageFiles[i]), function.GetLastModified(imageFiles[i]));
                            usr_AnhMini.ControlClicked += flpAnhDaTimThay_Click;
                            usr_AnhMini.checkBox.Visible = false;
                            flpAnhDaTimThay.Controls.Add(usr_AnhMini);
                        }
                    }
                }
            }
        }

        private void flpAnhDaTimThay_Click(object sender, EventArgs e)
        {
            if (sender is usr_AnhMini clickedControl)
            {
                frm_XemAnh frm_XemAnh = new frm_XemAnh(clickedControl.linkanh);
                frm_XemAnh.Show();
            }
        }

        public void CopyFilesToDirectory(List<string> filePaths, string destinationDirectory)
        {
            try
            {
                if (!Directory.Exists(destinationDirectory))
                {
                    Directory.CreateDirectory(destinationDirectory);
                }

                foreach (string filePath in filePaths)
                {
                    if (File.Exists(filePath))
                    {
                        string fileName = Path.GetFileName(filePath);
                        string destFilePath = Path.Combine(destinationDirectory, fileName);

                        int count = 1;
                        while (File.Exists(destFilePath))
                        {
                            string tempFileName = $"{Path.GetFileNameWithoutExtension(fileName)}({count}){Path.GetExtension(fileName)}";
                            destFilePath = Path.Combine(destinationDirectory, tempFileName);
                            count++;
                        }

                        File.Copy(filePath, destFilePath);
                    }
                    else
                    {
                        MessageBox.Show($"File không tồn tại: {filePath}");
                    }
                }

                MessageBox.Show($"Lưu hoàn tất vào thư mục {destinationDirectory}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Có lỗi xảy ra: {ex.Message}");
            }
        }

        private void btnLuuAnhDaTimKiem_Click(object sender, EventArgs e)
        {
            if (flpAnhDaTimThay.Controls != null)
            {
                using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
                {
                    DialogResult result = folderBrowserDialog.ShowDialog();

                    if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(folderBrowserDialog.SelectedPath))
                    {
                        if (pathsTimKiem != null)
                        {
                            for (int i = pathsTimKiem.Count-1; i >= 0; i--)
                            {
                                if (pathsTimKiem[i] != "true")
                                {
                                    imageFiles_TamThoi.Remove(imageFiles_TamThoi[i]);
                                }
                            }
                        }

                        string selectedFolder = folderBrowserDialog.SelectedPath;
                        CopyFilesToDirectory(imageFiles_TamThoi, selectedFolder);
                    }
                }
            }
            else
            {
                MessageBox.Show("Không có ảnh để lưu", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnChonAnhTrichXuat_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image files (*.jpg;*.jpeg;*.png;*.bmp)|*.jpg;*.jpeg;*.png;*.bmp|All files (*.*)|*.*";
                openFileDialog.Title = "Chọn file ảnh";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    txtPathAnhTrichXuat.Text = openFileDialog.FileName;
                    pictureBox_TrichXuat.SizeMode = PictureBoxSizeMode.Zoom;
                    pictureBox_TrichXuat.Image = Image.FromFile(openFileDialog.FileName);
                }
            }
        }

        private async Task<List<string>> LayPathKhuonMatTrichXuat(string path)
        {
            var response = await api.TrichXuatKhuonMatAnh(path);

            if (response != null)
            {
                return response;
            }
            else
            {
                MessageBox.Show("Failed to get a response from the API.");
                return null;
            }
        }

        private async void btnTrichXuat_Click(object sender, EventArgs e)
        {
            if (txtPathAnhTrichXuat.Text == string.Empty)
            {
                MessageBox.Show("Chưa chọn ảnh để trích xuất", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                flpAnhDaTrichXuat.Controls.Clear();
                pathsTrichXuat = await LayPathKhuonMatTrichXuat(txtPathAnhTrichXuat.Text);
                if (pathsTrichXuat != null)
                {
                    for (int i = 0; i < pathsTrichXuat.Count; i++)
                    {
                        usr_AnhMini usr_AnhMini = new usr_AnhMini(pathsTrichXuat[i], Path.GetFileName(pathsTrichXuat[i]), function.GetLastModified(pathsTrichXuat[i]));
                        usr_AnhMini.ControlClicked += flpAnhDaTrichXuat_Click;
                        usr_AnhMini.checkBox.Visible = false;
                        flpAnhDaTrichXuat.Controls.Add(usr_AnhMini);
                    }
                }
            }
        }

        private void flpAnhDaTrichXuat_Click(object sender, EventArgs e)
        {
            if (sender is usr_AnhMini clickedControl)
            {
                frm_XemAnh frm_XemAnh = new frm_XemAnh(clickedControl.linkanh);
                frm_XemAnh.Show();
            }
        }

        private void btnLuuAnhDaTrichXuat_Click(object sender, EventArgs e)
        {
            if (flpAnhDaTrichXuat.Controls != null)
            {
                using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
                {
                    DialogResult result = folderBrowserDialog.ShowDialog();

                    if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(folderBrowserDialog.SelectedPath))
                    {
                        string selectedFolder = folderBrowserDialog.SelectedPath;
                        CopyFilesToDirectory(pathsTrichXuat, selectedFolder);
                    }
                }
            }
            else
            {
                MessageBox.Show("Không có ảnh để lưu", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnChonAnhMau1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image files (*.jpg;*.jpeg;*.png;*.bmp)|*.jpg;*.jpeg;*.png;*.bmp|All files (*.*)|*.*";
                openFileDialog.Title = "Chọn file ảnh";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    if (openFileDialog.FileName != txtPathAnhMau2.Text)
                    {
                        txtPathAnhMau1.Text = openFileDialog.FileName;
                        pictureBox_AnhMau1.SizeMode = PictureBoxSizeMode.Zoom;
                        pictureBox_AnhMau1.Image = Image.FromFile(openFileDialog.FileName);
                    }
                    else
                    {
                        MessageBox.Show("Không chọn ảnh giống nhau", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
        }

        private void btnChonAnhMau2_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image files (*.jpg;*.jpeg;*.png;*.bmp)|*.jpg;*.jpeg;*.png;*.bmp|All files (*.*)|*.*";
                openFileDialog.Title = "Chọn file ảnh";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    if (openFileDialog.FileName != txtPathAnhMau1.Text)
                    {
                        txtPathAnhMau2.Text = openFileDialog.FileName;
                        pictureBox_AnhMau2.SizeMode = PictureBoxSizeMode.Zoom;
                        pictureBox_AnhMau2.Image = Image.FromFile(openFileDialog.FileName);
                    }
                    else
                    {
                        MessageBox.Show("Không chọn ảnh giống nhau", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
        }

        private void GetImageInFolder_NhieuKhuonMat(string folderPath)
        {
            if (imageFiles_NhieuKhuonMat != null)
            {
                imageFiles_NhieuKhuonMat.Clear();
            }

            if (folderPath != string.Empty)
            {
                try
                {
                    // Các đuôi file dạng ảnh
                    string[] imageExtensions = new string[] { ".jpg", ".jpeg", ".png", ".bmp", ".gif", ".tiff", ".webp" };

                    // Lấy tất cả các tệp dạng ảnh trong thư mục
                    imageFiles_NhieuKhuonMat = Directory.GetFiles(folderPath)
                                             .Where(file => imageExtensions.Contains(Path.GetExtension(file).ToLower()))
                                             .ToList();


                    var subDirectories = Directory.GetDirectories(folderPath);
                    foreach (var subDir in subDirectories)
                    {
                        var subDirFiles = Directory.GetFiles(subDir)
                                                   .Where(file => imageExtensions.Contains(Path.GetExtension(file).ToLower()))
                                                   .ToList();
                        imageFiles_NhieuKhuonMat.AddRange(subDirFiles);
                    }
                    imageFiles_NhieuKhuonMat_TamThoi = imageFiles_NhieuKhuonMat;

                }
                catch (Exception ex)
                {
                    frm_Notification frm_Notification = new frm_Notification("error", ex.ToString());
                    frm_Notification.ShowDialog();
                }
            }
        }

        private void btnChonPathTimKiem_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
            {
                DialogResult result = folderBrowserDialog.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(folderBrowserDialog.SelectedPath))
                {
                    string selectedFolder = folderBrowserDialog.SelectedPath;
                    txtPathTimKiem.Text = selectedFolder;
                    GetImageInFolder_NhieuKhuonMat(selectedFolder);
                }
            }
        }

        private async Task<List<string>> LayPathNhieuKhuonMatTimKiem(string path1, string path2, List<string> listPath)
        {
            string[] listPathArray = listPath.ToArray();

            var response = await api.LayPathNhieuKhuonMatTimKiem(path1, path2, listPathArray);

            if (response != null)
            {
                return response;
            }
            else
            {
                MessageBox.Show("Failed to get a response from the API.");
                return null;
            }
        }

        private async void btnTimKiemNhieuKhuonMat_Click(object sender, EventArgs e)
        {
            if (txtPathAnhMau1.Text == string.Empty)
            {
                MessageBox.Show("Chưa chọn ảnh mẫu 1", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (txtPathAnhMau2.Text == string.Empty)
            {
                MessageBox.Show("Chưa chọn ảnh mẫu 2", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (txtPathTimKiem.Text == string.Empty)
            {
                MessageBox.Show("Chưa chọn thư mục tìm kiếm", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                flpAnhNhieuKhuonMat.Controls.Clear();
                pathsTimKiemNhieuKhuonMat = await LayPathNhieuKhuonMatTimKiem(txtPathAnhMau1.Text, txtPathAnhMau2.Text, imageFiles_NhieuKhuonMat);
                if (pathsTimKiemNhieuKhuonMat != null)
                {
                    for (int i = 0; i < pathsTimKiemNhieuKhuonMat.Count; i++)
                    {
                        if (pathsTimKiemNhieuKhuonMat[i] == "true")
                        {
                            usr_AnhMini usr_AnhMini = new usr_AnhMini(imageFiles_NhieuKhuonMat[i], Path.GetFileName(imageFiles_NhieuKhuonMat[i]), function.GetLastModified(imageFiles_NhieuKhuonMat[i]));
                            usr_AnhMini.ControlClicked += flpAnhNhieuKhuonMat_Click;
                            usr_AnhMini.checkBox.Visible = false;
                            flpAnhNhieuKhuonMat.Controls.Add(usr_AnhMini);
                        }
                    }
                }
            }
        }

        private void flpAnhNhieuKhuonMat_Click(object sender, EventArgs e)
        {
            if (sender is usr_AnhMini clickedControl)
            {
                frm_XemAnh frm_XemAnh = new frm_XemAnh(clickedControl.linkanh);
                frm_XemAnh.Show();
            }
        }

        private void btnLuuAnhNhieuKhuonMat_Click(object sender, EventArgs e)
        {
            if (flpAnhNhieuKhuonMat.Controls != null)
            {
                using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
                {
                    DialogResult result = folderBrowserDialog.ShowDialog();

                    if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(folderBrowserDialog.SelectedPath))
                    {
                        if (pathsTimKiemNhieuKhuonMat != null)
                        {
                            for (int i = pathsTimKiemNhieuKhuonMat.Count - 1; i >= 0; i--)
                            {
                                if (pathsTimKiemNhieuKhuonMat[i] != "true")
                                {
                                    imageFiles_NhieuKhuonMat_TamThoi.Remove(imageFiles_NhieuKhuonMat_TamThoi[i]);
                                }
                            }
                        }

                        string selectedFolder = folderBrowserDialog.SelectedPath;
                        CopyFilesToDirectory(imageFiles_NhieuKhuonMat_TamThoi, selectedFolder);
                    }
                }
            }
            else
            {
                MessageBox.Show("Không có ảnh để lưu", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
