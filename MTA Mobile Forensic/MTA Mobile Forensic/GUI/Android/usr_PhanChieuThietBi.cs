using MTA_Mobile_Forensic.GUI.Share;
using MTA_Mobile_Forensic.Model;
using MTA_Mobile_Forensic.Support;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace MTA_Mobile_Forensic.GUI.Android
{
    public partial class usr_PhanChieuThietBi : UserControl
    {
        // Import các hàm từ User32.dll để xử lý cửa sổ
        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool SetWindowLong(IntPtr hWnd, int nIndex, uint dwNewLong);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        private const int GWL_STYLE = -16;
        private const uint WS_VISIBLE = 0x10000000;
        private const int SW_MAXIMIZE = 3;
        private IntPtr scrcpyHandle = IntPtr.Zero;

        string query = "";
        string str = "";
        string folderPath = "";
        Process adbProcess;
        List<string> fullPathImages_Videos = new List<string>();
        int toado_X = 0;
        int toado_Y = 0;
        adb adb = new adb();
        function function = new function();
        System.Windows.Forms.Timer autoScrollTimer_LuotLen;
        System.Windows.Forms.Timer autoScrollTimer_LuotXuong;

        System.Windows.Forms.Timer autoScrollTimer_LuotLen_ScreenShot;
        System.Windows.Forms.Timer autoScrollTimer_LuotXuong_ScreenShot;

        public usr_PhanChieuThietBi()
        {
            InitializeComponent();

            LaunchScrcpy();
            XacDinhDiemLuot();

            autoScrollTimer_LuotLen = new System.Windows.Forms.Timer();
            autoScrollTimer_LuotLen.Interval = 1000;
            autoScrollTimer_LuotLen.Tick += AutoScrollTimer_LuotLen_Tick;

            autoScrollTimer_LuotXuong = new System.Windows.Forms.Timer();
            autoScrollTimer_LuotXuong.Interval = 1000;
            autoScrollTimer_LuotXuong.Tick += AutoScrollTimer_LuotXuong_Tick;

            autoScrollTimer_LuotLen_ScreenShot = new System.Windows.Forms.Timer();
            autoScrollTimer_LuotLen_ScreenShot.Interval = 1000;
            autoScrollTimer_LuotLen_ScreenShot.Tick += AutoScrollTimer_LuotLen_ScreenShot_Tick;

            autoScrollTimer_LuotXuong_ScreenShot = new System.Windows.Forms.Timer();
            autoScrollTimer_LuotXuong_ScreenShot.Interval = 1000;
            autoScrollTimer_LuotXuong_ScreenShot.Tick += AutoScrollTimer_LuotXuong_ScreenShot_Tick;
        }

        private void LaunchScrcpy()
        {
            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = "scrcpy.exe",
                    Arguments = $"-s {DeviceInfo.serialDevice}", // Thêm các tham số cho scrcpy nếu cần
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                };

                Process process = new Process
                {
                    StartInfo = startInfo
                };
                process.Start();

                Thread.Sleep(2000);

                IntPtr scrcpyHandle = process.MainWindowHandle;
                if (scrcpyHandle == IntPtr.Zero)
                {
                    scrcpyHandle = FindScrcpyWindow();
                }

                if (scrcpyHandle != IntPtr.Zero)
                {
                    // Nhúng cửa sổ scrcpy vào Panel
                    SetParent(scrcpyHandle, panelScrcpy.Handle);

                    // Đảm bảo cửa sổ scrcpy được hiển thị và ở chế độ maximized
                    ShowWindow(scrcpyHandle, SW_MAXIMIZE);

                    // Điều chỉnh kích thước và vị trí của cửa sổ scrcpy để nó lấp đầy Panel
                    ResizeScrcpyWindow();
                }
                else
                {
                    MessageBox.Show("Không tìm thấy cửa sổ scrcpy.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Có lỗi xảy ra: " + ex.Message);
            }
        }

        private IntPtr FindScrcpyWindow()
        {
            IntPtr hWnd = IntPtr.Zero;
            foreach (Process pList in Process.GetProcesses())
            {
                if (pList.MainWindowTitle.Contains("scrcpy"))
                {
                    hWnd = pList.MainWindowHandle;
                    break;
                }
            }
            return hWnd;
        }

        private void panelEx1_Resize(object sender, EventArgs e)
        {
            try
            {
                splitContainer1.SplitterDistance = (int)(6.5 * panelEx1.Width / 10);
                if (scrcpyHandle != IntPtr.Zero)
                {
                    ShowWindow(scrcpyHandle, SW_MAXIMIZE);
                    ResizeScrcpyWindow();
                }
            }
            catch { }
        }

        private void ResizeScrcpyWindow()
        {
            MoveWindow(scrcpyHandle, 0, 0, panelScrcpy.Width, panelScrcpy.Height, true);
        }

        private void btnBatSangManHinh_Click(object sender, EventArgs e)
        {
            query = "shell input keyevent 26";
            str = adb.adbCommand(query);
        }

        private void btnKhoaManHinh_Click(object sender, EventArgs e)
        {
            query = "shell input keyevent 26";
            str = adb.adbCommand(query);
        }

        private void XacDinhDiemLuot()
        {
            query = "shell wm size";
            str = adb.adbCommand(query);
            if (str == string.Empty)
            {
                str = "Physical size: 720x1480";
            }
            str = str.Replace("Physical size:", "").Trim();

            string[] parts = str.Split('x');
            int width = int.Parse(parts[0]);
            int height = int.Parse(parts[1]);

            toado_X = width - 200;
            toado_Y = height - 200;
        }

        private void btnLuotTuDong_Click(object sender, EventArgs e)
        {
            if (btnLuotTuDong.Text == "Hủy lướt tự động")
            {
                btnLuotTuDong.Text = "Lướt tự động";
                if (autoScrollTimer_LuotLen.Enabled == true)
                {
                    autoScrollTimer_LuotLen.Stop();
                }

                if (autoScrollTimer_LuotXuong.Enabled == true)
                {
                    autoScrollTimer_LuotXuong.Stop();
                }
            }
        }

        private void btnLuotLen_Click(object sender, EventArgs e)
        {
            if (btnLuotTuDong.Text == "Lướt tự động")
            {
                btnLuotTuDong.Text = "Hủy lướt tự động";
                autoScrollTimer_LuotLen.Start();
            }
        }

        private void btnLuotXuong_Click(object sender, EventArgs e)
        {
            if (btnLuotTuDong.Text == "Lướt tự động")
            {
                btnLuotTuDong.Text = "Hủy lướt tự động";
                autoScrollTimer_LuotXuong.Start();
            }
        }

        private void AutoScrollTimer_LuotLen_Tick(object sender, EventArgs e)
        {
            query = $"shell input swipe {toado_X} {toado_Y} {toado_X} 200 500";
            str = adb.adbCommand(query);
        }

        private void AutoScrollTimer_LuotXuong_Tick(object sender, EventArgs e)
        {
            query = $"shell input swipe {toado_X} 200 {toado_X} {toado_Y} 500";
            str = adb.adbCommand(query);
        }

        private void AutoScrollTimer_LuotLen_ScreenShot_Tick(object sender, EventArgs e)
        {
            query = $"shell input swipe {toado_X} {toado_Y} {toado_X} 200 500";
            str = adb.adbCommand(query);
            btnChupAnhManHinh_Click(sender, e);
        }

        private void AutoScrollTimer_LuotXuong_ScreenShot_Tick(object sender, EventArgs e)
        {
            query = $"shell input swipe {toado_X} 200 {toado_X} {toado_Y} 500";
            str = adb.adbCommand(query);
            btnChupAnhManHinh_Click(sender, e);
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            LaunchScrcpy();
        }

        private void btnLuotVaChup_Click(object sender, EventArgs e)
        {
            if (btnLuotVaChup.Text == "Hủy lướt và chụp")
            {
                btnLuotVaChup.Text = "Lướt và chụp";
                if (autoScrollTimer_LuotLen_ScreenShot.Enabled == true)
                {
                    autoScrollTimer_LuotLen_ScreenShot.Stop();
                }

                if (autoScrollTimer_LuotXuong_ScreenShot.Enabled == true)
                {
                    autoScrollTimer_LuotXuong_ScreenShot.Stop();
                }
            }
        }

        private void btnLuotLenChup_Click(object sender, EventArgs e)
        {
            if (btnLuotVaChup.Text == "Lướt và chụp")
            {
                btnLuotVaChup.Text = "Hủy lướt và chụp";
                autoScrollTimer_LuotLen_ScreenShot.Start();
            }

        }

        private void btnLuotXuongChup_Click(object sender, EventArgs e)
        {
            if (btnLuotVaChup.Text == "Lướt và chụp")
            {
                btnLuotVaChup.Text = "Hủy lướt và chụp";
                autoScrollTimer_LuotXuong_ScreenShot.Start();
            }
        }

        private void btnChupAnhManHinh_Click(object sender, EventArgs e)
        {
            string sourceDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string projectDirectory = Directory.GetParent(sourceDirectory).Parent.Parent.FullName;
            string filePath = Path.Combine(projectDirectory, "Cache");

            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            DateTime dateTime = DateTime.Now;
            string fileName = dateTime.ToString("HH.mm.ss_dd.MM.yyyy") + ".png";
            string fullFilePath = Path.Combine(filePath, fileName);

            string tempFilePath = "/sdcard/screenshot.png";

            string captureCommand = $"shell screencap -p {tempFilePath}";
            adb.adbCommand(captureCommand);

            string pullCommand = $"pull {tempFilePath} \"{fullFilePath}\"";
            string result = adb.adbCommand(pullCommand);
            fullPathImages_Videos.Add(fullFilePath);
            AddImageTo_flpDuLieuThuDuoc(fullFilePath, Path.GetFileName(fullFilePath), function.GetLastModified(fullFilePath));
        }

        private void AddImageTo_flpDuLieuThuDuoc(string path, string filename, string lastmodified)
        {
            usr_AnhMini usr_AnhMini = new usr_AnhMini(path, filename, lastmodified);
            usr_AnhMini.ControlClicked += flpDuLieuThuDuoc_Click;
            flpDuLieuThuDuoc.Controls.Add(usr_AnhMini);
        }

        private void AddVideoTo_flpDuLieuThuDuoc(string path, string filename, string lastmodified)
        {
            usr_VideoMini usr_VideoMini = new usr_VideoMini(path, filename, lastmodified);
            usr_VideoMini.ControlClicked += flpDuLieuThuDuoc_Click;
            flpDuLieuThuDuoc.Controls.Add(usr_VideoMini);
        }

        private void flpDuLieuThuDuoc_Click(object sender, EventArgs e)
        {
            if (sender is UserControl clickedControl)
            {
                int index = flpDuLieuThuDuoc.Controls.GetChildIndex(clickedControl); // Lấy chỉ số của clickedControl

                if (clickedControl is usr_AnhMini)
                {
                    frm_XemAnh frm_XemAnh = new frm_XemAnh(fullPathImages_Videos[index]);
                    frm_XemAnh.Show();
                }
                else if (clickedControl is usr_VideoMini)
                {
                    frm_XemVideo frm_XemVideo = new frm_XemVideo(fullPathImages_Videos[index]);
                    frm_XemVideo.Show();
                }
            }
        }

        private void btnQuayVideoManHinh_Click(object sender, EventArgs e)
        {
            if (btnQuayVideoManHinh.Text == "Quay video màn hình")
            {

                string deviceSerial = GetDeviceSerial();
                if (string.IsNullOrEmpty(deviceSerial))
                {
                    MessageBox.Show("Không tìm thấy số Serial của thiết bị", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    btnQuayVideoManHinh.Text = "Hủy quay video";
                    Hien_panel_RecordScreen();
                    string outputFilePath = "/sdcard/screenrecord.mp4";
                    StartScreenRecording(deviceSerial, outputFilePath);
                }
            }
            else if (btnQuayVideoManHinh.Text == "Hủy quay video")
            {
                btnQuayVideoManHinh.Text = "Quay video màn hình";
                An_panel_RecordScreen();
                StopScreenRecording();

                string sourceDirectory = AppDomain.CurrentDomain.BaseDirectory;
                string projectDirectory = Directory.GetParent(sourceDirectory).Parent.Parent.FullName;
                string filePath = Path.Combine(projectDirectory, "Cache");

                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }

                DateTime dateTime = DateTime.Now;
                string fileName = dateTime.ToString("HH.mm.ss_dd.MM.yyyy") + ".mp4";
                string fullFilePath = Path.Combine(filePath, fileName);
                string outputFilePath = "/sdcard/screenrecord.mp4";

                string pullCommand = $"pull {outputFilePath} \"{fullFilePath}\"";
                string result = adb.adbCommand(pullCommand);

                fullPathImages_Videos.Add(fullFilePath);
                AddVideoTo_flpDuLieuThuDuoc(fullFilePath, Path.GetFileNameWithoutExtension(fullFilePath), function.GetLastModified(fullFilePath));
            }
        }

        public string GetDeviceSerial()
        {
            query = "devices";
            str = adb.adbCommand(query);

            var lines = str.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            var deviceLine = lines.FirstOrDefault(line => line.EndsWith("device"));
            string deviceSerial = "";
            if (deviceLine != null)
            {
                deviceSerial = deviceLine.Split('\t').First();
            }
            return deviceSerial;
        }

        public void StartScreenRecording(string deviceSerial, string outputFilePath)
        {
            adbProcess = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "adb",
                    Arguments = $"-s {deviceSerial} shell screenrecord {outputFilePath}",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };
            adbProcess.Start();
        }

        public void StopScreenRecording()
        {
            if (adbProcess != null && !adbProcess.HasExited)
            {
                adbProcess.Kill();
                adbProcess.WaitForExit();
                adbProcess = null;
            }
        }

        private void An_panel_RecordScreen()
        {
            panel_RecordScreen.Visible = false;
        }

        private void Hien_panel_RecordScreen()
        {
            panel_RecordScreen.Visible = true;
        }

        private void btnChonTatCa_Click(object sender, EventArgs e)
        {
            foreach (UserControl control in flpDuLieuThuDuoc.Controls)
            {
                if (control is usr_AnhMini anhMiniControl)
                {
                    anhMiniControl.checkBox.Checked = true;
                }
                else if (control is usr_VideoMini videoMiniControl)
                {
                    videoMiniControl.checkBox.Checked = true;
                }
            }
        }


        private void btnBoChonTatCa_Click(object sender, EventArgs e)
        {
            foreach (UserControl control in flpDuLieuThuDuoc.Controls)
            {
                if (control is usr_AnhMini anhMiniControl)
                {
                    anhMiniControl.checkBox.Checked = false;
                }
                else if (control is usr_VideoMini videoMiniControl)
                {
                    videoMiniControl.checkBox.Checked = false;
                }
            }
        }

        private void btnLayDuLieu_Click(object sender, EventArgs e)
        {
            int index = 0;
            int index_checkBox = 0;
            using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
            {
                folderBrowserDialog.Description = "Chọn thư mục lưu dữ liệu thu được";
                DialogResult result = folderBrowserDialog.ShowDialog();
                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(folderBrowserDialog.SelectedPath))
                {
                    folderPath = folderBrowserDialog.SelectedPath;

                    foreach (UserControl control in flpDuLieuThuDuoc.Controls)
                    {
                        if (control is usr_AnhMini anhMiniControl)
                        {
                            if (anhMiniControl.checkBox.Checked == true)
                            {
                                index_checkBox++;
                                CopyFileToFolder(fullPathImages_Videos[index], folderPath);
                            }
                        }
                        else if (control is usr_VideoMini videoMiniControl)
                        {
                            if (videoMiniControl.checkBox.Checked == true)
                            {
                                index_checkBox++;
                                CopyFileToFolder(fullPathImages_Videos[index], folderPath);
                            }
                        }
                        index++;
                    }
                    if (index_checkBox == 0)
                    {
                        MessageBox.Show("Chưa chọn dữ liệu để lưu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        MessageBox.Show($"Lưu thành công {index_checkBox} file đến {folderPath}");
                    }
                }
            }
        }

        public void CopyFileToFolder(string filePath, string folderPath)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    throw new FileNotFoundException("The source file does not exist.", filePath);
                }

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                string fileName = Path.GetFileName(filePath);
                string destFilePath = Path.Combine(folderPath, fileName);

                File.Copy(filePath, destFilePath, true);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        private void btnXoaDuLieuThuDuoc_Click(object sender, EventArgs e)
        {
            flpDuLieuThuDuoc.Controls.Clear();
        }
    }
}
