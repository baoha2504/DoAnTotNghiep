using MiniSoftware;
using MTA_Mobile_Forensic.GUI.Share;
using MTA_Mobile_Forensic.Model;
using MTA_Mobile_Forensic.Support;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MTA_Mobile_Forensic.GUI.IOS
{
    public partial class usr_CuocGoi_IOS : UserControl
    {
        api api = new api();
        function function = new function();
        string pathFile = "";
        List<CallHistoryIOS> list_call = new List<CallHistoryIOS>();

        public usr_CuocGoi_IOS()
        {
            InitializeComponent();
            dataGridView.RowTemplate.Height = 35;
            LoadData();
        }

        public async void LoadData()
        {
            pathFile = function.FindFile(DeviceInfo.pathBackup, "CallHistory.storedata");
            if (!string.IsNullOrEmpty(pathFile))
            {
                var calls= await api.LayDanhSachCuocGoi_IOS(pathFile);
                if (calls != null)
                {
                    list_call = calls;
                    for (int i = 0; i < calls.Count; i++)
                    {
                        dataGridView.Rows.Add();
                        dataGridView.Rows[i].Cells["Column1"].Value = i + 1;
                        try
                        {
                            dataGridView.Rows[i].Cells["Column2"].Value = function.ConvertToCustomFormat(calls[i].date);
                        }
                        catch { }
                        dataGridView.Rows[i].Cells["Column3"].Value = calls[i].location;
                        if (!string.IsNullOrEmpty(calls[i].normal))
                        {
                            dataGridView.Rows[i].Cells["Column4"].Value = calls[i].normal;
                        }
                        else
                        {
                            dataGridView.Rows[i].Cells["Column4"].Value = "Không tìm thấy";
                        }
                        
                        dataGridView.Rows[i].Cells["Column5"].Value = "Sao chép SĐT ▼";
                    }
                }
            }
            else { }
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

        private void btnXuat_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                folderDialog.Description = "Chọn thư mục lưu báo cáo";
                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    string selectedPath = folderDialog.SelectedPath;
                    string fileName = $"Báo cáo về cuộc gọi của thiết bị_{DeviceInfo.nameDevice}_{DeviceInfo.serialDevice}.docx";
                    string PATH_EXPORT = Path.Combine(selectedPath, fileName);

                    string sourceDirectory = AppDomain.CurrentDomain.BaseDirectory;
                    string projectDirectory = Directory.GetParent(sourceDirectory).Parent.Parent.FullName;
                    string PATH_TEMPLATE = Path.Combine(projectDirectory, "Data", "Document", "report_call_log.docx");
                    if (File.Exists(PATH_TEMPLATE))
                    {
                        var calls_export = new List<object>();

                        for (int i = 0; i < list_call.Count; i++)
                        {
                            var call_export = new
                            {
                                Sodienthoai = list_call[i].normal,
                                Thoigian = list_call[i].date,
                                Vitri = list_call[i].location,
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

        private void panelEx1_Resize(object sender, EventArgs e)
        {
            if (dataGridView.Columns.Count > 0)
            {
                dataGridView.Columns[2].Width = 2 * (panelEx1.Width - 180) / 10;
                dataGridView.Columns[3].Width = (int)3.5 * (panelEx1.Width - 180) / 10;
                dataGridView.Columns[4].Width = (int)4.5 * (panelEx1.Width - 180) / 10;
            }
        }
    }
}
