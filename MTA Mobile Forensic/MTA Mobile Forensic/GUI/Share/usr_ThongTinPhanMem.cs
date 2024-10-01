using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MTA_Mobile_Forensic.GUI.Share
{
    public partial class usr_ThongTinPhanMem : UserControl
    {
        public usr_ThongTinPhanMem()
        {
            InitializeComponent();
            try
            {
                string sourceDirectory = AppDomain.CurrentDomain.BaseDirectory;
                string projectDirectory = Directory.GetParent(sourceDirectory).Parent.Parent.FullName;
                string filePath = Path.Combine(projectDirectory, "Data", "Document", "ThongTinPhanMem.pdf");
                pdfViewer.LoadDocument(filePath);
            }
            catch { }
        }
    }
}
