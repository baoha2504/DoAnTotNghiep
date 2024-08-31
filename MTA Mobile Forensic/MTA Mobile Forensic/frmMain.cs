using MTA_Mobile_Forensic.GUI.Android;
using MTA_Mobile_Forensic.GUI.Forensic;
using MTA_Mobile_Forensic.GUI.IOS;
using System;
using System.Windows.Forms;

namespace MTA_Mobile_Forensic
{
    public partial class frmMain : DevExpress.XtraBars.FluentDesignSystem.FluentDesignForm
    {
        string type_device_connected = "";

        public frmMain()
        {
            InitializeComponent();
            accordionControl1.Width = 260;
            //HienThiForm(0, "");
            HienThiForm(1, "IPHONE");
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            btnDieuTraAudio_Click(sender, e);
        }

        //Trang chủ

        usr_KetNoiThietBi usr_KetNoiThietBi;

        //Điều tra dữ liệu
        usr_TinNhan usr_TinNhan;
        usr_TinNhan_IOS usr_TinNhan_IOS;
        usr_CuocGoi usr_CuocGoi;
        usr_CuocGoi_IOS usr_CuocGoi_IOS;
        usr_DanhBa usr_DanhBa;
        usr_DanhBa_IOS usr_DanhBa_IOS;
        usr_Lich usr_Lich;
        usr_Lich_IOS usr_Lich_IOS;
        usr_Anh usr_Anh;
        usr_Video usr_Video;
        usr_GhiAm usr_GhiAm;
        usr_UngDung usr_UngDung;
        usr_UngDung_IOS usr_UngDung_IOS;
        usr_DuLieuLog usr_DuLieuLog;
        usr_CaiDat usr_CaiDat;
        usr_FileHeThong usr_FileHeThong;
        usr_ThongTinKhac usr_ThongTinKhac;
        usr_LichSuTruyCap_IOS usr_LichSuTruyCap_IOS;


        //Điều khiển thiết bị
        usr_CaiDatUngDung usr_CaiDatUngDung;
        usr_CaiDatUngDung_IOS usr_CaiDatUngDung_IOS;
        usr_PhanChieuThietBi usr_PhanChieuThietBi;

        //Điều tra thông tin
        usr_DieuTraNguoiDung usr_DieuTraNguoiDung;
        usr_DieuTraAnh usr_DieuTraAnh;

        usr_DieuTraAudio usr_DieuTraAudio;
        usr_DieuTraTaiLieu usr_DieuTraTaiLieu;
        usr_DieuTraMap usr_DieuTraMap;
        usr_DieuTraFile usr_DieuTraFile;

        //Sao lưu khôi phục
        usr_SaoLuuKhoiPhuc usr_SaoLuuKhoiPhuc;
        usr_SaoLuuKhoiPhuc_IOS usr_SaoLuuKhoiPhuc_IOS;

        //Nâng cao
        usr_ThayDoiMatKhau usr_ThayDoiMatKhau;
        usr_MoKhoaThietBi usr_MoKhoaThietBi;
        usr_RootJailbreakThietBi usr_RootJailbreakThietBi;

        //Trợ giúp

