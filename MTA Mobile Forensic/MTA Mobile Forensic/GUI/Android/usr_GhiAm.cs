using MTA_Mobile_Forensic.GUI.Share;
using MTA_Mobile_Forensic.Model;
using MTA_Mobile_Forensic.Support;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MTA_Mobile_Forensic.GUI.Android
{
    public partial class usr_GhiAm : UserControl
    {
        public usr_GhiAm()
        {
            InitializeComponent();
            axWindowsMediaPlayer1.settings.volume = 100;
            if (DeviceInfo.serialDevice != string.Empty)
            {
                txtTimKiem.Text = DeviceInfo.pathBackup;
            }
        }

        string query = "";
        string linkghiamdachon = "";
        int solancat = 0;
        function function = new function();
        exiftool exiftool = new exiftool();
        ffmpeg ffmpeg = new ffmpeg();
        int itemsPerPage = 20;
        int currentPage = 0;
        private List<string> recordFiles;

        private void GetRecordInFolder(string folderPath)
        {
            if (folderPath != string.Empty)
            {
                try
                {
                    string[] recordExtensions = new string[] { ".mp3", ".wav", ".ogg", ".flac", ".aac", ".m4a", ".wma" };

                    recordFiles = Directory.GetFiles(folderPath)
                                             .Where(file => recordExtensions.Contains(Path.GetExtension(file).ToLower()))
                                             .ToList();

                    if (cbbTuyChon.Text != "Trong thư mục")
                    {
                        List<string> smallestSubfolders = GetSmallestSubfolders(folderPath);
                        foreach (var subfolder in smallestSubfolders)
                        {
                            var subDirFiles = Directory.GetFiles(subfolder)
                                .Where(file => recordExtensions.Contains(Path.GetExtension(file).ToLower()))
                                .ToList();
                            recordFiles.AddRange(subDirFiles);
                        }
                    }
                    Add_usr_GhiAmMini(currentPage);
                }
                catch (Exception ex)
                {
                    frm_Notification frm_Notification = new frm_Notification("error", ex.ToString());
                    frm_Notification.ShowDialog();
                }
            }
        }

        private List<string> GetSmallestSubfolders(string rootFolderPath)
        {
            List<string> result = new List<string>();

            // Duyệt qua tất cả các thư mục con trong thư mục gốc
            foreach (string subfolder in Directory.GetDirectories(rootFolderPath, "*", SearchOption.AllDirectories))
            {
                // Kiểm tra xem thư mục này có chứa thư mục con nào không
                if (Directory.GetDirectories(subfolder).Length == 0)
                {
                    result.Add(subfolder);
                }
            }

            return result;
        }

        private void Add_usr_GhiAmMini(int pageNumber)
        {
            flpDSGhiAm.Controls.Clear();

            int start = pageNumber * itemsPerPage;
            int end = Math.Min(start + itemsPerPage, recordFiles.Count);

            txtTrangHienTai.Text = $"Trang {pageNumber + 1} / {Math.Ceiling((double)recordFiles.Count / itemsPerPage)}";

            for (int i = start; i < end; i++)
            {
                try
                {
                    usr_GhiAmMini usr_GhiAmMini = new usr_GhiAmMini(recordFiles[i], Path.GetFileName(recordFiles[i]), function.GetLastModified(recordFiles[i]));
                    usr_GhiAmMini.ControlClicked += flpDSGhiAm_Click;
                    flpDSGhiAm.Controls.Add(usr_GhiAmMini);
                }
                catch { }
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

        private void flpDSGhiAm_Click(object sender, EventArgs e)
        {
            if (sender is usr_GhiAmMini clickedControl)
            {
                flpDSFileDaCat.Controls.Clear();
                CheckInfoRecord(clickedControl.path);
                axWindowsMediaPlayer1.URL = clickedControl.path;
                axWindowsMediaPlayer1.Ctlcontrols.play();
                linkghiamdachon = clickedControl.path;
            }
        }

        private void CheckInfoRecord(string pathRecord)
        {
            try
            {
                query = $"\"{pathRecord}\"";
                string str = exiftool.exiftoolCommand(query);
                txtThongTinGhiAm.Text = str;
            }
            catch (Exception ex)
            {
                txtThongTinGhiAm.Text = "Error: " + ex.Message;
            }
        }

        private void flpDSGhiAm_Resize(object sender, EventArgs e)
        {
            //foreach (UserControl control in flpDSGhiAm.Controls)
            //{
            //    control.Width = flpDSGhiAm.Width;
            //}
        }

        private void btnTrangTruoc_Click(object sender, EventArgs e)
        {
            btnTrangTiep.Enabled = true;
            if (currentPage > 0)
            {
                currentPage--;
                Add_usr_GhiAmMini(currentPage);
            }
            else
            {
                btnTrangTruoc.Enabled = false;
            }
        }

        private void btnTrangTiep_Click(object sender, EventArgs e)
        {
            btnTrangTruoc.Enabled = true;
            if (currentPage < (recordFiles.Count - 1) / itemsPerPage)
            {
                currentPage++;
                Add_usr_GhiAmMini(currentPage);
            }
            else
            {
                btnTrangTiep.Enabled = false;
            }
        }

        private void cbbTuyChon_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetRecordInFolder(txtTimKiem.Text);
        }

        private void txtTimKiem_TextChanged(object sender, EventArgs e)
        {
            GetRecordInFolder(txtTimKiem.Text);
        }

        private void btnXuat_Click(object sender, EventArgs e)
        {

        }

        private void btnNhap_Click(object sender, EventArgs e)
        {

        }

        private void btnChonTatCa_Click(object sender, EventArgs e)
        {
            foreach (usr_GhiAmMini control in flpDSGhiAm.Controls)
            {
                control.checkBox.Checked = true;
            }
        }

        private void btnBoChonTatCa_Click(object sender, EventArgs e)
        {
            foreach (usr_GhiAmMini control in flpDSGhiAm.Controls)
            {
                control.checkBox.Checked = false;
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            if(txtTimKiem.Text != DeviceInfo.pathBackup)
            {
                txtTimKiem.Text = DeviceInfo.pathBackup;
            }
            flpDSGhiAm.Controls.Clear();
            GetRecordInFolder(txtTimKiem.Text);
        }

        private async void btnCatGhiAm_Click(object sender, EventArgs e)
        {
            try
            {
                solancat++;
                TimeSpan startTime = timeSpanEdit1.TimeSpan;
                TimeSpan endTime = timeSpanEdit2.TimeSpan;

                //string outputFilePath = @"C:\Users\hacon\Desktop\sound_recorder\BEAT_THIEN_LY_CUT.mp3";

                string audioFileName = Path.GetFileNameWithoutExtension(linkghiamdachon);
                string outputDirectory = Path.Combine(Path.GetDirectoryName(linkghiamdachon), audioFileName);

                if (!Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }

                string outputFilePath = Path.Combine(outputDirectory, Path.GetFileName(audioFileName) + "_Cut" + solancat.ToString() + Path.GetExtension(linkghiamdachon));

                await Task.Run(() => CutAudioFile(linkghiamdachon, outputFilePath, startTime, endTime));

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CutAudioFile(string inputFilePath, string outputFilePath, TimeSpan startTime, TimeSpan endTime)
        {
            try
            {
                if (startTime >= endTime)
                {
                    throw new ArgumentException("Start time must be less than end time.");
                }

                TimeSpan duration = endTime - startTime;

                string arguments = $"-i \"{inputFilePath}\" -ss {startTime} -t {duration} -c copy \"{outputFilePath}\"";

                ffmpeg.ffmpegCommand(arguments);

                Task.Run(() =>
                {
                    while (!File.Exists(outputFilePath))
                    {
                        Thread.Sleep(500);
                    }

                    flpDSFileDaCat.Invoke((MethodInvoker)delegate
                    {
                        try
                        {
                            usr_GhiAmMini usr_GhiAmMini = new usr_GhiAmMini(outputFilePath, Path.GetFileName(outputFilePath), function.GetLastModified(outputFilePath));
                            usr_GhiAmMini.ControlClicked += flpDSFileDaCat_Click;
                            usr_GhiAmMini.checkBox.Visible = false;
                            flpDSFileDaCat.Controls.Add(usr_GhiAmMini);
                        }
                        catch { }
                    });
                    //MessageBox.Show("Audio file has been successfully cut.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void flpDSFileDaCat_Click(object sender, EventArgs e)
        {
            if (sender is usr_GhiAmMini clickedControl)
            {
                CheckInfoRecord(clickedControl.path);
                axWindowsMediaPlayer1.URL = clickedControl.path;
                axWindowsMediaPlayer1.Ctlcontrols.play();
                linkghiamdachon = clickedControl.path;
            }
        }
    }
}
