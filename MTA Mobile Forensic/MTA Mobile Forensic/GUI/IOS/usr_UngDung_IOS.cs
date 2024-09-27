using DevComponents.DotNetBar.Controls;
using MiniSoftware;
using MTA_Mobile_Forensic.GUI.Share;
using MTA_Mobile_Forensic.Model;
using MTA_Mobile_Forensic.Support;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace MTA_Mobile_Forensic.GUI.IOS
{
    public partial class usr_UngDung_IOS : UserControl
    {
        libimobiledevice libimobiledevice = new libimobiledevice();

        public usr_UngDung_IOS()
        {
            InitializeComponent();
            dataGridView.RowTemplate.Height = 60;
            dataGridView.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 9);
            LoadData();
        }

        public List<AppInfoIOS> ParseAppInfo(string input)
        {
            var appInfos = new List<AppInfoIOS>();
            var lines = input.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var line in lines)
            {
                var parts = line.Split(new[] { ", " }, StringSplitOptions.None);

                if (parts.Length == 3)
                {
                    var appInfo = new AppInfoIOS
                    {
                        CFBundleIdentifier = parts[0].Trim(),
                        CFBundleVersion = parts[1].Trim().Trim('"'),
                        CFBundleDisplayName = parts[2].Trim().Trim('"')
                    };

                    appInfos.Add(appInfo);
                }
            }

            return appInfos;
        }

        public void LoadData()
        {
            string text = libimobiledevice.ideviceinstallerCommand($"-u {DeviceInfo.serialDevice} -l");
            var apps = ParseAppInfo(text);

            if (apps != null)
            {
                for (int i = 0; i < apps.Count; i++)
                {
                    dataGridView.Rows.Add();
                    //dataGridView.Rows[i].Cells["Column1"].Value = ;
                    dataGridView.Rows[i].Cells["Column1"].Value = i + 1;
                    dataGridView.Rows[i].Cells["Column2"].Value = apps[i].CFBundleDisplayName;
                    dataGridView.Rows[i].Cells["Column3"].Value = apps[i].CFBundleVersion;
                    dataGridView.Rows[i].Cells["Column4"].Value = apps[i].CFBundleIdentifier;
                }
            }
        }

        private void panelEx1_Resize(object sender, EventArgs e)
        {
            if (dataGridView.Columns.Count > 0)
            {
                dataGridView.Columns[2].Width = (panelEx1.Width - 150) / 3;
                dataGridView.Columns[3].Width = (panelEx1.Width - 150) / 3;
                dataGridView.Columns[4].Width = (panelEx1.Width - 150) / 3;
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
                    string fileName = $"Báo cáo về ứng dụng của thiết bị_{DeviceInfo.nameDevice}_{DeviceInfo.serialDevice}.docx";
                    string PATH_EXPORT = Path.Combine(selectedPath, fileName);

                    string sourceDirectory = AppDomain.CurrentDomain.BaseDirectory;
                    string projectDirectory = Directory.GetParent(sourceDirectory).Parent.Parent.FullName;
                    string PATH_TEMPLATE = Path.Combine(projectDirectory, "Data", "Document", "report_application.docx");
                    if (File.Exists(PATH_TEMPLATE))
                    {
                        var aplications_export = new List<object>();


                        foreach (DataGridViewRow row in dataGridView.Rows)
                        {
                            if (!row.IsNewRow)
                            {
                                var aplication_export = new
                                {
                                    tenungdung = row.Cells[2].Value?.ToString(),
                                    goi = row.Cells[4].Value?.ToString(),
                                    phienban = row.Cells[3].Value?.ToString(),
                                    ngaycaidat = "Không xác định",
                                    ngaycapnhatcuoi = "Không xác định",
                                };
                                aplications_export.Add(aplication_export);
                            }
                        }

                        var value = new
                        {
                            ct = aplications_export,
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
    }
}
