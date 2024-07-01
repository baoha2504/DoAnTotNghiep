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
            btnVideo_Click(sender, e);
        }

        usr_TinNhan usr_TinNhan;
        usr_CuocGoi usr_CuocGoi;
        usr_DanhBa usr_DanhBa;
        usr_Lich usr_Lich;
        usr_Anh usr_Anh;
        usr_Video usr_Video;


        private void btnTrangChu_Click(object sender, EventArgs e)
        {

        }

        private void btnThongBao_Click(object sender, EventArgs e)
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
            lblTieuDe1.Caption = "Điều tra dữ liệu";
            lblTieuDe2.Caption = "Tin nhắn";
        }

        private void btnCuocGoi_Click(object sender, EventArgs e)
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
            lblTieuDe1.Caption = "Điều tra dữ liệu";
            lblTieuDe2.Caption = "Cuộc gọi";
        }

        private void btnDanhBa_Click(object sender, EventArgs e)
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
            lblTieuDe1.Caption = "Điều tra dữ liệu";
            lblTieuDe2.Caption = "Danh bạ";
        }

        private void btnLich_Click(object sender, EventArgs e)
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
            lblTieuDe1.Caption = "Điều tra dữ liệu";
            lblTieuDe2.Caption = "Lịch";
        }

        private void btnAnh_Click(object sender, EventArgs e)
        {
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
            lblTieuDe1.Caption = "Điều tra dữ liệu";
            lblTieuDe2.Caption = "Ảnh";
        }

        private void btnVideo_Click(object sender, EventArgs e)
        {
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
            lblTieuDe1.Caption = "Điều tra dữ liệu";
            lblTieuDe2.Caption = "Video";
        }

        private void btnUngDung_Click(object sender, EventArgs e)
        {

        }

        private void btnCaiDatCuaThietBi_Click(object sender, EventArgs e)
        {

        }

        private void btnLichSuTruyCap_Click(object sender, EventArgs e)
        {

        }

        private void btnMangDaKetNoi_Click(object sender, EventArgs e)
        {

        }

        private void btnTheSim_Click(object sender, EventArgs e)
        {

        }

        private void btnMaHash_Click(object sender, EventArgs e)
        {

        }

        private void btnThongTinKhac_Click(object sender, EventArgs e)
        {

        }

        private void btnFileSystem_Click(object sender, EventArgs e)
        {

        }

        private void btnCaiDatUngDung_Click(object sender, EventArgs e)
        {

        }

        private void btnGoCaiDat_Click(object sender, EventArgs e)
        {

        }

        private void btnThayDoiMatKhau_Click(object sender, EventArgs e)
        {

        }

        private void btnDeviceMirroring_Click(object sender, EventArgs e)
        {

        }

        private void btnChupAnhManHinh_Click(object sender, EventArgs e)
        {

        }

        private void btnDieuTraNguoiDung_Click(object sender, EventArgs e)
        {

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

        }

        private void btnVirusTotal_Click(object sender, EventArgs e)
        {

        }

        private void btnSaoLuuDuLieu_Click(object sender, EventArgs e)
        {

        }

        private void btnKhoiPhucDuLieu_Click(object sender, EventArgs e)
        {

        }

        private void btnImportDuLieu_Click(object sender, EventArgs e)
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
