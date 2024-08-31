using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
namespace MTA_Mobile_Forensic.GUI.Share
{
    public partial class usr_TinNhanMini : UserControl
    {
        public usr_TinNhanMini()
        {
            InitializeComponent();
        }

        public string diachi = "";
        public string tinnhan = "";
        public string thoigian = "";
        public string dateSent = "";
        public string read = "";
        public string status = "";
        public string serviceCenter = "";
        public string simId = "";
        public int sentMessage = -1;
        public event EventHandler ControlClicked;

        public usr_TinNhanMini(string diachi, string tinnhan, string thoigian, string dateSent, string read, string status, string serviceCenter, string simId, int sentMessage)
        {
            InitializeComponent();

            this.diachi = diachi;
            this.tinnhan = tinnhan;
            this.thoigian = thoigian;
            this.dateSent = dateSent;
            this.read = read;
            this.status = status;
            this.serviceCenter = serviceCenter;
            this.simId = simId;
            this.sentMessage = sentMessage;

            txtAddress.Text = diachi;
            txtTinNhan.Text = tinnhan;
            txtThoiGian.Text = thoigian;

            string sourceDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string projectDirectory = Directory.GetParent(sourceDirectory).Parent.Parent.FullName;
            string imagePath = Path.Combine(projectDirectory, "Data", "Image");
            string fullImagePath = Path.Combine(imagePath, "message.png");


            if (sentMessage == 0)
            {
                // tin nhắn nhận được
                panel2.BackgroundImage = Image.FromFile(fullImagePath);
            }
            else if (sentMessage == 1)
            {
                // tin nhắn gửi đi
                // giữ hình đã setup sẵn
            }
        }

        private void panel1_MouseEnter(object sender, EventArgs e)
        {
            // Cast sender back to Panel to access its properties
            if (sender is Panel panel)
            {
                // Change the Panel's BackColor when mouse enters
                panel.BackColor = Color.LightBlue;
            }
        }

        private void panel1_MouseLeave(object sender, EventArgs e)
        {
            // Cast sender back to Panel to access its properties
            if (sender is Panel panel)
            {
                // Change the Panel's BackColor back to its original color when mouse leaves
                panel.BackColor = Color.White;
            }
        }

        private void panel1_Click(object sender, EventArgs e)
        {
            ControlClicked?.Invoke(this, EventArgs.Empty);
        }

        private void panel1_DoubleClick(object sender, EventArgs e)
        {
            checkBox.Checked = !checkBox.Checked;
        }
    }
}
