using System;
using System.Drawing;
using System.Security.Policy;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using MTA_Mobile_Forensic.GUI.Forensic;

namespace MTA_Mobile_Forensic.GUI.Forensic
{
    public partial class usr_DieuTraNguoiDung : UserControl
    {
        private ToolTip toolTip;
        public usr_DieuTraNguoiDung()
        {
            InitializeComponent();
            toolTip = new ToolTip();
            toolTip.SetToolTip(pictureBox_SearchGoogle, "Tìm kiếm Google");
            toolTip.SetToolTip(pictureBox_SearchFacebook, "Tìm kiếm Facebook");
            toolTip.SetToolTip(pictureBox_SearchZalo, "Tìm kiếm Zalo");
            toolTip.SetToolTip(pictureBox_SearchTelegram, "Tìm kiếm Telegram");
        }

        private void panelEx1_Resize(object sender, EventArgs e)
        {
            try
            {
                splitContainer1.SplitterDistance = (int)(panelEx1.Height / 2) - 20;
                splitContainer2.SplitterDistance = (int)(panelEx1.Width / 2) - 20;
                splitContainer3.SplitterDistance = (int)(panelEx1.Width / 2) - 20;
            }
            catch { }
        }

        private async void LoadWeb_Google(string link)
        {
            await webView_Google.EnsureCoreWebView2Async(null);
            webView_Google.CoreWebView2.Navigate(link);
        }

        private async void ClearWeb_Google()
        {
            await webView_Google.EnsureCoreWebView2Async(null);
            webView_Google.CoreWebView2.Navigate("about:blank");
        }

        private async void LoadWeb_Facebook(string link)
        {
            await webView_Facebook.EnsureCoreWebView2Async(null);
            webView_Facebook.CoreWebView2.Navigate(link);
        }

        private async void ClearWeb_Facebook()
        {
            await webView_Facebook.EnsureCoreWebView2Async(null);
            webView_Facebook.CoreWebView2.Navigate("about:blank");
        }

        private async void LoadWeb_Zalo(string link)
        {
            await webView_Zalo.EnsureCoreWebView2Async(null);
            webView_Zalo.CoreWebView2.Navigate(link);
        }

        private async void ClearWeb_Zalo()
        {
            await webView_Zalo.EnsureCoreWebView2Async(null);
            webView_Zalo.CoreWebView2.Navigate("about:blank");
        }

        private async void LoadWeb_Telegram(string link)
        {
            await webView_Telegram.EnsureCoreWebView2Async(null);
            webView_Telegram.CoreWebView2.Navigate(link);
        }

        private async void ClearWeb_Telegram()
        {
            await webView_Telegram.EnsureCoreWebView2Async(null);
            webView_Telegram.CoreWebView2.Navigate("about:blank");
        }

        //========= GOOGLE
        public bool IsValidUrl(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return false;
            }

            if (Uri.TryCreate(url, UriKind.Absolute, out Uri uriResult))
            {
                return (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
            }

            return false;
        }

