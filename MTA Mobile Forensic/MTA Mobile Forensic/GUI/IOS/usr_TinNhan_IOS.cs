using MiniSoftware;
using MTA_Mobile_Forensic.GUI.Share;
using MTA_Mobile_Forensic.Model;
using MTA_Mobile_Forensic.Support;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace MTA_Mobile_Forensic.GUI.IOS
{
    public partial class usr_TinNhan_IOS : UserControl
    {
        public usr_TinNhan_IOS()
        {
            InitializeComponent();
            LoadData();
        }

        function function = new function();
        int itemsPerPage = 20;
        int currentPage = 0;
        List<SmsIOS> smsIOs = new List<SmsIOS>();
        List<SmsIOS> smsIOs_TimKiem;
        api api = new api();
        string pathFile = "";

        public async void LoadData()
        {
            pathFile = function.FindFile(DeviceInfo.pathBackup, "sms.db");
            if (!string.IsNullOrEmpty(pathFile))
            {
                var sms = await api.LayDanhSachTinNhan_IOS(pathFile);
                if (sms != null)
                {
                    for (int i = sms.Count - 1; i >= 0; i--)
                    {
                        if (string.IsNullOrEmpty(sms[i].text))
                        {
                            sms[i].destinationcaller = "Trống";
                        }
                        if (sms[i].service.Trim() == "iMessage")
                        {
                            sms.Remove(sms[i]);
                        }
                        try
                        {
                            int handle = Int32.Parse(sms[i].handle);
                            if (handle < 200)
                            {
                                sms[i].service = "1";
                            }
                            else
                            {
                                sms[i].service = "0";
                            }
                        }
                        catch
                        {
                            sms[i].service = "0";
                        }
                    }
                    smsIOs = sms;
                    Add_usr_TinNhanMini(currentPage);
                }
            }
        }

        private void TimKiemTinNhan()
        {
            currentPage = 0;
            string searchText = txtTimKiem.Text.ToLower();
            GeneralVariables.noidungtimkiem_tinnhan = searchText;

            if (txtTimKiem.Text != String.Empty)
            {
                smsIOs_TimKiem = new List<SmsIOS>();
                foreach (var item in smsIOs)
                {
                    if (item.destinationcaller.ToLower().Contains(searchText) || item.text.ToLower().Contains(searchText))
                    {
                        smsIOs_TimKiem.Add(item);
                    }
                }
                Add_usr_TinNhanMini_TimKiem(currentPage);
            }
            else
            {
                Add_usr_TinNhanMini(currentPage);
            }
        }

        private void btnTrangTruoc_Click(object sender, EventArgs e)
        {
            btnTrangTiep.Enabled = true;
            if (currentPage > 0)
            {
                currentPage--;
                if (txtTimKiem.Text == String.Empty)
                {
                    Add_usr_TinNhanMini(currentPage);
                }
                else
                {
                    Add_usr_TinNhanMini_TimKiem(currentPage);
                }
            }
            else
            {
                btnTrangTruoc.Enabled = false;
            }
        }

        private void btnTrangTiep_Click(object sender, EventArgs e)
        {
            btnTrangTruoc.Enabled = true;
            if (currentPage < (smsIOs.Count - 1) / itemsPerPage)
            {
                currentPage++;
                if (txtTimKiem.Text == String.Empty)
                {
                    Add_usr_TinNhanMini(currentPage);
                }
                else
                {
                    Add_usr_TinNhanMini_TimKiem(currentPage);
                }
            }
            else
            {
                btnTrangTiep.Enabled = false;
            }
        }

        private void Add_usr_TinNhanMini(int pageNumber)
        {
            flpDSTinNhan.Controls.Clear();

            int start = pageNumber * itemsPerPage;
            int end = Math.Min(start + itemsPerPage, smsIOs.Count);

            txtTrangHienTai.Text = $"Trang {pageNumber + 1} / {Math.Ceiling((double)smsIOs.Count / itemsPerPage)}";

            for (int i = start; i < end; i++)
            {
                usr_TinNhanMini usr_TinNhanMini = new usr_TinNhanMini(smsIOs[i].destinationcaller, smsIOs[i].text, function.ConvertToCustomFormat(smsIOs[i].dateread), function.ConvertToCustomFormat(smsIOs[i].date), "", "", smsIOs[i].service, "", Int32.Parse(smsIOs[i].service));
                usr_TinNhanMini.ControlClicked += flpDSTinNhan_Click;
                usr_TinNhanMini.Width = flpDSTinNhan.Width;
                flpDSTinNhan.Controls.Add(usr_TinNhanMini);
            }
        }

        private void Add_usr_TinNhanMini_TimKiem(int pageNumber)
        {
            flpDSTinNhan.Controls.Clear();

            int start = pageNumber * itemsPerPage;
            int end = Math.Min(start + itemsPerPage, smsIOs_TimKiem.Count);

            txtTrangHienTai.Text = $"Trang {pageNumber + 1} / {Math.Ceiling((double)smsIOs_TimKiem.Count / itemsPerPage)}";

            for (int i = start; i < end; i++)
            {
                usr_TinNhanMini usr_TinNhanMini = new usr_TinNhanMini(smsIOs_TimKiem[i].destinationcaller, smsIOs_TimKiem[i].text, function.ConvertToCustomFormat(smsIOs_TimKiem[i].dateread), function.ConvertToCustomFormat(smsIOs_TimKiem[i].date), "", "", smsIOs_TimKiem[i].service, "", Int32.Parse(smsIOs_TimKiem[i].service));
                usr_TinNhanMini.ControlClicked += flpDSTinNhan_Click;
                usr_TinNhanMini.Width = flpDSTinNhan.Width;
                flpDSTinNhan.Controls.Add(usr_TinNhanMini);
            }
        }

        private void flpDSTinNhan_Click(object sender, EventArgs e)
        {
            if (sender is usr_TinNhanMini clickedControl)
            {
                if (txtAddress.Text != clickedControl.diachi)
                {
                    // load tin nhắn vào flpChiTietTinNhan
                    Load_flpChiTietTinNhan(clickedControl.diachi);
                }

                txtAddress.Text = clickedControl.diachi;
                if (clickedControl.sentMessage == 1)
                {
                    lblAddress.Text = "Người gửi";
                }
                else if (clickedControl.sentMessage == 0)
                {
                    lblAddress.Text = "Người nhận";
                }
                txtThoiGianGui.Text = clickedControl.dateSent;
                if (clickedControl.thoigian != "07:00:00 01/01/2001")
                {
                    txtThoiGianNhan.Text = clickedControl.thoigian;
                }
                else
                {
                    txtThoiGianNhan.Text = "Không xác định";
                }
                txtDichVu.Text = "SMS";
            }
        }

        private void Load_flpChiTietTinNhan(string address)
        {
            flpChiTietTinNhan.Controls.Clear();
            try
            {
                foreach (var item in smsIOs)
                {
                    if (item.destinationcaller == address)
                    {
                        if (item.service == "0")
                        {
                            // tin nhắn nhận được
                            usr_TinNhanTroChuyenNhan usr_TinNhanTroChuyenNhan = new usr_TinNhanTroChuyenNhan(item.text, function.ConvertToCustomFormat(item.date), flpChiTietTinNhan.Width);
                            usr_TinNhanTroChuyenNhan.Width = flpChiTietTinNhan.Width;
                            flpChiTietTinNhan.Controls.Add(usr_TinNhanTroChuyenNhan);
                        }
                        else if (item.service == "1")
                        {
                            // tin nhắn gửi đi
                            usr_TinNhanTroChuyenGui usr_TinNhanTroChuyenGui = new usr_TinNhanTroChuyenGui(item.text, function.ConvertToCustomFormat(item.date), flpChiTietTinNhan.Width);
                            usr_TinNhanTroChuyenGui.Width = flpChiTietTinNhan.Width;
                            flpChiTietTinNhan.Controls.Add(usr_TinNhanTroChuyenGui);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.ToString()}", "Lỗi");
            }
        }

        private void flpDSTinNhan_SizeChanged(object sender, EventArgs e)
        {
            foreach (UserControl control in flpDSTinNhan.Controls)
            {
                control.Width = flpDSTinNhan.Width;
            }
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            TimKiemTinNhan();
        }

        private void btnXuat_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                folderDialog.Description = "Chọn thư mục lưu báo cáo";
                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    string selectedPath = folderDialog.SelectedPath;
                    string fileName = $"Báo cáo về tin nhắn của thiết bị_{DeviceInfo.nameDevice}_{DeviceInfo.serialDevice}.docx";
                    string PATH_EXPORT = Path.Combine(selectedPath, fileName);

                    string sourceDirectory = AppDomain.CurrentDomain.BaseDirectory;
                    string projectDirectory = Directory.GetParent(sourceDirectory).Parent.Parent.FullName;
                    string PATH_TEMPLATE = Path.Combine(projectDirectory, "Data", "Document", "report_message.docx");
                    if (File.Exists(PATH_TEMPLATE))
                    {
                        var messages_export = new List<MessageExport>();

                        for (int i = 0; i < smsIOs.Count; i++)
                        {
                            var message_export = new MessageExport
                            {
                                Address = smsIOs[i].destinationcaller,
                                Body = smsIOs[i].text,
                                Date = smsIOs[i].date,
                                SimId = "1"
                            };

                            if (smsIOs[i].service == "1")
                            {
                                message_export.SentMessage = "Gửi";
                            }
                            else if (smsIOs[i].service == "0")
                            {
                                message_export.SentMessage = "Nhận";
                            }
                            messages_export.Add(message_export);
                        }

                        var value = new
                        {
                            ct = messages_export,
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

        private void btnChonTatCa_Click(object sender, EventArgs e)
        {
            foreach (usr_TinNhanMini control in flpDSTinNhan.Controls)
            {
                control.checkBox.Checked = true;
            }
        }

        private void btnBoChonTatCa_Click(object sender, EventArgs e)
        {
            foreach (usr_TinNhanMini control in flpDSTinNhan.Controls)
            {
                control.checkBox.Checked = false;
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            flpDSTinNhan.Controls.Clear();
            flpChiTietTinNhan.Controls.Clear();
            txtAddress.Text = string.Empty;
            txtThoiGianGui.Text = string.Empty;
            txtThoiGianNhan.Text = string.Empty;
            label.Text = string.Empty;
            txtTimKiem.Text = string.Empty;

            LoadData();
        }

        private void txtTimKiem_TextChanged(object sender, EventArgs e)
        {
            if(txtTimKiem.Text == string.Empty)
            {
                flpDSTinNhan.Controls.Clear();
                LoadData();
            }
        }
    }
}
