using DevExpress.XtraBars;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MTA_Mobile_Forensic.GUI.Android;
using MTA_Mobile_Forensic.GUI.Share;
using MTA_Mobile_Forensic.GUI.Forensic;

namespace MTA_Mobile_Forensic
{
    public partial class frmMain : DevExpress.XtraBars.FluentDesignSystem.FluentDesignForm
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            btnDieuTraMap_Click(sender, e);
        }

        //Trang chủ


        //Kết nối thiết bị


        //Tổng quan


        //Điều tra dữ liệu
        usr_TinNhan usr_TinNhan;
        usr_CuocGoi usr_CuocGoi;
        usr_DanhBa usr_DanhBa;
        usr_Lich usr_Lich;
        usr_Anh usr_Anh;
        usr_Video usr_Video;
        usr_GhiAm usr_GhiAm;
        usr_UngDung usr_UngDung;
        usr_CaiDat usr_CaiDat;
        usr_FileHeThong usr_FileHeThong;
        usr_ThongTinKhac usr_ThongTinKhac;


        //Điều khiển thiết bị
        usr_CaiDatUngDung usr_CaiDatUngDung;
        usr_PhanChieuThietBi usr_PhanChieuThietBi;

        //Điều tra thông tin
        usr_DieuTraNguoiDung usr_DieuTraNguoiDung;


        usr_DieuTraMap usr_DieuTraMap;

        //Sao lưu khôi phục


        //Nâng cao


        //Hướng dẫn sử dụng


        //Thông tin phần mềm


        private void btnTrangChu_Click(object sender, EventArgs e)
        {

        }

        private void btnTatCaThietBi_Click(object sender, EventArgs e)
        {

        }

        private void btnKetNoiThietBi_Click(object sender, EventArgs e)
        {

        }

        private void btnTongQuanSoLieuDieuTra_Click(object sender, EventArgs e)
        {

        }

        private void btnTongQuanThietBiKetNoi_Click(object sender, EventArgs e)
        {

        }

        private void btnTinNhan_Click(object sender, EventArgs e)
        {
            lblTieuDe1.Caption = "Điều tra dữ liệu";
            lblTieuDe2.Caption = "Tin nhắn";
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

        private void btnCuocGoi_Click(object sender, EventArgs e)
        {
            lblTieuDe1.Caption = "Điều tra dữ liệu";
            lblTieuDe2.Caption = "Cuộc gọi";
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

        private void btnDanhBa_Click(object sender, EventArgs e)
        {
            lblTieuDe1.Caption = "Điều tra dữ liệu";
            lblTieuDe2.Caption = "Danh bạ";
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

        private void btnLich_Click(object sender, EventArgs e)
        {
            lblTieuDe1.Caption = "Điều tra dữ liệu";
            lblTieuDe2.Caption = "Lịch";
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

        }

        private void btnDieuTraVideo_Click(object sender, EventArgs e)
        {

        }

        private void btnDieuTraAudio_Click(object sender, EventArgs e)
        {

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

        private void btnVirusTotal_Click(object sender, EventArgs e)
        {

        }

        private void btnSaoLuu_KhoiPhuc_Click(object sender, EventArgs e)
        {

        }

        private void btnKhoiPhucDuLieuDaXoa_Click(object sender, EventArgs e)
        {

        }

        private void btnThayDoiMatKhau_Click(object sender, EventArgs e)
        {

        }

        private void btnMoKhoaThietBi_Click(object sender, EventArgs e)
        {

        }

        private void btnRoot_JailbreakThietBi_Click(object sender, EventArgs e)
        {

        }
    }
}
