using MTA_Mobile_Forensic.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MTA_Mobile_Forensic.GUI.Share
{
    public partial class frmPathBackUp : Form
    {
        public frmPathBackUp()
        {
            InitializeComponent();
            txtPathBackup.Text = DeviceInfo.pathBackup;
            this.MaximizeBox = false;
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            DeviceInfo.pathBackup = txtPathBackup.Text;
            this.Close();
        }

        private void btnOpenPath_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
            {
                DialogResult result = folderBrowserDialog.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(folderBrowserDialog.SelectedPath))
                {
                    string selectedFolder = folderBrowserDialog.SelectedPath;
                    txtPathBackup.Text = selectedFolder;
                }
            }
        }
    }
}