        public void HienThiForm(int hienthi, string type)
        {
            CheckTypeDevice(type);
            if (hienthi == 1)
            {
                btnTinNhan.Enabled = true;
                btnCuocGoi.Enabled = true;
                btnDanhBa.Enabled = true;
                btnLich.Enabled = true;
                btnAnh.Enabled = true;
                btnVideo.Enabled = true;
                btnGhiAm.Enabled = true;
                btnUngDung.Enabled = true;
                btnDuLieuLog.Enabled = true;
                btnCaiDatCuaThietBi.Enabled = true;
                btnLichSuTruyCap.Enabled = true;
                btnFileSystem.Enabled = true;
                btnThongTinKhac.Enabled = true;
                btnCaiDatUngDung.Enabled = true;
                btnPhanChieuThietBi.Enabled = true;
                btnDieuTraNguoiDung.Enabled = true;
                btnDieuTraAnh.Enabled = true;
                btnDieuTraVideo.Enabled = true;
                btnDieuTraAudio.Enabled = true;
                btnDieuTraTaiLieu.Enabled = true;
                btnDieuTraMap.Enabled = true;
                btnDieuTraFile.Enabled = true;
                btnSaoLuu_KhoiPhuc.Enabled = true;
                btnPhucHoiDuLieu.Enabled = true;
                btnThayDoiMatKhau.Enabled = true;
                btnMoKhoaThietBi.Enabled = true;
                btnRoot_JailbreakThietBi.Enabled = true;
            }
            else if (hienthi == 0)
            {
                btnTinNhan.Enabled = false;
                btnCuocGoi.Enabled = false;
                btnDanhBa.Enabled = false;
                btnLich.Enabled = false;
                btnAnh.Enabled = false;
                btnVideo.Enabled = false;
                btnGhiAm.Enabled = false;
                btnUngDung.Enabled = false;
                btnDuLieuLog.Enabled = false;
                btnCaiDatCuaThietBi.Enabled = false;
                btnLichSuTruyCap.Enabled = false;
                btnFileSystem.Enabled = false;
                btnThongTinKhac.Enabled = false;
                btnCaiDatUngDung.Enabled = false;
                btnPhanChieuThietBi.Enabled = false;
                btnDieuTraNguoiDung.Enabled = false;
                btnDieuTraAnh.Enabled = false;
                btnDieuTraVideo.Enabled = false;
                btnDieuTraAudio.Enabled = false;
                btnDieuTraTaiLieu.Enabled = false;
                btnDieuTraMap.Enabled = false;
                btnDieuTraFile.Enabled = false;
                btnSaoLuu_KhoiPhuc.Enabled = false;
                btnPhucHoiDuLieu.Enabled = false;
                btnThayDoiMatKhau.Enabled = false;
                btnMoKhoaThietBi.Enabled = false;
                btnRoot_JailbreakThietBi.Enabled = false;
            }
        }

        private void CheckTypeDevice(string type)
        {
            if (type == "IPHONE")
            {
                // ẩn form nào
                btnDuLieuLog.Visible = false;
                btnCaiDatCuaThietBi.Visible = false;
                btnLichSuTruyCap.Visible = false;
                btnFileSystem.Visible = false;
                btnPhanChieuThietBi.Visible = false;
                btnThayDoiMatKhau.Visible = false;
                btnMoKhoaThietBi.Visible = false;
                btnThongTinKhac.Visible = false;
                btnLichSuTruyCap.Visible = true;

                type_device_connected = "IPHONE";
            }
            else if (type == "ANDROID")
            {
                // hiển thị tất cả
                btnDuLieuLog.Visible = true;
                btnCaiDatCuaThietBi.Visible = true;
                btnLichSuTruyCap.Visible = true;
                btnFileSystem.Visible = true;
                btnPhanChieuThietBi.Visible = true;
                btnThayDoiMatKhau.Visible = true;
                btnMoKhoaThietBi.Visible = true;
                btnThongTinKhac.Visible = true;
                btnLichSuTruyCap.Visible = false;

                type_device_connected = "ANDROID";
            }
        }


        private void btnTrangChu_Click(object sender, EventArgs e)
        {

        }

        private void btnKetNoiThietBi_Click(object sender, EventArgs e)
        {
            lblTieuDe1.Caption = "Trang chủ";
            lblTieuDe2.Caption = "Kết nối thiết bị";
            if (usr_KetNoiThietBi == null)
            {
                usr_KetNoiThietBi = new usr_KetNoiThietBi(this);
                usr_KetNoiThietBi.Dock = DockStyle.Fill;
                mainContainer.Controls.Add(usr_KetNoiThietBi);
                usr_KetNoiThietBi.BringToFront();
            }
            else
            {
                usr_KetNoiThietBi.BringToFront();
            }
        }


