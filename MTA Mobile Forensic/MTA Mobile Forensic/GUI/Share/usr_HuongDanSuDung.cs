using System;
using System.IO;
using System.Windows.Forms;

namespace MTA_Mobile_Forensic.GUI.Share
{
    public partial class usr_HuongDanSuDung : UserControl
    {
        public usr_HuongDanSuDung()
        {
            InitializeComponent();
            try
            {
                string sourceDirectory = AppDomain.CurrentDomain.BaseDirectory;
                string projectDirectory = Directory.GetParent(sourceDirectory).Parent.Parent.FullName;
                string filePath = Path.Combine(projectDirectory, "Data", "Document", "HuongDanSuDung.pdf");
                pdfViewer.LoadDocument(filePath);
            }
            catch { }
        }
    }
}
