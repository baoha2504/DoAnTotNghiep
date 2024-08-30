using MTA_Mobile_Forensic.Model;
using MTA_Mobile_Forensic.Support;
using System;
using System.Windows.Forms;

namespace MTA_Mobile_Forensic.GUI.IOS
{
    public partial class usr_LichSuTruyCap_IOS : UserControl
    {
        api api = new api();
        function function = new function();
        string pathFile = "";

        public usr_LichSuTruyCap_IOS()
        {
            InitializeComponent();
            dataGridView.RowTemplate.Height = 35;
            LoadData();
        }

        public async void LoadData()
        {
            pathFile = function.FindFile(DeviceInfo.pathBackup, "History.db");
            if (pathFile != string.Empty)
            {
                var UrlIOSs = await api.LayDanhSachURL_IOS(pathFile);
                if (UrlIOSs != null)
                {
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