        private void btnTinNhan_Click(object sender, EventArgs e)
        {
            lblTieuDe1.Caption = "Điều tra dữ liệu";
            lblTieuDe2.Caption = "Tin nhắn";
            if (type_device_connected == "IPHONE")
            {
                if (usr_TinNhan_IOS == null)
                {
                    usr_TinNhan_IOS = new usr_TinNhan_IOS();
                    usr_TinNhan_IOS.Dock = DockStyle.Fill;
                    mainContainer.Controls.Add(usr_TinNhan_IOS);
                    usr_TinNhan_IOS.BringToFront();
                }
                else
                {
                    usr_TinNhan_IOS.BringToFront();
                }
            }
            else if (type_device_connected == "ANDROID")
            {
                if (usr_TinNhan == null)
                {
                    usr_TinNhan = new usr_TinNhan();
                    usr_TinNhan.Dock = DockStyle.Fill;
                    mainContainer.Controls.Add(usr_TinNhan);
                    usr_TinNhan.BringToFront();
                }
                else
                {
                    usr_TinNhan.BringToFront();
                }
            }
        }

        private void btnCuocGoi_Click(object sender, EventArgs e)
        {
            lblTieuDe1.Caption = "Điều tra dữ liệu";
            lblTieuDe2.Caption = "Cuộc gọi";
            if (type_device_connected == "IPHONE")
            {
                if (usr_CuocGoi_IOS == null)
                {
                    usr_CuocGoi_IOS = new usr_CuocGoi_IOS();
                    usr_CuocGoi_IOS.Dock = DockStyle.Fill;
                    mainContainer.Controls.Add(usr_CuocGoi_IOS);
                    usr_CuocGoi_IOS.BringToFront();
                }
                else
                {
                    usr_CuocGoi_IOS.BringToFront();
                }
            }
            else if (type_device_connected == "ANDROID")
            {
                if (usr_CuocGoi == null)
                {
                    usr_CuocGoi = new usr_CuocGoi();
                    usr_CuocGoi.Dock = DockStyle.Fill;
                    mainContainer.Controls.Add(usr_CuocGoi);
                    usr_CuocGoi.BringToFront();
                }
                else
                {
                    usr_CuocGoi.BringToFront();
                }
            }
        }

        private void btnDanhBa_Click(object sender, EventArgs e)
        {
            lblTieuDe1.Caption = "Điều tra dữ liệu";
            lblTieuDe2.Caption = "Danh bạ";
            if (type_device_connected == "IPHONE")
            {
                if (usr_DanhBa_IOS == null)
                {
                    usr_DanhBa_IOS = new usr_DanhBa_IOS();
                    usr_DanhBa_IOS.Dock = DockStyle.Fill;
                    mainContainer.Controls.Add(usr_DanhBa_IOS);
                    usr_DanhBa_IOS.BringToFront();
                }
                else
                {
                    usr_DanhBa_IOS.BringToFront();
                }
            }
            else if (type_device_connected == "ANDROID")
            {
                if (usr_DanhBa == null)
                {
                    usr_DanhBa = new usr_DanhBa();
                    usr_DanhBa.Dock = DockStyle.Fill;
                    mainContainer.Controls.Add(usr_DanhBa);
                    usr_DanhBa.BringToFront();
                }
                else
                {
                    usr_DanhBa.BringToFront();
                }
            }
        }

        private void btnLich_Click(object sender, EventArgs e)
        {
            lblTieuDe1.Caption = "Điều tra dữ liệu";
            lblTieuDe2.Caption = "Lịch";
            if (type_device_connected == "IPHONE")
            {
                if (usr_Lich_IOS == null)
                {
                    usr_Lich_IOS = new usr_Lich_IOS();
                    usr_Lich_IOS.Dock = DockStyle.Fill;
                    mainContainer.Controls.Add(usr_Lich_IOS);
                    usr_Lich_IOS.BringToFront();
                }
                else
                {
                    usr_Lich_IOS.BringToFront();
                }
            }
            else if (type_device_connected == "ANDROID")
            {
                if (usr_Lich == null)
                {
                    usr_Lich = new usr_Lich();
                    usr_Lich.Dock = DockStyle.Fill;
                    mainContainer.Controls.Add(usr_Lich);
                    usr_Lich.BringToFront();
                }
                else
                {
                    usr_Lich.BringToFront();
                }
            }
        }

        private void btnAnh_Click(object sender, EventArgs e)
        {
            lblTieuDe1.Caption = "Điều tra dữ liệu";
            lblTieuDe2.Caption = "Ảnh";
            if (usr_Anh == null)
            {
                usr_Anh = new usr_Anh();
                usr_Anh.Dock = DockStyle.Fill;
                mainContainer.Controls.Add(usr_Anh);
                usr_Anh.BringToFront();
            }
            else
            {
                usr_Anh.BringToFront();
            }
        }

