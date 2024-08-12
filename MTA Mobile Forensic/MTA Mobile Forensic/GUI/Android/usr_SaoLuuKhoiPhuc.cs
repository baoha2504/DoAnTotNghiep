using MTA_Mobile_Forensic.Support;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace MTA_Mobile_Forensic.GUI.Android
{
    public partial class usr_SaoLuuKhoiPhuc : UserControl
    {

        string query = "";
        string str = "";
        string fullFilePath = "";
        string fullFilePath_Restore = "";
        abe abe = new abe();
        winrar winrar = new winrar();
        function function = new function();
        System.Windows.Forms.Timer _timer_Backup;
        System.Windows.Forms.Timer _timer_Restore;
        System.Windows.Forms.Timer _timer_Giainen;
        int _progress_Backup;
        int _progress_Restore;
        int _progress_Giainen;
        public usr_SaoLuuKhoiPhuc()
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

            _timer_Giainen = new System.Windows.Forms.Timer();
            _timer_Giainen.Interval = 1000;
            _timer_Giainen.Tick += Timer_Tick_Giainen;
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

        private void Timer_Tick_Giainen(object sender, EventArgs e)
        {
            if (_progress_Giainen < 100)
            {
                _progress_Giainen += 3;
                progressBar_Giainen.Value = _progress_Giainen;
            }
            else
            {
                _timer_Giainen.Stop();
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


        //===================== BACKUP =====================
        private void checkBox_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox.Checked == true)
            {
                txtMatKhau.Visible = true;
            }
            else
            {
                txtMatKhau.Visible = false;
                txtMatKhau.Text = string.Empty;
            }
        }

        private void btnOpenPathBackup_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
            {
                DialogResult result = folderBrowserDialog.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(folderBrowserDialog.SelectedPath))
                {
                    string selectedFolder = folderBrowserDialog.SelectedPath;
                    DateTime dateTime = DateTime.Now;
                    string fileName = "Backup_export_" + dateTime.ToString("HH.mm.ss_dd.MM.yyyy") + ".ab";
                    fullFilePath = Path.Combine(selectedFolder, fileName);
                    txtPathBackup.Text = selectedFolder;
                    btnMoThuMuc.Enabled = false;
                    btnGiaiNen.Enabled = false;
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
                if (checkBox.Checked == true)
                {
                    if (txtMatKhau.Text != string.Empty)
                    {
                        query = $"adb backup -apk -shared -all -f \"{fullFilePath}\" -k {txtMatKhau.Text}";
                        await BackupAsync(query);
                    }
                    else
                    {
                        MessageBox.Show("Chưa nhập mật khẩu", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    query = $"adb backup -apk -shared -all -f \"{fullFilePath}\"";
                    await BackupAsync(query);
                }

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
            if (info == "error" && progressBar_Backup.Value == 100)
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
                btnGiaiNen.Enabled = true;
                pictureBox_Backup.Visible = true;
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

                    if (!txtBackup.Text.Contains("ERROR"))
                    {
                        HienThongBaoCaiDat_Backup("success");
                        txtBackup.AppendText("Backup completed" + Environment.NewLine);
                    }
                }));
            });
        }

        private void btnMoThuMuc_Click(object sender, EventArgs e)
        {
            if (txtPathBackup.Text != string.Empty)
            {
                var result = MessageBox.Show("Mở thư mục chứa file backup", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    OpenExplorer(fullFilePath);
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


        //===================== RESTORE =====================
        private void btnOpenPathRestore_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "APK files (*.ab)|*.ab|All files (*.*)|*.*";
                openFileDialog.Title = "Chọn file backup";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    txtPathRestore.Text = openFileDialog.FileName;
                    fullFilePath_Restore = openFileDialog.FileName;
                    txtPathRestore.Text += $" ({function.GetFileSizeInMB(openFileDialog.FileName)})";
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
                pictureBox_Android.Visible = true;
            }
            else
            {
                pictureBox_Android.Visible = false;
            }
        }

        private async void btnKhoiPhuc_Click(object sender, EventArgs e)
        {
            if (txtPathRestore.Text != string.Empty)
            {
                query = $"adb restore \"{fullFilePath_Restore}\"";
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
            if (info == "error" && progressBar_Restore.Value == 100)
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

        private async void btnGiaiNen_Click(object sender, EventArgs e)
        {
            progressBar_Giainen.Value = 0;
            _timer_Giainen.Start();
            await Task.Run(() => GiaiNen());
        }

        private void GiaiNen()
        {
            try
            {
                if (Directory.Exists(Path.GetDirectoryName(fullFilePath)))
                {
                    string pathTar = fullFilePath.Replace(".ab", ".tar");
                    query = $"unpack \"{fullFilePath}\" \"{pathTar}\"";
                    str = abe.abeCommand(query);

                    if (str == string.Empty)
                    {
                        string pathFolder = pathTar.Replace(".tar", "");

                        if (!Directory.Exists(pathFolder))
                        {
                            Directory.CreateDirectory(pathFolder);
                        }

                        query = $" x \"{pathTar}\" \"{pathFolder}\"";
                        str = winrar.winrarCommand(query);

                        this.Invoke(new Action(() =>
                        {
                            _timer_Giainen.Stop();
                            progressBar_Giainen.Value = 100;
                            txtThongBaoGiaiNen.ForeColor = Color.Green;
                            txtThongBaoGiaiNen.Text = $"Giải nén thành công";
                        }));
                        
                        DeleteFileIfExists(pathTar);
                    }
                    else
                    {
                        this.Invoke(new Action(() =>
                        {
                            _timer_Giainen.Stop();
                            progressBar_Giainen.Value = 0;
                            txtThongBaoGiaiNen.ForeColor = Color.Red;
                            txtThongBaoGiaiNen.Text = $"Giải nén không thành công";
                        }));
                    }
                }
            }
            catch
            {
                this.Invoke(new Action(() =>
                {
                    _timer_Giainen.Stop();
                    progressBar_Giainen.Value = 0;
                    txtThongBaoGiaiNen.ForeColor = Color.Red;
                    txtThongBaoGiaiNen.Text = $"Giải nén không thành công";
                }));
            }
        }

        private void DeleteFileIfExists(string filePath)
        {
            if (File.Exists(filePath))
            {
                try
                {
                    File.Delete(filePath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi xóa file: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show($"File '{filePath}' không tồn tại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnPhucHoi_Click(object sender, EventArgs e)
        {
            MessageBox.Show($"Chức năng phục hồi đang phát triển", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
