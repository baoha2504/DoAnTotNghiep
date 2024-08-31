using MTA_Mobile_Forensic.Model;
using MTA_Mobile_Forensic.Support;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MTA_Mobile_Forensic.GUI.IOS
{
    public partial class usr_SaoLuuKhoiPhuc_IOS : UserControl
    {
        api api = new api();
        string query = "";
        string fullFilePath = "";
        string fullFilePath_Untrack = "";
        string fullFilePath_Restore = "";
        System.Windows.Forms.Timer _timer_Backup;
        System.Windows.Forms.Timer _timer_Restore;
        int _progress_Backup;
        int _progress_Restore;

        public usr_SaoLuuKhoiPhuc_IOS()
        {
            InitializeComponent();

            toolTip.SetToolTip(btnOpenPathBackup, "Chọn thư mục đích");
            toolTip.SetToolTip(btnOpenPathRestore, "Chọn file sao lưu");
            toolTip.SetToolTip(btnMoThuMuc, "Mở thư mục đã sao lưu");

            _timer_Backup = new System.Windows.Forms.Timer();
            _timer_Backup.Interval = 1000;
            _timer_Backup.Tick += Timer_Tick_Backup;

            _timer_Restore = new System.Windows.Forms.Timer();
            _timer_Restore.Interval = 1000;
            _timer_Restore.Tick += Timer_Tick_Restore;
        }

        private void Timer_Tick_Backup(object sender, EventArgs e)
        {
            if (_progress_Backup < 100)
            {
                _progress_Backup += 1;
                progressBar_Backup.Value = _progress_Backup;
            }
            else
            {
                _timer_Backup.Stop();
            }
        }

        private void Timer_Tick_Restore(object sender, EventArgs e)
        {
            if (_progress_Restore < 100)
            {
                _progress_Restore += 1;
                progressBar_Restore.Value = _progress_Restore;
            }
            else
            {
                _timer_Restore.Stop();
            }
        }

        private void panelEx1_Resize(object sender, EventArgs e)
        {
            try
            {
                splitContainer1.SplitterDistance = 5;
                splitContainer3.SplitterDistance = (int)(panelEx1.Width / 2);
            }
            catch { }
        }

        private void btnOpenPathBackup_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
            {
                DialogResult result = folderBrowserDialog.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(folderBrowserDialog.SelectedPath))
                {
                    string selectedFolder = folderBrowserDialog.SelectedPath;
                    fullFilePath = selectedFolder;
                    txtPathBackup.Text = selectedFolder;
                    btnMoThuMuc.Enabled = false;
                    pictureBox_Backup.Visible = false;
                    txtBackup.Clear();
                    progressBar_Backup.Value = 0;
                    _progress_Backup = 0;
                }
            }
        }

        private async void btnSaoLuu_Click(object sender, EventArgs e)
        {
            if (txtPathBackup.Text != string.Empty)
            {
                string command = "";
                txtBackup.Text = string.Empty;
                txtPathUntrack.Text = string.Empty;
                if (DeviceInfo.serialDevice != string.Empty)
                {
                    command = $"-u {DeviceInfo.serialDevice}";
                }
                try
                {
                    query = $"idevicebackup2.exe {command} backup \"{fullFilePath}\"";
                    await BackupAsync(query);
                }
                catch { }
            }
            else
            {
                MessageBox.Show("Chưa chọn thư mục đích sao lưu", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void HienThongBaoCaiDat_Backup(string info)
        {
            string sourceDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string projectDirectory = Directory.GetParent(sourceDirectory).Parent.Parent.FullName;
            string imagePath = Path.Combine(projectDirectory, "Data", "Image");
            if (info == "error")
            {
                string imageName = "error-16x16.png";
                string fullImagePath = Path.Combine(imagePath, imageName);
                pictureBox_Backup.Image = Image.FromFile(fullImagePath);
                pictureBox_Backup.Visible = true;
            }
            else if (info == "success" && progressBar_Backup.Value == 100)
            {
                string imageName = "checked-16x16.png";
                string fullImagePath = Path.Combine(imagePath, imageName);
                pictureBox_Backup.Image = Image.FromFile(fullImagePath);
                btnMoThuMuc.Enabled = true;
                pictureBox_Backup.Visible = true;
                LoadUntrack();
            }
        }

        public async void LoadUntrack()
        {
            if (DeviceInfo.serialDevice != string.Empty)
            {
                pictureBoxLoad.Visible = true;
                fullFilePath_Untrack = await api.UntrackBackup_IOS(fullFilePath, DeviceInfo.serialDevice);
                if (!string.IsNullOrEmpty(fullFilePath_Untrack))
                {
                    txtPathUntrack.Text = fullFilePath_Untrack;
                    DeviceInfo.pathBackup = fullFilePath_Untrack;
                    btnMoThuMuc.Enabled = true;
                    pictureBoxLoad.Visible = false;
                    MessageBox.Show("Untrack thư mục sao lưu thành công!");
                }
            }
        }

        private async Task BackupAsync(string query)
        {
            _timer_Backup.Start();

            await Task.Run(() =>
            {
                // Create the process start info
                ProcessStartInfo processStartInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                // Start the process
                Process process = new Process
                {
                    StartInfo = processStartInfo
                };

                process.OutputDataReceived += (sender, args) =>
                {
                    if (!string.IsNullOrEmpty(args.Data))
                    {
                        // Update the TextBox with output
                        this.Invoke(new Action(() =>
                        {
                            txtBackup.AppendText(args.Data + Environment.NewLine);
                        }));
                    }
                };

                process.ErrorDataReceived += (sender, args) =>
                {
                    if (!string.IsNullOrEmpty(args.Data))
                    {
                        // Update the TextBox with error
                        this.Invoke(new Action(() =>
                        {
                            if (txtBackup.Text.Contains("ERROR"))
                            {
                                HienThongBaoCaiDat_Backup("error");
                                txtBackup.AppendText("ERROR: " + args.Data + Environment.NewLine);
                            }
                        }));
                    }
                };

                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();

                // Write the adb command to the process
                using (StreamWriter sw = process.StandardInput)
                {
                    if (sw.BaseStream.CanWrite)
                    {
                        sw.WriteLine(query);
                    }
                }

                // Wait for the process to exit
                process.WaitForExit();

                // Update the ProgressBar and TextBox when done
                this.Invoke(new Action(() =>
                {
                    _timer_Backup.Stop();
                    progressBar_Backup.Value = 100;

                    if (txtBackup.Text.Contains("94% Finished"))
                    {
                        HienThongBaoCaiDat_Backup("success");
                        txtBackup.AppendText("Backup completed" + Environment.NewLine);
                    }
                }));
            });
        }

        private void btnMoThuMuc_Click(object sender, EventArgs e)
        {
            if (fullFilePath_Untrack != string.Empty)
            {
                var result = MessageBox.Show("Mở thư mục chứa file backup", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    OpenExplorer(fullFilePath_Untrack);
                }
            }
        }

        private void OpenExplorer(string filePath)
        {
            try
            {
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

        private void btnUntrackThuCong_Click(object sender, EventArgs e)
        {
            if (txtBackup.Text.Contains("94% Finished"))
            {
                LoadUntrack();
            }
            else
            {
                MessageBox.Show("Bản backup chưa hoàn thiện", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public string GetParentDirectoryPath(string path)
        {
            var directoryInfo = new DirectoryInfo(path);
            return directoryInfo.Parent?.FullName;
        }

        private void btnOpenPathRestore_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
            {
                DialogResult result = folderBrowserDialog.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(folderBrowserDialog.SelectedPath))
                {
                    string selectedFolder = folderBrowserDialog.SelectedPath;
                    selectedFolder = GetParentDirectoryPath(selectedFolder);
                    txtPathRestore.Text = selectedFolder;
                    fullFilePath_Restore = selectedFolder;
                    pictureBox_Restore.Visible = false;
                    txtRestore.Clear();
                    progressBar_Restore.Value = 0;
                    _progress_Restore = 0;
                }
            }
        }

        private void txtPathRestore_TextChanged(object sender, EventArgs e)
        {
            if (txtPathRestore.Text != string.Empty)
            {
                pictureBox_IOS.Visible = true;
            }
            else
            {
                pictureBox_IOS.Visible = false;
            }
        }

        private async void btnKhoiPhuc_Click(object sender, EventArgs e)
        {
            if (txtPathRestore.Text != string.Empty)
            {
                string command = "";
                if (DeviceInfo.serialDevice != string.Empty)
                {
                    command = $"-u {DeviceInfo.serialDevice}";
                }
                query = $"idevicebackup2.exe {command} restore --no-reboot \"{fullFilePath_Restore}\"";
                await RestoreAsync(query);
            }
            else
            {
                MessageBox.Show("Chưa chọn file sao lưu", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void HienThongBaoCaiDat_Restore(string info)
        {
            string sourceDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string projectDirectory = Directory.GetParent(sourceDirectory).Parent.Parent.FullName;
            string imagePath = Path.Combine(projectDirectory, "Data", "Image");
            if (info == "error")
            {
                string imageName = "error-16x16.png";
                string fullImagePath = Path.Combine(imagePath, imageName);
                pictureBox_Restore.Image = Image.FromFile(fullImagePath);
                pictureBox_Restore.Visible = true;
            }
            else if (info == "success" && progressBar_Restore.Value == 100)
            {
                string imageName = "checked-16x16.png";
                string fullImagePath = Path.Combine(imagePath, imageName);
                pictureBox_Restore.Image = Image.FromFile(fullImagePath);
                pictureBox_Restore.Visible = true;
            }
        }

        private async Task RestoreAsync(string query)
        {
            _timer_Restore.Start();

            await Task.Run(() =>
            {
                // Create the process start info
                ProcessStartInfo processStartInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                // Start the process
                Process process = new Process
                {
                    StartInfo = processStartInfo
                };

                process.OutputDataReceived += (sender, args) =>
                {
                    if (!string.IsNullOrEmpty(args.Data))
                    {
                        // Update the TextBox with output
                        this.Invoke(new Action(() =>
                        {
                            txtRestore.AppendText(args.Data + Environment.NewLine);
                        }));
                    }
                };

                process.ErrorDataReceived += (sender, args) =>
                {
                    if (!string.IsNullOrEmpty(args.Data))
                    {
                        // Update the TextBox with error
                        this.Invoke(new Action(() =>
                        {
                            if (txtRestore.Text.Contains("ERROR"))
                            {
                                HienThongBaoCaiDat_Restore("error");
                                txtRestore.AppendText("ERROR: " + args.Data + Environment.NewLine);
                            }
                        }));
                    }
                };

                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();

                // Write the adb command to the process
                using (StreamWriter sw = process.StandardInput)
                {
                    if (sw.BaseStream.CanWrite)
                    {
                        sw.WriteLine(query);
                    }
                }

                // Wait for the process to exit
                process.WaitForExit();

                // Update the ProgressBar and TextBox when done
                this.Invoke(new Action(() =>
                {
                    _timer_Restore.Stop();
                    progressBar_Restore.Value = 100;

                    if (!txtRestore.Text.Contains("ERROR"))
                    {
                        HienThongBaoCaiDat_Restore("success");
                        txtRestore.AppendText("Restore completed" + Environment.NewLine);
                    }
                }));
            });
        }
    }
}