        private void btnVideo_Click(object sender, EventArgs e)
        {
            lblTieuDe1.Caption = "Điều tra dữ liệu";
            lblTieuDe2.Caption = "Video";
            if (usr_Video == null)
            {
                usr_Video = new usr_Video();
                usr_Video.Dock = DockStyle.Fill;
                mainContainer.Controls.Add(usr_Video);
                usr_Video.BringToFront();
            }
            else
            {
                usr_Video.BringToFront();
            }
        }

        private void btnGhiAm_Click(object sender, EventArgs e)
        {
            lblTieuDe1.Caption = "Điều tra dữ liệu";
            lblTieuDe2.Caption = "Ghi âm";
            if (usr_GhiAm == null)
            {
                usr_GhiAm = new usr_GhiAm();
                usr_GhiAm.Dock = DockStyle.Fill;
                mainContainer.Controls.Add(usr_GhiAm);
                usr_GhiAm.BringToFront();
            }
            else
            {
                usr_GhiAm.BringToFront();
            }
        }

        private void btnUngDung_Click(object sender, EventArgs e)
        {
            lblTieuDe1.Caption = "Điều tra dữ liệu";
            lblTieuDe2.Caption = "Ứng dụng";
            if (type_device_connected == "IPHONE")
            {
                if (usr_UngDung_IOS == null)
                {
                    usr_UngDung_IOS = new usr_UngDung_IOS();
                    usr_UngDung_IOS.Dock = DockStyle.Fill;
                    mainContainer.Controls.Add(usr_UngDung_IOS);
                    usr_UngDung_IOS.BringToFront();
                }
                else
                {
                    usr_UngDung_IOS.BringToFront();
                }
            }
            else if (type_device_connected == "ANDROID")
            {
                if (usr_UngDung == null)
                {
                    usr_UngDung = new usr_UngDung();
                    usr_UngDung.Dock = DockStyle.Fill;
                    mainContainer.Controls.Add(usr_UngDung);
                    usr_UngDung.BringToFront();
                }
                else
                {
                    usr_UngDung.BringToFront();
                }
            }
        }

        private void btnDuLieuLog_Click(object sender, EventArgs e)
        {
            lblTieuDe1.Caption = "Điều tra dữ liệu";
            lblTieuDe2.Caption = "Log hệ thống";
            if (usr_DuLieuLog == null)
            {
                usr_DuLieuLog = new usr_DuLieuLog();
                usr_DuLieuLog.Dock = DockStyle.Fill;
                mainContainer.Controls.Add(usr_DuLieuLog);
                usr_DuLieuLog.BringToFront();
            }
            else
            {
                usr_DuLieuLog.BringToFront();
            }
        }

        private void btnCaiDatCuaThietBi_Click(object sender, EventArgs e)
        {
            lblTieuDe1.Caption = "Điều tra dữ liệu";
            lblTieuDe2.Caption = "Cài đặt trên thiết bị";
            if (usr_CaiDat == null)
            {
                usr_CaiDat = new usr_CaiDat();
                usr_CaiDat.Dock = DockStyle.Fill;
                mainContainer.Controls.Add(usr_CaiDat);
                usr_CaiDat.BringToFront();
            }
            else
            {
                usr_CaiDat.BringToFront();
            }
        }

        private void btnLichSuTruyCap_Click(object sender, EventArgs e)
        {
            lblTieuDe1.Caption = "Điều tra dữ liệu";
            lblTieuDe2.Caption = "Lịch sử truy cập của thiết bị";
            if (type_device_connected == "IPHONE")
            {
                if (usr_LichSuTruyCap_IOS == null)
                {
                    usr_LichSuTruyCap_IOS = new usr_LichSuTruyCap_IOS();
                    usr_LichSuTruyCap_IOS.Dock = DockStyle.Fill;
                    mainContainer.Controls.Add(usr_LichSuTruyCap_IOS);
                    usr_LichSuTruyCap_IOS.BringToFront();
                }
                else
                {
                    usr_LichSuTruyCap_IOS.BringToFront();
                }
            }
            else if (type_device_connected == "ANDROID")
            {

            }
        }

