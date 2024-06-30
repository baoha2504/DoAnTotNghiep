using System.IO;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace MTA_Mobile_Forensic.GUI.Share
{
    public partial class frm_Notification : Form
    {
        public frm_Notification()
        {
            InitializeComponent();
        }

        private Timer timer;

        public frm_Notification(string loai, string thongbao)
        {
            InitializeComponent();
            XuLy(loai, thongbao);
            timer = new Timer();
            timer.Interval = 1500;
            if (loai == "success" || loai == "info")
            {
                timer.Tick += Timer_Tick;
            }
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            // Dừng Timer và đóng form khi hết thời gian
            timer.Stop();
            this.Close();

        }

        private void XuLy(string loai, string thongbao)
        {
            LoadImage(loai);
            pnThongBao.Text = thongbao;
        }

        private void LoadImage(string loai)
        {
            string sourceDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string projectDirectory = Directory.GetParent(sourceDirectory).Parent.Parent.FullName;
            string imagePath = Path.Combine(projectDirectory, "Data", "Image");
            
            string imageName = "info.png";

            if (loai == "success")
            {
                 imageName = "checked.png";
            }
            else if (loai == "warning")
            {
                imageName = "warning.png";
            }
            else if (loai == "error")
            {
                imageName = "error.png";
            }

            // Tạo đường dẫn đầy đủ tới file ảnh
            string fullImagePath = Path.Combine(imagePath, imageName);

            // Kiểm tra xem file ảnh có tồn tại không
            if (File.Exists(fullImagePath))
            {
                // Gán ảnh cho PictureBox
                pictureBox1.Image = Image.FromFile(fullImagePath);
            }
            else
            {
                // Hiển thị thông báo nếu file ảnh không tồn tại
                MessageBox.Show("File ảnh không tồn tại: " + fullImagePath);
            }
        }

        private void btnSaoChep_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(pnThongBao.Text))
            {
                Clipboard.SetText(pnThongBao.Text);
            }
            else
            {
                MessageBox.Show("Không có nội dung để sao chép!");
            }
        }
    }
}
