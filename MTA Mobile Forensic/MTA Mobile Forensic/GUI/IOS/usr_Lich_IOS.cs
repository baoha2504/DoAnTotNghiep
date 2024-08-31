﻿using MTA_Mobile_Forensic.Model;
using MTA_Mobile_Forensic.Support;
using System;
using System.Windows.Forms;

namespace MTA_Mobile_Forensic.GUI.IOS
{
    public partial class usr_Lich_IOS : UserControl
    {
        api api = new api();
        function function = new function();
        string pathFile = "";

        public usr_Lich_IOS()
        {
            InitializeComponent();
            dataGridView.RowTemplate.Height = 35;
            LoadData();
        }

        public async void LoadData()
        {
            pathFile = function.FindFile(DeviceInfo.pathBackup, "Calendar.sqlitedb");
            if (pathFile != string.Empty)
            {
                var calendars = await api.LayDanhSachLich_IOS(pathFile);
                if (calendars != null)
                {
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