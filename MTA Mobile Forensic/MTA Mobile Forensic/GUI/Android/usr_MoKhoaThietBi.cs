using MTA_Mobile_Forensic.Support;
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MTA_Mobile_Forensic.GUI.Android
{
    public partial class usr_MoKhoaThietBi : UserControl
    {
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
        int toado_X = 0;
        int toado_Y = 0;

        adb adb = new adb();

        public usr_MoKhoaThietBi()
        {
            InitializeComponent();
            LaunchScrcpy();
            XacDinhDiemLuot();
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

        private void LaunchScrcpy()
        {
            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = "scrcpy.exe",
                    Arguments = "", // Thêm các tham số cho scrcpy nếu cần
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
                    SetParent(scrcpyHandle, panelScrcpy.Handle);
                    ShowWindow(scrcpyHandle, SW_MAXIMIZE);
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

        private void ResizeScrcpyWindow()
        {
            MoveWindow(scrcpyHandle, 0, 0, panelScrcpy.Width, panelScrcpy.Height, true);
        }

        private void panelEx1_Resize(object sender, EventArgs e)
        {
            try
            {
                splitContainer1.SplitterDistance = (int)(6.5 * panelEx1.Width / 10);
                panel40.Width = (int)((panel39.Width - 15) / 2);
                if (scrcpyHandle != IntPtr.Zero)
                {
                    ShowWindow(scrcpyHandle, SW_MAXIMIZE);
                    ResizeScrcpyWindow();
                }
            }
            catch { }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            LaunchScrcpy();
        }

        private void btnBatSangManHinh_Click(object sender, EventArgs e)
        {
            query = "shell input keyevent 26";
            str = adb.adbCommand(query);
        }

        private void checkBox_TamDung_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_TamDung.Checked == true)
            {
                numericUpDown_TamDung.Visible = true;
                numericUpDown_TamDung.Value = 40;
                txtThongTinTamDung.Visible = true;
            }
            else
            {
                numericUpDown_TamDung.Visible = false;
                numericUpDown_TamDung.Value = 0;
                txtThongTinTamDung.Visible = false;
            }
        }

        private void btnMoKhoaThuCong_Click(object sender, EventArgs e)
        {
            query = "shell input keyevent 26";
            str = adb.adbCommand(query);
            query = $"shell input swipe {toado_X} {toado_Y} {toado_X} 200 500";
            str = adb.adbCommand(query);
            query = $"shell input text \"{txtMatKhauThuCong.Text.Trim()}\"";
            str = adb.adbCommand(query);
        }

        private void btnChonFile_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "APK files (*.txt)|*.txt|All files (*.*)|*.*";
                openFileDialog.Title = "Chọn file .txt";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    txtPathFile.Text = openFileDialog.FileName;
                    pictureBoxFile.Visible = true;
                }
            }
        }

        private async void btnMoKhoaTuDong_Click(object sender, EventArgs e)
        {
            txtQuaTrinhMoKhoaTuDong.Text = string.Empty;
            if (!CheckDisplayPower())
            {
                if (File.Exists(txtPathFile.Text))
                {
                    string query = "shell input keyevent 26";
                    string str = adb.adbCommand(query);

                    query = $"shell input swipe {toado_X} {toado_Y} {toado_X} 200 500";
                    str = adb.adbCommand(query);

                    using (StreamReader sr = new StreamReader(txtPathFile.Text))
                    {
                        string line;
                        int count = 0;
                        while ((line = sr.ReadLine()) != null)
                        {
                            await ThuMatKhau(line);
                            count++;
                            if (count >= 5)
                            {
                                txtThongTinTamDung.Text = "Tạm dừng 40s";
                                await Task.Delay(Int32.Parse(numericUpDown_TamDung.Text) * 1000);

                                query = "shell input keyevent 26";
                                str = adb.adbCommand(query);

                                query = $"shell input swipe {toado_X} {toado_Y} {toado_X} 200 500";
                                str = adb.adbCommand(query);

                                txtThongTinTamDung.Text = string.Empty;
                            }
                            else
                            {
                                txtThongTinTamDung.Text = string.Empty;
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show($"Không tìm thấy file {txtPathFile.Text}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show($"Màn hình đang ở trạng thái khóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private async Task ThuMatKhau(string line)
        {
            await Task.Run(() =>
            {
                query = $"shell input text \"{line.Trim()}\"";
                str = adb.adbCommand(query);

                if (CheckDisplayPower())
                {
                    this.Invoke(new Action(() =>
                    {
                        txtQuaTrinhMoKhoaTuDong.Text += $"►►► Thử nghiệm với mật khẩu {line}" + Environment.NewLine;
                        //break;
                    }));
                }
                else
                {
                    this.Invoke(new Action(() =>
                    {
                        txtQuaTrinhMoKhoaTuDong.Text += $"Thử nghiệm không thành công mật khẩu: {line}" + Environment.NewLine;
                    }));
                }
            });
        }

        private string GetValue(string input, string key)
        {
            string pattern = $@"{key}: (.*?)\r\n";

            Match match = Regex.Match(input, pattern);
            if (match.Success)
            {
                return match.Groups[1].Value;
            }
            else
            {
                return "";
            }
        }

        private bool CheckDisplayPower()
        {
            query = "shell dumpsys power";
            string str_InfoPowerDisplay = adb.adbCommand(query);
            if (str_InfoPowerDisplay == string.Empty)
            {
                str_InfoPowerDisplay = "Display Power: state=OFF\r\n\r\nBattery saving stats:\r\n  Battery Saver is currently: ON\r\n    Last ON time: 2024-07-10 08:02:20.831 -33m9s696ms\r\n    Last OFF time: 2024-07-10 08:02:15.595 -33m14s932ms\r\n    Times enabled: 204";
            }
            str_InfoPowerDisplay += "\r\n";

            string Display_Power = GetValue(str_InfoPowerDisplay, "Display Power");
            if (Display_Power.Replace("state=", "") == "ON") { return true; }
            else { return false; }
        }
    }
}
