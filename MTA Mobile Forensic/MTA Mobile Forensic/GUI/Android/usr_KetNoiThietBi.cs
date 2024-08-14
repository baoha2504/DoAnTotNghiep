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
        libimobiledevice libimobiledevice = new libimobiledevice();
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

        public List<string> GetDeviceList(string adbOutput)
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

        public List<string> CountAndPrintStrings(string input)
        {
            string[] lines = input.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

            List<string> resultList = new List<string>(lines);

            foreach (string line in resultList)
            {
                Console.WriteLine(line);
            }

            return resultList;
        }


        public string GetValueFromInput(string input, string parameter)
        {
            string[] lines = input.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string line in lines)
            {
                string[] parts = line.Split(new[] { ':' }, 2);
                if (parts.Length == 2 && parts[0].Trim() == parameter)
                {
                    return parts[1].Trim();
                }
            }

            return null;
        }

        private static readonly Dictionary<string, string> iPhoneModels = new Dictionary<string, string>
    {
        { "iPhone5,1", "5" },
        { "iPhone5,2", "5" },
        { "iPhone5,3", "5c" },
        { "iPhone5,4", "5c" },
        { "iPhone6,1", "5s" },
        { "iPhone6,2", "5s" },
        { "iPhone7,1", "6 Plus" },
        { "iPhone7,2", "6" },
        { "iPhone8,1", "6s" },
        { "iPhone8,2", "6s Plus" },
        { "iPhone8,4", "SE (1st generation)" },
        { "iPhone9,1", "7" },
        { "iPhone9,2", "7 Plus" },
        { "iPhone9,3", "7" },
        { "iPhone9,4", "7 Plus" },
        { "iPhone10,1", "8" },
        { "iPhone10,2", "8 Plus" },
        { "iPhone10,3", "X" },
        { "iPhone10,4", "8" },
        { "iPhone10,5", "8 Plus" },
        { "iPhone10,6", "X" },
        { "iPhone11,2", "XS" },
        { "iPhone11,4", "XS Max" },
        { "iPhone11,6", "XS Max" },
        { "iPhone11,8", "XR" },
        { "iPhone12,1", "11" },
        { "iPhone12,3", "11 Pro" },
        { "iPhone12,5", "11 Pro Max" },
        { "iPhone12,8", "SE (2nd generation)" },
        { "iPhone13,1", "12 mini" },
        { "iPhone13,2", "12" },
        { "iPhone13,3", "12 Pro" },
        { "iPhone13,4", "12 Pro Max" },
        { "iPhone14,2", "13 Pro" },
        { "iPhone14,3", "13 Pro Max" },
        { "iPhone14,4", "13 mini" },
        { "iPhone14,5", "13" },
        { "iPhone14,6", "SE (3rd generation)" },
        { "iPhone14,7", "14" },
        { "iPhone14,8", "14 Plus" },
        { "iPhone15,2", "14 Pro" },
        { "iPhone15,3", "14 Pro Max" }
    };

        public string XuLyType(string type)
        {
            if (iPhoneModels.TryGetValue(type, out string modelName))
            {
                return modelName;
            }
            else
            {
                return "Unknown iPhone model";
            }
        }

        private void CheckDevices()
        {
            query = "devices";
            string str = adb.adbCommand(query);
            int sothietbi = 0;
            if (str.Trim() != "List of devices attached")
            {
                // có thiết bị
                List<string> deviceList = GetDeviceList(str);
                sothietbi += deviceList.Count;

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

            query = "-l";
            str = libimobiledevice.idevice_idCommand(query);
            List<string> deviceList_IOS = CountAndPrintStrings(str);
            sothietbi += deviceList_IOS.Count;
            foreach (var device in deviceList_IOS)
            {
                string text = libimobiledevice.ideviceinfoCommand($"-u {device}");
                string hangthietbi = GetValueFromInput(text, "DeviceClass").Trim().ToUpper();
                string tenthietbi = GetValueFromInput(text, "DeviceName").Trim();
                string loai = GetValueFromInput(text, "ProductType");
                loai = XuLyType(loai);
                tenthietbi = $"{hangthietbi} {loai} - {tenthietbi}";
                string imei = GetValueFromInput(text, "InternationalMobileEquipmentIdentity");
                string phienban = GetValueFromInput(text, "ProductVersion").Trim();
                if (phienban != string.Empty)
                {
                    phienban = $"IOS {phienban}";
                }
                string capnhatlancuoi = GetValueFromInput(text, "LastBackupDate");
                if (string.IsNullOrEmpty(capnhatlancuoi))
                {
                    capnhatlancuoi = "Chưa cập nhật";
                }
                usr_ThietBiMini usr_ThietBiMini = new usr_ThietBiMini(device, tenthietbi, imei, phienban, capnhatlancuoi);
                usr_ThietBiMini.ControlClicked += flpDSThietBi_Click;
                flpDSThietBi.Controls.Add(usr_ThietBiMini);
            }

            if (sothietbi == 0)
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
                if (tenthietbi.Contains("IPHONE"))
                {
                    frmMain.HienThiForm(1, "IPHONE");
                }
                else
                {
                    frmMain.HienThiForm(1, "ANDROID");
                }
            }
            else
            {
                if (frmMain != null)
                {
                    if (frmMain.lblText != null)
                    {
                        frmMain.lblText.Caption = string.Empty;
                    }

                    if (frmMain.lblTenThietBi != null)
                    {
                        frmMain.lblTenThietBi.Caption = string.Empty;
                    }

                    frmMain.HienThiForm(0, "");
                }
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
                LoadThongTin(clickedControl.loai);
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

        private void LoadThongTin(string type)
        {
            if (type == "ANDROID")
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
            else if (type == "IPHONE")
            {
                string text = libimobiledevice.ideviceinfoCommand($"-u {DeviceInfo.serialDevice}");

                txtHangDienThoai.Text = "IPHONE";
                string loai = GetValueFromInput(text, "ProductType").Trim();
                loai = XuLyType(loai);
                txtModelCuaThietBi.Text = loai;
                txtMaCuaThietBi.Text = GetValueFromInput(text, "UniqueDeviceID").Trim();
                txtTenCuaThietBi.Text = GetValueFromInput(text, "DeviceName").Trim();
                string phienban = GetValueFromInput(text, "ProductVersion").Trim();
                if (phienban != string.Empty)
                {
                    phienban = $"IOS {phienban}";
                }
                txtPhienBanHeDieuHanh.Text = phienban;
                string capnhatlancuoi = GetValueFromInput(text, "LastBackupDate");
                if (string.IsNullOrEmpty(capnhatlancuoi))
                {
                    capnhatlancuoi = "Chưa cập nhật";
                }
                txtNgayCapNhatGanNhat.Text = capnhatlancuoi;
                txtThongTinVeCPU.Text = GetValueFromInput(text, "CPUArchitecture").Trim();
                txtBanBuildHDH.Text = GetValueFromInput(text, "BuildVersion").Trim();
                txtTenPhienBanBuild.Text = "Không tìm thấy";
                txtSoSerialCuaThietBi.Text = GetValueFromInput(text, "SerialNumber").Trim();
                txtPhienBanBootloader.Text = GetValueFromInput(text, "FirmwareVersion").Trim();
                txtThongTinPhanCung.Text = GetValueFromInput(text, "HardwareModel").Trim() + " - " + GetValueFromInput(text, "HardwarePlatform").Trim();
                txtChayTrenGiaLap.Text = "Không";
                txtLoaiBuildHDH.Text = GetValueFromInput(text, "MLBSerialNumber").Trim();
                txtSoIMEI.Text = GetValueFromInput(text, "InternationalMobileEquipmentIdentity").Trim();
            }
        }
    }
}
