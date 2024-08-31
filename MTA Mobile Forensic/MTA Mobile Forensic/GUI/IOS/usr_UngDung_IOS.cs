using MTA_Mobile_Forensic.Model;
using MTA_Mobile_Forensic.Support;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

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

        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            dataGridView.Rows.Clear();
            LoadData();
        }
    }
}
