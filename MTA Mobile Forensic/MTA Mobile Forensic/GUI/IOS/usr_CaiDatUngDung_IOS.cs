using MTA_Mobile_Forensic.Model;
using MTA_Mobile_Forensic.Support;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace MTA_Mobile_Forensic.GUI.IOS
{
    public partial class usr_CaiDatUngDung_IOS : UserControl
    {
        libimobiledevice libimobiledevice = new libimobiledevice();
        function function = new function();
        System.Windows.Forms.Timer _timer;
        System.Windows.Forms.Timer _timer_GoCaiDat;
        int _progress;
        int _progress_GoCaiDat;

        public usr_CaiDatUngDung_IOS()
        {
            InitializeComponent();

            _timer = new System.Windows.Forms.Timer();
            _timer.Interval = 1500;
            _timer.Tick += Timer_Tick;

            _timer_GoCaiDat = new System.Windows.Forms.Timer();
            _timer_GoCaiDat.Interval = 1500;
            _timer_GoCaiDat.Tick += Timer_GoCaiDat_Tick;

            AnThongTinCaiDat();
            AnThongTinGoCaiDat();

            LoadData();
        }

        private void panelMain_Resize(object sender, EventArgs e)
        {
            try
            {
                splitContainer1.SplitterDistance = (int)(panelMain.Height / 2);
                splitContainer2.SplitterDistance = (int)(panelMain.Width / 2);
                splitContainer3.SplitterDistance = (int)(panelMain.Width / 2);
            }
            catch
            {

            }
        }

        public List<AppInfoIOS> ParseAppInfo(string input)
        {
            var appInfos = new List<AppInfoIOS>();
            var lines = input.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var line in lines)
            {
                var parts = line.Split(new[] { ", " }, StringSplitOptions.None);

                if (parts.Length == 3)
                {
                    var appInfo = new AppInfoIOS
                    {
                        CFBundleIdentifier = parts[0].Trim(),
                        CFBundleVersion = parts[1].Trim().Trim('"'),
                        CFBundleDisplayName = parts[2].Trim().Trim('"')
                    };

                    appInfos.Add(appInfo);
                }
            }

            return appInfos;
        }

        public void LoadData()
        {
            string text = libimobiledevice.ideviceinstallerCommand($"-u {DeviceInfo.serialDevice} -l");
            var apps = ParseAppInfo(text);
            dataGridView.Rows.Clear();

            if (apps != null)
            {
                for (int i = 0; i < apps.Count; i++)
                {
                    dataGridView.Rows.Add();
                    dataGridView.Rows[i].Cells["Column1"].Value = i + 1;
                    dataGridView.Rows[i].Cells["Column2"].Value = apps[i].CFBundleDisplayName;
                    dataGridView.Rows[i].Cells["Column3"].Value = apps[i].CFBundleVersion;
                    dataGridView.Rows[i].Cells["Column4"].Value = apps[i].CFBundleIdentifier;
                    dataGridView.Rows[i].Cells["Column5"].Value = "Gỡ ▼";
                }
            }
        }

        private void btnChonFolder_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "IPA files (*.ipa)|*.ipa|All files (*.*)|*.*";
                openFileDialog.Title = "Chọn file IPA";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    txtPathFileAPK.Text = openFileDialog.FileName;
                    lblDungLuongFileAPK.Text = function.GetFileSizeInMB(openFileDialog.FileName);
                    lblTenFileAPK.Text = Path.GetFileName(openFileDialog.FileName);
                    HienThongTinCaiDat();
                    AnThongBaoCaiDat();
                }
            }
        }

        // Cài đặt
        private void AnThongTinCaiDat()
        {
            panel10.Visible = false;
            panel12.Visible = false;
        }

        private void HienThongTinCaiDat()
        {
            panel10.Visible = true;
            panel12.Visible = true;
        }

        private void AnThongBaoCaiDat()
        {
            pictureBox_ThongBaoCaiDat.Visible = false;
            txtQuaTrinhCaiDat.Text = string.Empty;
            progressBarX_CaiDat.Value = 0;
        }

        private void HienThongBaoCaiDat(string info)
        {
            string sourceDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string projectDirectory = Directory.GetParent(sourceDirectory).Parent.Parent.FullName;
            string imagePath = Path.Combine(projectDirectory, "Data", "Image");
            if (info == "error")
            {
                string imageName = "error.png";
                string fullImagePath = Path.Combine(imagePath, imageName);
                pictureBox_ThongBaoCaiDat.Image = Image.FromFile(fullImagePath);
            }
            else if (info == "success" && progressBarX_CaiDat.Value == 100)
            {
                string imageName = "checked.png";
                string fullImagePath = Path.Combine(imagePath, imageName);
                pictureBox_ThongBaoCaiDat.Image = Image.FromFile(fullImagePath);
            }
            pictureBox_ThongBaoCaiDat.Visible = true;
        }


        // Gỡ cài đặt
        private void AnThongTinGoCaiDat()
        {
            panel15.Visible = false;
            panel28.Visible = false;
        }

        private void HienThongTinGoCaiDat()
        {
            panel15.Visible = true;
            panel28.Visible = true;
        }

        private void AnThongBaoGoCaiDat()
        {
            pictureBox_ThongBaoGoCaiDat.Visible = false;
            txtQuaTrinhGoCaiDat.Text = string.Empty;
            progressBarX_GoCaiDat.Value = 0;
        }

        private void HienThongBaoGoCaiDat(string info)
        {
            string sourceDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string projectDirectory = Directory.GetParent(sourceDirectory).Parent.Parent.FullName;
            string imagePath = Path.Combine(projectDirectory, "Data", "Image");
            if (info == "error")
            {
                string imageName = "error.png";
                string fullImagePath = Path.Combine(imagePath, imageName);
                pictureBox_ThongBaoGoCaiDat.Image = Image.FromFile(fullImagePath);
            }
            else if (info == "success" && progressBarX_GoCaiDat.Value == 100)
            {
                string imageName = "checked.png";
                string fullImagePath = Path.Combine(imagePath, imageName);
                pictureBox_ThongBaoGoCaiDat.Image = Image.FromFile(fullImagePath);
            }
            pictureBox_ThongBaoGoCaiDat.Visible = true;
        }

        private void txtPathFileAPK_TextChanged(object sender, EventArgs e)
        {
            HienThongTinCaiDat();
            AnThongBaoCaiDat();
        }

        private void lblTenUngDung_TextChanged(object sender, EventArgs e)
        {
            HienThongTinGoCaiDat();
            AnThongBaoGoCaiDat();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (_progress < 100)
            {
                _progress += 10;
                progressBarX_CaiDat.Value = _progress;
            }
            else
            {
                _timer.Stop();
            }
        }

        private async Task InstallIPAAsync(string apkPath)
        {
            txtQuaTrinhCaiDat.Clear();
            progressBarX_CaiDat.Value = 0;
            _progress = 0;
            _timer.Start();

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
                            txtQuaTrinhCaiDat.AppendText(args.Data + Environment.NewLine);
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
                            if (!txtQuaTrinhCaiDat.Text.Contains("ERROR"))
                            {
                                HienThongBaoCaiDat("error");
                                txtQuaTrinhCaiDat.AppendText("ERROR: " + args.Data + Environment.NewLine);
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
                        string command = "";
                        if (DeviceInfo.serialDevice != string.Empty)
                        {
                            command = $"-u {DeviceInfo.serialDevice}";
                        }
                        sw.WriteLine($"ideviceinstaller.exe {command} -i \"{apkPath}\"");
                    }
                }

                // Wait for the process to exit
                process.WaitForExit();

                // Update the ProgressBar and TextBox when done
                this.Invoke(new Action(() =>
                {
                    _timer.Stop();
                    progressBarX_CaiDat.Value = 100;

                    if (txtQuaTrinhCaiDat.Text.Contains("Success"))
                    {
                        HienThongBaoCaiDat("success");
                        txtQuaTrinhCaiDat.AppendText("Installation completed" + Environment.NewLine);
                        var result = MessageBox.Show("Reload lại danh sách ứng dụng?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (result == DialogResult.Yes)
                        {
                            LoadData();
                        }
                    }
                }));
            });
        }

        private async void btnCaiDat_Click(object sender, EventArgs e)
        {
            await InstallIPAAsync(txtPathFileAPK.Text);
        }

        private async void btnGoCaiDat_Click(object sender, EventArgs e)
        {
            await UninstallPackageAsync(lblGoi.Text);
        }

        private void Timer_GoCaiDat_Tick(object sender, EventArgs e)
        {
            if (_progress_GoCaiDat < 100)
            {
                _progress_GoCaiDat += 10;
                progressBarX_GoCaiDat.Value = _progress_GoCaiDat;
            }
            else
            {
                _timer_GoCaiDat.Stop();
            }
        }

        private async Task UninstallPackageAsync(string package)
        {
            txtQuaTrinhGoCaiDat.Clear();
            progressBarX_GoCaiDat.Value = 0;
            _progress_GoCaiDat = 0;
            _timer_GoCaiDat.Start();

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
                            txtQuaTrinhGoCaiDat.AppendText(args.Data + Environment.NewLine);
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
                            if (!txtQuaTrinhGoCaiDat.Text.Contains("Success"))
                            {
                                HienThongBaoGoCaiDat("error");
                                txtQuaTrinhGoCaiDat.AppendText("ERROR: " + args.Data + Environment.NewLine);
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
                        string command = "";

                        if (DeviceInfo.serialDevice != string.Empty)
                        {
                            command = $"-u {DeviceInfo.serialDevice}";
                        }
                        sw.WriteLine($"ideviceinstaller.exe {command} -U {package}");
                    }
                }

                // Wait for the process to exit
                process.WaitForExit();

                // Update the ProgressBar and TextBox when done
                this.Invoke(new Action(() =>
                {
                    _timer_GoCaiDat.Stop();
                    progressBarX_GoCaiDat.Value = 100;

                    if (txtQuaTrinhGoCaiDat.Text.Contains("Complete"))
                    {
                        HienThongBaoGoCaiDat("success");
                        txtQuaTrinhGoCaiDat.AppendText("Uninstall completed" + Environment.NewLine);
                        var result = MessageBox.Show("Reload lại danh sách ứng dụng?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (result == DialogResult.Yes)
                        {
                            LoadData();
                        }
                    }
                }));
            });
        }

        private void dataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataGridView.Columns["Column5"].Index && e.RowIndex >= 0)
            {
                try
                {
                    string tenungdung = (string)dataGridView.Rows[e.RowIndex].Cells["Column2"].Value;
                    lblTenUngDung.Text = tenungdung;
                    string goi = (string)dataGridView.Rows[e.RowIndex].Cells["Column4"].Value;
                    lblGoi.Text = goi;
                }
                catch { }
            }
        }
    }
}
