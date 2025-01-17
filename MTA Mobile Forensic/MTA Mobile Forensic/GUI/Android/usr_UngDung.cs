﻿using MiniSoftware;
using MTA_Mobile_Forensic.GUI.Share;
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
    public partial class usr_UngDung : UserControl
    {
        api api = new api();
        adb adb = new adb();
        string query = "";
        string fullImagePath = "";
        ImageList imageList;
        List<string> matchValues = new List<string>();
        List<string> matchValues_TimKiem = new List<string>();
        List<AppInfo> apps = new List<AppInfo>();
        List<AppInfo> apps_TimKiem = new List<AppInfo>();
        int itemsPerPage = 20;
        int currentPage = 0;
        MatchCollection matches;
        Timer searchTimer;

        public usr_UngDung()
        {
            InitializeComponent();
            Load_UngDung();
            searchTimer = new Timer();
            searchTimer.Interval = 500;
            searchTimer.Tick += SearchTimer_Tick;

            listView.Columns.Add("Ảnh", 70, HorizontalAlignment.Center);
            listView.Columns.Add("Package", 200, HorizontalAlignment.Left);
            listView.Columns.Add("Tên ứng dụng", 300, HorizontalAlignment.Left);
            listView.Columns.Add("Ngày cài đặt", 170, HorizontalAlignment.Center);
            listView.Columns.Add("Cập nhật lần cuối", 170, HorizontalAlignment.Center);
            listView.Columns.Add("Phiên bản", 180, HorizontalAlignment.Center);
        }

        private void Load_UngDung()
        {
            imageList = new ImageList
            {
                ImageSize = new Size(50, 50)
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
            if (cbbTuyChon.Text == "Người dùng")
            {
                query = "shell pm list packages -3";
                string str = adb.adbCommand(query);
                if (str == string.Empty)
                {
                    str = "package:com.miui.screenrecorder\r\npackage:com.android.cts.priv.ctsshim\r\npackage:com.qualcomm.qti.qms.service.telemetry\r\npackage:com.google.android.youtube\r\npackage:com.qualcomm.qti.qcolor\r\npackage:com.android.internal.display.cutout.emulation.corner\r\npackage:com.google.android.ext.services\r\npackage:com.qualcomm.qti.improvetouch.service\r\npackage:com.android.internal.display.cutout.emulation.double\r\npackage:com.android.providers.telephony\r\npackage:com.android.dynsystem\r\npackage:com.miui.powerkeeper\r\npackage:com.xiaomi.miplay_client\r\npackage:com.google.android.googlequicksearchbox\r\npackage:cn.wps.xiaomi.abroad.lite\r\npackage:com.miui.fm\r\npackage:com.t7.busmap\r\npackage:com.android.providers.calendar\r\npackage:com.zing.zalo\r\npackage:org.telegram.messenger\r\npackage:com.android.providers.media";
                }
                string pattern = @"package:(\S+)";

                matches = Regex.Matches(str, pattern);

                Add_UngDung(matches, currentPage);
            }
            else if (cbbTuyChon.Text == "Tất cả")
            {
                query = "shell pm list packages";
                string str = adb.adbCommand(query);
                if (str == string.Empty)
                {
                    str = "package:com.miui.screenrecorder\r\npackage:com.android.cts.priv.ctsshim\r\npackage:com.qualcomm.qti.qms.service.telemetry\r\npackage:com.google.android.youtube\r\npackage:com.qualcomm.qti.qcolor\r\npackage:com.android.internal.display.cutout.emulation.corner\r\npackage:com.google.android.ext.services\r\npackage:com.qualcomm.qti.improvetouch.service\r\npackage:com.android.internal.display.cutout.emulation.double\r\npackage:com.android.providers.telephony\r\npackage:com.android.dynsystem\r\npackage:com.miui.powerkeeper\r\npackage:com.xiaomi.miplay_client\r\npackage:com.google.android.googlequicksearchbox\r\npackage:cn.wps.xiaomi.abroad.lite\r\npackage:com.miui.fm\r\npackage:com.t7.busmap\r\npackage:com.android.providers.calendar\r\npackage:com.zing.zalo\r\npackage:org.telegram.messenger\r\npackage:com.android.providers.media";
                }
                string pattern = @"package:(\S+)";

                matches = Regex.Matches(str, pattern);

                Add_UngDung(matches, currentPage);
            }
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

            item.SubItems.Add(package);
            item.SubItems.Add(tenungdung);
            item.SubItems.Add(ngaycaidat);
            item.SubItems.Add(capnhatlancuoi);
            item.SubItems.Add(phienban);

            listView.Items.Add(item);
            return item;
        }

        private void listView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView.SelectedItems.Count > 0)
            {
                ListViewItem selectedItem = listView.SelectedItems[0];

                frm_XemUngDung frm = new frm_XemUngDung(selectedItem.SubItems[1].Text, selectedItem.SubItems[2].Text, selectedItem.SubItems[3].Text, selectedItem.SubItems[4].Text, selectedItem.SubItems[5].Text);
                frm.Show();
            }
        }

        private void SearchTimer_Tick(object sender, EventArgs e)
        {
            searchTimer.Stop();
            Add_UngDung(matches, currentPage);
        }

        private void btnXuat_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                folderDialog.Description = "Chọn thư mục lưu báo cáo";
                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    string selectedPath = folderDialog.SelectedPath;
                    string fileName = $"Báo cáo về ứng dụng của thiết bị_{DeviceInfo.nameDevice}_{DeviceInfo.serialDevice}.docx";
                    string PATH_EXPORT = Path.Combine(selectedPath, fileName);

                    string sourceDirectory = AppDomain.CurrentDomain.BaseDirectory;
                    string projectDirectory = Directory.GetParent(sourceDirectory).Parent.Parent.FullName;
                    string PATH_TEMPLATE = Path.Combine(projectDirectory, "Data", "Document", "report_application.docx");
                    if (File.Exists(PATH_TEMPLATE))
                    {
                        var aplications_export = new List<object>();
                        foreach (ListViewItem item in listView.Items)
                        {
                            var aplication_export = new
                            {
                                tenungdung = item.SubItems[2].Text,
                                goi = item.SubItems[1].Text,
                                phienban = item.SubItems[5].Text,
                                ngaycaidat = item.SubItems[3].Text,
                                ngaycapnhatcuoi = item.SubItems[4].Text,
                            };
                            aplications_export.Add(aplication_export);

                        }

                        var value = new
                        {
                            ct = aplications_export,
                            phut = DateTime.Now.ToString("mm"),
                            gio = DateTime.Now.ToString("HH"),
                            ngay = DateTime.Now.ToString("dd"),
                            thang = DateTime.Now.ToString("MM"),
                            nam = DateTime.Now.ToString("yyyy"),
                            device_name = DeviceInfo.nameDevice,
                            device_serial = DeviceInfo.serialDevice,
                            path_backup = DeviceInfo.pathBackup,
                        };

                        MiniWord.SaveAsByTemplate(PATH_EXPORT, PATH_TEMPLATE, value);

                        Process.Start(PATH_EXPORT);
                        MessageBox.Show("Xuất file thành công", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("File không tồn tại: " + PATH_EXPORT, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnNhap_Click(object sender, EventArgs e)
        {

        }

        private void btnChonTatCa_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView.Items)
            {
                item.Checked = true;
            }
        }

        private void btnBoChonTatCa_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView.Items)
            {
                item.Checked = false;
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            Clear_ListView();
            Load_UngDung();
        }

        private void btnTrangTruoc_Click(object sender, EventArgs e)
        {
            btnTrangTiep.Enabled = true;
            if (currentPage > 0)
            {
                currentPage--;
                if (txtTimKiem.Text == string.Empty)
                {
                    Add_UngDung(matches, currentPage);
                }
                else
                {
                    Add_UngDung_TimKiem(matches, currentPage);
                }
                searchTimer.Stop();
                searchTimer.Start();
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
                if (txtTimKiem.Text == string.Empty)
                {
                    Add_UngDung(matches, currentPage);
                }
                else
                {
                    Add_UngDung_TimKiem(matches, currentPage);
                }
                searchTimer.Stop();
                searchTimer.Start();
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
                if (apps != null)
                {
                    apps.Clear();
                }

                apps = await LayThongTinUngDung(matchValues);
                if (apps != null)
                {
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
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.ToString()}", "Lỗi");
            }
        }

        private void txtTimKiem_TextChanged(object sender, EventArgs e)
        {
            if (txtTimKiem.Text == String.Empty)
            {
                currentPage = 0;
                Add_UngDung(matches, currentPage);
            }
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            currentPage = 0;
            Add_UngDung_TimKiem(matches, currentPage);
        }

        private async void Add_UngDung_TimKiem(MatchCollection matches, int pageNumber)
        {
            Clear_ListView();
            try
            {
                int start = pageNumber * itemsPerPage;
                int end = Math.Min(start + itemsPerPage, matches.Count);

                string ngaycaidat = "";
                string capnhatlancuoi = "";
                string phienban = "";
                string searchText = txtTimKiem.Text.ToLower();

                matchValues_TimKiem.Clear();
                for (int i = start; i < end; i++)
                {
                    if (matches[i].Groups[1].Value.ToLower().Contains(searchText))
                    {
                        matchValues_TimKiem.Add(matches[i].Groups[1].Value);
                    }
                }

                apps_TimKiem.Clear();
                apps_TimKiem = await LayThongTinUngDung(matchValues_TimKiem);

                end = Math.Min(start + itemsPerPage, matchValues_TimKiem.Count);

                txtTrangHienTai.Text = $"Trang {pageNumber + 1} / {Math.Ceiling((double)matchValues_TimKiem.Count / itemsPerPage)}";

                for (int i = start; i < end; i++)
                {
                    query = $"shell dumpsys package {matchValues_TimKiem[i - start]}";

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

                    if (apps_TimKiem[i - start].tenungdung != "" && apps_TimKiem[i - start].duongdananh != "")
                    {
                        AddData(matchValues_TimKiem[i - start], apps_TimKiem[i - start].tenungdung, ngaycaidat, capnhatlancuoi, phienban, apps_TimKiem[i - start].duongdananh);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.ToString()}", "Lỗi");
            }
        }

        private void cbbTuyChon_SelectedIndexChanged(object sender, EventArgs e)
        {
            Clear_ListView();
            Load_UngDung();
        }
    }
}