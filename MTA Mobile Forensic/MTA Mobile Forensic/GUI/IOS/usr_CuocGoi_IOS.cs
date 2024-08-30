using MTA_Mobile_Forensic.Model;
using MTA_Mobile_Forensic.Support;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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

        public usr_CuocGoi_IOS()
        {
            InitializeComponent();
            dataGridView.RowTemplate.Height = 35;
            LoadData();
        }

        public async void LoadData()
        {
            pathFile = function.FindFile(DeviceInfo.pathBackup, "CallHistory.storedata");
            if (pathFile != string.Empty)
            {
                var calls= await api.LayDanhSachCuocGoi_IOS(pathFile);
                if (calls != null)
                {
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
