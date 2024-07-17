using System;
using System.Windows.Forms;

namespace MTA_Mobile_Forensic.GUI.Android
{
    public partial class usr_SaoLuuKhoiPhuc : UserControl
    {
        public usr_SaoLuuKhoiPhuc()
        {
            InitializeComponent();
        }

        private void panelEx1_Resize(object sender, EventArgs e)
        {
            try
            {
                splitContainer1.SplitterDistance = (int)(panelEx1.Height / 3);
                splitContainer2.SplitterDistance = (int)(panelEx1.Width / 2);
                splitContainer3.SplitterDistance = (int)(panelEx1.Width / 2);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thay đổi kích thước: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
