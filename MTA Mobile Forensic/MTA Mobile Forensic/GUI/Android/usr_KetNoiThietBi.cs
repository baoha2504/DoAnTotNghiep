using MTA_Mobile_Forensic.GUI.Share;
using MTA_Mobile_Forensic.Model;
using MTA_Mobile_Forensic.Support;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace MTA_Mobile_Forensic.GUI.Android
{
    public partial class usr_KetNoiThietBi : UserControl
    {
        adb adb = new adb();
        string query = "";
        frmMain frmMain;

        public usr_KetNoiThietBi(frmMain fr)
        {
            InitializeComponent();
            CheckDevices();
            frmMain = fr;
        }

        private void panelEx1_Resize(object sender, EventArgs e)
        {
            try
            {
                splitContainer1.SplitterDistance = (int)(panelEx1.Width / 3);
                foreach (UserControl control in flpDSThietBi.Controls)
                {
                    control.Width = flpDSThietBi.Width;
                    panel15.Width = (int)(groupPanel2.Width / 6.5);
                }
            }
            catch { }
        }

        public static List<string> GetDeviceList(string adbOutput)
        {
            List<string> devices = new List<string>();
            string[] lines = adbOutput.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string line in lines)
            {
                if (line.Contains("device") && !line.Contains("List of devices attached"))
                {
                    string[] parts = line.Split(new[] { '\t', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length > 0)
                    {
                        devices.Add(parts[0]); // Lấy ID của thiết bị
                    }
                }
            }

            return devices;
        }

        private void CheckDevices()
        {
            query = "devices";
            string str = adb.adbCommand(query);
            if (str.Trim() != "List of devices attached")
            {
                // có thiết bị
                List<string> deviceList = GetDeviceList(str);

                foreach (var device in deviceList)
                {
                    string thamso = $"-s {device} ";
                    string hangthietbi = adb.adbCommand(thamso + "shell getprop ro.product.brand");
                    string tenthietbi = hangthietbi.ToUpper().Trim() + " " + adb.adbCommand(thamso + "shell getprop ro.product.model").ToUpper().Trim();
                    string imei = adb.adbCommand(thamso + "shell \"service call iphonesubinfo 4 | cut -c 52-66 | tr -d '.[:space:]'\"");
                    string phienban = adb.adbCommand(thamso + "shell getprop ro.build.version.release");
                    if (phienban != string.Empty)
                    {
                        phienban = $"Android {phienban}";
                    }
                    string capnhatlancuoi = adb.adbCommand(thamso + "shell getprop ro.build.version.security_patch");
                    usr_ThietBiMini usr_ThietBiMini = new usr_ThietBiMini(device, tenthietbi, imei, phienban, capnhatlancuoi);
                    usr_ThietBiMini.ControlClicked += flpDSThietBi_Click;
                    flpDSThietBi.Controls.Add(usr_ThietBiMini);
                }

            }
            else
            {
                //không có thiết bị nào
                Label myLabel = new Label();
                myLabel.Text = "Không tìm thấy thiết bị nào";
                myLabel.AutoSize = true;
                myLabel.Font = new Font("Arial", 9);
                myLabel.ForeColor = Color.Red;
                flpDSThietBi.Controls.Add(myLabel);

                LoadCaption_frmMain("");
                DeviceInfo.serialDevice = string.Empty;
                DeviceInfo.nameDevice = string.Empty;
            }
        }

        private void LoadCaption_frmMain(string tenthietbi)
        {
            if (tenthietbi != string.Empty)
            {
                frmMain.lblText.Caption = "Thiết bị kết nối:";
                frmMain.lblTenThietBi.Caption = tenthietbi.Trim();
                frmMain.HienThiForm(1);
            }
            else
            {
                frmMain.lblText.Caption = string.Empty;
                frmMain.lblTenThietBi.Caption = string.Empty;
                frmMain.HienThiForm(0);
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            flpDSThietBi.Controls.Clear();
            pictureBox_Phone.Image = null;
            XoaText();
            CheckDevices();
            LoadCaption_frmMain("");
        }

        private void flpDSThietBi_Click(object sender, EventArgs e)
        {
            if (sender is usr_ThietBiMini clickedControl)
            {
                pictureBox_Phone.Image = Image.FromFile(clickedControl.pathImage);
                XoaText();
                DeviceInfo.serialDevice = clickedControl.idthietbi.Trim();
                DeviceInfo.nameDevice = clickedControl.tenthietbi;
                LoadThongTin();
                LoadCaption_frmMain(clickedControl.tenthietbi);
            }
        }

        private void XoaText()
        {
            txtHangDienThoai.Text = string.Empty;
            txtModelCuaThietBi.Text = string.Empty;
            txtMaCuaThietBi.Text = string.Empty;
            txtTenCuaThietBi.Text = string.Empty;
            txtPhienBanHeDieuHanh.Text = string.Empty;
            txtNgayCapNhatGanNhat.Text = string.Empty;
            txtThongTinVeCPU.Text = string.Empty;
            txtBanBuildHDH.Text = string.Empty;
            txtTenPhienBanBuild.Text = string.Empty;
            txtSoSerialCuaThietBi.Text = string.Empty;
            txtPhienBanBootloader.Text = string.Empty;
            txtThongTinPhanCung.Text = string.Empty;
            txtChayTrenGiaLap.Text = string.Empty;
            txtLoaiBuildHDH.Text = string.Empty;
            txtSoIMEI.Text = string.Empty;
        }

        private void LoadThongTin()
        {
            txtHangDienThoai.Text = adb.adbCommand("shell getprop ro.product.brand").Trim().ToUpper();
            txtModelCuaThietBi.Text = adb.adbCommand("shell getprop ro.product.model").Trim();
            txtMaCuaThietBi.Text = adb.adbCommand("shell getprop ro.product.device").Trim();
            txtTenCuaThietBi.Text = adb.adbCommand("shell getprop ro.product.name").Trim();
            txtPhienBanHeDieuHanh.Text = "Android " + adb.adbCommand("shell getprop ro.build.version.release").Trim();
            txtNgayCapNhatGanNhat.Text = adb.adbCommand("shell getprop ro.build.version.security_patch").Trim();
            txtThongTinVeCPU.Text = adb.adbCommand("shell getprop ro.product.cpu.abi").Trim();
            txtBanBuildHDH.Text = adb.adbCommand("shell getprop ro.build.id").Trim();
            txtTenPhienBanBuild.Text = adb.adbCommand("shell getprop ro.build.display.id").Trim();
            txtSoSerialCuaThietBi.Text = adb.adbCommand("shell getprop ro.boot.serialno").Trim();
            txtPhienBanBootloader.Text = adb.adbCommand("shell getprop ro.bootloader").Trim();
            txtThongTinPhanCung.Text = adb.adbCommand("shell getprop ro.hardware").Trim();
            string chaygialap = adb.adbCommand("shell getprop ro.kernel.qemu").Trim();
            if (chaygialap == "0")
            {
                txtChayTrenGiaLap.Text = "Không";
            }
            else if (chaygialap == "1")
            {
                txtChayTrenGiaLap.Text = "Đang chạy";
            }
            else
            {
                txtChayTrenGiaLap.Text = "Không xác định";
            }
            txtLoaiBuildHDH.Text = adb.adbCommand("shell getprop ro.build.type").Trim();
            txtSoIMEI.Text = adb.adbCommand("shell \"service call iphonesubinfo 4 | cut -c 52-66 | tr -d '.[:space:]'\"").Trim();
        }
    }
}