        private void btnFileSystem_Click(object sender, EventArgs e)
        {
            lblTieuDe1.Caption = "Điều tra dữ liệu";
            lblTieuDe2.Caption = "File hệ thống";
            if (usr_FileHeThong == null)
            {
                usr_FileHeThong = new usr_FileHeThong();
                usr_FileHeThong.Dock = DockStyle.Fill;
                mainContainer.Controls.Add(usr_FileHeThong);
                usr_FileHeThong.BringToFront();
            }
            else
            {
                usr_FileHeThong.BringToFront();
            }
        }

        private void btnThongTinKhac_Click(object sender, EventArgs e)
        {
            lblTieuDe1.Caption = "Điều tra dữ liệu";
            lblTieuDe2.Caption = "Thông tin khác";
            if (usr_ThongTinKhac == null)
            {
                usr_ThongTinKhac = new usr_ThongTinKhac();
                usr_ThongTinKhac.Dock = DockStyle.Fill;
                mainContainer.Controls.Add(usr_ThongTinKhac);
                usr_ThongTinKhac.BringToFront();
            }
            else
            {
                usr_ThongTinKhac.BringToFront();
            }
        }

        private void btnCaiDatUngDung_Click(object sender, EventArgs e)
        {
            lblTieuDe1.Caption = "Điều khiển thiết bị";
            lblTieuDe2.Caption = "Cài đặt và gỡ cài đặt";
            if (type_device_connected == "IPHONE")
            {
                if (usr_CaiDatUngDung_IOS == null)
                {
                    usr_CaiDatUngDung_IOS = new usr_CaiDatUngDung_IOS();
                    usr_CaiDatUngDung_IOS.Dock = DockStyle.Fill;
                    mainContainer.Controls.Add(usr_CaiDatUngDung_IOS);
                    usr_CaiDatUngDung_IOS.BringToFront();
                }
                else
                {
                    usr_CaiDatUngDung_IOS.BringToFront();
                }
            }
            else if (type_device_connected == "ANDROID")
            {
                if (usr_CaiDatUngDung == null)
                {
                    usr_CaiDatUngDung = new usr_CaiDatUngDung();
                    usr_CaiDatUngDung.Dock = DockStyle.Fill;
                    mainContainer.Controls.Add(usr_CaiDatUngDung);
                    usr_CaiDatUngDung.BringToFront();
                }
                else
                {
                    usr_CaiDatUngDung.BringToFront();
                }
            }
        }

        private void btnPhanChieuThietBi_Click(object sender, EventArgs e)
        {
            lblTieuDe1.Caption = "Điều khiển thiết bị";
            lblTieuDe2.Caption = "Phản chiếu thiết bị";
            if (usr_PhanChieuThietBi == null)
            {
                usr_PhanChieuThietBi = new usr_PhanChieuThietBi();
                usr_PhanChieuThietBi.Dock = DockStyle.Fill;
                mainContainer.Controls.Add(usr_PhanChieuThietBi);
                usr_PhanChieuThietBi.BringToFront();
            }
            else
            {
                usr_PhanChieuThietBi.BringToFront();
            }
        }

        private void btnDieuTraNguoiDung_Click(object sender, EventArgs e)
        {
            lblTieuDe1.Caption = "Điều tra thông tin";
            lblTieuDe2.Caption = "Thông tin người dùng";
            if (usr_DieuTraNguoiDung == null)
            {
                usr_DieuTraNguoiDung = new usr_DieuTraNguoiDung();
                usr_DieuTraNguoiDung.Dock = DockStyle.Fill;
                mainContainer.Controls.Add(usr_DieuTraNguoiDung);
                usr_DieuTraNguoiDung.BringToFront();
            }
            else
            {
                usr_DieuTraNguoiDung.BringToFront();
            }
        }

