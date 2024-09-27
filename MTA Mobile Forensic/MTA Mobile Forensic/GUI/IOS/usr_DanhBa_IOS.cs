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
    public partial class usr_DanhBa_IOS : UserControl
    {
        api api = new api();
        function function = new function();
        string pathFile = "";
        List<ContactIOS> list_contact = new List<ContactIOS>();

        public usr_DanhBa_IOS()
        {
            InitializeComponent();
            dataGridView.RowTemplate.Height = 35;
            LoadData();
        }

        public async void LoadData()
        {
            pathFile = function.FindFile(DeviceInfo.pathBackup, "AddressBook.sqlitedb");
            if (!string.IsNullOrEmpty(pathFile))
            {
                var contacts = await api.LayDanhSachDanhBa_IOS(pathFile);
                if (contacts != null)
                {
                    list_contact = contacts;
                    for (int i = 0; i < contacts.Count; i++)
                    {
                        dataGridView.Rows.Add();
                        dataGridView.Rows[i].Cells["Column1"].Value = i + 1;
                        dataGridView.Rows[i].Cells["Column2"].Value = contacts[i].name;
                        dataGridView.Rows[i].Cells["Column3"].Value = contacts[i].value;
                        dataGridView.Rows[i].Cells["Column4"].Value = function.ConvertToCustomFormat(contacts[i].creationdate);
                        dataGridView.Rows[i].Cells["Column5"].Value = function.ConvertToCustomFormat(contacts[i].modificationdate);
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
                    string fileName = $"Báo cáo về danh bạ của thiết bị_{DeviceInfo.nameDevice}_{DeviceInfo.serialDevice}.docx";
                    string PATH_EXPORT = Path.Combine(selectedPath, fileName);

                    string sourceDirectory = AppDomain.CurrentDomain.BaseDirectory;
                    string projectDirectory = Directory.GetParent(sourceDirectory).Parent.Parent.FullName;
                    string PATH_TEMPLATE = Path.Combine(projectDirectory, "Data", "Document", "report_contact.docx");
                    if (File.Exists(PATH_TEMPLATE))
                    {
                        var contacts_export = new List<object>();

                        for (int i = 0; i < list_contact.Count; i++)
                        {
                            var contact_export = new
                            {
                                DisplayName = list_contact[i].name,
                                Data = list_contact[i].value,
                                DataCreate = list_contact[i].creationdate,
                                AccountName = list_contact[i].modificationdate,
                            };
                            contacts_export.Add(contact_export);
                        }

                        var value = new
                        {
                            ct = contacts_export,
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

        }

        private void panelEx1_Resize(object sender, EventArgs e)
        {
            if (dataGridView.Columns.Count > 0)
            {
                dataGridView.Columns[2].Width = (panelEx1.Width - 150) / 4;
                dataGridView.Columns[3].Width = (panelEx1.Width - 150) / 4;
                dataGridView.Columns[4].Width = (panelEx1.Width - 150) / 4;
                dataGridView.Columns[5].Width = (panelEx1.Width - 150) / 4;
            }
        }
    }
}
