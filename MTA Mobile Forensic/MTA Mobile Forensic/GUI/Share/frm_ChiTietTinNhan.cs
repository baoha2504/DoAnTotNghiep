using System;
using System.Drawing;
using System.Windows.Forms;

namespace MTA_Mobile_Forensic.GUI.Share
{
    public partial class frm_ChiTietTinNhan : Form
    {
        public frm_ChiTietTinNhan(string loaitinnhan, string diachi, string thoigiangui, string thoigiannhan, string noidung, string timkiem)
        {
            InitializeComponent();
            if (thoigiangui == "07:00:00 01/01/1970") { thoigiangui = "Chưa xác định"; }
            if (thoigiannhan == "07:00:00 01/01/1970") { thoigiannhan = "Chưa xác định"; }
            if (loaitinnhan == "Tin nhắn gửi")
            {
                txtLoaiTinNhan.Text = loaitinnhan;
                txtDiaChi.Text = diachi;
                txtThoiGianGui.Text = thoigiannhan;
                txtThoiGianNhan.Text = thoigiangui;
                txtNoiDung.Text = noidung;
            }
            else
            {
                txtLoaiTinNhan.Text = loaitinnhan;
                txtDiaChi.Text = diachi;
                txtThoiGianGui.Text = thoigiangui;
                txtThoiGianNhan.Text = thoigiannhan;
                txtNoiDung.Text = noidung;
            }
            if (timkiem != string.Empty)
            {
                HighlightText(txtNoiDung, timkiem);
            }
        }

        private void HighlightText(RichTextBox richTextBox, string text)
        {
            // Đặt lại định dạng ban đầu
            richTextBox.SelectionStart = 0;
            richTextBox.SelectionLength = richTextBox.Text.Length;
            richTextBox.SelectionFont = new Font(richTextBox.Font, FontStyle.Regular);
            richTextBox.SelectionColor = Color.Black;

            int startIndex = 0;
            while ((startIndex = richTextBox.Text.IndexOf(text, startIndex, StringComparison.OrdinalIgnoreCase)) != -1)
            {
                // Chọn và định dạng từ tìm thấy
                richTextBox.Select(startIndex, text.Length);
                richTextBox.SelectionFont = new Font(richTextBox.Font, FontStyle.Bold);
                richTextBox.SelectionColor = Color.Red;

                // Cập nhật vị trí bắt đầu tìm kiếm tiếp theo
                startIndex += text.Length;
            }

            // Đảm bảo không có đoạn văn bản nào bị chọn sau khi xử lý
            richTextBox.SelectionStart = richTextBox.Text.Length;
            richTextBox.SelectionLength = 0;
        }

    }
}
