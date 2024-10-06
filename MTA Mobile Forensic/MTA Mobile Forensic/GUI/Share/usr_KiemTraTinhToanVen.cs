using MTA_Mobile_Forensic.Support;
using System;
using System.Windows.Forms;

namespace MTA_Mobile_Forensic.GUI.Share
{
    public partial class usr_KiemTraTinhToanVen : UserControl
    {
        hash hash = new hash();
        public usr_KiemTraTinhToanVen()
        {
            InitializeComponent();
        }

        private void panelEx1_Resize(object sender, EventArgs e)
        {
            try
            {
                splitContainer1.SplitterDistance = (int)(panelEx1.Width / 2);
            }
            catch { }
        }

        private void btnChonFileMau_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "APK files (*.ab)|*.ab|All files (*.*)|*.*";
                openFileDialog.Title = "Chọn file backup";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    txtPathMau.Text = openFileDialog.FileName;
                    CheckFileMau(openFileDialog.FileName);
                }
            }
        }

        private void btnChonFileKiemTra_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "APK files (*.ab)|*.ab|All files (*.*)|*.*";
                openFileDialog.Title = "Chọn file backup";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    if (openFileDialog.FileName != txtPathMau.Text)
                    {
                        txtPathKiemTra.Text = openFileDialog.FileName;
                        CheckFileKiemTra(openFileDialog.FileName);
                    }
                    else
                    {
                        MessageBox.Show("2 file vừa chọn trùng nhau!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void CheckFileMau(string path)
        {
            try
            {
                txtMaHoaMD5_Mau.Text = hash.ComputeFileMD5(path);
                txtMaHoaSHA128_Mau.Text = hash.ComputeFileSHA128(path);
                txtMaHoaSHA256_Mau.Text = hash.ComputeFileSHA256(path);
                txtMaHoaSHA512_Mau.Text = hash.ComputeFileSHA512(path);
            }
            catch { }
        }

        private void CheckFileKiemTra(string path)
        {
            try
            {
                txtMaHoaMD5_KiemTra.Text = hash.ComputeFileMD5(path);
                txtMaHoaSHA128_KiemTra.Text = hash.ComputeFileSHA128(path);
                txtMaHoaSHA256_KiemTra.Text = hash.ComputeFileSHA256(path);
                txtMaHoaSHA512_KiemTra.Text = hash.ComputeFileSHA512(path);
            }
            catch { }
        }

        private void btnKiemTra_Click(object sender, EventArgs e)
        {
            if (txtMaHoaMD5_Mau.Text == txtMaHoaMD5_KiemTra.Text &&
                txtMaHoaSHA128_Mau.Text == txtMaHoaSHA128_KiemTra.Text &&
                txtMaHoaSHA256_Mau.Text == txtMaHoaSHA256_KiemTra.Text &&
                txtMaHoaSHA512_Mau.Text == txtMaHoaSHA512_KiemTra.Text &&
                txtPathMau.Text != string.Empty &&
                txtPathKiemTra.Text != string.Empty)
            {
                MessageBox.Show("File sao lưu vẫn toàn vẹn!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("File sao lưu đã bị thay đổi!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
