using MTA_Mobile_Forensic.GUI.Share;
using MTA_Mobile_Forensic.Model;
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
    public partial class usr_DieuTraAudio : UserControl
    {
        public usr_DieuTraAudio()
        {
            InitializeComponent();
            tabControl1.SelectedTabIndex = 2;
            tab1_axWindowsMediaPlayer1.settings.volume = 100;
            tab1_axWindowsMediaPlayer2.settings.volume = 100;
            tab2_axWindowsMediaPlayer1.settings.volume = 100;
            tab2_axWindowsMediaPlayer2.settings.volume = 100;
            tab3_axWindowsMediaPlayer.settings.volume = 100;
        }

        int setup1 = 0;
        int setup2 = 0;
        int setup3 = 0;
        api api = new api();
        function function = new function();
        private List<string> audioFiles;
        private List<string> videoFiles;
        private List<string> tab3_audioFiles;
        private List<string> tab3_videoFiles;
        private List<string> tab1_response;
        private List<string> tab2_response;
        private List<AudioResponse> tab3_responses = new List<AudioResponse>();

        private void tabControl1_SelectedTabChanged(object sender, DevComponents.DotNetBar.TabStripTabChangedEventArgs e)
        {
            try
            {
                int selectedIndex = tabControl1.SelectedTabIndex;
                if (selectedIndex == 0 && setup1 == 0)
                {
                    setup1 = 1;
                    splitContainer1.SplitterDistance = (int)(panelEx1.Width / 2.5);
                    splitContainer4.SplitterDistance = (int)((panel18.Height - 35) / 2.5);
                    splitContainer2.SplitterDistance = (int)((panel18.Height + 20) / 2.5);
                }
                else if (selectedIndex == 1 && setup2 == 0)
                {
                    setup2 = 1;
                    splitContainer3.SplitterDistance = (int)(panelEx1.Width / 2.5);
                    splitContainer5.SplitterDistance = (int)((panel12.Height - 35) / 2.5);
                    splitContainer6.SplitterDistance = (int)((panel12.Height + 20) / 2.5);
                }
                else if (selectedIndex == 2 && setup3 == 0)
                {
                    setup3 = 1;
                    splitContainer7.SplitterDistance = (int)(panelEx1.Width / 2.5);
                    splitContainer9.SplitterDistance = (int)(panel65.Width / 2);
                }
            }
            catch { }
        }

        private List<string> GetSmallestSubfolders(string rootFolderPath)
        {
            List<string> result = new List<string>();

            foreach (string subfolder in Directory.GetDirectories(rootFolderPath, "*", SearchOption.AllDirectories))
            {
                if (Directory.GetDirectories(subfolder).Length == 0)
                {
                    result.Add(subfolder);
                }
            }
            return result;
        }

        #region================================  TAB1  ====================================================================
        private void tab1_btnChonAmThanhMau_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Audio files (*.mp3;*.wav;*.wma)|*.mp3;*.wav;*.wma|All files (*.*)|*.*";
                openFileDialog.Title = "Chọn file âm thanh";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    tab1_txtPathAmThanhMau.Text = openFileDialog.FileName;
                    tab1_axWindowsMediaPlayer1.URL = openFileDialog.FileName;
                }
            }
        }

        private void tab1_btnXoaPathAmThanhMau_Click(object sender, EventArgs e)
        {
            tab1_txtPathAmThanhMau.Text = string.Empty;
            tab1_axWindowsMediaPlayer1.URL = string.Empty;
        }

        private void tab1_GetAudioInFolder(string folderPath)
        {
            if (audioFiles != null)
            {
                audioFiles.Clear();
            }

            if (folderPath != string.Empty)
            {
                try
                {
                    // Các đuôi file dạng âm thanh
                    string[] audioExtensions = new string[] { ".mp3", ".wav", ".wma", ".aac", ".flac", ".ogg", ".m4a" };

                    // Lấy tất cả các tệp dạng âm thanh trong thư mục
                    audioFiles = Directory.GetFiles(folderPath)
                                          .Where(file => audioExtensions.Contains(Path.GetExtension(file).ToLower()))
                                          .ToList();

                    List<string> smallestSubfolders = GetSmallestSubfolders(folderPath);
                    foreach (var subfolder in smallestSubfolders)
                    {
                        var subDirFiles = Directory.GetFiles(subfolder)
                            .Where(file => audioExtensions.Contains(Path.GetExtension(file).ToLower()))
                            .ToList();
                        audioFiles.AddRange(subDirFiles);
                    }
                }
                catch (Exception ex)
                {
                    frm_Notification frm_Notification = new frm_Notification("error", ex.ToString());
                    frm_Notification.ShowDialog();
                }
            }
        }

        private async Task TimKiemSuTonTaiGiongNoi(string path, List<string> listPath)
        {
            List<string> results = new List<string>();
            int batchSize = 10;
            tab1_response = new List<string>();

            for (int i = 0; i < listPath.Count; i += batchSize)
            {
                List<string> batch = listPath.Skip(i).Take(batchSize).ToList();
                string[] batchArray = batch.ToArray();

                var response = await api.TimKiemGiongNoiAudio(path, batchArray);

                if (response != null)
                {
                    results.AddRange(response);
                    tab1_response.AddRange(response);

                    for (int j = 0; j < batch.Count; j++)
                    {
                        if (response[j] != "false")
                        {
                            int index = i + j;
                            usr_GhiAmMini usr_GhiAmMini = new usr_GhiAmMini(audioFiles[index], Path.GetFileName(audioFiles[index]), function.GetLastModified(audioFiles[index]));
                            usr_GhiAmMini.ControlClicked += tab1_flpAmThanhDaTimKiem_Click;
                            usr_GhiAmMini.checkBox.Visible = false;
                            tab1_flpAmThanhDaTimKiem.Controls.Add(usr_GhiAmMini);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Failed to get a response from the API.");
                }
            }
            if (tab1_flpAmThanhDaTimKiem.Controls.Count == 0)
            {
                Label noAudioLabel = new Label();
                noAudioLabel.Text = "Không tìm thấy âm thanh nào";
                noAudioLabel.AutoSize = true;
                noAudioLabel.ForeColor = Color.Red;
                noAudioLabel.Font = new Font(noAudioLabel.Font.FontFamily, 9);
                tab1_flpAmThanhDaTimKiem.Controls.Add(noAudioLabel);
                tab1_btnLuuAmThanh.Enabled = false;
            }
        }


        private void tab1_ChonThuMucAmThanh_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
            {
                DialogResult result = folderBrowserDialog.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(folderBrowserDialog.SelectedPath))
                {
                    string selectedFolder = folderBrowserDialog.SelectedPath;
                    tab1_txtPathFolderAmThanh.Text = selectedFolder;
                    tab1_GetAudioInFolder(selectedFolder);
                }
            }
        }

        private void tab1_flpAmThanhDaTimKiem_Click(object sender, EventArgs e)
        {
            if (sender is usr_GhiAmMini clickedControl)
            {
                tab1_axWindowsMediaPlayer2.URL = clickedControl.path;
                tab1_txtPathAmThanhDaChon.Text = clickedControl.path;

                try
                {
                    int index = audioFiles.IndexOf(clickedControl.path);
                    tab1_txtPhatHienAmThanh.Text = tab1_response[index].Replace("\n", Environment.NewLine);
                }
                catch { }
            }
        }

        private async void tab1_btnTimKiemAmThanh_Click(object sender, EventArgs e)
        {
            if (tab1_txtPathAmThanhMau.Text == string.Empty)
            {
                MessageBox.Show("Chưa chọn âm thanh mẫu", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (tab1_txtPathFolderAmThanh.Text == string.Empty)
            {
                MessageBox.Show("Chưa chọn thư mục tìm kiếm", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                tab1_flpAmThanhDaTimKiem.Controls.Clear();
                tab1_btnLuuAmThanh.Enabled = true;
                tab1_btnTimKiemAmThanh.Text = "Đang tìm kiếm ...";
                await TimKiemSuTonTaiGiongNoi(tab1_txtPathAmThanhMau.Text, audioFiles);
                tab1_btnTimKiemAmThanh.Text = "Tìm kiếm";
            }
        }

        private void tab1_btnXoaPathAmThanhDaChon_Click(object sender, EventArgs e)
        {
            tab1_axWindowsMediaPlayer2.URL = string.Empty;
            tab1_txtPathAmThanhDaChon.Text = string.Empty;
            tab1_txtPhatHienAmThanh.Text = string.Empty;
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

        private void tab1_btnLuuAmThanh_Click(object sender, EventArgs e)
        {
            if (tab1_flpAmThanhDaTimKiem.Controls != null)
            {
                using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
                {
                    List<string> audioFiles_TamThoi = new List<string>();
                    DialogResult result = folderBrowserDialog.ShowDialog();

                    if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(folderBrowserDialog.SelectedPath))
                    {
                        foreach (usr_GhiAmMini control in tab1_flpAmThanhDaTimKiem.Controls)
                        {
                            audioFiles_TamThoi.Add(control.path);
                        }
                        string selectedFolder = folderBrowserDialog.SelectedPath;
                        CopyFilesToDirectory(audioFiles_TamThoi, selectedFolder);
                    }
                }
            }
            else
            {
                MessageBox.Show("Không có ảnh để lưu", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        #endregion

        #region================================  TAB2  ====================================================================
        private void tab2_btnChonAmThanhMau_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Audio files (*.mp3;*.wav;*.wma)|*.mp3;*.wav;*.wma|All files (*.*)|*.*";
                openFileDialog.Title = "Chọn file âm thanh";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    tab2_txtPathAmThanhMau.Text = openFileDialog.FileName;
                    tab2_axWindowsMediaPlayer1.URL = openFileDialog.FileName;
                }
            }
        }

        private void tab2_btnXoaPathAmThanhMau_Click(object sender, EventArgs e)
        {
            tab2_txtPathAmThanhMau.Text = string.Empty;
            tab2_axWindowsMediaPlayer1.URL = string.Empty;
        }

        private void tab2_GetVideoInFolder(string folderPath)
        {
            if (videoFiles != null)
            {
                videoFiles.Clear();
            }

            if (folderPath != string.Empty)
            {
                try
                {
                    // Các đuôi file dạng video
                    string[] videoExtensions = new string[] { ".mp4", ".avi", ".mkv", ".mov", ".wmv", ".flv", ".webm", ".mpeg", ".mpg" };

                    // Lấy tất cả các tệp dạng video trong thư mục
                    videoFiles = Directory.GetFiles(folderPath)
                                          .Where(file => videoExtensions.Contains(Path.GetExtension(file).ToLower()))
                                          .ToList();


                    List<string> smallestSubfolders = GetSmallestSubfolders(folderPath);
                    foreach (var subfolder in smallestSubfolders)
                    {
                        var subDirFiles = Directory.GetFiles(subfolder)
                            .Where(file => videoExtensions.Contains(Path.GetExtension(file).ToLower()))
                            .ToList();
                        videoFiles.AddRange(subDirFiles);
                    }
                }
                catch (Exception ex)
                {
                    frm_Notification frm_Notification = new frm_Notification("error", ex.ToString());
                    frm_Notification.ShowDialog();
                }
            }
        }

        private async Task TimKiemSuTonTaiGiongNoi_Video(string path, List<string> listPath)
        {
            List<string> results = new List<string>();
            int batchSize = 10;
            tab2_response = new List<string>();

            for (int i = 0; i < listPath.Count; i += batchSize)
            {
                List<string> batch = listPath.Skip(i).Take(batchSize).ToList();
                string[] batchArray = batch.ToArray();

                var response = await api.TimKiemGiongNoiVideo(path, batchArray);

                if (response != null)
                {
                    results.AddRange(response);
                    tab2_response.AddRange(response);

                    for (int j = 0; j < batch.Count; j++)
                    {
                        if (response[j] != "false")
                        {
                            int index = i + j;
                            usr_VideoMini usr_VideoMini = new usr_VideoMini(videoFiles[index], Path.GetFileName(videoFiles[index]), function.GetLastModified(videoFiles[index]));
                            usr_VideoMini.ControlClicked += tab2_flpVideoDaTimKiem_Click;
                            usr_VideoMini.checkBox.Visible = false;
                            tab2_flpVideoDaTimKiem.Controls.Add(usr_VideoMini);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Failed to get a response from the API.");
                }
            }
            if (tab2_flpVideoDaTimKiem.Controls.Count == 0)
            {
                Label noAudioLabel = new Label();
                noAudioLabel.Text = "Không tìm thấy video nào";
                noAudioLabel.AutoSize = true;
                noAudioLabel.ForeColor = Color.Red;
                noAudioLabel.Font = new Font(noAudioLabel.Font.FontFamily, 9);
                tab2_flpVideoDaTimKiem.Controls.Add(noAudioLabel);
                tab2_btnLuuVideo.Enabled = false;
            }
        }

        private void tab2_ChonThuMucVideo_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
            {
                DialogResult result = folderBrowserDialog.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(folderBrowserDialog.SelectedPath))
                {
                    string selectedFolder = folderBrowserDialog.SelectedPath;
                    tab2_txtPathFolderVideo.Text = selectedFolder;
                    tab2_GetVideoInFolder(selectedFolder);
                }
            }
        }

        private async void tab2_btnTimKiemVideo_Click(object sender, EventArgs e)
        {
            if (tab2_txtPathAmThanhMau.Text == string.Empty)
            {
                MessageBox.Show("Chưa chọn âm thanh mẫu", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (tab2_txtPathFolderVideo.Text == string.Empty)
            {
                MessageBox.Show("Chưa chọn thư mục tìm kiếm", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                tab2_flpVideoDaTimKiem.Controls.Clear();
                tab2_btnLuuVideo.Enabled = true;
                tab2_btnTimKiemVideo.Text = "Đang tìm kiếm ...";
                await TimKiemSuTonTaiGiongNoi_Video(tab2_txtPathAmThanhMau.Text, videoFiles);
                tab2_btnTimKiemVideo.Text = "Tìm kiếm";
            }
        }

        private void tab2_flpVideoDaTimKiem_Click(object sender, EventArgs e)
        {
            if (sender is usr_VideoMini clickedControl)
            {
                tab2_axWindowsMediaPlayer2.URL = clickedControl.linkvideo;
                tab2_txtPathVideoDaChon.Text = clickedControl.linkvideo;

                try
                {
                    int index = videoFiles.IndexOf(clickedControl.linkvideo);
                    tab2_txtPhatHienVideo.Text = tab2_response[index].Replace("\n", Environment.NewLine);
                }
                catch { }
            }
        }

        private void tab2_btnXoaPathVideoDaChon_Click(object sender, EventArgs e)
        {
            tab2_axWindowsMediaPlayer2.URL = string.Empty;
            tab2_txtPathVideoDaChon.Text = string.Empty;
            tab2_txtPhatHienVideo.Text = string.Empty;
        }

        private void tab2_btnLuuVideo_Click(object sender, EventArgs e)
        {
            if (tab2_flpVideoDaTimKiem.Controls != null)
            {
                using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
                {
                    List<string> videoFiles_TamThoi = new List<string>();
                    DialogResult result = folderBrowserDialog.ShowDialog();

                    if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(folderBrowserDialog.SelectedPath))
                    {
                        foreach (usr_VideoMini control in tab2_flpVideoDaTimKiem.Controls)
                        {
                            videoFiles_TamThoi.Add(control.linkvideo);
                        }
                        string selectedFolder = folderBrowserDialog.SelectedPath;
                        CopyFilesToDirectory(videoFiles_TamThoi, selectedFolder);
                    }
                }
            }
            else
            {
                MessageBox.Show("Không có video để lưu", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        #endregion

        #region================================  TAB3  ====================================================================
        private void tab3_GetAudioInFolder(string folderPath)
        {
            if (tab3_audioFiles != null)
            {
                tab3_audioFiles.Clear();
            }

            if (folderPath != string.Empty)
            {
                try
                {
                    // Các đuôi file dạng âm thanh
                    string[] audioExtensions = new string[] { ".mp3", ".wav", ".wma", ".aac", ".flac", ".ogg", ".m4a" };

                    // Lấy tất cả các tệp dạng âm thanh trong thư mục
                    tab3_audioFiles = Directory.GetFiles(folderPath)
                                          .Where(file => audioExtensions.Contains(Path.GetExtension(file).ToLower()))
                                          .ToList();


                    List<string> smallestSubfolders = GetSmallestSubfolders(folderPath);
                    foreach (var subfolder in smallestSubfolders)
                    {
                        var subDirFiles = Directory.GetFiles(subfolder)
                            .Where(file => audioExtensions.Contains(Path.GetExtension(file).ToLower()))
                            .ToList();
                        tab3_audioFiles.AddRange(subDirFiles);
                    }
                }
                catch (Exception ex)
                {
                    frm_Notification frm_Notification = new frm_Notification("error", ex.ToString());
                    frm_Notification.ShowDialog();
                }
            }
        }


        private void tab3_GetVideoInFolder(string folderPath)
        {
            if (tab3_videoFiles != null)
            {
                tab3_videoFiles.Clear();
            }

            if (folderPath != string.Empty)
            {
                try
                {
                    // Các đuôi file dạng video
                    string[] videoExtensions = new string[] { ".mp4", ".avi", ".mkv", ".mov", ".wmv", ".flv", ".webm", ".mpeg", ".mpg" };

                    // Lấy tất cả các tệp dạng video trong thư mục
                    tab3_videoFiles = Directory.GetFiles(folderPath)
                                          .Where(file => videoExtensions.Contains(Path.GetExtension(file).ToLower()))
                                          .ToList();

                    var subDirectories = Directory.GetDirectories(folderPath);
                    foreach (var subDir in subDirectories)
                    {
                        var subDirFiles = Directory.GetFiles(subDir)
                                                   .Where(file => videoExtensions.Contains(Path.GetExtension(file).ToLower()))
                                                   .ToList();
                        tab3_videoFiles.AddRange(subDirFiles);
                    }
                }
                catch (Exception ex)
                {
                    frm_Notification frm_Notification = new frm_Notification("error", ex.ToString());
                    frm_Notification.ShowDialog();
                }
            }
        }

        private void tab3_btnChonPath_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
            {
                DialogResult result = folderBrowserDialog.ShowDialog();
                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(folderBrowserDialog.SelectedPath))
                {
                    string selectedFolder = folderBrowserDialog.SelectedPath;
                    tab3_flpFile.Controls.Clear();
                    tab3_GetAudioInFolder(selectedFolder);
                    tab3_GetVideoInFolder(selectedFolder);
                }
            }
            foreach (var item in tab3_audioFiles)
            {
                usr_GhiAmMini usr_GhiAmMini = new usr_GhiAmMini(item, Path.GetFileName(item), function.GetLastModified(item));
                usr_GhiAmMini.ControlClicked += tab3_flpFile_Click;
                usr_GhiAmMini.checkBox.Visible = false;
                tab3_flpFile.Controls.Add(usr_GhiAmMini);
            }

            foreach (var item in tab3_videoFiles)
            {
                usr_VideoMini usr_VideoMini = new usr_VideoMini(item, Path.GetFileName(item), function.GetLastModified(item));
                usr_VideoMini.ControlClicked += tab3_flpFile_Click;
                usr_VideoMini.checkBox.Visible = false;
                tab3_flpFile.Controls.Add(usr_VideoMini);
            }
        }

        private async Task<AudioResponse> TrichXuatThongTinTuAudio(string path)
        {
            try
            {
                var response = await api.TrichXuatThongTinAudio(path);

                if (response != null)
                {
                    tab3_responses.Add(response);
                    return response;
                }
                else
                {
                    MessageBox.Show("Failed to get a response from the API.");
                    return null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
                return null;
            }
        }

        private async Task<AudioResponse> TrichXuatThongTinAudioTrongVideo(string path)
        {
            try
            {
                var response = await api.TrichXuatThongTinAudioTrongVideo(path);

                if (response != null)
                {
                    tab3_responses.Add(response);
                    return response;
                }
                else
                {
                    MessageBox.Show("Failed to get a response from the API.");
                    return null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
                return null;
            }
        }


        private async void tab3_flpFile_Click(object sender, EventArgs e)
        {
            try
            {
                if (sender is usr_GhiAmMini ghiamMini)
                {
                    AudioResponse response = new AudioResponse();
                    tab3_txtTenFileDaChon.Text = ghiamMini.path;
                    tab3_axWindowsMediaPlayer.URL = ghiamMini.path;
                    int index = tab3_responses.FindIndex(m => m.path == ghiamMini.path);
                    if (index != -1)
                    {
                        tab3_txtCamXuc.Text = tab3_responses[index].sentiment;
                        tab3_txtAmThanhText.Text = tab3_responses[index].text;
                    }
                    else // chưa có trong những file đã tìm kiếm
                    {
                        tab3_pictureBoxLoad.Visible = true;
                        tab3_txtCamXuc.Text = string.Empty;
                        tab3_txtAmThanhText.Text = string.Empty;
                        response = await TrichXuatThongTinTuAudio(ghiamMini.path);
                        tab3_pictureBoxLoad.Visible = false;
                        tab3_txtCamXuc.Text = response.sentiment;
                        tab3_txtAmThanhText.Text = response.text;
                    }
                }
                else if (sender is usr_VideoMini videoMini)
                {
                    AudioResponse response = new AudioResponse();
                    tab3_txtTenFileDaChon.Text = videoMini.linkvideo;
                    tab3_axWindowsMediaPlayer.URL = videoMini.linkvideo;
                    int index = tab3_responses.FindIndex(m => m.path == videoMini.linkvideo);
                    if (index != -1)
                    {
                        tab3_txtCamXuc.Text = tab3_responses[index].sentiment;
                        tab3_txtAmThanhText.Text = tab3_responses[index].text;
                    }
                    else // chưa có trong những file đã tìm kiếm
                    {
                        tab3_pictureBoxLoad.Visible = true;
                        tab3_txtCamXuc.Text = string.Empty;
                        tab3_txtAmThanhText.Text = string.Empty;
                        response = await TrichXuatThongTinAudioTrongVideo(videoMini.linkvideo);
                        tab3_pictureBoxLoad.Visible = false;
                        tab3_txtCamXuc.Text = response.sentiment;
                        tab3_txtAmThanhText.Text = response.text;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.ToString()}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void tab3_btnBoChonFile_Click(object sender, EventArgs e)
        {
            tab3_txtTenFileDaChon.Text = string.Empty;
            tab3_axWindowsMediaPlayer.URL = string.Empty;
            tab3_txtCamXuc.Text = string.Empty;
            tab3_txtAmThanhText.Text = string.Empty;
            tab3_pictureBoxLoad.Visible = false;
        }

        private void btnTrangTruoc_Click(object sender, EventArgs e)
        {

        }

        private void btnTrangTiep_Click(object sender, EventArgs e)
        {

        }

        private void tab3_txtCamXuc_TextChanged(object sender, EventArgs e)
        {
            if (tab3_txtCamXuc.Text == "Tích cực")
            {
                tab3_txtCamXuc.ForeColor = Color.Green;
            }
            else if (tab3_txtCamXuc.Text == "Trung lập")
            {
                tab3_txtCamXuc.ForeColor = Color.Blue;
            }
            else if (tab3_txtCamXuc.Text == "Tiêu cực")
            {
                tab3_txtCamXuc.ForeColor = Color.Red;
            }
            else
            {
                tab3_txtCamXuc.ForeColor = Color.Black;
            }
        }
        #endregion
    }
}