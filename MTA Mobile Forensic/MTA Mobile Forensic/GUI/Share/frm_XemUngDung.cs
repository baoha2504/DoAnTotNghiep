using System.Windows.Forms;

namespace MTA_Mobile_Forensic.GUI.Share
{
    public partial class frm_XemUngDung : Form
    {
        public frm_XemUngDung()
        {
            InitializeComponent();
        }

        public frm_XemUngDung(string package, string tenungdung, string ngaycaidat, string capnhatlancuoi, string phienban)
        {
            InitializeComponent();
            this.Text = "Ứng dụng " + tenungdung;

            txtGoi.Text = package;
            txtTenUngDung.Text = tenungdung;
            txtNgayCaiDat.Text = ngaycaidat;
            txtCapNhatLanCuoi.Text = capnhatlancuoi;
            txtPhienBan.Text = phienban;

            LoadWeb(package);
        }

        private async void LoadWeb(string package)
        {
            string link = $"https://apkpure.net/vn/{package}";
            await webView21.EnsureCoreWebView2Async(null);
            webView21.CoreWebView2.Navigate(link);
        }
    }
}
