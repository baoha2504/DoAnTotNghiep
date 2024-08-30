using MTA_Mobile_Forensic.Model;
using MTA_Mobile_Forensic.Support;
using System;
using System.Windows.Forms;

namespace MTA_Mobile_Forensic.GUI.IOS
{
    public partial class usr_DanhBa_IOS : UserControl
    {
        api api = new api();
        function function = new function();
        string pathFile = "";

        public usr_DanhBa_IOS()
        {
            InitializeComponent();
            dataGridView.RowTemplate.Height = 35;
            LoadData();
        }

        public async void LoadData()
        {
            pathFile = function.FindFile(DeviceInfo.pathBackup, "AddressBook.sqlitedb");
            if (pathFile != string.Empty)
            {
                var contacts = await api.LayDanhSachDanhBa_IOS(pathFile);
                if (contacts != null)
                {
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
