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
    public partial class usr_Lich_IOS : UserControl
    {
        api api = new api();
        function function = new function();
        string pathFile = "";
        List<CalendarIOS> list_calendar = new List<CalendarIOS>();

        public usr_Lich_IOS()
        {
            InitializeComponent();
            dataGridView.RowTemplate.Height = 35;
            LoadData();
        }

        public async void LoadData()
        {
            pathFile = function.FindFile(DeviceInfo.pathBackup, "Calendar.sqlitedb");
            if (!string.IsNullOrEmpty(pathFile))
            {
                var calendars = await api.LayDanhSachLich_IOS(pathFile);
                if (calendars != null)
                {
                    list_calendar = calendars;
                    for (int i = 0; i < calendars.Count; i++)
                    {
                        dataGridView.Rows.Add();
                        dataGridView.Rows[i].Cells["Column2"].Value = i + 1;
                        dataGridView.Rows[i].Cells["Column3"].Value = calendars[i].summary;
                        dataGridView.Rows[i].Cells["Column4"].Value = function.ConvertToCustomFormat(calendars[i].startdateconverted);
                        dataGridView.Rows[i].Cells["Column5"].Value = function.ConvertToCustomFormat(calendars[i].enddateconverted);
                        dataGridView.Rows[i].Cells["Column6"].Value = calendars[i].address;
                        dataGridView.Rows[i].Cells["Column7"].Value = calendars[i].displayname;
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
                    string fileName = $"Báo cáo về lịch của thiết bị_{DeviceInfo.nameDevice}_{DeviceInfo.serialDevice}.docx";
                    string PATH_EXPORT = Path.Combine(selectedPath, fileName);

                    string sourceDirectory = AppDomain.CurrentDomain.BaseDirectory;
                    string projectDirectory = Directory.GetParent(sourceDirectory).Parent.Parent.FullName;
                    string PATH_TEMPLATE = Path.Combine(projectDirectory, "Data", "Document", "report_calendar.docx");
                    if (File.Exists(PATH_TEMPLATE))
                    {
                        var calendars_export = new List<object>();

                        for (int i = 0; i < list_calendar.Count; i++)
                        {
                            var calendar_export = new
                            {
                                CalendarDisplayName = list_calendar[i].summary,
                                Dtstart = list_calendar[i].startdateconverted,
                                Dtend = list_calendar[i].enddateconverted,
                                AccountCreated = list_calendar[i].address + "/" + list_calendar[i].displayname,
                            };
                            calendars_export.Add(calendar_export);
                        }

                        var value = new
                        {
                            ct = calendars_export,
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
                dataGridView.Columns[2].Width = 2 * (panelEx1.Width - 150) / 6;
                dataGridView.Columns[3].Width = (panelEx1.Width - 150) / 6;
                dataGridView.Columns[4].Width = (panelEx1.Width - 150) / 6;
                dataGridView.Columns[5].Width = (panelEx1.Width - 150) / 6;
                dataGridView.Columns[6].Width = (panelEx1.Width - 150) / 6;
            }
        }
    }
}
