using MTA_Mobile_Forensic.Model;
using MTA_Mobile_Forensic.Support;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace MTA_Mobile_Forensic.GUI.Android
{
    public partial class usr_DanhSachTrinhSat : UserControl
    {
        api api = new api();
        List<Account_TrinhSat> list_Account = new List<Account_TrinhSat>();
        List<LoginHistory_TrinhSat> list_LoginHistory = new List<LoginHistory_TrinhSat>();

        public usr_DanhSachTrinhSat()
        {
            InitializeComponent();
            dataGridView_DSTaiKhoan.RowTemplate.Height = 35;
            dataGridView_DSLichSuDangNhap.RowTemplate.Height = 35;
            LoadListAccount();
        }

        private void panelEx1_Resize(object sender, EventArgs e)
        {
            try
            {
                splitContainer1.SplitterDistance = (int)(4.5 * panelEx1.Width / 10);
            }
            catch { }
        }

        private async void LoadListAccount()
        {
            var accounts = await api.LayDanhSachTaiKhoanTrinhSat();
            dataGridView_DSTaiKhoan.Rows.Clear();
            if (accounts != null)
            {
                list_Account = accounts;
                for (int i = 0; i < list_Account.Count; i++)
                {
                    dataGridView_DSTaiKhoan.Rows.Add();
                    dataGridView_DSTaiKhoan.Rows[i].Cells["Column1"].Value = "Chọn";
                    dataGridView_DSTaiKhoan.Rows[i].Cells["Column2"].Value = list_Account[i].account_id;
                    dataGridView_DSTaiKhoan.Rows[i].Cells["Column3"].Value = list_Account[i].displayname;
                    dataGridView_DSTaiKhoan.Rows[i].Cells["Column4"].Value = list_Account[i].username;
                    dataGridView_DSTaiKhoan.Rows[i].Cells["Column5"].Value = list_Account[i].password;
                }
            }
            else
            {
                accounts = null;
            }
        }

        private async void LoadHistoryLoginByAccountID(int id)
        {
            var histories = await api.LayDanhSachLichSuDangNhapTrinhSat(id);
            dataGridView_DSLichSuDangNhap.Rows.Clear();
            pictureBoxLoad.Visible = false;
            if (histories != null)
            {
                list_LoginHistory = histories;
                for (int i = 0; i < list_LoginHistory.Count; i++)
                {
                    dataGridView_DSLichSuDangNhap.Rows.Add();
                    dataGridView_DSLichSuDangNhap.Rows[i].Cells["Column6"].Value = i + 1;
                    dataGridView_DSLichSuDangNhap.Rows[i].Cells["Column7"].Value = ((DateTime)list_LoginHistory[i].date_time).ToString("dd/MM/yyyy HH:mm:ss");
                    dataGridView_DSLichSuDangNhap.Rows[i].Cells["Column8"].Value = list_LoginHistory[i].device_name;
                    dataGridView_DSLichSuDangNhap.Rows[i].Cells["Column9"].Value = list_LoginHistory[i].device_serial;
                    dataGridView_DSLichSuDangNhap.Rows[i].Cells["Column10"].Value = list_LoginHistory[i].pincode;
                }
            }
            else
            {
                list_LoginHistory = null;
            }
        }

        private void dataGridView_DSTaiKhoan_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataGridView_DSTaiKhoan.Columns["Column1"].Index && e.RowIndex >= 0)
            {
                int rowID = e.RowIndex;
                var cellValue = dataGridView_DSTaiKhoan.Rows[rowID].Cells["Column1"].Value?.ToString();
                int account_id = (int)dataGridView_DSTaiKhoan.Rows[e.RowIndex].Cells["Column2"].Value;

                foreach (DataGridViewRow row in dataGridView_DSTaiKhoan.Rows)
                {
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        cell.Style.BackColor = Color.White;
                    }
                }

                foreach (DataGridViewCell cell in dataGridView_DSTaiKhoan.Rows[rowID].Cells)
                {
                    cell.Style.BackColor = Color.LightGreen;
                }

                pictureBoxLoad.Visible = true;
                LoadHistoryLoginByAccountID(account_id);
            }
        }


        private void btnTimKiemDoiTuong_Click(object sender, EventArgs e)
        {
            string searchText = txtTimKiemDoiTuong.Text.Trim().ToLower();

            for (int i = 0; i < dataGridView_DSTaiKhoan.Rows.Count - 1; i++)
            {
                string tennguoidung = ((string)dataGridView_DSTaiKhoan.Rows[i].Cells["Column3"].Value).ToLower();
                string tendangnhap = ((string)dataGridView_DSTaiKhoan.Rows[i].Cells["Column4"].Value).ToLower();

                if (tennguoidung.Contains(searchText) || tendangnhap.Contains(searchText))
                {
                    dataGridView_DSTaiKhoan.Rows[i].Visible = true;
                }
                else
                {
                    dataGridView_DSTaiKhoan.Rows[i].Visible = false;
                }
            }
        }

        private void txtTimKiemDoiTuong_TextChanged(object sender, EventArgs e)
        {
            if (txtTimKiemDoiTuong.Text == string.Empty)
            {
                for (int i = 0; i < dataGridView_DSTaiKhoan.Rows.Count; i++)
                {
                    dataGridView_DSTaiKhoan.Rows[i].Visible = true;
                }
            }
        }

        private void btnTimKiemLichSu_Click(object sender, EventArgs e)
        {
            string searchText = txtTimKiemLichSu.Text.Trim().ToLower();

            for (int i = 0; i < dataGridView_DSLichSuDangNhap.Rows.Count - 1; i++)
            {
                string thoigiandangnhap = ((string)dataGridView_DSLichSuDangNhap.Rows[i].Cells["Column7"].Value).ToLower();
                string tenthietbi = ((string)dataGridView_DSLichSuDangNhap.Rows[i].Cells["Column8"].Value).ToLower();

                if (thoigiandangnhap.Contains(searchText) || tenthietbi.Contains(searchText))
                {
                    dataGridView_DSLichSuDangNhap.Rows[i].Visible = true;
                }
                else
                {
                    dataGridView_DSLichSuDangNhap.Rows[i].Visible = false;
                }
            }
        }

        private void txtTimKiemLichSu_TextChanged(object sender, EventArgs e)
        {
            if (txtTimKiemLichSu.Text == string.Empty)
            {
                for (int i = 0; i < dataGridView_DSLichSuDangNhap.Rows.Count; i++)
                {
                    dataGridView_DSLichSuDangNhap.Rows[i].Visible = true;
                }
            }
        }

        private void btnXuatMatKhau_Click(object sender, EventArgs e)
        {
            if (list_LoginHistory.Count > 0)
            {
                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.Filter = "Text file (*.txt)|*.txt";
                    saveFileDialog.Title = "Chọn nơi lưu file mật khẩu";
                    saveFileDialog.FileName = "pin_passwords.txt";

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        string filePath = saveFileDialog.FileName;

                        try
                        {
                            HashSet<string> uniquePasswords = new HashSet<string>();
                            foreach (var histories in list_LoginHistory)
                            {
                                uniquePasswords.Add(histories.pincode);
                            }
                            using (StreamWriter writer = new StreamWriter(filePath))
                            {
                                foreach (var password in uniquePasswords)
                                {
                                    writer.WriteLine(password);
                                }
                            }
                            MessageBox.Show("Xuất mật khẩu PIN thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Lỗi khi xuất mật khẩu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            LoadListAccount();
            dataGridView_DSLichSuDangNhap.Rows.Clear();
            txtTimKiemDoiTuong.Text = string.Empty;
            txtTimKiemLichSu.Text = string.Empty;
        }
    }
}
