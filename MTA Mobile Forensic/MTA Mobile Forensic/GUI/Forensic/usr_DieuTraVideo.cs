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
    public partial class usr_DieuTraVideo : UserControl
    {
        int setup1 = 0;
        int setup2 = 0;
        string tab1_pathImage_dachon = "";
        string tab2_pathVideo_dachon = "";
        string tab1_pathFolder_dachon = "";
        private List<string> tab1_videoFiles = new List<string>();
        private List<string> tab2_videoFiles = new List<string>();
        private List<string> tab1_response = new List<string>();
        private List<string> tab2_response = new List<string>();
        private int tab1_sttVideo = -1;
        function function = new function();
        api api = new api();

        public usr_DieuTraVideo()
        {
            InitializeComponent();
            tabControl1.SelectedTabIndex = 0;
        }

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
                    splitContainer2.SplitterDistance = (int)(panelEx1.Width / 2.5);
                }
            }
            catch { }
        }

        private void tab1_btnChonImage_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image files (*.jpg;*.jpeg;*.png;*.bmp)|*.jpg;*.jpeg;*.png;*.bmp|All files (*.*)|*.*";
                openFileDialog.Title = "Chọn file ảnh";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    tab1_txtPathImage.Text = openFileDialog.FileName;
                    tab1_Image.SizeMode = PictureBoxSizeMode.Zoom;
                    tab1_Image.Image = System.Drawing.Image.FromFile(openFileDialog.FileName);
                    tab1_pathImage_dachon = openFileDialog.FileName;
                    for (int i = 0; i < tab1_videoFiles.Count; i++)
                    {
                        tab1_response.Add($"Item");
                    }
                }
            }
        }

        private void GetVideoInFolder(string folderPath)
        {
            if (tab1_videoFiles != null)
            {
                tab1_videoFiles.Clear();
            }

            if (folderPath != string.Empty)
            {
                try
                {
                    string[] videoExtensions = new string[] { ".mp4", ".avi", ".mov", ".wmv", ".flv", ".mkv", ".webm", ".m4v", ".mpeg", ".mpg" };

                    tab1_videoFiles = Directory.GetFiles(folderPath, "*.*", SearchOption.AllDirectories)
                                       .Where(file => videoExtensions.Contains(Path.GetExtension(file).ToLower()))
                                       .ToList();

                    for (int i = 0; i < tab1_videoFiles.Count; i++)
                    {
                        try
                        {
                            usr_VideoMini usr_VideoMini = new usr_VideoMini(tab1_videoFiles[i], Path.GetFileName(tab1_videoFiles[i]), function.GetLastModified(tab1_videoFiles[i]));
                            usr_VideoMini.ControlClicked += tab1_flpDSVideo_Click;
                            tab1_flpDSVideo.Controls.Add(usr_VideoMini);
                        }
                        catch { }
                    }
                }
                catch (Exception ex)
                {
                    frm_Notification frm_Notification = new frm_Notification("error", ex.ToString());
                    frm_Notification.ShowDialog();
                }
            }
        }

        private void tab1_btnChonFolder_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
            {
                DialogResult result = folderBrowserDialog.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(folderBrowserDialog.SelectedPath))
                {
                    tab1_flpDSVideo.Controls.Clear();
                    string selectedFolder = folderBrowserDialog.SelectedPath;
                    tab1_txtPathFolder.Text = selectedFolder;
                    tab1_pathFolder_dachon = selectedFolder;
                    GetVideoInFolder(selectedFolder);
                    for (int i = 0; i < tab1_videoFiles.Count; i++)
                    {
                        tab1_response.Add($"Item");
                    }
                }
            }
        }

        private void LoadVideo(string path)
        {
            tab1_axWindowsMediaPlayer1.URL = path;
            tab1_axWindowsMediaPlayer1.Ctlcontrols.stop();
        }

        private async Task TimKiemKhuonMatVideo(string path, List<string> listPath)
        {
            List<string> results = new List<string>();
            int batchSize = 10;

            for (int i = 0; i < listPath.Count; i += batchSize)
            {
                List<string> batch = listPath.Skip(i).Take(batchSize).ToList();
                string[] batchArray = batch.ToArray();

                var response = await api.TimKiemKhuonMatVideo(path, batchArray);

                if (response != null)
                {
                    results.AddRange(response);

                    for (int j = 0; j < batch.Count; j++)
                    {
                        tab1_response[tab1_sttVideo] = response[j];
                        if (response[j] != "false")
                        {
                            tab1_txtThongTinTimDuoc.ForeColor = Color.Green;
                            tab1_txtThongTinTimDuoc.Text = response[j].Replace("\n", Environment.NewLine);
                        }
                        else
                        {
                            tab1_txtThongTinTimDuoc.ForeColor = Color.Red;
                            tab1_txtThongTinTimDuoc.Text = "Không xuất hiện khuôn mặt trong video";
                        }
                    }
                    tab1_ImageLoad.Visible = false;
                }
                else
                {
                    MessageBox.Show("Failed to get a response from the API.");
                }
            }
        }

        private void tab1_flpDSVideo_Click(object sender, EventArgs e)
        {
            if (sender is usr_VideoMini clickedControl)
            {
                var controls = tab1_flpDSVideo.Controls;

                int index = controls.IndexOf(clickedControl);
                tab1_sttVideo = index;
                LoadVideo(clickedControl.linkvideo);
                tab1_txtPathVideo.Text = clickedControl.linkvideo;
            }
        }

        private async void tab1_btnTimKiemKhuonMat_Click(object sender, EventArgs e)
        {
            if (tab1_txtPathImage.Text != string.Empty || tab1_txtPathVideo.Text != string.Empty)
            {
                if (tab1_response[tab1_sttVideo] != "Item")
                {
                    if (tab1_response[tab1_sttVideo] != "false")
                    {
                        tab1_txtThongTinTimDuoc.ForeColor = Color.Green;
                        tab1_txtThongTinTimDuoc.Text = tab1_response[tab1_sttVideo].Replace("\n", Environment.NewLine);
                    }
                    else
                    {
                        tab1_txtThongTinTimDuoc.ForeColor = Color.Red;
                        tab1_txtThongTinTimDuoc.Text = "Không xuất hiện khuôn mặt trong video";
                    }
                }
                else
                {
                    tab1_txtThongTinTimDuoc.Text = string.Empty;
                    tab1_ImageLoad.Visible = true;
                    List<string> pathVideos = new List<string>();
                    pathVideos.Add(tab1_txtPathVideo.Text);
                    await TimKiemKhuonMatVideo(tab1_pathImage_dachon, pathVideos);
                }
            }
            else
            {
                MessageBox.Show("Chưa chọn ảnh hoặc video!");
            }
        }

        private void tab2_GetVideoInFolder(string folderPath)
        {
            tab2_videoFiles = new List<string>();

            if (folderPath != string.Empty)
            {
                try
                {
                    string[] videoExtensions = new string[] { ".mp4", ".avi", ".mov", ".wmv", ".flv", ".mkv", ".webm", ".m4v", ".mpeg", ".mpg" };

                    tab2_videoFiles = Directory.GetFiles(folderPath, "*.*", SearchOption.AllDirectories)
                                       .Where(file => videoExtensions.Contains(Path.GetExtension(file).ToLower()))
                                       .ToList();

                    for (int i = 0; i < tab2_videoFiles.Count; i++)
                    {
                        try
                        {
                            usr_VideoMini usr_VideoMini = new usr_VideoMini(tab2_videoFiles[i], Path.GetFileName(tab2_videoFiles[i]), function.GetLastModified(tab2_videoFiles[i]));
                            usr_VideoMini.ControlClicked += tab2_flpDSVideo_Click;
                            tab2_flpDSVideo.Controls.Add(usr_VideoMini);
                        }
                        catch { }
                    }
                }
                catch (Exception ex)
                {
                    frm_Notification frm_Notification = new frm_Notification("error", ex.ToString());
                    frm_Notification.ShowDialog();
                }
            }
        }

        private void tab2_btnChonFolder_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
            {
                folderBrowserDialog.Description = "Chọn thư mục chứa các video";
                DialogResult result = folderBrowserDialog.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(folderBrowserDialog.SelectedPath))
                {
                    tab2_flpDSVideo.Controls.Clear();
                    string selectedFolder = folderBrowserDialog.SelectedPath;
                    tab2_txtPathFolderVideo.Text = selectedFolder;
                    tab2_GetVideoInFolder(selectedFolder);
                }
            }
        }

        private void LoadVideo2(string path)
        {
            tab2_axWindowsMediaPlayer1.URL = path;
            tab2_axWindowsMediaPlayer1.Ctlcontrols.stop();
        }

        private async Task TrichXuatKhuonMatTuVideo(string path, string outputdirectory)
        {
            var response = await api.TrichXuatKhuonMatVideo(path, outputdirectory);

            if (response != null)
            {
                tab2_response = response;
                if (tab2_response.Count > 0)
                {
                    for (int i = 0; i < tab2_response.Count; i++)
                    {
                        try
                        {
                            usr_AnhMini usr_AnhMini = new usr_AnhMini(tab2_response[i], Path.GetFileName(tab2_response[i]), function.GetLastModified(tab2_response[i]));
                            usr_AnhMini.ControlClicked += tab2_flpDSFace_Click;
                            tab2_flpDSFace.Controls.Add(usr_AnhMini);
                        }
                        catch { }
                    }
                }
                else
                {
                    Label myLabel = new Label();
                    myLabel.Text = "Không tìm thấy khuôn mặt nào";
                    myLabel.AutoSize = true;
                    myLabel.Font = new Font("Arial", 9);
                    myLabel.ForeColor = Color.Red;
                    tab2_flpDSFace.Controls.Add(myLabel);
                }
            }
            else
            {
                MessageBox.Show("Failed to get a response from the API.");
            }
            tab2_ImageLoad.Visible = false;
        }

        public string RemoveFileExtension(string filePath)
        {
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(filePath);
            string directoryPath = Path.GetDirectoryName(filePath);
            return Path.Combine(directoryPath, fileNameWithoutExtension);
        }

        private void tab2_flpDSFace_Click(object sender, EventArgs e)
        {
            if (sender is usr_AnhMini clickedControl)
            {
                frm_XemAnh frm_XemAnh = new frm_XemAnh(clickedControl.linkanh);
                frm_XemAnh.ShowDialog();
            }
        }

        private void tab2_flpDSVideo_Click(object sender, EventArgs e)
        {
            if (sender is usr_VideoMini clickedControl)
            {
                tab2_txtPathVideo.Text = clickedControl.linkvideo;
                tab2_pathVideo_dachon = clickedControl.linkvideo;
                LoadVideo2(clickedControl.linkvideo);
            }
        }

        private async void tab2_TrichXuatKhuonMat_Click(object sender, EventArgs e)
        {
            if (tab2_pathVideo_dachon != string.Empty)
            {
                tab2_ImageLoad.Visible = true;
                tab2_flpDSFace.Controls.Clear();
                await TrichXuatKhuonMatTuVideo(tab2_pathVideo_dachon, RemoveFileExtension(tab2_pathVideo_dachon));
            }
        }

        private void tab2_btnChonTatCa_Click(object sender, EventArgs e)
        {
            foreach (usr_AnhMini control in tab2_flpDSFace.Controls)
            {
                control.checkBox.Checked = true;
            }
        }

        private void tab2_btnBoChonTatCa_Click(object sender, EventArgs e)
        {
            foreach (usr_AnhMini control in tab2_flpDSFace.Controls)
            {
                control.checkBox.Checked = false;
            }
        }

        private void MoveImage(string imagePath, string pathFolderNew)
        {
            if (!Directory.Exists(pathFolderNew))
            {
                Directory.CreateDirectory(pathFolderNew);
            }
            try
            {
                // Lấy tên file ảnh từ đường dẫn gốc
                string fileName = Path.GetFileName(imagePath);

                // Đường dẫn đích mới cho ảnh
                string newFilePath = Path.Combine(pathFolderNew, fileName);

                // Di chuyển ảnh từ vị trí cũ đến thư mục mới
                File.Copy(imagePath, newFilePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi di chuyển ảnh {imagePath}: {ex.Message}");
            }
        }

        private void tab2_btnLuuAnh_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
            {
                folderBrowserDialog.Description = "Chọn thư mục lưu ảnh";
                DialogResult result = folderBrowserDialog.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(folderBrowserDialog.SelectedPath))
                {
                    string selectedFolder = folderBrowserDialog.SelectedPath;
                    int dem = 0;
                    foreach (usr_AnhMini control in tab2_flpDSFace.Controls)
                    {
                        if (control.checkBox.Checked)
                        {
                            int index = tab2_flpDSFace.Controls.IndexOf(control);
                            MoveImage(tab2_response[index], selectedFolder);
                            dem++;
                        }
                    }
                    if (dem > 0)
                    {
                        MessageBox.Show("Lưu ảnh thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Chưa chọn ảnh", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }

                }
            }
        }
    }
}