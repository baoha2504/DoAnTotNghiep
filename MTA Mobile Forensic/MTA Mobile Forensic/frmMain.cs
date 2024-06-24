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
            btnTinNhan_Click(sender, e);
        }

        usr_TinNhan usr_TinNhan;
        usr_CuocGoi usr_CuocGoi;

        private void btnTrangChu_Click(object sender, EventArgs e)
        {
            //if (usr_ThemHocVien == null)
            //{
            //    usr_ThemHocVien usr_ThemHocVien = new usr_ThemHocVien();
            //    usr_ThemHocVien.Dock = DockStyle.Fill;
            //    mainContainer.Controls.Add(usr_ThemHocVien);
            //    usr_ThemHocVien.BringToFront();
            //}
            //else
            //{
            //    usr_ThemHocVien.BringToFront();
            //}
            //lblTieuDe.Caption = "Thêm học viên";
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

        }

        private void btnEmail_Click(object sender, EventArgs e)
        {

        }

        private void btnAnh_Click(object sender, EventArgs e)
        {

        }

        private void btnVideo_Click(object sender, EventArgs e)
        {

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
