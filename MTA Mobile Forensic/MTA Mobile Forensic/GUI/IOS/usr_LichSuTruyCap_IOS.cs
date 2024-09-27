using MiniSoftware;
using MTA_Mobile_Forensic.GUI.Share;
using MTA_Mobile_Forensic.Model;
using MTA_Mobile_Forensic.Support;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace MTA_Mobile_Forensic.GUI.IOS
{
    public partial class usr_LichSuTruyCap_IOS : UserControl
    {
        api api = new api();
        function function = new function();
        string pathFile = "";
        List<UrlIOS> list_urls = new List<UrlIOS>();

        public usr_LichSuTruyCap_IOS()
        {
            InitializeComponent();
            dataGridView.RowTemplate.Height = 35;
            LoadData();
        }

        public async void LoadData()
        {
            pathFile = function.FindFile(DeviceInfo.pathBackup, "History.db");
            if (!string.IsNullOrEmpty(pathFile))
            {
                var UrlIOSs = await api.LayDanhSachURL_IOS(pathFile);
                if (UrlIOSs != null)
                {
                    list_urls = UrlIOSs;    
                    for (int i = 0; i < UrlIOSs.Count; i++)
                    {
                        dataGridView.Rows.Add();
                        dataGridView.Rows[i].Cells["Column1"].Value = i + 1;
                        try
                        {
                            dataGridView.Rows[i].Cells["Column2"].Value = function.ConvertToCustomFormat(UrlIOSs[i].visittime);
                        }
                        catch { }
                        dataGridView.Rows[i].Cells["Column3"].Value = UrlIOSs[i].title;
                        dataGridView.Rows[i].Cells["Column4"].Value = UrlIOSs[i].url;
                        dataGridView.Rows[i].Cells["Column5"].Value = "Sao chép liên kết ▼";
                    }
                }
            }
        }

        private void btnXuat_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                folderDialog.Description = "Chọn thư mục lưu báo cáo";
                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    string selectedPath = folderDialog.SelectedPath;
                    string fileName = $"Báo cáo về lịch sử truy cập của thiết bị_{DeviceInfo.nameDevice}_{DeviceInfo.serialDevice}.docx";
                    string PATH_EXPORT = Path.Combine(selectedPath, fileName);

                    string sourceDirectory = AppDomain.CurrentDomain.BaseDirectory;
                    string projectDirectory = Directory.GetParent(sourceDirectory).Parent.Parent.FullName;
                    string PATH_TEMPLATE = Path.Combine(projectDirectory, "Data", "Document", "report_history_browser.docx");
                    if (File.Exists(PATH_TEMPLATE))
                    {
                        var histories_browser_export = new List<object>();

                        for (int i = 0; i< list_urls.Count; i++)
                        {
                            var history_browser_export = new
                            {
                                thoigian = list_urls[i].visittime,
                                tieude = list_urls[i].title,
                                duongdan = list_urls[i].url,
                            };
                            histories_browser_export.Add(history_browser_export);
                        }

                        var value = new
                        {
                            ct = histories_browser_export,
                            phut = DateTime.Now.ToString("mm"),
                            gio = DateTime.Now.ToString("HH"),
                            ngay = DateTime.Now.ToString("dd"),
                            thang = DateTime.Now.ToString("MM"),
                            nam = DateTime.Now.ToString("yyyy"),
                            device_name = DeviceInfo.nameDevice,
                            device_serial = DeviceInfo.serialDevice,
                            path_backup = DeviceInfo.pathBackup,
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

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            dataGridView.Rows.Clear();
            LoadData();
        }

        private void dataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataGridView.Columns["Column5"].Index && e.RowIndex >= 0)
            {
                try
                {
                    string mvt = (string)dataGridView.Rows[e.RowIndex].Cells["Column4"].Value;
                    if (!string.IsNullOrEmpty(mvt))
                    {
                        Clipboard.SetText(mvt);
                    }
                }
                catch { }
            }
        }

        private void panelEx1_Resize(object sender, EventArgs e)
        {
            if (dataGridView.Columns.Count > 0)
            {
                dataGridView.Columns[2].Width = 2 * (panelEx1.Width - 150) / 10;
                dataGridView.Columns[3].Width = (int)3.5 * (panelEx1.Width - 150) / 10;
                dataGridView.Columns[4].Width = (int)4.5 * (panelEx1.Width - 150) / 10;
            }
        }
    }
}
