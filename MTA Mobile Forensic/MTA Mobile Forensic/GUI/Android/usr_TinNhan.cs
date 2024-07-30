using MTA_Mobile_Forensic.GUI.Share;
using MTA_Mobile_Forensic.Model;
using MTA_Mobile_Forensic.Support;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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
        int itemsPerPage = 20;
        int currentPage = 0;
        List<TinNhan> messages = new List<TinNhan>();
        List<TinNhan> messages_TimKiem;

        private void Load_flpDSTinNhan()
        {
            messages.Clear();
            string option1 = "sim_id";
            string option2 = "advanced_seen";

            // add tin nhắn đã nhận
            query = "shell content query --uri content://sms/inbox --projection address:date:date_sent:read:status:body:service_center:sim_id:advanced_seen";
            string str_Nhan = adb.adbCommand(query);
            if (str_Nhan == string.Empty)
            {
                option1 = "sim_slot";
                option2 = "type";
                query = $"shell content query --uri content://sms/inbox --projection address:date:date_sent:read:status:body:service_center:{option1}:{option2}";
                str_Nhan = adb.adbCommand(query);
                if (str_Nhan == string.Empty)
                {
                    str_Nhan = "Row: 0 address=CATP_HaNoi, date=1719137928795, date_sent=1719137920000, read=0, status=-1, body=(TB) CATP Hà Nội cảnh báo: Hiện nay, tội phạm sử dụng công nghệ cao lợi dụng việc giải quyết thủ tục hành chính trực tuyến để thực hiện hành vi lừa đảo chiếm đoạt tài sản. Đề nghị người dân nâng cao cảnh giác, không tải, cài đặt, truy cập các đường link, ứng dụng theo yêu cầu của người lạ. Khuyến cáo nhân dân cần lưu ý: Các cơ quan chức năng không làm việc (định danh, nâng cấp tài khoản…) qua điện thoại, mạng xã hội; các website của cơ quan nhà nước có định dạng “.gov.vn”. Khi phát hiện thông tin nghi vấn cần trình báo ngay với Phòng cảnh sát hình sự - CATP Hà Nội, số điện thoại: 0692196242 hoặc cơ quan Công an gần nhất để được hướng dẫn., service_center=+8491020542, sim_id=7, advanced_seen=0\r\nRow: 1 address=VIETTELPLUS, date=1719044961568, date_sent=1719044948000, read=0, status=-1, body=[TB] Quý khách đừng quên sử dụng điểm Viettel++ để đổi lưu lượng data, phút gọi/tin nhắn nội mạng hoặc các ưu đãi giá trị khác. Truy cập My Viettel tại https://myvt.page.link/vt10 để đổi điểm. Chi tiết LH 198 (0đ). Trân trọng!, service_center=+84980200904, sim_id=1, advanced_seen=0\r\nRow: 2 address=VIETTELPLUS, date=1719028274666, date_sent=1719028271000, read=0, status=-1, body=Viettel kinh chuc Quy khach mot ngay sinh nhat ngap tran niem vui va hanh phuc ben gia dinh cung nguoi than. Tran trong cam on Quy khach da yeu men va dong hanh cung Viettel., service_center=+84980200614, sim_id=1, advanced_seen=0\r\nRow: 3 address=VTMONEY, date=1719026537949, date_sent=1719026535000, read=1, status=-1, body=Chuc mung sinh nhat Quy khach  HA CONG QUOC BAO ! Viettel Money kinh chuc Quy khach tuoi moi nhieu suc khoe, hanh phuc va thanh cong. Dung quen su dung qua tang voucher sinh nhat khi mo ung dung Viettel Money hoac truy cap https://km.vtmoney.vn/314y/CMSNsms ., service_center=+84980200615, sim_id=1, advanced_seen=3\r\nRow: 4 address=191, date=1719020893266, date_sent=1719020883000, read=0, status=-1, body=[TB] DÙNG DATA GIÁ SIÊU RẺ!\r\n1. Soạn ST5K gửi 191: 5.000đ/ngày có 500MB.\r\n2. Soạn 1N gửi 191: 10.000đ/ngày có 5GB, 5p gọi ngoại mạng, miễn phí 10p/cuộc gọi nội mạng, nhắn tin nội mạng, xem TV360.\r\nƯu đãi sử dụng đến 24h ngày đăng ký. Các gói cước gia hạn theo ngày. LH 198 (0đ)., service_center=+84980200615, sim_id=1, advanced_seen=0\r\nRow: 5 address=BO_TTTT, date=1718955620444, date_sent=1718955602000, read=0, status=-1, body=[TB] Bộ TTTT khuyến cáo người dân nâng cao cảnh giác, không chuyển tiền hoặc cung cấp thông tin cá nhân cho người lạ qua điện thoại. Nếu có hiện tượng trên, đề nghị trình báo ngay cho cơ quan Công an để xử lý hoặc thông báo đến số điện thoại trực ban hình sự 0692348560 của Cục Cảnh sát hình sự để được hướng dẫn kịp thời., service_center=+84980200905, sim_id=1, advanced_seen=0\r\nRow: 6 address=VIETTELCSKH, date=1718882172358, date_sent=1718882158000, read=0, status=-1, body=[TB] Kính gửi Quý khách, để cập nhật các chương trình chăm sóc Khách hàng và khuyến mại của Viettel, hãy chọn “Quan tâm” kênh Zalo Viettel Chăm sóc Khách hàng tại\r\nhttps://zalo.me/1570758701534064697 để nhận thông tin nhanh nhất. Trân trọng., service_center=+84980200904, sim_id=1, advanced_seen=0\r\nRow: 7 address=VTMONEY, date=1718850486931, date_sent=1718850487000, read=1, status=-1, body=[TB] Nhac ban Viettel Money co rat nhieu UU DAI HOT dat xe tu Be, Gojek,... Doi ngay diem Viettel++ tai https://km.vtmoney.vn/314y/dichuyes ban nhe!, service_center=+84980200623, sim_id=1, advanced_seen=3\r\nRow: 8 address=+84369012932, date=1718768942589, date_sent=1718768931000, read=0, status=-1, body=YfqisiCasKiêmsMôxiNqàyMjễnPhjSieêuXjinhMôjtNngìnTjỷMjễnPhis*)GhWvEB.xScH-RD.de<)WujVẻiHoôwNjayxOpMqy, service_center=+84980200615, sim_id=1, advanced_seen=0\r\nRow: 9 address=MyVNPT, date=1718642368293, date_sent=1718642366000, read=1, status=-1, body=124405 la ma xac thuc OTP tren MyVNPT cua Quy khach. Ma co hieu luc trong 2 phut. De dam bao an toan, Quy khach vui long khong chia se ma nay voi bat ky ai., service_center=+8491020533, sim_id=7, advanced_seen=3\r\nRow: 10 address=MyVNPT, date=1718641576051, date_sent=1718641574000, read=0, status=-1, body=156868 la ma xac thuc OTP tren MyVNPT cua Quy khach. Ma co hieu luc trong 2 phut. De dam bao an toan, Quy khach vui long khong chia se ma nay voi bat ky ai., service_center=+8491020533, sim_id=7, advanced_seen=0\r\nRow: 11 address=VTMONEY, date=1718618987008, date_sent=1718618987000, read=1, status=-1, body=[TB] Ban dang co voucher SINH NHAT Viettel Money tang. Mo app hoac truy cap https://km.vtmoney.vn/314y/CMSNsms de su dung Ban nhe!, service_center=+84980200614, sim_id=1, advanced_seen=3\r\nRow: 12 address=+84369012932, date=1718768942589, date_sent=1718768931000, read=0, status=-1, body=YfqisiCasKiêmsMôxiNqàyMjễnPhjSieêuXjinhMôjtNngìnTjỷMjễnPhis*)GhWvEB.xScH-RD.de<)WujVẻiHoôwNjayxOpMqy, service_center=+84980200615, sim_id=1, advanced_seen=0\r\nRow: 13 address=+84369012932, date=1718768942589, date_sent=1718768931000, read=0, status=-1, body=YfqisiCasKiêmsMôxiNqàyMjễnPhjSieêuXjinhMôjtNngìnTjỷMjễnPhis*)GhWvEB.xScH-RD.de<)WujVẻiHoôwNjayxOpMqy, service_center=+84980200615, sim_id=1, advanced_seen=0";
                }
            }

            string pattern_Nhan = @"Row:.*?(?=(Row:|$))";
            var lines_Nhan = Regex.Matches(str_Nhan, pattern_Nhan, RegexOptions.Singleline);

            foreach (var line in lines_Nhan)
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
                string serviceCenter = function.GetValue(line.ToString(), "service_center", option1);
                string simId = function.GetValue(line.ToString(), option1, option2);

                TinNhan tinNhan = new TinNhan();
                tinNhan.address = address;
                tinNhan.body = body;
                tinNhan.date = date;
                tinNhan.dateSent = dateSent;
                tinNhan.read = read;
                tinNhan.status = status;
                tinNhan.serviceCenter = serviceCenter;
                tinNhan.simId = simId;
                tinNhan.sentMessage = 0;

                messages.Add(tinNhan);
            }

            // add tin nhắn gửi đi
            query = "shell content query --uri content://sms/sent --projection address:date:date_sent:read:status:body:service_center:sim_id:advanced_seen";
            string str_Gui = adb.adbCommand(query);
            if (str_Gui == string.Empty)
            {
                option1 = "sim_slot";
                option2 = "type";
                query = $"shell content query --uri content://sms/sent --projection address:date:date_sent:read:status:body:service_center:{option1}:{option2}";
                str_Gui = adb.adbCommand(query);
                if (str_Gui == string.Empty)
                {
                    str_Gui = "Row: 0 address=+84329281596, date=1720232865035, date_sent=0, read=1, status=-1, body=Em đi rồi mà máy không có mạng hả, service_center=NULL, sim_id=1, advanced_seen=3\r\nRow: 1 address=+84329281596, date=1718289513363, date_sent=0, read=1, status=-1, body=Đang ngồi trông em á:))). Tối anh đi sh vội nên k nhắn đc cho em á. Anh về rồi, service_center=NULL, sim_id=1, advanced_seen=3\r\nRow: 2 address=+84329281596, date=1717034536311, date_sent=0, read=1, status=-1, body=Nay dỗi anh hả😄, service_center=NULL, sim_id=1, advanced_seen=3\r\nRow: 3 address=6020, date=1716379409322, date_sent=0, read=1, status=-1, body=ZALO, service_center=NULL, sim_id=7, advanced_seen=3\r\nRow: 4 address=191, date=1715763972043, date_sent=0, read=1, status=-1, body=35nam, service_center=NULL, sim_id=1, advanced_seen=3\r\nRow: 5 address=+84329281596, date=1715505974799, date_sent=0, read=1, status=-1, body=Ừ, anh tưởng em ngủ, nt nãy giờ không thấy em tl😄, service_center=NULL, sim_id=1, advanced_seen=3\r\nRow: 6 address=+84868219091, date=1710634571051, date_sent=0, read=1, status=-1, body=Cho anh đỡ phải chờ, service_center=NULL, sim_id=1, advanced_seen=3\r\nRow: 7 address=+84868219091, date=1710634555517, date_sent=0, read=1, status=-1, body=Lâu cũng được anh ạ, anh cứ gọi trước cho em tầm 15p là em xuống, service_center=NULL, sim_id=1, advanced_seen=3\r\nRow: 8 address=0868219091, date=1710633741940, date_sent=0, read=1, status=-1, body=Anh chuyển sang ship cổng bể bơi cho em anh nha, service_center=NULL, sim_id=1, advanced_seen=3\r\nRow: 9 address=0868219091, date=1710601844095, date_sent=0, read=1, status=-1, body=Sáng anh mang đồ qua cổng phụ đại học điện lực cho em được không, service_center=NULL, sim_id=1, advanced_seen=3\r\nRow: 10 address=0868219091, date=1710389258321, date_sent=0, read=1, status=-1, body=Chiều anh mang đơn hàng của Bảo qua cổng bể bơi hộ em với, service_center=NULL, sim_id=1, advanced_seen=3\r\nRow: 11 address=0368260580, date=1707743508665, date_sent=0, read=1, status=-1, body=Cháu Bảo đây, chú có xe về chưa chú, service_center=NULL, sim_id=1, advanced_seen=3\r\nRow: 12 address=+84961563123, date=1707699935147, date_sent=0, read=1, status=-1, body=Ok a, service_center=NULL, sim_id=1, advanced_seen=3\r\nRow: 13 address=888, date=1706583274867, date_sent=0, read=1, status=-1, body=Data, service_center=NULL, sim_id=6, advanced_seen=3\r\nRow: 14 address=888, date=1706582380584, date_sent=0, read=1, status=-1, body=Data, service_center=NULL, sim_id=6, advanced_seen=3\r\nRow: 15 address=191, date=1706455540052, date_sent=0, read=1, status=-1, body=KTTK, service_center=NULL, sim_id=1, advanced_seen=3\r\nRow: 16 address=191, date=1705102769221, date_sent=0, read=1, status=-1, body=KTTK, service_center=NULL, sim_id=1, advanced_seen=3\r\nRow: 17 address=191, date=1703866824923, date_sent=0, read=1, status=-1, body=2023, service_center=NULL, sim_id=1, advanced_seen=3\r\nRow: 18 address=+84329281596, date=1703434281406, date_sent=0, read=1, status=-1, body=Ngày lượn 2 vòng. Nhất em luôn đấy, service_center=NULL, sim_id=1, advanced_seen=3\r\nRow: 19 address=+84329281596, date=1703434149746, date_sent=0, read=1, status=-1, body=Anh chờ chứ. Chiều em nói đi khu sinh thái. Thế là đổi kế hoạch à, service_center=NULL, sim_id=1, advanced_seen=3\r\nRow: 20 address=+84329281596, date=1703434048145, date_sent=0, read=1, status=-1, body=Cứ tưởng năm nay ở lại đến đêm ăn trọn lễ với bà con giáo xứ chứ em😄, service_center=NULL, sim_id=1, advanced_seen=3\r\nRow: 21 address=+84329281596, date=1703433964475, date_sent=0, read=1, status=-1, body=Em chưa về à, service_center=NULL, sim_id=1, advanced_seen=3\r\nRow: 22 address=0984998013, date=1703173120942, date_sent=0, read=1, status=-1, body=Khoảng 17h15 chiều t7 tuần này bắt đầu anh ạ, service_center=NULL, sim_id=1, advanced_seen=3\r\nRow: 23 address=0984998013, date=1703166181697, date_sent=0, read=1, status=-1, body=Chiều t7 anh có sắp xếp được không anh, service_center=NULL, sim_id=1, advanced_seen=3\r\nRow: 24 address=+84329281596, date=1702713734323, date_sent=0, read=1, status=-1, body=Anh đang ở dưới này rồi, service_center=NULL, sim_id=1, advanced_seen=3\r\nRow: 25 address=+84329281596, date=1702312383688, date_sent=0, read=1, status=-1, body=Em ngủ chưa, service_center=NULL, sim_id=1, advanced_seen=3\r\nRow: 26 address=+84329281596, date=1699710213754, date_sent=0, read=1, status=-1, body=Anh về đến nơi đc 15p rồi á, service_center=NULL, sim_id=1, advanced_seen=3\r\nRow: 27 address=0329281596, date=1699195608443, date_sent=0, read=1, status=-1, body=Anh tắm giặt rồi dọn dẹp đồ xong hết rồi em, service_center=NULL, sim_id=1, advanced_seen=3\r\nRow: 28 address=+84971287729, date=1695905902382, date_sent=0, read=1, status=-1, body=Thế chú nói đại đội mình có nhiều người trượt không ạ?, service_center=NULL, sim_id=1, advanced_seen=3\r\nRow: 29 address=0971287729, date=1695903481842, date_sent=0, read=1, status=-1, body=Anh ơi, em Bảo đây. Anh nghe ai nói em trượt thể lực vậy anh. Để em biết đường tác động sớm cái anh ạ:)), service_center=NULL, sim_id=1, advanced_seen=3\r\nRow: 30 address=0329281596, date=1695903037335, date_sent=0, read=1, status=-1, body=Ừ, service_center=NULL, sim_id=1, advanced_seen=3\r\nRow: 31 address=8066, date=1688895126435, date_sent=0, read=1, status=-1, body=WALLS WFYVRE9HMA, service_center=NULL, sim_id=1, advanced_seen=3\r\nRow: 32 address=0329281596, date=1687097920659, date_sent=0, read=1, status=-1, body=Em đi đâu vậy?, service_center=NULL, sim_id=1, advanced_seen=3\r\nRow: 33 address=0363671683, date=1686825361986, date_sent=0, read=1, status=-1, body=Có điểm thi cấp 3 rồi mẹ: Hiếu 41.4₫, Thảo 38.5₫, Hà My 40.1₫, service_center=NULL, sim_id=1, advanced_seen=3\r\nRow: 34 address=0329281596, date=1685640188965, date_sent=0, read=1, status=-1, body=Ừ, bây giờ anh ngủ. Em cố ngủ đi nhé, service_center=NULL, sim_id=1, advanced_seen=3\r\nRow: 35 address=0329281596, date=1685640029321, date_sent=0, read=1, status=-1, body=Anh không làm gì cho em được cả. Em thử lên giường cố nằm mà ngủ, mồ hôi cũng được. Mai tắm nạ, service_center=NULL, sim_id=1, advanced_seen=3\r\nRow: 36 address=0329281596, date=1685639180050, date_sent=0, read=1, status=-1, body=Em ngủ được chưa, bây giờ anh lên giường, service_center=NULL, sim_id=1, advanced_seen=3\r\nRow: 37 address=0329281596, date=1681007039438, date_sent=0, read=1, status=-1, body=Em yêu giận anh hay đang ngủ mà anh vẫn chưa thấy đâu vậy?, service_center=NULL, sim_id=1, advanced_seen=3\r\nRow: 38 address=1414, date=1680393964319, date_sent=0, read=1, status=-1, body=TTTB, service_center=NULL, sim_id=1, advanced_seen=3\r\nRow: 39 address=0329281596, date=1678724652134, date_sent=0, read=1, status=-1, body=Em đâu rồi, service_center=NULL, sim_id=1, advanced_seen=3\r\nRow: 40 address=0329281596, date=1678620283783, date_sent=0, read=1, status=-1, body=Tôi mời bạn đến iSharing. Nhấp vào liên kết bên dưới để thêm tôi. https://app.isharing.me/qVcT/67sm773s, service_center=NULL, sim_id=1, advanced_seen=3\r\nRow: 41 address=0962677299, date=1678415440785, date_sent=0, read=1, status=-1, body=Em vừa gửi qua zalo cho anh rồi đấy ạ, service_center=NULL, sim_id=1, advanced_seen=3\r\nRow: 42 address=0962677299, date=1678414910634, date_sent=0, read=1, status=-1, body=Mẫu đó là lavabo L-288V anh ạ, service_center=NULL, sim_id=1, advanced_seen=3\r\nRow: 43 address=0962677299, date=1678414878524, date_sent=0, read=1, status=-1, body=Anh xem có mua rồi lắp hộ em được không ạ, chỉ huy của em giục lắm rồi, service_center=NULL, sim_id=1, advanced_seen=3\r\nRow: 44 address=0962677299, date=1678414605764, date_sent=0, read=1, status=-1, body=Em hỏi mua thì giá khoảng 900k anh ạ, service_center=NULL, sim_id=1, advanced_seen=3\r\nRow: 45 address=0962677299, date=1678414552262, date_sent=0, read=1, status=-1, body=Hoặc là anh mua rồi lắp hộ em. Hết bao nhiêu em gửi anh ạ, service_center=NULL, sim_id=1, advanced_seen=3\r\nRow: 46 address=0962677299, date=1678414190181, date_sent=0, read=1, status=-1, body=Anh ơi, em mua bên ngoài rồi nhờ anh lắp được không, service_center=NULL, sim_id=1, advanced_seen=3\r\nRow: 47 address=+84329281596, date=1676639399865, date_sent=0, read=1, status=-1, body=Anh gọi nch với em đc không, service_center=NULL, sim_id=1, advanced_seen=3\r\nRow: 48 address=0329281596, date=1676638120846, date_sent=0, read=1, status=-1, body=Chiều mai khoảng 3h15 anh qua đến nơi. Em mang đồ xuống sảnh hộ anh được không?, service_center=NULL, sim_id=1, advanced_seen=3\r\nRow: 49 address=0329281596, date=1676565027094, date_sent=0, read=1, status=-1, body=Muộn rồi anh không gọi nữa, anh sẽ gọi đến khi em chịu nch với anh, service_center=NULL, sim_id=1, advanced_seen=3\r\nRow: 50 address=0329281596, date=1676563244366, date_sent=0, read=1, status=-1, body=Em mở fb ra nch, service_center=NULL, sim_id=1, advanced_seen=3";
                }
            }
            string pattern_Gui = @"Row:.*?(?=(Row:|$))";
            var lines_Gui = Regex.Matches(str_Gui, pattern_Gui, RegexOptions.Singleline);
            foreach (var line in lines_Gui)
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
                string serviceCenter = function.GetValue(line.ToString(), "service_center", option1);
                string simId = function.GetValue(line.ToString(), option1, option2);

                TinNhan tinNhan = new TinNhan();
                tinNhan.address = address;
                tinNhan.body = body;
                tinNhan.date = date;
                tinNhan.dateSent = dateSent;
                tinNhan.read = read;
                tinNhan.status = status;
                tinNhan.serviceCenter = serviceCenter;
                tinNhan.simId = simId;
                tinNhan.sentMessage = 1;

                messages.Add(tinNhan);
            }

            // sắp xếp tin nhắn
            string format = "HH:mm:ss dd/MM/yyyy";
            messages = messages
            .Where(m => DateTime.TryParseExact(m.date, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out _))  // Lọc các mục có thể chuyển đổi thành DateTime
            .OrderByDescending(m => DateTime.ParseExact(m.date, format, CultureInfo.InvariantCulture))
            .ToList();
        }

        private void SearchTimer_Tick(object sender, EventArgs e)
        {
            searchTimer.Stop();
            TimKiemTinNhan();
        }

        private void usr_TinNhan_Load(object sender, EventArgs e)
        {
            Load_flpDSTinNhan();
            Add_usr_TinNhanMini(currentPage);
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
                if (txtAddress.Text != clickedControl.diachi)
                {
                    // load tin nhắn vào flpChiTietTinNhan
                    Load_flpChiTietTinNhan(clickedControl.diachi);
                }

                txtAddress.Text = clickedControl.diachi;
                if (clickedControl.dateSent != "07:00:00 01/01/1970")
                {
                    lblAddress.Text = "Người gửi";
                    txtThoiGianGui.Text = clickedControl.dateSent;
                    txtThoiGianNhan.Text = clickedControl.thoigian;
                }
                else
                {
                    lblAddress.Text = "Người nhận";
                    txtThoiGianGui.Text = clickedControl.thoigian;
                    txtThoiGianNhan.Text = "Không xác định";
                }

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

        private void Load_flpChiTietTinNhan(string address)
        {
            flpChiTietTinNhan.Controls.Clear();
            try
            {
                foreach (var item in messages)
                {
                    if (item.address == address)
                    {
                        if (item.sentMessage == 0)
                        {
                            // tin nhắn nhận được
                            usr_TinNhanTroChuyenNhan usr_TinNhanTroChuyenNhan = new usr_TinNhanTroChuyenNhan(item.body, item.date, flpChiTietTinNhan.Width);
                            usr_TinNhanTroChuyenNhan.Width = flpChiTietTinNhan.Width;
                            flpChiTietTinNhan.Controls.Add(usr_TinNhanTroChuyenNhan);
                        }
                        else if (item.sentMessage == 1)
                        {
                            // tin nhắn gửi đi
                            usr_TinNhanTroChuyenGui usr_TinNhanTroChuyenGui = new usr_TinNhanTroChuyenGui(item.body, item.date, flpChiTietTinNhan.Width);
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

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            flpDSTinNhan.Controls.Clear();
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
            currentPage = 0;
            string searchText = txtTimKiem.Text.ToLower();

            if (txtTimKiem.Text != String.Empty)
            {
                messages_TimKiem = new List<TinNhan>();
                foreach (var item in messages)
                {
                    if (item.address.ToLower().Contains(searchText) || item.address.ToLower().Contains(searchText) || item.address.ToLower().Contains(searchText))
                    {
                        messages_TimKiem.Add(item);
                    }
                }
                Add_usr_TinNhanMini_TimKiem(currentPage);
            }
            else
            {
                Add_usr_TinNhanMini(currentPage);
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
                    if (control.simId == "1")
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
            if (currentPage < (messages.Count - 1) / itemsPerPage)
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
            int end = Math.Min(start + itemsPerPage, messages.Count);

            txtTrangHienTai.Text = $"Trang {pageNumber + 1} / {Math.Ceiling((double)messages.Count / itemsPerPage)}";

            for (int i = start; i < end; i++)
            {
                usr_TinNhanMini usr_TinNhanMini = new usr_TinNhanMini(messages[i].address, messages[i].body, messages[i].date, messages[i].dateSent, messages[i].read, messages[i].status, messages[i].serviceCenter, messages[i].simId, messages[i].sentMessage);
                usr_TinNhanMini.ControlClicked += flpDSTinNhan_Click;
                usr_TinNhanMini.Width = flpDSTinNhan.Width;
                flpDSTinNhan.Controls.Add(usr_TinNhanMini);
            }
        }

        private void Add_usr_TinNhanMini_TimKiem(int pageNumber)
        {
            flpDSTinNhan.Controls.Clear();

            int start = pageNumber * itemsPerPage;
            int end = Math.Min(start + itemsPerPage, messages_TimKiem.Count);

            txtTrangHienTai.Text = $"Trang {pageNumber + 1} / {Math.Ceiling((double)messages_TimKiem.Count / itemsPerPage)}";

            for (int i = start; i < end; i++)
            {
                usr_TinNhanMini usr_TinNhanMini = new usr_TinNhanMini(messages_TimKiem[i].address, messages_TimKiem[i].body, messages_TimKiem[i].date, messages_TimKiem[i].dateSent, messages_TimKiem[i].read, messages_TimKiem[i].status, messages_TimKiem[i].serviceCenter, messages_TimKiem[i].simId, messages_TimKiem[i].sentMessage);
                usr_TinNhanMini.ControlClicked += flpDSTinNhan_Click;
                usr_TinNhanMini.Width = flpDSTinNhan.Width;
                flpDSTinNhan.Controls.Add(usr_TinNhanMini);
            }
        }
    }
}
