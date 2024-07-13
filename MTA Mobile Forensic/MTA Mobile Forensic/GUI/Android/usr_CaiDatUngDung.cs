using MTA_Mobile_Forensic.Model;
using MTA_Mobile_Forensic.Support;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace MTA_Mobile_Forensic.GUI.Android
{
    public partial class usr_CaiDatUngDung : UserControl
    {
        api api = new api();
        adb adb = new adb();
        function function = new function();
        string query = "";
        string fullImagePath = "";
        ImageList imageList;
        List<string> matchValues = new List<string>();
        List<AppInfo> apps = new List<AppInfo>();
        int itemsPerPage = 20;
        int currentPage = 0;
        MatchCollection matches;
        System.Windows.Forms.Timer _timer;
        System.Windows.Forms.Timer _timer_GoCaiDat;
        int _progress;
        int _progress_GoCaiDat;


        public usr_CaiDatUngDung()
        {
            InitializeComponent();

            _timer = new System.Windows.Forms.Timer();
            _timer.Interval = 1500;
            _timer.Tick += Timer_Tick;

            _timer_GoCaiDat = new System.Windows.Forms.Timer();
            _timer_GoCaiDat.Interval = 1500;
            _timer_GoCaiDat.Tick += Timer_GoCaiDat_Tick;

            Load_UngDung();

            AnThongTinCaiDat();
            AnThongTinGoCaiDat();

            listView.Columns.Add("Ảnh", (int)(1 * groupPanel_UngDung.Width / 10), HorizontalAlignment.Center);
            listView.Columns.Add("Tên ứng dụng", (int)(3.5 * groupPanel_UngDung.Width / 10), HorizontalAlignment.Center);
            listView.Columns.Add("Ngày cài đặt", (int)(2.5 * groupPanel_UngDung.Width / 10), HorizontalAlignment.Center);
            listView.Columns.Add("Gói", (int)(2.5 * groupPanel_UngDung.Width / 10), HorizontalAlignment.Center);
        }

        private void panelMain_Resize(object sender, EventArgs e)
        {
            try
            {
                splitContainer1.SplitterDistance = (int)(panelMain.Height / 2);
                splitContainer2.SplitterDistance = (int)(panelMain.Width / 2);
                splitContainer3.SplitterDistance = (int)(panelMain.Width / 2);

                listView.Columns[0].Width = (int)(1 * groupPanel_UngDung.Width / 10);
                listView.Columns[1].Width = (int)(3.5 * groupPanel_UngDung.Width / 10);
                listView.Columns[2].Width = (int)(2.5 * groupPanel_UngDung.Width / 10);
                listView.Columns[3].Width = (int)(2.5 * groupPanel_UngDung.Width / 10);
            }
            catch
            {

            }
        }

        private void Load_UngDung()
        {
            imageList = new ImageList
            {
                ImageSize = new Size(30, 30)
            };

            listView.SmallImageList = imageList;

            LoadAnhMau();
            LayDanhSachPackage();
        }

        private void Clear_ListView()
        {
            listView.Items.Clear();
        }

        private void LayDanhSachPackage()
        {

            query = "shell pm list packages -3";
            string str = adb.adbCommand(query);
            if (str == string.Empty)
            {
                str = "package:com.zing.zalo\r\npackage:org.telegram.messenger\r\npackage:com.instagram.android\r\npackage:com.ss.android.ugc.trill\r\npackage:com.google.android.apps.photos\r\npackage:com.sec.android.app.sbrowser\r\npackage:com.facebook.lite\r\npackage:com.facebook.orca\r\npackage:com.sec.android.app.popupcalculator\r\npackage:com.sec.android.app.kidshome\r\npackage:com.sec.android.easyMover\r\npackage:cn.wps.moffice_eng";
            }
            string pattern = @"package:(\S+)";

            matches = Regex.Matches(str, pattern);

            Add_UngDung(matches, currentPage);

        }

        private string LayThongTinNgayCaiDat(string input)
        {
            string firstInstallTimePattern = @"firstInstallTime=(\S+ \S+)";
            Match firstInstallTimeMatch = Regex.Match(input, firstInstallTimePattern);
            if (firstInstallTimeMatch.Success)
            {
                return firstInstallTimeMatch.Groups[1].Value;
            }
            else
            {
                return "";
            }
        }

        private string LayThongTinNgayCapNhatCuoi(string input)
        {
            string lastUpdateTimePattern = @"lastUpdateTime=(\S+ \S+)";
            Match lastUpdateTimeMatch = Regex.Match(input, lastUpdateTimePattern);
            if (lastUpdateTimeMatch.Success)
            {
                return lastUpdateTimeMatch.Groups[1].Value;
            }
            else
            {
                return "";
            }
        }

        private string LayThongTinPhienBan(string input)
        {
            string versionNamePattern = @"versionName=(\S+)";
            Match versionNameMatch = Regex.Match(input, versionNamePattern);
            if (versionNameMatch.Success)
            {
                return versionNameMatch.Groups[1].Value;
            }
            else
            {
                return "";
            }
        }

        private async Task<List<AppInfo>> LayThongTinUngDung(List<string> packages)
        {
            string[] packageArray = packages.ToArray();

            var response = await api.GetDataApplication(packageArray);

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

        private void LoadAnhMau()
        {
            string sourceDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string projectDirectory = Directory.GetParent(sourceDirectory).Parent.Parent.FullName;
            string imagePath = Path.Combine(projectDirectory, "Data", "Image");
            string imageName = "app.png";
            fullImagePath = Path.Combine(imagePath, imageName);
        }

        private ListViewItem AddData(string package, string tenungdung, string ngaycaidat, string capnhatlancuoi, string phienban, string linkanh)
        {
            try
            {
                using (WebClient webClient = new WebClient())
                {
                    byte[] imageBytes = webClient.DownloadData(linkanh);
                    using (var ms = new System.IO.MemoryStream(imageBytes))
                    {
                        Image img = Image.FromStream(ms);
                        imageList.Images.Add(img);
                    }
                }
            }
            catch
            {
                imageList.Images.Add(Image.FromFile(fullImagePath));
            }

            ListViewItem item = new ListViewItem
            {
                ImageIndex = imageList.Images.Count - 1
            };

            item.SubItems.Add(tenungdung);
            item.SubItems.Add(ngaycaidat);
            item.SubItems.Add(package);
            //item.SubItems.Add(capnhatlancuoi);
            //item.SubItems.Add(phienban);

            listView.Items.Add(item);
            return item;
        }

        private void btnTrangTruoc_Click(object sender, EventArgs e)
        {
            btnTrangTiep.Enabled = true;
            if (currentPage > 0)
            {
                currentPage--;
                Add_UngDung(matches, currentPage);
            }
            else
            {
                btnTrangTruoc.Enabled = false;
            }
        }

        private void btnTrangTiep_Click(object sender, EventArgs e)
        {
            btnTrangTruoc.Enabled = true;
            if (currentPage < (matches.Count - 1) / itemsPerPage)
            {
                currentPage++;
                Add_UngDung(matches, currentPage);
            }
            else
            {
                btnTrangTiep.Enabled = false;
            }
        }

        private async void Add_UngDung(MatchCollection matches, int pageNumber)
        {
            Clear_ListView();
            try
            {
                int start = pageNumber * itemsPerPage;
                int end = Math.Min(start + itemsPerPage, matches.Count);

                txtTrangHienTai.Text = $"Trang {pageNumber + 1} / {Math.Ceiling((double)matches.Count / itemsPerPage)}";

                string ngaycaidat = "";
                string capnhatlancuoi = "";
                string phienban = "";

                matchValues.Clear();
                for (int i = start; i < end; i++)
                {
                    matchValues.Add(matches[i].Groups[1].Value);
                }

                apps.Clear();
                apps = await LayThongTinUngDung(matchValues);

                for (int i = start; i < end; i++)
                {
                    query = $"shell dumpsys package {matches[i].Groups[1].Value}";

                    string str2 = adb.adbCommand(query);
                    if (str2 != string.Empty)
                    {
                        ngaycaidat = LayThongTinNgayCaiDat(str2);
                        capnhatlancuoi = LayThongTinNgayCapNhatCuoi(str2);
                        phienban = LayThongTinPhienBan(str2);
                    }
                    else
                    {
                        ngaycaidat = "Không tìm thấy";
                        capnhatlancuoi = "Không tìm thấy";
                        phienban = "Không tìm thấy";
                    }

                    if (apps[i - start].tenungdung != "" && apps[i - start].duongdananh != "")
                    {
                        AddData(matches[i].Groups[1].Value, apps[i - start].tenungdung, ngaycaidat, capnhatlancuoi, phienban, apps[i - start].duongdananh);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.ToString()}", "Lỗi");
            }
        }

        private void listView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView.SelectedItems.Count > 0)
            {
                ListViewItem selectedItem = listView.SelectedItems[0];
                lblTenUngDung.Text = selectedItem.SubItems[1].Text;
                lblGoi.Text = selectedItem.SubItems[3].Text;
                pictureBox_AnhUngDung.Image = selectedItem.ImageList.Images[selectedItem.ImageIndex];
            }
        }

        private void btnChonFolder_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "APK files (*.apk)|*.apk|All files (*.*)|*.*";
                openFileDialog.Title = "Chọn file APK";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    txtPathFileAPK.Text = openFileDialog.FileName;
                    lblDungLuongFileAPK.Text = function.GetFileSizeInMB(openFileDialog.FileName);
                    lblTenFileAPK.Text = Path.GetFileName(openFileDialog.FileName);
                    LoadWeb(Path.GetFileNameWithoutExtension(openFileDialog.FileName.Replace("_APKPure", "")));
                    HienThongTinCaiDat();
                    AnThongBaoCaiDat();
                }
            }
        }

        private async void LoadWeb(string filename)
        {
            try
            {
                string link = $"https://apkpure.net/search?q={filename}";
                await webView21.EnsureCoreWebView2Async(null);
                webView21.CoreWebView2.Navigate(link);
            }
            catch
            {

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
            if (info == "error" && progressBarX_CaiDat.Value == 100)
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
            if (info == "error" && progressBarX_GoCaiDat.Value == 100)
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

        private void lblTenFileAPK_TextChanged(object sender, EventArgs e)
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

        private async Task InstallAPKAsync(string apkPath)
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
                            if (!txtQuaTrinhCaiDat.Text.Contains("Success"))
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
                        sw.WriteLine($"adb install -r -d \"{apkPath}\"");
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
                            Load_UngDung();
                        }
                    }
                }));
            });
        }


        private async void btnCaiDat_Click(object sender, EventArgs e)
        {
            await InstallAPKAsync(txtPathFileAPK.Text);
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
                        sw.WriteLine($"adb uninstall {package}");
                    }
                }

                // Wait for the process to exit
                process.WaitForExit();

                // Update the ProgressBar and TextBox when done
                this.Invoke(new Action(() =>
                {
                    _timer_GoCaiDat.Stop();
                    progressBarX_GoCaiDat.Value = 100;

                    if (txtQuaTrinhGoCaiDat.Text.Contains("Success"))
                    {
                        HienThongBaoGoCaiDat("success");
                        txtQuaTrinhGoCaiDat.AppendText("Uninstall completed" + Environment.NewLine);
                        var result = MessageBox.Show("Reload lại danh sách ứng dụng?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (result == DialogResult.Yes)
                        {
                            Load_UngDung();
                        }
                    }
                }));
            });
        }
    }
}