        private void btnDieuTraAnh_Click(object sender, EventArgs e)
        {
            lblTieuDe1.Caption = "Điều tra thông tin";
            lblTieuDe2.Caption = "Ảnh";
            if (usr_DieuTraAnh == null)
            {
                usr_DieuTraAnh = new usr_DieuTraAnh();
                usr_DieuTraAnh.Dock = DockStyle.Fill;
                mainContainer.Controls.Add(usr_DieuTraAnh);
                usr_DieuTraAnh.BringToFront();
            }
            else
            {
                usr_DieuTraAnh.BringToFront();
            }
        }

        private void btnDieuTraVideo_Click(object sender, EventArgs e)
        {

        }

        private void btnDieuTraAudio_Click(object sender, EventArgs e)
        {
            lblTieuDe1.Caption = "Điều tra thông tin";
            lblTieuDe2.Caption = "Âm thanh";
            if (usr_DieuTraAudio == null)
            {
                usr_DieuTraAudio = new usr_DieuTraAudio();
                usr_DieuTraAudio.Dock = DockStyle.Fill;
                mainContainer.Controls.Add(usr_DieuTraAudio);
                usr_DieuTraAudio.BringToFront();
            }
            else
            {
                usr_DieuTraAudio.BringToFront();
            }
        }

        private void btnDieuTraTaiLieu_Click(object sender, EventArgs e)
        {
            lblTieuDe1.Caption = "Điều tra thông tin";
            lblTieuDe2.Caption = "Tài liệu";
            if (usr_DieuTraTaiLieu == null)
            {
                usr_DieuTraTaiLieu = new usr_DieuTraTaiLieu();
                usr_DieuTraTaiLieu.Dock = DockStyle.Fill;
                mainContainer.Controls.Add(usr_DieuTraTaiLieu);
                usr_DieuTraTaiLieu.BringToFront();
            }
            else
            {
                usr_DieuTraTaiLieu.BringToFront();
            }
        }

        private void btnDieuTraMap_Click(object sender, EventArgs e)
        {
            lblTieuDe1.Caption = "Điều tra thông tin";
            lblTieuDe2.Caption = "Map";
            if (usr_DieuTraMap == null)
            {
                usr_DieuTraMap = new usr_DieuTraMap();
                usr_DieuTraMap.Dock = DockStyle.Fill;
                mainContainer.Controls.Add(usr_DieuTraMap);
                usr_DieuTraMap.BringToFront();
            }
            else
            {
                usr_DieuTraMap.BringToFront();
            }
        }

        private void btnDieuTraFile_Click(object sender, EventArgs e)
        {
            lblTieuDe1.Caption = "Điều tra thông tin";
            lblTieuDe2.Caption = "File";
            if (usr_DieuTraFile == null)
            {
                usr_DieuTraFile = new usr_DieuTraFile();
                usr_DieuTraFile.Dock = DockStyle.Fill;
                mainContainer.Controls.Add(usr_DieuTraFile);
                usr_DieuTraFile.BringToFront();
            }
            else
            {
                usr_DieuTraFile.BringToFront();
            }
        }

        private void btnSaoLuu_KhoiPhuc_Click(object sender, EventArgs e)
        {
            lblTieuDe1.Caption = "Sao lưu và khôi phục";
            lblTieuDe2.Caption = "Sao lưu và khôi phục dữ liệu";
            if (type_device_connected == "IPHONE")
            {
                if (usr_SaoLuuKhoiPhuc_IOS == null)
                {
                    usr_SaoLuuKhoiPhuc_IOS = new usr_SaoLuuKhoiPhuc_IOS();
                    usr_SaoLuuKhoiPhuc_IOS.Dock = DockStyle.Fill;
                    mainContainer.Controls.Add(usr_SaoLuuKhoiPhuc_IOS);
                    usr_SaoLuuKhoiPhuc_IOS.BringToFront();
                }
                else
                {
                    usr_SaoLuuKhoiPhuc_IOS.BringToFront();
                }
            }
            else if (type_device_connected == "ANDROID")
            {
                if (usr_SaoLuuKhoiPhuc == null)
                {
                    usr_SaoLuuKhoiPhuc = new usr_SaoLuuKhoiPhuc();
                    usr_SaoLuuKhoiPhuc.Dock = DockStyle.Fill;
                    mainContainer.Controls.Add(usr_SaoLuuKhoiPhuc);
                    usr_SaoLuuKhoiPhuc.BringToFront();
                }
                else
                {
                    usr_SaoLuuKhoiPhuc.BringToFront();
                }
            }
        }

