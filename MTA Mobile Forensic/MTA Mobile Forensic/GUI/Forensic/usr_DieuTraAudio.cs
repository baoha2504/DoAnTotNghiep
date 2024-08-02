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
    public partial class usr_DieuTraAudio : UserControl
    {
        public usr_DieuTraAudio()
        {
            InitializeComponent();
            tabControl1.SelectedTabIndex = 0;
            tab1_axWindowsMediaPlayer1.settings.volume = 100;
            tab1_axWindowsMediaPlayer2.settings.volume = 100;
        }

        int setup1 = 0;
        int setup2 = 0;
        int setup3 = 0;
        api api = new api();
        function function = new function();
        private List<string> audioFiles;

        private List<string> tab1_response;

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
                    splitContainer2.SplitterDistance = (int)((panel18.Height+ 20) / 2.5);
                }
                else if (selectedIndex == 1 && setup2 == 0)
                {
                    setup2 = 1;
                    //splitContainer3.SplitterDistance = (int)(panelEx1.Width / 2.5);

                }
                else if (selectedIndex == 2 && setup3 == 0)
                {
                    setup3 = 1;
                    //splitContainer2.SplitterDistance = (int)(panelEx1.Width / 2.5);
                }
            }
            catch { }
        }
        #region================================  TAB1 ====================================================================
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

                    var subDirectories = Directory.GetDirectories(folderPath);
                    foreach (var subDir in subDirectories)
                    {
                        var subDirFiles = Directory.GetFiles(subDir)
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
    }
}
//#region================================  TAB2  ====================================================================
//#endregion

//#region================================  TAB3  ====================================================================
//#endregion