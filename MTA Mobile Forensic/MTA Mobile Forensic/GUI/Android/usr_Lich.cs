using MiniSoftware;
using MTA_Mobile_Forensic.GUI.Share;
using MTA_Mobile_Forensic.Model;
using MTA_Mobile_Forensic.Support;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace MTA_Mobile_Forensic.GUI.Android
{
    public partial class usr_Lich : UserControl
    {
        public usr_Lich()
        {
            InitializeComponent();

            searchTimer = new Timer();
            searchTimer.Interval = 1000; // 1 giây
            searchTimer.Tick += SearchTimer_Tick;
        }

        adb adb = new adb();
        function function = new function();
        string query = "";
        Timer searchTimer;


        private void Load_flpLich()
        {
            query = "shell content query --uri content://com.android.calendar/events --projection account_type:account_name:lastDate:dtstart:dtend:title:calendar_displayName:eventTimezone:calendar_timezone:dirty";
            string str = adb.adbCommand(query);
            if (str == string.Empty)
            { 
                str = "Row: 0 account_type=com.google, account_name=hacongquoclun1@gmail.com, lastDate=1549843200000, dtstart=1549756800000, dtend=1549843200000, title=Tết Nguyên Đán, calendar_displayName=Ngày lễ ở Việt Nam, eventTimezone=UTC, calendar_timezone=Asia/Ho_Chi_Minh, dirty=0\r\nRow: 1 account_type=com.google, account_name=hacongquoclun1@gmail.com, lastDate=1556668800000, dtstart=1556582400000, dtend=1556668800000, title=Ngày giải phóng, calendar_displayName=Ngày lễ ở Việt Nam, eventTimezone=UTC, calendar_timezone=Asia/Ho_Chi_Minh, dirty=0\r\nRow: 2 account_type=com.google, account_name=hacongquoclun1@gmail.com, lastDate=1567296000000, dtstart=1567209600000, dtend=1567296000000, title=Quốc khánh (ngày lễ bổ sung), calendar_displayName=Ngày lễ ở Việt Nam, eventTimezone=UTC, calendar_timezone=Asia/Ho_Chi_Minh, dirty=0\r\nRow: 3 account_type=com.google, account_name=hacongquoclun1@gmail.com, lastDate=1577232000000, dtstart=1577145600000, dtend=1577232000000, title=Đêm Giáng sinh, calendar_displayName=Ngày lễ ở Việt Nam, eventTimezone=UTC, calendar_timezone=Asia/Ho_Chi_Minh, dirty=0\r\nRow: 4 account_type=com.google, account_name=hacongquoclun1@gmail.com, lastDate=1577318400000, dtstart=1577232000000, dtend=1577318400000, title=Giáng sinh/Nôen, calendar_displayName=Ngày lễ ở Việt Nam, eventTimezone=UTC, calendar_timezone=Asia/Ho_Chi_Minh, dirty=0\r\nRow: 5 account_type=com.google, account_name=hacongquoclun1@gmail.com, lastDate=1579910400000, dtstart=1579824000000, dtend=1579910400000, title=Tết đêm giao thừa, calendar_displayName=Ngày lễ ở Việt Nam, eventTimezone=UTC, calendar_timezone=Asia/Ho_Chi_Minh, dirty=0\r\nRow: 6 account_type=com.google, account_name=hacongquoclun1@gmail.com, lastDate=1588291200000, dtstart=1588204800000, dtend=1588291200000, title=Ngày giải phóng, calendar_displayName=Ngày lễ ở Việt Nam, eventTimezone=UTC, calendar_timezone=Asia/Ho_Chi_Minh, dirty=0\r\nRow: 7 account_type=com.google, account_name=hacongquoclun1@gmail.com, lastDate=1608854400000, dtstart=1608768000000, dtend=1608854400000, title=Đêm Giáng sinh, calendar_displayName=Ngày lễ ở Việt Nam, eventTimezone=UTC, calendar_timezone=Asia/Ho_Chi_Minh, dirty=0\r\nRow: 8 account_type=com.google, account_name=hacongquoclun1@gmail.com, lastDate=1608940800000, dtstart=1608854400000, dtend=1608940800000, title=Giáng sinh/Nôen, calendar_displayName=Ngày lễ ở Việt Nam, eventTimezone=UTC, calendar_timezone=Asia/Ho_Chi_Minh, dirty=0\r\nRow: 9 account_type=com.google, account_name=hacongquoclun1@gmail.com, lastDate=1609545600000, dtstart=1609459200000, dtend=1609545600000, title=Tết dương lịch, calendar_displayName=Ngày lễ ở Việt Nam, eventTimezone=UTC, calendar_timezone=Asia/Ho_Chi_Minh, dirty=0\r\nRow: 10 account_type=com.google, account_name=hacongquoclun1@gmail.com, lastDate=1613174400000, dtstart=1613088000000, dtend=1613174400000, title=Tết Nguyên Đán, calendar_displayName=Ngày lễ ở Việt Nam, eventTimezone=UTC, calendar_timezone=Asia/Ho_Chi_Minh, dirty=0\r\nRow: 11 account_type=com.google, account_name=hacongquoclun1@gmail.com, lastDate=1619827200000, dtstart=1619740800000, dtend=1619827200000, title=Ngày giải phóng, calendar_displayName=Ngày lễ ở Việt Nam, eventTimezone=UTC, calendar_timezone=Asia/Ho_Chi_Minh, dirty=0\r\nRow: 12 account_type=com.google, account_name=hacongquoclun1@gmail.com, lastDate=1630713600000, dtstart=1630627200000, dtend=1630713600000, title=Quốc khánh (ngày lễ bổ sung), calendar_displayName=Ngày lễ ở Việt Nam, eventTimezone=UTC, calendar_timezone=Asia/Ho_Chi_Minh, dirty=0\r\nRow: 13 account_type=com.google, account_name=hacongquoclun1@gmail.com, lastDate=1640390400000, dtstart=1640304000000, dtend=1640390400000, title=Đêm Giáng sinh, calendar_displayName=Ngày lễ ở Việt Nam, eventTimezone=UTC, calendar_timezone=Asia/Ho_Chi_Minh, dirty=0\r\nRow: 14 account_type=com.google, account_name=hacongquoclun1@gmail.com, lastDate=1640995200000, dtstart=1640908800000, dtend=1640995200000, title=Đêm giao thừa, calendar_displayName=Ngày lễ ở Việt Nam, eventTimezone=UTC, calendar_timezone=Asia/Ho_Chi_Minh, dirty=0\r\nRow: 15 account_type=com.google, account_name=hacongquoclun1@gmail.com, lastDate=1641081600000, dtstart=1640995200000, dtend=1641081600000, title=Tết dương lịch, calendar_displayName=Ngày lễ ở Việt Nam, eventTimezone=UTC, calendar_timezone=Asia/Ho_Chi_Minh, dirty=0\r\nRow: 16 account_type=com.google, account_name=hacongquoclun1@gmail.com, lastDate=1643673600000, dtstart=1643587200000, dtend=1643673600000, title=Tết đêm giao thừa, calendar_displayName=Ngày lễ ở Việt Nam, eventTimezone=UTC, calendar_timezone=Asia/Ho_Chi_Minh, dirty=0\r\nRow: 17 account_type=com.google, account_name=hacongquoclun1@gmail.com, lastDate=1643846400000, dtstart=1643760000000, dtend=1643846400000, title=Tết Nguyên Đán, calendar_displayName=Ngày lễ ở Việt Nam, eventTimezone=UTC, calendar_timezone=Asia/Ho_Chi_Minh, dirty=0\r\nRow: 18 account_type=com.google, account_name=hacongquoclun1@gmail.com, lastDate=1643932800000, dtstart=1643846400000, dtend=1643932800000, title=Tết Nguyên Đán, calendar_displayName=Ngày lễ ở Việt Nam, eventTimezone=UTC, calendar_timezone=Asia/Ho_Chi_Minh, dirty=0\r\nRow: 19 account_type=com.google, account_name=hacongquoclun1@gmail.com, lastDate=1644019200000, dtstart=1643932800000, dtend=1644019200000, title=Tết Nguyên Đán, calendar_displayName=Ngày lễ ở Việt Nam, eventTimezone=UTC, calendar_timezone=Asia/Ho_Chi_Minh, dirty=0\r\nRow: 20 account_type=com.google, account_name=hacongquoclun1@gmail.com, lastDate=1649635200000, dtstart=1649548800000, dtend=1649635200000, title=Giỗ tổ Hùng Vương, calendar_displayName=Ngày lễ ở Việt Nam, eventTimezone=UTC, calendar_timezone=Asia/Ho_Chi_Minh, dirty=0\r\nRow: 21 account_type=com.google, account_name=hacongquoclun1@gmail.com, lastDate=1651363200000, dtstart=1651276800000, dtend=1651363200000, title=Ngày giải phóng, calendar_displayName=Ngày lễ ở Việt Nam, eventTimezone=UTC, calendar_timezone=Asia/Ho_Chi_Minh, dirty=0\r\nRow: 22 account_type=com.google, account_name=hacongquoclun1@gmail.com, lastDate=1651536000000, dtstart=1651449600000, dtend=1651536000000, title=ngày nghỉ Ngày giải phóng, calendar_displayName=Ngày lễ ở Việt Nam, eventTimezone=UTC, calendar_timezone=Asia/Ho_Chi_Minh, dirty=0\r\nRow: 23 account_type=com.google, account_name=hacongquoclun1@gmail.com, lastDate=1651622400000, dtstart=1651536000000, dtend=1651622400000, title=ngày nghỉ Ngày Quốc tế Lao động, calendar_displayName=Ngày lễ ở Việt Nam, eventTimezone=UTC, calendar_timezone=Asia/Ho_Chi_Minh, dirty=0\r\nRow: 24 account_type=com.google, account_name=hacongquoclun1@gmail.com, lastDate=1672012800000, dtstart=1671926400000, dtend=1672012800000, title=Giáng sinh/Nôen, calendar_displayName=Ngày lễ ở Việt Nam, eventTimezone=UTC, calendar_timezone=Asia/Ho_Chi_Minh, dirty=0\r\nRow: 25 account_type=com.google, account_name=hacongquoclun1@gmail.com, lastDate=1674345600000, dtstart=1674259200000, dtend=1674345600000, title=Tết đêm giao thừa, calendar_displayName=Ngày lễ ở Việt Nam, eventTimezone=UTC, calendar_timezone=Asia/Ho_Chi_Minh, dirty=0\r\nRow: 26 account_type=com.google, account_name=hacongquoclun1@gmail.com, lastDate=1674604800000, dtstart=1674518400000, dtend=1674604800000, title=Tết Nguyên Đán, calendar_displayName=Ngày lễ ở Việt Nam, eventTimezone=UTC, calendar_timezone=Asia/Ho_Chi_Minh, dirty=0\r\nRow: 27 account_type=com.google, account_name=hacongquoclun1@gmail.com, lastDate=1674691200000, dtstart=1674604800000, dtend=1674691200000, title=Tết Nguyên Đán, calendar_displayName=Ngày lễ ở Việt Nam, eventTimezone=UTC, calendar_timezone=Asia/Ho_Chi_Minh, dirty=0\r\nRow: 28 account_type=com.google, account_name=hacongquoclun1@gmail.com, lastDate=1693699200000, dtstart=1693612800000, dtend=1693699200000, title=Quốc khánh, calendar_displayName=Ngày lễ ở Việt Nam, eventTimezone=UTC, calendar_timezone=Asia/Ho_Chi_Minh, dirty=0\r\nRow: 29 account_type=com.google, account_name=hacongquoclun1@gmail.com, lastDate=1713484800000, dtstart=1713398400000, dtend=1713484800000, title=Giỗ tổ Hùng Vương, calendar_displayName=Ngày lễ ở Việt Nam, eventTimezone=UTC, calendar_timezone=Asia/Ho_Chi_Minh, dirty=0\r\nRow: 30 account_type=com.google, account_name=hacongquoclun1@gmail.com, lastDate=1725321600000, dtstart=1725235200000, dtend=1725321600000, title=Quốc khánh, calendar_displayName=Ngày lễ ở Việt Nam, eventTimezone=UTC, calendar_timezone=Asia/Ho_Chi_Minh, dirty=0\r\nRow: 31 account_type=com.google, account_name=hacongquoclun1@gmail.com, lastDate=1725408000000, dtstart=1725321600000, dtend=1725408000000, title=Quốc khánh (ngày lễ bổ sung), calendar_displayName=Ngày lễ ở Việt Nam, eventTimezone=UTC, calendar_timezone=Asia/Ho_Chi_Minh, dirty=0\r\nRow: 32 account_type=com.google, account_name=hacongquoclun1@gmail.com, lastDate=1735776000000, dtstart=1735689600000, dtend=1735776000000, title=Tết dương lịch, calendar_displayName=Ngày lễ ở Việt Nam, eventTimezone=UTC, calendar_timezone=Asia/Ho_Chi_Minh, dirty=0\r\nRow: 33 account_type=com.google, account_name=hacongquoclun1@gmail.com, lastDate=1745193600000, dtstart=1745107200000, dtend=1745193600000, title=chủa nhật phục sinh, calendar_displayName=Ngày lễ ở Việt Nam, eventTimezone=UTC, calendar_timezone=Asia/Ho_Chi_Minh, dirty=0\r\nRow: 34 account_type=com.google, account_name=hacongquoclun1@gmail.com, lastDate=1549152000000, dtstart=1549065600000, dtend=1549152000000, title=Tết Nguyên Đán, calendar_displayName=Ngày lễ ở Việt Nam, eventTimezone=UTC, calendar_timezone=Asia/Ho_Chi_Minh, dirty=0\r\nRow: 35 account_type=com.google, account_name=hacongquoclun1@gmail.com, lastDate=1555200000000, dtstart=1555113600000, dtend=1555200000000, title=Ngày nghỉ lễ, calendar_displayName=Ngày lễ ở Việt Nam, eventTimezone=UTC, calendar_timezone=Asia/Ho_Chi_Minh, dirty=0\r\nRow: 36 account_type=com.google, account_name=hacongquoclun1@gmail.com, lastDate=1555286400000, dtstart=1555200000000, dtend=1555286400000, title=Giỗ tổ Hùng Vương, calendar_displayName=Ngày lễ ở Việt Nam, eventTimezone=UTC, calendar_timezone=Asia/Ho_Chi_Minh, dirty=0\r\nRow: 37 account_type=com.google, account_name=hacongquoclun1@gmail.com, lastDate=1555372800000, dtstart=1555286400000, dtend=1555372800000, title=Ngày nghỉ lễ, calendar_displayName=Ngày lễ ở Việt Nam, eventTimezone=UTC, calendar_timezone=Asia/Ho_Chi_Minh, dirty=0\r\nRow: 38 account_type=com.google, account_name=hacongquoclun1@gmail.com, lastDate=1555891200000, dtstart=1555804800000, dtend=1555891200000, title=chủa nhật phục sinh, calendar_displayName=Ngày lễ ở Việt Nam, eventTimezone=UTC, calendar_timezone=Asia/Ho_Chi_Minh, dirty=0\r\nRow: 39 account_type=com.google, account_name=hacongquoclun1@gmail.com, lastDate=1556755200000, dtstart=1556668800000, dtend=1556755200000, title=Ngày Quốc tế Lao động, calendar_displayName=Ngày lễ ở Việt Nam, eventTimezone=UTC, calendar_timezone=Asia/Ho_Chi_Minh, dirty=0\r\nRow: 40 account_type=com.google, account_name=hacongquoclun1@gmail.com, lastDate=1579824000000, dtstart=1579737600000, dtend=1579824000000, title=Tết, calendar_displayName=Ngày lễ ở Việt Nam, eventTimezone=UTC, calendar_timezone=Asia/Ho_Chi_Minh, dirty=0\r\nRow: 41 account_type=com.google, account_name=hacongquoclun1@gmail.com, lastDate=1580169600000, dtstart=1580083200000, dtend=1580169600000, title=Tết Nguyên Đán, calendar_displayName=Ngày lễ ở Việt Nam, eventTimezone=UTC, calendar_timezone=Asia/Ho_Chi_Minh, dirty=0\r\nRow: 42 account_type=com.google, account_name=hacongquoclun1@gmail.com, lastDate=1580256000000, dtstart=1580169600000, dtend=1580256000000, title=Tết Nguyên Đán, calendar_displayName=Ngày lễ ở Việt Nam, eventTimezone=UTC, calendar_timezone=Asia/Ho_Chi_Minh, dirty=0\r\nRow: 43 account_type=com.google, account_name=hacongquoclun1@gmail.com, lastDate=1585872000000, dtstart=1585785600000, dtend=1585872000000, title=Giỗ tổ Hùng Vương, calendar_displayName=Ngày lễ ở Việt Nam, eventTimezone=UTC, calendar_timezone=Asia/Ho_Chi_Minh, dirty=0\r\nRow: 44 account_type=com.google, account_name=hacongquoclun1@gmail.com, lastDate=1586736000000, dtstart=1586649600000, dtend=1586736000000, title=chủa nhật phục sinh, calendar_displayName=Ngày lễ ở Việt Nam, eventTimezone=UTC, calendar_timezone=Asia/Ho_Chi_Minh, dirty=0\r\nRow: 45 account_type=com.google, account_name=hacongquoclun1@gmail.com, lastDate=1588377600000, dtstart=1588291200000, dtend=1588377600000, title=Ngày Quốc tế Lao động, calendar_displayName=Ngày lễ ở Việt Nam, eventTimezone=UTC, calendar_timezone=Asia/Ho_Chi_Minh, dirty=0\r\nRow: 46 account_type=com.google, account_name=hacongquoclun1@gmail.com, lastDate=1599091200000, dtstart=1599004800000, dtend=1599091200000, title=Quốc khánh, calendar_displayName=Ngày lễ ở Việt Nam, eventTimezone=UTC, calendar_timezone=Asia/Ho_Chi_Minh, dirty=0\r\nRow: 47 account_type=com.google, account_name=hacongquoclun1@gmail.com, lastDate=1613260800000, dtstart=1613174400000, dtend=1613260800000, title=Tết Nguyên Đán, calendar_displayName=Ngày lễ ở Việt Nam, eventTimezone=UTC, calendar_timezone=Asia/Ho_Chi_Minh, dirty=0\r\nRow: 48 account_type=com.google, account_name=hacongquoclun1@gmail.com, lastDate=1617580800000, dtstart=1617494400000, dtend=1617580800000, title=chủa nhật phục sinh, calendar_displayName=Ngày lễ ở Việt Nam, eventTimezone=UTC, calendar_timezone=Asia/Ho_Chi_Minh, dirty=0\r\nRow: 49 account_type=com.google, account_name=hacongquoclun1@gmail.com, lastDate=1640476800000, dtstart=1640390400000, dtend=1640476800000, title=Giáng sinh/Nôen, calendar_displayName=Ngày lễ ở Việt Nam, eventTimezone=UTC, calendar_timezone=Asia/Ho_Chi_Minh, dirty=0\r\nRow: 50 account_type=com.google, account_name=hacongquoclun1@gmail.com, lastDate=1649721600000, dtstart=1649635200000, dtend=1649721600000, title=ngày nghỉ Giỗ tổ Hùng Vương, calendar_displayName=Ngày lễ ở Việt Nam, eventTimezone=UTC, calendar_timezone=Asia/Ho_Chi_Minh, dirty=0\r\nRow: 139 account_type=com.google, account_name=hacongquoclun@gmail.com, lastDate=1719421200000, dtstart=1719417600000, dtend=1719421200000, title=Kỷ niệm yêu nhau, calendar_displayName=hacongquoclun@gmail.com, eventTimezone=Asia/Ho_Chi_Minh, calendar_timezone=UTC, dirty=1";
            }
            string pattern = @"Row:.*?(?=(Row:|$))";
            var lines = Regex.Matches(str, pattern, RegexOptions.Singleline);

            foreach (var line in lines)
            {
                // Lấy các giá trị cần thiết
                string account_type = function.GetValue(line.ToString(), "account_type", "account_name");
                string account_name = function.GetValue(line.ToString(), "account_name", "lastDate");
                string lastDate = function.GetValue(line.ToString(), "lastDate", "dtstart");
                string ngay = function.ConvertTimeStamp_Day(lastDate);
                lastDate = function.ConvertTimeStamp(lastDate);
                string dtstart = function.GetValue(line.ToString(), "dtstart", "dtend");
                dtstart = function.ConvertTimeStamp(dtstart);
                string dtend = function.GetValue(line.ToString(), "dtend", "title");
                dtend = function.ConvertTimeStamp(dtend);
                string title = function.GetValue(line.ToString(), "title", "calendar_displayName");
                string calendar_displayName = function.GetValue(line.ToString(), "calendar_displayName", "eventTimezone");
                string eventTimezone = function.GetValue(line.ToString(), "eventTimezone", "calendar_timezone");
                string calendar_timezone = function.GetValue(line.ToString(), "calendar_timezone", "dirty");

                usr_LichMini usr_LichMini = new usr_LichMini(account_type, account_name, lastDate, dtstart, dtend, title, calendar_displayName, eventTimezone, calendar_timezone, ngay);
                usr_LichMini.ControlClicked += flpLich_Click;
                usr_LichMini.Width = flpLich.Width;
                flpLich.Controls.Add(usr_LichMini);
            }
        }

        private void SearchTimer_Tick(object sender, EventArgs e)
        {
            searchTimer.Stop();
            TimKiemLich();
        }

        private void usr_Lich_Load(object sender, EventArgs e)
        {
            Load_flpLich();
        }

        private void flpLich_Click(object sender, EventArgs e)
        {
            if (sender is usr_LichMini clickedControl)
            {
                txtChuDe.Text = clickedControl.calendar_displayName;
                txtSuKien.Text = clickedControl.title;
                txtThoiGianBatDau.Text = clickedControl.dtstart;
                txtThoiGianKetThuc.Text = clickedControl.dtend;
                txtNgayCuoiCung.Text = clickedControl.lastDate;
                txtTimeZone.Text = clickedControl.eventTimezone;
                txtMuiGio.Text = clickedControl.calendar_timezone;
                if (clickedControl.account_type == "com.google")
                {
                    txtLuuTai.Text = "Tài khoản Google";
                }
                else
                {
                    txtLuuTai.Text = "Cục bộ trên thiết bị";
                }

                txtTaiKhoan.Text = clickedControl.account_name;
            }
        }

        private void flpLich_Resize(object sender, EventArgs e)
        {
            foreach (UserControl control in flpLich.Controls)
            {
                control.Width = flpLich.Width;
            }
        }

        private void TimKiemLich()
        {
            string searchText = txtTimKiem.Text.ToLower(); // Chuyển về lowercase để tìm kiếm không phân biệt chữ hoa chữ thường
            bool hasSearchText = !string.IsNullOrEmpty(searchText); // Kiểm tra xem có từ khóa tìm kiếm không

            foreach (usr_LichMini control in flpLich.Controls)
            {
                bool isVisible = true; // Mặc định hiển thị control

                if (hasSearchText)
                {
                    // Kiểm tra điều kiện tìm kiếm
                    isVisible = control.title.ToLower().Contains(searchText)
                             || control.lastDate.ToLower().Contains(searchText);
                }

                control.Visible = isVisible;
            }
        }

        private void txtTimKiem_TextChanged(object sender, EventArgs e)
        {
            searchTimer.Stop();
            searchTimer.Start();
        }

        private void cbbTuyChon_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (usr_LichMini control in flpLich.Controls)
            {
                if (cbbTuyChon.Text == "Tất cả")
                {
                    control.Visible = true;
                }
                else if (cbbTuyChon.Text == "Google")
                {
                    if (control.account_type == "com.google")
                    {
                        control.Visible = true;
                    }
                    else
                    {
                        control.Visible = false;
                    }
                }
                else if (cbbTuyChon.Text == "Điện thoại")
                {
                    if (control.account_type != "com.google")
                    {
                        control.Visible = true;
                    }
                    else
                    {
                        control.Visible = false;
                    }
                }
            }
        }

        private void btnXuat_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                folderDialog.Description = "Chọn thư mục lưu báo cáo";
                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    string selectedPath = folderDialog.SelectedPath;
                    string fileName = $"Báo cáo về lịch của thiết bị_{DeviceInfo.nameDevice}_{DeviceInfo.serialDevice}.docx";
                    string PATH_EXPORT = Path.Combine(selectedPath, fileName);

                    string sourceDirectory = AppDomain.CurrentDomain.BaseDirectory;
                    string projectDirectory = Directory.GetParent(sourceDirectory).Parent.Parent.FullName;
                    string PATH_TEMPLATE = Path.Combine(projectDirectory, "Data", "Document", "report_calendar.docx");
                    if (File.Exists(PATH_TEMPLATE))
                    {
                        var calendars_export = new List<object>();

                        foreach (usr_LichMini control in flpLich.Controls)
                        {
                            var calendar_export = new
                            {
                                CalendarDisplayName = control.title,
                                Dtstart = control.dtstart,
                                Dtend = control.dtend,
                                AccountCreated = control.account_type + "/" + control.account_name,
                            };
                            calendars_export.Add(calendar_export);
                        }

                        var value = new
                        {
                            ct = calendars_export,
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
            foreach (usr_LichMini control in flpLich.Controls)
            {
                control.checkBox.Checked = true;
            }
        }

        private void btnBoChonTatCa_Click(object sender, EventArgs e)
        {
            foreach (usr_LichMini control in flpLich.Controls)
            {
                control.checkBox.Checked = false;
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            flpLich.Controls.Clear();

            txtSuKien.Text = string.Empty;
            txtThoiGianBatDau.Text = string.Empty;
            txtThoiGianKetThuc.Text = string.Empty;
            txtNgayCuoiCung.Text = string.Empty;
            txtTimeZone.Text = string.Empty;
            txtMuiGio.Text = string.Empty;
            txtLuuTai.Text = string.Empty;
            txtTaiKhoan.Text = string.Empty;
            cbbTuyChon.Text = "Tất cả";

            usr_Lich_Load(sender, e);
        }
    }
}
