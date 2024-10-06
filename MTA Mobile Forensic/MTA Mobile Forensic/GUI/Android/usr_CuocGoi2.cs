using MiniSoftware;
using MTA_Mobile_Forensic.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace MTA_Mobile_Forensic.GUI.Android
{
    public partial class usr_CuocGoi2 : UserControl
    {
        List<CallRecord> callRecords = new List<CallRecord>();

        public usr_CuocGoi2()
        {
            InitializeComponent();
            dataGridView.RowTemplate.Height = 30;
            dataGridView.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 9);
        }

        private void panelEx1_Resize(object sender, EventArgs e)
        {
            dataGridView.Columns["Column1"].Width = (int)(dataGridView.Width * 1 / 15);
            dataGridView.Columns["Column2"].Width = (int)(dataGridView.Width * 1 / 15);
            dataGridView.Columns["Column3"].Width = (int)(dataGridView.Width * 3 / 15);
            dataGridView.Columns["Column4"].Width = (int)(dataGridView.Width * 3 / 15);
            dataGridView.Columns["Column5"].Width = (int)(dataGridView.Width * 3 / 15);
            dataGridView.Columns["Column6"].Width = (int)(dataGridView.Width * 3 / 15);
        }

        private void LoadJsonToDataGridView(string pathFileJson)
        {
            try
            {
                string jsonData = File.ReadAllText(pathFileJson);
                List<CallRecord> calls = JsonConvert.DeserializeObject<List<CallRecord>>(jsonData);

                if (calls != null)
                {
                    callRecords = calls;
                    Load_DataGridView();
                }
                else
                {
                    callRecords.Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }


        private void Load_DataGridView()
        {
            for (int i = callRecords.Count - 1; i >= 0; i--)
            {
                int rowIndex = dataGridView.Rows.Add();
                dataGridView.Rows[rowIndex].Cells["Column2"].Value = callRecords.Count - i; // Số thứ tự
                dataGridView.Rows[rowIndex].Cells["Column3"].Value = callRecords[i].Number;
                dataGridView.Rows[rowIndex].Cells["Column4"].Value = CheckType(callRecords[i].Type);
                dataGridView.Rows[rowIndex].Cells["Column5"].Value = ConvertTimestampToDateTime(callRecords[i].Date);
                dataGridView.Rows[rowIndex].Cells["Column6"].Value = ConvertSecondsToTimeFormat(callRecords[i].Duration);
            }
        }

        public string ConvertSecondsToTimeFormat(string secondsInput)
        {
            try
            {
                int totalSeconds = int.Parse(secondsInput);

                int hours = totalSeconds / 3600;
                int minutes = (totalSeconds % 3600) / 60;
                int seconds = totalSeconds % 60;

                return $"{hours:D2}:{minutes:D2}:{seconds:D2}";
            }
            catch
            {
                return "";
            }
        }


        public string ConvertTimestampToDateTime(string timestamp)
        {
            try
            {
                long milliseconds = long.Parse(timestamp);
                DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeMilliseconds(milliseconds);
                return dateTimeOffset.DateTime.ToString("dd/MM/yyyy HH:mm:ss");
            }
            catch
            {
                return "";
            }
        }


        private string CheckType(string input)
        {
            if (input == "1")
            {
                return "Cuộc gọi đi";
            }
            else if (input == "2")
            {
                return "Cuộc gọi đến";
            }
            else if (input == "3")
            {
                return "Cuộc gọi nhỡ";
            }
            else if (input == "4")
            {
                return "Cuộc gọi từ chối";
            }
            else if (input == "5")
            {
                return "Hộp thư thoại";
            }
            else
            {
                return "";
            }
        }

        private void btnChonFile_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Json files (*.json)|*.json|All files (*.*)|*.*";
                openFileDialog.Title = "Chọn file JSON";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    txtPathFileJson.Text = openFileDialog.FileName;
                    LoadJsonToDataGridView(openFileDialog.FileName);
                }
            }
        }

        private void btnXuat_Click(object sender, EventArgs e)
        {
            if (callRecords != null && callRecords.Count >= 0)
            {
                using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
                {
                    folderDialog.Description = "Chọn thư mục lưu báo cáo";
                    if (folderDialog.ShowDialog() == DialogResult.OK)
                    {
                        string selectedPath = folderDialog.SelectedPath;
                        string fileNameJson = Path.GetFileNameWithoutExtension(txtPathFileJson.Text).Replace("call_logs_", "");
                        string fileName = $"Báo cáo về cuộc gọi của thiết bị_{fileNameJson}.docx";
                        string PATH_EXPORT = Path.Combine(selectedPath, fileName);

                        string sourceDirectory = AppDomain.CurrentDomain.BaseDirectory;
                        string projectDirectory = Directory.GetParent(sourceDirectory).Parent.Parent.FullName;
                        string PATH_TEMPLATE = Path.Combine(projectDirectory, "Data", "Document", "report_call_log_android.docx");
                        if (File.Exists(PATH_TEMPLATE))
                        {
                            var calls_export = new List<object>();

                            for (int i = 0; i < callRecords.Count; i++)
                            {
                                var call_export = new
                                {
                                    date = ConvertTimestampToDateTime(callRecords[i].Date),
                                    type = CheckType(callRecords[i].Type),
                                    number = callRecords[i].Number,
                                    duration = ConvertSecondsToTimeFormat(callRecords[i].Duration),
                                };
                                calls_export.Add(call_export);
                            }

                            var value = new
                            {
                                ct = calls_export,
                                phut = DateTime.Now.ToString("mm"),
                                gio = DateTime.Now.ToString("HH"),
                                ngay = DateTime.Now.ToString("dd"),
                                thang = DateTime.Now.ToString("MM"),
                                nam = DateTime.Now.ToString("yyyy"),
                                device_name = DeviceInfo.nameDevice,
                                device_serial = DeviceInfo.serialDevice,
                                path_backup = DeviceInfo.pathBackup,
                                tenthietbi = fileNameJson,
                            };

                            MiniWord.SaveAsByTemplate(PATH_EXPORT, PATH_TEMPLATE, value);

                            Process.Start(PATH_EXPORT);
                            MessageBox.Show("Xuất file thành công", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("File không tồn tại: " + PATH_EXPORT, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Không có lịch sử cuộc gọi!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string searchText = txtNoiDungTimKiem.Text.Trim().ToLower();

            for (int i = 0; i < dataGridView.Rows.Count; i++)
            {
                string sodienthoai = ((string)dataGridView.Rows[i].Cells["Column3"].Value).ToLower();
                string loaicuocgoi = ((string)dataGridView.Rows[i].Cells["Column4"].Value).ToLower();

                if (sodienthoai.Contains(searchText) || loaicuocgoi.Contains(searchText))
                {
                    dataGridView.Rows[i].Visible = true;
                }
                else
                {
                    dataGridView.Rows[i].Visible = false;
                }
            }
        }

        private void txtNoiDungTimKiem_TextChanged(object sender, EventArgs e)
        {
            if (txtNoiDungTimKiem.Text == string.Empty)
            {
                for (int i = 0; i < dataGridView.Rows.Count; i++)
                {
                    dataGridView.Rows[i].Visible = true;
                }
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            dataGridView.Rows.Clear();
            callRecords.Clear();
            txtPathFileJson.Text = string.Empty;
            txtNoiDungTimKiem.Text = string.Empty;
        }
    }
}
