using MTA_Mobile_Forensic.GUI.Share;
using MTA_Mobile_Forensic.Support;
using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace MTA_Mobile_Forensic.GUI.Android
{
    public partial class usr_TinNhan : UserControl
    {
        public usr_TinNhan()
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

        private void Load_flpDSTinNhan()
        {
            query = "shell content query --uri content://sms/inbox --projection address:date:date_sent:read:status:body:service_center:sim_id:advanced_seen";
            string str = adb.adbCommand(query);
            string str1 = "Row: 0 address=CATP_HaNoi, date=1719137928795, date_sent=1719137920000, read=0, status=-1, body=(TB) CATP Hà Nội cảnh báo: Hiện nay, tội phạm sử dụng công nghệ cao lợi dụng việc giải quyết thủ tục hành chính trực tuyến để thực hiện hành vi lừa đảo chiếm đoạt tài sản. Đề nghị người dân nâng cao cảnh giác, không tải, cài đặt, truy cập các đường link, ứng dụng theo yêu cầu của người lạ. Khuyến cáo nhân dân cần lưu ý: Các cơ quan chức năng không làm việc (định danh, nâng cấp tài khoản…) qua điện thoại, mạng xã hội; các website của cơ quan nhà nước có định dạng “.gov.vn”. Khi phát hiện thông tin nghi vấn cần trình báo ngay với Phòng cảnh sát hình sự - CATP Hà Nội, số điện thoại: 0692196242 hoặc cơ quan Công an gần nhất để được hướng dẫn., service_center=+8491020542, sim_id=7, advanced_seen=0\r\nRow: 1 address=VIETTELPLUS, date=1719044961568, date_sent=1719044948000, read=0, status=-1, body=[TB] Quý khách đừng quên sử dụng điểm Viettel++ để đổi lưu lượng data, phút gọi/tin nhắn nội mạng hoặc các ưu đãi giá trị khác. Truy cập My Viettel tại https://myvt.page.link/vt10 để đổi điểm. Chi tiết LH 198 (0đ). Trân trọng!, service_center=+84980200904, sim_id=1, advanced_seen=0\r\nRow: 2 address=VIETTELPLUS, date=1719028274666, date_sent=1719028271000, read=0, status=-1, body=Viettel kinh chuc Quy khach mot ngay sinh nhat ngap tran niem vui va hanh phuc ben gia dinh cung nguoi than. Tran trong cam on Quy khach da yeu men va dong hanh cung Viettel., service_center=+84980200614, sim_id=1, advanced_seen=0\r\nRow: 3 address=VTMONEY, date=1719026537949, date_sent=1719026535000, read=1, status=-1, body=Chuc mung sinh nhat Quy khach  HA CONG QUOC BAO ! Viettel Money kinh chuc Quy khach tuoi moi nhieu suc khoe, hanh phuc va thanh cong. Dung quen su dung qua tang voucher sinh nhat khi mo ung dung Viettel Money hoac truy cap https://km.vtmoney.vn/314y/CMSNsms ., service_center=+84980200615, sim_id=1, advanced_seen=3\r\nRow: 4 address=191, date=1719020893266, date_sent=1719020883000, read=0, status=-1, body=[TB] DÙNG DATA GIÁ SIÊU RẺ!\r\n1. Soạn ST5K gửi 191: 5.000đ/ngày có 500MB.\r\n2. Soạn 1N gửi 191: 10.000đ/ngày có 5GB, 5p gọi ngoại mạng, miễn phí 10p/cuộc gọi nội mạng, nhắn tin nội mạng, xem TV360.\r\nƯu đãi sử dụng đến 24h ngày đăng ký. Các gói cước gia hạn theo ngày. LH 198 (0đ)., service_center=+84980200615, sim_id=1, advanced_seen=0\r\nRow: 5 address=BO_TTTT, date=1718955620444, date_sent=1718955602000, read=0, status=-1, body=[TB] Bộ TTTT khuyến cáo người dân nâng cao cảnh giác, không chuyển tiền hoặc cung cấp thông tin cá nhân cho người lạ qua điện thoại. Nếu có hiện tượng trên, đề nghị trình báo ngay cho cơ quan Công an để xử lý hoặc thông báo đến số điện thoại trực ban hình sự 0692348560 của Cục Cảnh sát hình sự để được hướng dẫn kịp thời., service_center=+84980200905, sim_id=1, advanced_seen=0\r\nRow: 6 address=VIETTELCSKH, date=1718882172358, date_sent=1718882158000, read=0, status=-1, body=[TB] Kính gửi Quý khách, để cập nhật các chương trình chăm sóc Khách hàng và khuyến mại của Viettel, hãy chọn “Quan tâm” kênh Zalo Viettel Chăm sóc Khách hàng tại\r\nhttps://zalo.me/1570758701534064697 để nhận thông tin nhanh nhất. Trân trọng., service_center=+84980200904, sim_id=1, advanced_seen=0\r\nRow: 7 address=VTMONEY, date=1718850486931, date_sent=1718850487000, read=1, status=-1, body=[TB] Nhac ban Viettel Money co rat nhieu UU DAI HOT dat xe tu Be, Gojek,... Doi ngay diem Viettel++ tai https://km.vtmoney.vn/314y/dichuyes ban nhe!, service_center=+84980200623, sim_id=1, advanced_seen=3\r\nRow: 8 address=+84369012932, date=1718768942589, date_sent=1718768931000, read=0, status=-1, body=YfqisiCasKiêmsMôxiNqàyMjễnPhjSieêuXjinhMôjtNngìnTjỷMjễnPhis*)GhWvEB.xScH-RD.de<)WujVẻiHoôwNjayxOpMqy, service_center=+84980200615, sim_id=1, advanced_seen=0\r\nRow: 9 address=MyVNPT, date=1718642368293, date_sent=1718642366000, read=1, status=-1, body=124405 la ma xac thuc OTP tren MyVNPT cua Quy khach. Ma co hieu luc trong 2 phut. De dam bao an toan, Quy khach vui long khong chia se ma nay voi bat ky ai., service_center=+8491020533, sim_id=7, advanced_seen=3\r\nRow: 10 address=MyVNPT, date=1718641576051, date_sent=1718641574000, read=0, status=-1, body=156868 la ma xac thuc OTP tren MyVNPT cua Quy khach. Ma co hieu luc trong 2 phut. De dam bao an toan, Quy khach vui long khong chia se ma nay voi bat ky ai., service_center=+8491020533, sim_id=7, advanced_seen=0\r\nRow: 11 address=VTMONEY, date=1718618987008, date_sent=1718618987000, read=1, status=-1, body=[TB] Ban dang co voucher SINH NHAT Viettel Money tang. Mo app hoac truy cap https://km.vtmoney.vn/314y/CMSNsms de su dung Ban nhe!, service_center=+84980200614, sim_id=1, advanced_seen=3\r\nRow: 12 address=+84369012932, date=1718768942589, date_sent=1718768931000, read=0, status=-1, body=YfqisiCasKiêmsMôxiNqàyMjễnPhjSieêuXjinhMôjtNngìnTjỷMjễnPhis*)GhWvEB.xScH-RD.de<)WujVẻiHoôwNjayxOpMqy, service_center=+84980200615, sim_id=1, advanced_seen=0\r\nRow: 13 address=+84369012932, date=1718768942589, date_sent=1718768931000, read=0, status=-1, body=YfqisiCasKiêmsMôxiNqàyMjễnPhjSieêuXjinhMôjtNngìnTjỷMjễnPhis*)GhWvEB.xScH-RD.de<)WujVẻiHoôwNjayxOpMqy, service_center=+84980200615, sim_id=1, advanced_seen=0";
            string pattern = @"Row:.*?(?=(Row:|$))";
            var lines = Regex.Matches(str1, pattern, RegexOptions.Singleline);

            foreach (var line in lines)
            {
                // Lấy các giá trị cần thiết
                string address = function.GetValue(line.ToString(), "address", "date");
                string date = function.GetValue(line.ToString(), "date", "date_sent");
                date = function.ConvertTimeStamp(date);
                string dateSent = function.GetValue(line.ToString(), "date_sent", "read");
                dateSent = function.ConvertTimeStamp(dateSent);
                string read = function.GetValue(line.ToString(), "read", "status");
                string status = function.GetValue(line.ToString(), "status", "body");
                string body = function.GetValue(line.ToString(), "body", "service_center");
                string serviceCenter = function.GetValue(line.ToString(), "service_center", "sim_id");
                string simId = function.GetValue(line.ToString(), "sim_id", "advanced_seen");

                usr_TinNhanMini usr_TinNhanMini = new usr_TinNhanMini(address, body, date, dateSent, read, status, serviceCenter, simId);
                usr_TinNhanMini.ControlClicked += flpDSTinNhan_Click;
                usr_TinNhanMini.Width = flpDSTinNhan.Width;
                flpDSTinNhan.Controls.Add(usr_TinNhanMini);
            }
        }

        private void SearchTimer_Tick(object sender, EventArgs e)
        {
            searchTimer.Stop();
            TimKiemTinNhan();
        }

        private void usr_TinNhan_Load(object sender, EventArgs e)
        {
            Load_flpDSTinNhan();
        }

        private void flpDSTinNhan_SizeChanged(object sender, EventArgs e)
        {
            // Duyệt qua tất cả các Control trong parent
            foreach (UserControl control in flpDSTinNhan.Controls)
            {
                control.Width = flpDSTinNhan.Width;
            }
        }

        private void flpDSTinNhan_Click(object sender, EventArgs e)
        {
            // Lấy dữ liệu từ user_control được click vào
            if (sender is usr_TinNhanMini clickedControl)
            {
                txtAddress.Text = clickedControl.diachi;
                txtThoiGianGui.Text = clickedControl.dateSent;
                txtThoiGianNhan.Text = clickedControl.thoigian;
                if (clickedControl.read == "0")
                {
                    txtTrangThai.Text = "Chưa đọc";
                }
                else if (clickedControl.read == "1")
                {
                    txtTrangThai.Text = "Đã đọc";
                }
                if (clickedControl.simId == "1")
                {
                    txtSimNhan.Text = "Sim 1";
                }
                else
                {
                    txtSimNhan.Text = "Sim 2";
                }
                txtTrungTamDichVu.Text = clickedControl.serviceCenter;
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            flpDSTinNhan.Controls.Clear();
            txtAddress.Text = string.Empty;
            txtThoiGianGui.Text = string.Empty;
            txtThoiGianNhan.Text = string.Empty;
            txtTrangThai.Text = string.Empty;
            txtSimNhan.Text = string.Empty;
            txtTrungTamDichVu.Text = string.Empty;
            txtTimKiem.Text = string.Empty;
            cbbChonSim.Text = "Tất cả sim";

            usr_TinNhan_Load(sender, e);
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

        private void TimKiemTinNhan()
        {
            string searchText = txtTimKiem.Text.ToLower(); // Chuyển về lowercase để tìm kiếm không phân biệt chữ hoa chữ thường
            bool hasSearchText = !string.IsNullOrEmpty(searchText); // Kiểm tra xem có từ khóa tìm kiếm không

            foreach (usr_TinNhanMini control in flpDSTinNhan.Controls)
            {
                bool isVisible = true; // Mặc định hiển thị control

                if (hasSearchText)
                {
                    // Kiểm tra điều kiện tìm kiếm
                    isVisible = control.diachi.ToLower().Contains(searchText)
                             || control.tinnhan.ToLower().Contains(searchText)
                             || control.thoigian.ToLower().Contains(searchText);
                }

                control.Visible = isVisible;
            }
        }

        private void txtTimKiem_TextChanged(object sender, EventArgs e)
        {
            searchTimer.Stop();
            searchTimer.Start();
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            TimKiemTinNhan();
        }

        private void cbbChonSim_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (usr_TinNhanMini control in flpDSTinNhan.Controls)
            {
                if (cbbChonSim.Text == "Tất cả sim")
                {
                    control.Visible = true;
                }
                else if (cbbChonSim.Text == "Sim 1")
                {
                    if(control.simId == "1")
                    {
                        control.Visible = true;
                    }
                    else
                    {
                        control.Visible = false;
                    }
                }
                else if (cbbChonSim.Text == "Sim 2")
                {
                    if (control.simId != "1")
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
    }
}