        private void pictureBox_SearchGoogle_Click(object sender, EventArgs e)
        {
            if (comboBoxEx_Google.Text == "Cụm từ")
            {
                if (!IsValidUrl(txtSearch_Google.Text))
                {
                    string url = "https://www.google.com/search?q=" + Uri.EscapeDataString(txtSearch_Google.Text);
                    LoadWeb_Google(url);
                }
                else
                {
                    MessageBox.Show("Đây không phải cụm từ", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (comboBoxEx_Google.Text == "Đường dẫn")
            {
                if (IsValidUrl(txtSearch_Google.Text) && !IsFacebookUrl(txtSearch_Google.Text))
                {
                    LoadWeb_Google(txtSearch_Google.Text);
                }
                else
                {
                    MessageBox.Show("Không phải đường dẫn Google", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void pictureBox_SearchGoogle_DoubleClick(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Mở trang Google với kích thước lớn?",
                                             "Xác nhận",
                                             MessageBoxButtons.YesNo,
                                             MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                frm_OpenWeb frm_OpenWeb = new frm_OpenWeb(webView_Google.Source.ToString());
                frm_OpenWeb.ShowDialog();
            }
        }

        private void txtSearch_Google_TextChanged(object sender, EventArgs e)
        {
            if (txtSearch_Google.Text == string.Empty)
            {
                ClearWeb_Google();
            }
        }

        private void txtSearch_Google_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                pictureBox_SearchGoogle_Click(sender, e);
                e.SuppressKeyPress = true;
            }
        }

        private void comboBoxEx_Google_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtSearch_Google.Text = string.Empty;
            ClearWeb_Google();
        }


        //========= FACEBOOK
        public bool IsFacebookUrl(string url)
        {
            string pattern = @"^https:\/\/.*facebook\.com\/.*$";
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
            return regex.IsMatch(url);
        }

        private void pictureBox_SearchFacebook_Click(object sender, EventArgs e)
        {
            if (comboBoxEx_Facebook.Text == "Email hoặc SĐT")
            {
                string url = "https://web.facebook.com/login/identify/?ctx=recover&ars=facebook_login&from_login_screen=0";
                LoadWeb_Facebook(url);
            }
            else if (comboBoxEx_Facebook.Text == "Đường dẫn")
            {
                if (IsFacebookUrl(txtSearch_Facebook.Text))
                {
                    LoadWeb_Facebook(txtSearch_Facebook.Text);
                }
                else
                {
                    MessageBox.Show("Không phải đường dẫn Facebook", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void pictureBox_SearchFacebook_DoubleClick(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Mở trang Facebook với kích thước lớn?",
                                             "Xác nhận",
                                             MessageBoxButtons.YesNo,
                                             MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                frm_OpenWeb frm_OpenWeb = new frm_OpenWeb(webView_Facebook.Source.ToString());
                frm_OpenWeb.ShowDialog();
            }
        }

        private void txtSearch_Facebook_TextChanged(object sender, EventArgs e)
        {
            if (txtSearch_Facebook.Text == string.Empty)
            {
                ClearWeb_Facebook();
            }
        }

        private void txtSearch_Facebook_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                pictureBox_SearchFacebook_Click(sender, e);
                e.SuppressKeyPress = true;
            }
        }

        private void comboBoxEx_Facebook_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtSearch_Facebook.Text = string.Empty;
            if (comboBoxEx_Facebook.Text == "Email hoặc SĐT")
            {
                string url = "https://web.facebook.com/login/identify/?ctx=recover&ars=facebook_login&from_login_screen=0";
                LoadWeb_Facebook(url);
                txtSearch_Facebook.Enabled = false;
            }
            else if (comboBoxEx_Facebook.Text == "Đường dẫn")
            {
                ClearWeb_Facebook();
                txtSearch_Facebook.Enabled = true;
            }
        }


        //========= ZALO
        public bool IsPhoneNumber(string input)
        {
            string pattern = @"^(\+?\d{1,4}[\s-]?)?(\(?\d{3}\)?[\s-]?)?[\d\s-]{7,10}$";
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
            return regex.IsMatch(input);
        }

        private void pictureBox_SearchZalo_Click(object sender, EventArgs e)
        {
            if (IsPhoneNumber(txtSearch_Zalo.Text))
            {
                string url = "https://zalo.me/" + txtSearch_Zalo.Text;
                LoadWeb_Zalo(url);
            }
            else
            {
                MessageBox.Show("Không phải số điện thoại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void pictureBox_SearchZalo_DoubleClick(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Sao chép liên kết trang Zalo?",
                                             "Xác nhận",
                                             MessageBoxButtons.YesNo,
                                             MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Clipboard.SetText(webView_Zalo.Source.ToString());
            }
        }

        private void txtSearch_Zalo_TextChanged(object sender, EventArgs e)
        {
            if (txtSearch_Zalo.Text == string.Empty)
            {
                ClearWeb_Zalo();
            }
        }

        private void txtSearch_Zalo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                pictureBox_SearchZalo_Click(sender, e);
                e.SuppressKeyPress = true;
            }
        }

        private void txtSearch_Zalo_Click(object sender, EventArgs e)
        {
            if (txtSearch_Zalo.Text == "VD: 0358775132")
            {
                txtSearch_Zalo.Text = string.Empty;
            }
        }


        //========= TELEGRAM
        private void pictureBox_SearchTelegram_Click(object sender, EventArgs e)
        {
            if (txtSearch_Telegram.Text != string.Empty)
            {
                string url = "https://t.me/" + txtSearch_Telegram.Text;
                LoadWeb_Telegram(url);
            }
            else
            {
                MessageBox.Show("Không phải username", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void pictureBox_SearchTelegram_DoubleClick(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Sao chép liên kết trang Telegram?",
                                             "Xác nhận",
                                             MessageBoxButtons.YesNo,
                                             MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Clipboard.SetText(webView_Telegram.Source.ToString());
            }
        }

        private void txtSearch_Telegram_TextChanged(object sender, EventArgs e)
        {
            if (txtSearch_Telegram.Text == string.Empty)
            {
                ClearWeb_Telegram();
            }
        }

        private void txtSearch_Telegram_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                pictureBox_SearchTelegram_Click(sender, e);
                e.SuppressKeyPress = true;
            }
        }

        private void txtSearch_Telegram_Click(object sender, EventArgs e)
        {
            if (txtSearch_Telegram.Text == "VD: quocbaomta2504")
            {
                txtSearch_Telegram.Text = string.Empty;
            }
        }

        private void pictureBox_SearchGoogle_MouseEnter(object sender, EventArgs e)
        {
            pictureBox_SearchGoogle.BackColor = Color.Orange;
        }

        private void pictureBox_SearchGoogle_MouseLeave(object sender, EventArgs e)
        {
            pictureBox_SearchGoogle.BackColor = Color.Transparent;
        }

        private void pictureBox_SearchFacebook_MouseEnter(object sender, EventArgs e)
        {
            pictureBox_SearchFacebook.BackColor = Color.Orange;
        }

        private void pictureBox_SearchFacebook_MouseLeave(object sender, EventArgs e)
        {
            pictureBox_SearchFacebook.BackColor = Color.Transparent;
        }

        private void pictureBox_SearchZalo_MouseEnter(object sender, EventArgs e)
        {
            pictureBox_SearchZalo.BackColor = Color.Orange;
        }

        private void pictureBox_SearchZalo_MouseLeave(object sender, EventArgs e)
        {
            pictureBox_SearchZalo.BackColor = Color.Transparent;
        }

        private void pictureBox_SearchTelegram_MouseEnter(object sender, EventArgs e)
        {
            pictureBox_SearchTelegram.BackColor = Color.Orange;
        }

        private void pictureBox_SearchTelegram_MouseLeave(object sender, EventArgs e)
        {
            pictureBox_SearchTelegram.BackColor = Color.Transparent;
        }
    }
}
