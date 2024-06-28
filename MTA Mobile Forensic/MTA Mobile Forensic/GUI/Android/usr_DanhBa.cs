using MTA_Mobile_Forensic.GUI.Share;
using MTA_Mobile_Forensic.Support;
using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace MTA_Mobile_Forensic.GUI.Android
{
    public partial class usr_DanhBa : UserControl
    {
        public usr_DanhBa()
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

        private void Load_flpDanhBa()
        {
            query = "shell content query --uri content://com.android.contacts/data/phones/ --projection display_name:contact_last_updated_timestamp:company:account_type:account_name:nickname:data1:hash_id:data2";
            string str = adb.adbCommand(query);

            string str1 = "Row: 0 display_name=Khuyenmai, contact_last_updated_timestamp=1597566553974, company=NULL, account_type=com.xiaomi, account_name=6230443535, nickname=NULL, data1=*098#, hash_id=MblqqJwn+SsX4eRE1cD1Oz5GA64=\r\n, data2=2\r\nRow: 1 display_name=TK Goc, contact_last_updated_timestamp=1597566559072, company=NULL, account_type=com.xiaomi, account_name=6230443535, nickname=NULL, data1=*101#, hash_id=naHmAnzOuLaVP0HO6/nNklJMBzo=\r\n, data2=2\r\nRow: 2 display_name=TK KM, contact_last_updated_timestamp=1597566558969, company=NULL, account_type=com.xiaomi, account_name=6230443535, nickname=NULL, data1=*102#, hash_id=iq2ofoLn1HGqo3dZhSyfden8wyQ=\r\n, data2=2\r\nRow: 3 display_name=V Anh, contact_last_updated_timestamp=1652943488791, company=NULL, account_type=com.xiaomi, account_name=6230443535, nickname=NULL, data1=+84334753576, hash_id=olhYSerEf/zHJd4YVibAF6H1XAA=\r\n, data2=2\r\nRow: 4 display_name=Nga, contact_last_updated_timestamp=1688196106811, company=NULL, account_type=com.xiaomi, account_name=6230443535, nickname=NULL, data1=+84337147355, hash_id=gwOZoELz0Y6NR91gIEraVnzYZ1Y=\r\n, data2=2\r\nRow: 5 display_name=Minh Anh, contact_last_updated_timestamp=1692294015008, company=NULL, account_type=com.xiaomi, account_name=6230443535, nickname=NULL, data1=+84369853831, hash_id=71gZr0EHxjo5Bk5soBffqXTImYY=\r\n, data2=2\r\nRow: 6 display_name=Bắc vh, contact_last_updated_timestamp=1666581107684, company=NULL, account_type=com.xiaomi, account_name=6230443535, nickname=NULL, data1=+84983495331, hash_id=FLb+r6GSq5xJUDl3HoCRVlMh2Kg=\r\n, data2=2\r\nRow: 7 display_name=Anh Hùng Mta, contact_last_updated_timestamp=1718921521756, company=NULL, account_type=com.google, account_name=hacongquoclun@gmail.com, nickname=NULL, data1=0326 620 168, hash_id=IvHRfTqId04jyhBiv+9LWVddmnA=\r\n, data2=2\r\nRow: 8 display_name=Nhôm, contact_last_updated_timestamp=1703903991241, company=NULL, account_type=com.xiaomi, account_name=6230443535, nickname=NULL, data1=0326266957, hash_id=XmHFr+gu0fZ67vfzLTFHTEesu5M=\r\n, data2=2\r\nRow: 9 display_name=Anh Hùng Mta, contact_last_updated_timestamp=1718921521756, company=NULL, account_type=com.xiaomi, account_name=6230443535, nickname=NULL, data1=0326620168, hash_id=MrCnLWZB+vLm7O+Dn6RYRmzifBQ=\r\n, data2=2\r\nRow: 10 display_name=Tuyến, contact_last_updated_timestamp=1703903992145, company=NULL, account_type=com.xiaomi, account_name=6230443535, nickname=NULL, data1=0327541957, hash_id=AxysETvVmT5CeiCCF/r/Jdf5YB4=\r\n, data2=2\r\nRow: 11 display_name=Phương4, contact_last_updated_timestamp=1688196106811, company=NULL, account_type=com.xiaomi, account_name=6230443535, nickname=NULL, data1=0328642585, hash_id=vZlbgKtU5RJO0y0MW9rxWaFB36s=\r\n, data2=2\r\nRow: 12 display_name=Em, contact_last_updated_timestamp=1699104655418, company=NULL, account_type=com.xiaomi, account_name=6230443535, nickname=NULL, data1=0329 281 596, hash_id=zjCH6xo5A4j1iKdafpR9GRAVEpI=\r\n, data2=2\r\nRow: 13 display_name=Nga, contact_last_updated_timestamp=1688196106811, company=NULL, account_type=com.xiaomi, account_name=6230443535, nickname=NULL, data1=0329226027, hash_id=qOtzw9P2rIqGvoFgpYqMB7lZ4Ds=\r\n, data2=2\r\nRow: 14 display_name=Bà Nội, contact_last_updated_timestamp=1718964222489, company=NULL, account_type=com.xiaomi, account_name=6230443535, nickname=NULL, data1=0329738193, hash_id=sG8xvp8Zf4FgZJ9UysGTTn1gafg=\r\n, data2=2\r\nRow: 15 display_name=Ông Ngoạ, contact_last_updated_timestamp=1660827005152, company=NULL, account_type=com.xiaomi, account_name=6230443535, nickname=NULL, data1=0333481474, hash_id=0s8GeqQpfrTpWIGL6NAiIM03s78=\r\n, data2=2\r\nRow: 16 display_name=Sâm, contact_last_updated_timestamp=1717022196504, company=NULL, account_type=com.xiaomi, account_name=6230443535, nickname=NULL, data1=0333671090, hash_id=pIn4XxNM2bK5eFQ9zysqLWhKBBw=\r\n, data2=2\r\nRow: 17 display_name=Bác Than, contact_last_updated_timestamp=1673051325638, company=NULL, account_type=com.xiaomi, account_name=6230443535, nickname=NULL, data1=0335813832, hash_id=FqPB6Fo2Gb70qxfPdwnIr7e/rWU=\r\n, data2=2\r\nRow: 18 display_name=Thế Cường, contact_last_updated_timestamp=1718921523070, company=NULL, account_type=com.google, account_name=hacongquoclun@gmail.com, nickname=NULL, data1=0336 037 903, hash_id=QbQadX+eT/dE9WbWw4wIiDKL4h8=\r\n, data2=2\r\nRow: 19 display_name=Âu, contact_last_updated_timestamp=1703903990583, company=NULL, account_type=com.xiaomi, account_name=6230443535, nickname=NULL, data1=0337132872, hash_id=TqN0MEwh7ULJva+KIx5Yvd7Lgio=\r\n, data2=2\r\nRow: 20 display_name=Cô Hiền, contact_last_updated_timestamp=1688196106811, company=NULL, account_type=com.xiaomi, account_name=6230443535, nickname=NULL, data1=0337237232, hash_id=O594secqE/UsR84HBoIixxthUoY=\r\n, data2=2";
            string pattern = @"Row:.*?(?=(Row:|$))";
            var lines = Regex.Matches(str1, pattern, RegexOptions.Singleline);

            foreach (var line in lines)
            {
                string display_name = function.GetValue(line.ToString(), "display_name", "contact_last_updated_timestamp");
                string contact_last_updated_timestamp = function.GetValue(line.ToString(), "contact_last_updated_timestamp", "company");
                contact_last_updated_timestamp = function.ConvertTimeStamp(contact_last_updated_timestamp);
                string company = function.GetValue(line.ToString(), "company", "account_type");
                string account_type = function.GetValue(line.ToString(), "account_type", "account_name");
                string account_name = function.GetValue(line.ToString(), "account_name", "nickname");
                string nickname = function.GetValue(line.ToString(), "nickname", "data1");
                string data1 = function.GetValue(line.ToString(), "data1", "hash_id");
                string hash_id = function.GetValue(line.ToString(), "hash_id", "data2");

                usr_DanhBaMini usr_DanhBaMini = new usr_DanhBaMini(display_name, contact_last_updated_timestamp, company, account_type, account_name, nickname, data1, hash_id);
                usr_DanhBaMini.ControlClicked += flpDanhBa_Click;
                usr_DanhBaMini.Width = flpDanhBa.Width;
                flpDanhBa.Controls.Add(usr_DanhBaMini);
            }
        }

        private void SearchTimer_Tick(object sender, EventArgs e)
        {
            searchTimer.Stop();
            TimKiemDanhBa();
        }

        private void usr_DanhBa_Load(object sender, EventArgs e)
        {
            Load_flpDanhBa();
        }

        private void flpDanhBa_Click(object sender, EventArgs e)
        {
            // Lấy dữ liệu từ user_control được click vào
            if (sender is usr_DanhBaMini clickedControl)
            {
                txtTenDanhBa.Text = clickedControl.display_name;
                txtSoDienThoai.Text = clickedControl.data1;
                if (clickedControl.company == "NULL")
                {
                    txtCongTy.Text = "Không có";
                }
                else
                {
                    txtCongTy.Text = clickedControl.company;
                }

                if (clickedControl.account_type == "com.google")
                {
                    txtLuuTai.Text = "Tài khoản Google";
                }
                else if (clickedControl.account_type == "com.android.contacts")
                {
                    txtLuuTai.Text = "Cục bộ trên thiết bị";
                }
                else if (clickedControl.account_type == "com.xiaomi")
                {
                    txtLuuTai.Text = "Tài khoản Xiaomi";
                }
                else if (clickedControl.account_type == "vnd.sec.contact.phone")
                {
                    txtLuuTai.Text = "Tài khoản Samsung";
                }
                else if (clickedControl.account_type == "com.whatsapp")
                {
                    txtLuuTai.Text = "Tài khoản WhatsApp";
                }
                else if (clickedControl.account_type == "com.android.sim" || clickedControl.account_type == "com.android.simcard")
                {
                    txtLuuTai.Text = "Thẻ sim";
                }

                txtTaiKhoanLuu.Text = clickedControl.account_name;

                if (clickedControl.nickname == "NULL")
                {
                    txtNickname.Text = "Không có";
                }
                else
                {
                    txtNickname.Text = clickedControl.nickname;
                }
                txtCapNhatLanCuoi.Text = clickedControl.contact_last_updated_timestamp;
                txtMaBam.Text = clickedControl.hash_id;
            }
        }

        private void flpDanhBa_Resize(object sender, EventArgs e)
        {
            foreach (UserControl control in flpDanhBa.Controls)
            {
                control.Width = flpDanhBa.Width;
            }
        }

        private void TimKiemDanhBa()
        {
            string searchText = txtTimKiem.Text.ToLower(); // Chuyển về lowercase để tìm kiếm không phân biệt chữ hoa chữ thường
            bool hasSearchText = !string.IsNullOrEmpty(searchText); // Kiểm tra xem có từ khóa tìm kiếm không

            foreach (usr_DanhBaMini control in flpDanhBa.Controls)
            {
                bool isVisible = true; // Mặc định hiển thị control

                if (hasSearchText)
                {
                    // Kiểm tra điều kiện tìm kiếm
                    isVisible = control.display_name.ToLower().Contains(searchText)
                             || control.data1.ToLower().Contains(searchText);
                }

                control.Visible = isVisible;
            }
        }

        private void txtTimKiem_TextChanged(object sender, EventArgs e)
        {
            searchTimer.Stop();
            searchTimer.Start();
        }

        private void btnXuat_Click(object sender, EventArgs e)
        {

        }

        private void btnNhap_Click(object sender, EventArgs e)
        {

        }

        private void btnChonTatCa_Click(object sender, EventArgs e)
        {
            foreach (usr_DanhBaMini control in flpDanhBa.Controls)
            {
                control.checkBox.Checked = true;
            }
        }

        private void btnBoChonTatCa_Click(object sender, EventArgs e)
        {
            foreach (usr_DanhBaMini control in flpDanhBa.Controls)
            {
                control.checkBox.Checked = false;
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            flpDanhBa.Controls.Clear();
            txtTenDanhBa.Text = string.Empty;
            txtSoDienThoai.Text = string.Empty;
            txtCongTy.Text = string.Empty;
            txtLuuTai.Text = string.Empty;
            txtTaiKhoanLuu.Text = string.Empty;
            txtNickname.Text = string.Empty;
            txtCapNhatLanCuoi.Text = string.Empty;
            txtMaBam.Text = string.Empty;
            txtTimKiem.Text = string.Empty;
            cbbTuyChon.Text = "Tất cả";

            usr_DanhBa_Load(sender, e);
        }

        private void cbbTuyChon_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (usr_DanhBaMini control in flpDanhBa.Controls)
            {
                if (cbbTuyChon.Text == "Tất cả")
                {
                    control.Visible = true;
                }
                else if (cbbTuyChon.Text == "Điện thoại")
                {
                    if (control.account_type == "com.android.contacts" || control.account_type == "com.xiaomi" || control.account_type == "vnd.sec.contact.phone")
                    {
                        control.Visible = true;
                    }
                    else
                    {
                        control.Visible = false;
                    }
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
                else if (cbbTuyChon.Text == "Sim")
                {
                    if (control.account_type == "com.android.sim" || control.account_type == "com.android.simcard")
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