        private void btnPhucHoiDuLieu_Click(object sender, EventArgs e)
        {
            lblTieuDe1.Caption = "Điều khiển thiết bị";
            lblTieuDe2.Caption = "Phục hồi dữ liệu";
            if (type_device_connected == "IPHONE")
            {
                if (usr_CaiDatUngDung_IOS == null)
                {
                    usr_CaiDatUngDung_IOS = new usr_CaiDatUngDung_IOS();
                    usr_CaiDatUngDung_IOS.Dock = DockStyle.Fill;
                    mainContainer.Controls.Add(usr_CaiDatUngDung_IOS);
                    usr_CaiDatUngDung_IOS.BringToFront();
                }
                else
                {
                    usr_CaiDatUngDung_IOS.BringToFront();
                }
            }
            else if (type_device_connected == "ANDROID")
            {
                if (usr_CaiDatUngDung == null)
                {
                    usr_CaiDatUngDung = new usr_CaiDatUngDung();
                    usr_CaiDatUngDung.Dock = DockStyle.Fill;
                    mainContainer.Controls.Add(usr_CaiDatUngDung);
                    usr_CaiDatUngDung.BringToFront();
                }
                else
                {
                    usr_CaiDatUngDung.BringToFront();
                }
            }
        }

        private void btnThayDoiMatKhau_Click(object sender, EventArgs e)
        {
            lblTieuDe1.Caption = "Nâng cao";
            lblTieuDe2.Caption = "Thay đổi mật khẩu";
            if (usr_ThayDoiMatKhau == null)
            {
                usr_ThayDoiMatKhau = new usr_ThayDoiMatKhau();
                usr_ThayDoiMatKhau.Dock = DockStyle.Fill;
                mainContainer.Controls.Add(usr_ThayDoiMatKhau);
                usr_ThayDoiMatKhau.BringToFront();
            }
            else
            {
                usr_ThayDoiMatKhau.BringToFront();
            }
        }

        private void btnMoKhoaThietBi_Click(object sender, EventArgs e)
        {
            lblTieuDe1.Caption = "Nâng cao";
            lblTieuDe2.Caption = "Mở khóa thiết bị";
            if (usr_MoKhoaThietBi == null)
            {
                usr_MoKhoaThietBi = new usr_MoKhoaThietBi();
                usr_MoKhoaThietBi.Dock = DockStyle.Fill;
                mainContainer.Controls.Add(usr_MoKhoaThietBi);
                usr_MoKhoaThietBi.BringToFront();
            }
            else
            {
                usr_MoKhoaThietBi.BringToFront();
            }
        }

        private void btnRoot_JailbreakThietBi_Click(object sender, EventArgs e)
        {
            lblTieuDe1.Caption = "Nâng cao";
            lblTieuDe2.Caption = "Root thiết bị";

            if (type_device_connected == "IPHONE")
            {
                if (usr_RootJailbreakThietBi == null)
                {
                    usr_RootJailbreakThietBi = new usr_RootJailbreakThietBi();
                    usr_RootJailbreakThietBi.Dock = DockStyle.Fill;
                    mainContainer.Controls.Add(usr_RootJailbreakThietBi);
                    usr_RootJailbreakThietBi.BringToFront();
                    usr_RootJailbreakThietBi.TabIndex = 1;
                }
                else
                {
                    usr_RootJailbreakThietBi.BringToFront();
                    usr_RootJailbreakThietBi.TabIndex = 1;
                }
            }
            else if (type_device_connected == "ANDROID")
            {
                if (usr_RootJailbreakThietBi == null)
                {
                    usr_RootJailbreakThietBi = new usr_RootJailbreakThietBi();
                    usr_RootJailbreakThietBi.Dock = DockStyle.Fill;
                    mainContainer.Controls.Add(usr_RootJailbreakThietBi);
                    usr_RootJailbreakThietBi.BringToFront();
                    usr_RootJailbreakThietBi.TabIndex = 0;
                }
                else
                {
                    usr_RootJailbreakThietBi.BringToFront();
                    usr_RootJailbreakThietBi.TabIndex = 0;
                }
            }
        }
    }
}
