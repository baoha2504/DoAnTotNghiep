using MTA_Mobile_Forensic.GUI.Share;
using MTA_Mobile_Forensic.Support;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace MTA_Mobile_Forensic.GUI.Android
{
    public partial class usr_FileHeThong : UserControl
    {
        adb adb = new adb();
        string query = "";
        ImageList imageList;

        public usr_FileHeThong()
        {
            InitializeComponent();
            treeView1.BeforeExpand += TreeView1_BeforeExpand;

            Load_TreeFull();
            LoadData();

            flpFSFile.Focus();
        }

        private void LoadData()
        {
            string sourceDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string projectDirectory = Directory.GetParent(sourceDirectory).Parent.Parent.FullName;
            string imagePath = Path.Combine(projectDirectory, "Data", "Image");
            string fullImagePath = "";

            imageList = new ImageList();
            fullImagePath = Path.Combine(imagePath, "folder-16x16.png");
            imageList.Images.Add("folder", Image.FromFile(fullImagePath));

            fullImagePath = Path.Combine(imagePath, "image-16x16.png");
            imageList.Images.Add("image", Image.FromFile(fullImagePath));

            fullImagePath = Path.Combine(imagePath, "mp4-16x16.png");
            imageList.Images.Add("video", Image.FromFile(fullImagePath));

            fullImagePath = Path.Combine(imagePath, "mp3-16x16.png");
            imageList.Images.Add("audio", Image.FromFile(fullImagePath));

            fullImagePath = Path.Combine(imagePath, "doc-16x16.png");
            imageList.Images.Add("word", Image.FromFile(fullImagePath));

            fullImagePath = Path.Combine(imagePath, "xls-16x16.png");
            imageList.Images.Add("excel", Image.FromFile(fullImagePath));

            fullImagePath = Path.Combine(imagePath, "ppt16x16.png");
            imageList.Images.Add("powerpoint", Image.FromFile(fullImagePath));

            fullImagePath = Path.Combine(imagePath, "pdf-16x16.png");
            imageList.Images.Add("pdf", Image.FromFile(fullImagePath));

            fullImagePath = Path.Combine(imagePath, "apk-16x16.png");
            imageList.Images.Add("apk", Image.FromFile(fullImagePath));

            //fullImagePath = Path.Combine(imagePath, "file-16x16.png");
            fullImagePath = Path.Combine(imagePath, "folder-16x16.png");
            imageList.Images.Add("file", Image.FromFile(fullImagePath));
            treeView1.ImageList = imageList;
        }

        private void Load_TreeFull()
        {
            treeView1.Nodes.Clear();
            query = "shell ls -a";
            string str = adb.adbCommand(query);
            if (str == "")
            {
                str = "ls: ./init.zygote64_32.rc: Permission denied\r\nls: ./init.mishow.ctl.rc: Permission denied\r\nls: ./init.rc: Permission denied\r\nls: ./init.usb.rc: Permission denied\r\nls: ./ueventd.rc: Permission denied\r\nls: ./init.zygote32.rc: Permission denied\r\nls: ./init.recovery.hardware.rc: Permission denied\r\nls: ./init: Permission denied\r\nls: ./init.miui.google_revenue_share_v2.rc: Permission denied\r\nls: ./init.batterysecret.rc: Permission denied\r\nls: ./init.miui.cust.rc: Permission denied\r\nls: ./init.environ.rc: Permission denied\r\nls: ./init.miui.post_boot.sh: Permission denied\r\nls: ./init.miui.qadaemon.rc: Permission denied\r\nls: ./init.batteryd.rc: Permission denied\r\n.\r\n..\r\nacct\r\napex\r\nbin\r\nbugreports\r\ncache\r\ncharger\r\nconfig\r\ncust\r\nd\r\ndata\r\ndebug_ramdisk\r\ndefault.prop\r\ndev\r\netc\r\nlost+found\r\nmnt\r\nodm\r\noem\r\nproc\r\nproduct\r\nproduct_services\r\nres\r\nsbin\r\nsdcard\r\nstorage\r\nsys\r\nsystem\r\nvendor\r\nls: ./verity_key: Permission denied\r\nls: ./init.recovery.qcom.rc: Permission denied\r\nls: ./init.mi_thermald.rc: Permission denied\r\nls: ./init.miui.rc: Permission denied\r\nls: ./init.usb.configfs.rc: Permission denied\r\nls: ./init.miui.google_revenue_share.rc: Permission denied\r\nls: ./init.miui.nativedebug.rc: Permission denied\r\nls: ./init.miui.early_boot.sh: Permission denied";
            }

            string[] lines = str.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string line in lines)
            {
                string trimmedLine = line.Trim();
                if (trimmedLine != "." && trimmedLine != "..")
                {
                    TreeNode node = new TreeNode(trimmedLine);
                    node.Nodes.Add("Loading..."); // Thêm node giả để có thể mở rộng
                    treeView1.Nodes.Add(node);

                    // Kiểm tra nếu là thư mục, thêm ảnh thích hợp
                    string fullPath = Path.Combine(GetFullPath(node), trimmedLine);
                    if (IsDirectory(fullPath))
                        node.ImageKey = "folder";
                    else
                        node.ImageKey = GetFileTypeImageKey(trimmedLine);
                }
            }
        }

        private void TreeView1_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            TreeNode node = e.Node;

            if (node.Nodes.Count == 1 && node.Nodes[0].Text == "Loading...")
            {
                node.Nodes.Clear(); // Xóa node giả
                string path = GetFullPath(node);

                // Lấy và thêm các node con
                string[] lines = Get_TreeFromPath(path);

                foreach (string line in lines)
                {
                    string trimmedLine = line.Trim();
                    if (trimmedLine != "." && trimmedLine != "..")
                    {
                        TreeNode childNode = new TreeNode(trimmedLine);
                        childNode.Nodes.Add("Loading..."); // Thêm node giả cho các node con
                        node.Nodes.Add(childNode);

                        // Kiểm tra nếu là thư mục, thêm ảnh thích hợp
                        string childFullPath = Path.Combine(path, trimmedLine);
                        if (IsDirectory(childFullPath))
                            childNode.ImageKey = "folder";
                        else
                            childNode.ImageKey = GetFileTypeImageKey(trimmedLine);
                    }
                }
            }
        }

        private string GetFullPath(TreeNode node)
        {
            string path = node.Text;
            TreeNode parentNode = node.Parent;

            while (parentNode != null)
            {
                path = parentNode.Text + "/" + path;
                parentNode = parentNode.Parent;
            }

            return path;
        }

        private string[] Get_TreeFromPath(string path)
        {
            query = $"shell ls -a {path}";
            string str = adb.adbCommand(query);

            string[] lines = str.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

            return lines;
        }

        private bool IsDirectory(string path)
        {
            string extension = Path.GetExtension(path).ToLower();

            string[] fileExtensions = {
                // Image formats
                ".png", ".jpg", ".jpeg", ".gif", ".bmp", ".webp", ".tiff", ".ico",
                // Video formats
                ".mp4", ".avi", ".mov", ".mkv", ".flv", ".wmv", ".3gp", ".webm", ".m4v",
                // Audio formats
                ".mp3", ".wav", ".ogg", ".flac", ".aac", ".m4a", ".wma", ".amr", ".opus",
                // Document formats
                ".doc", ".docx", ".xls", ".xlsx", ".ppt", ".pptx", ".pdf", ".txt", ".rtf", ".odt", ".ods", ".odp",
                // Archive formats
                ".zip", ".rar", ".7z", ".tar", ".gz", ".bz2", ".xz", ".iso",
                // Others
                ".apk", ".exe", ".bat", ".sh", ".html", ".css", ".js", ".json", ".xml", ".csv"
            };

            return !Array.Exists(fileExtensions, ext => ext == extension);
        }

        private string GetFileTypeImageKey(string fileName)
        {
            string extension = Path.GetExtension(fileName).ToLower();

            if (IsImageExtension(extension))
                return "image";
            else if (IsVideoExtension(extension))
                return "video";
            else if (IsAudioExtension(extension))
                return "audio";
            else if (IsWordExtension(extension))
                return "word";
            else if (IsExcelExtension(extension))
                return "excel";
            else if (IsPowerpointExtension(extension))
                return "powerpoint";
            else if (IsPdfExtension(extension))
                return "pdf";
            else if (IsApkExtension(extension))
                return "apk";
            else
                return "file"; // Mặc định cho các loại file khác
        }

        private bool IsImageExtension(string extension)
        {
            string[] imageExtensions = { ".png", ".jpg", ".jpeg", ".gif", ".bmp" };
            return Array.Exists(imageExtensions, ext => ext == extension);
        }

        private bool IsVideoExtension(string extension)
        {
            string[] videoExtensions = { ".mp4", ".avi", ".mov", ".mkv" };
            return Array.Exists(videoExtensions, ext => ext == extension);
        }

        private bool IsAudioExtension(string extension)
        {
            string[] audioExtensions = { ".mp3", ".wav", ".ogg", ".flac", ".aac", ".m4a", ".wma" };
            return Array.Exists(audioExtensions, ext => ext == extension);
        }

        private bool IsWordExtension(string extension)
        {
            string[] documentExtensions = { ".doc", ".docx" };
            return Array.Exists(documentExtensions, ext => ext == extension);
        }

        private bool IsExcelExtension(string extension)
        {
            string[] documentExtensions = { ".xls", ".xlsx" };
            return Array.Exists(documentExtensions, ext => ext == extension);
        }

        private bool IsPowerpointExtension(string extension)
        {
            string[] documentExtensions = { ".ppt", ".pptx" };
            return Array.Exists(documentExtensions, ext => ext == extension);
        }

        private bool IsPdfExtension(string extension)
        {
            string[] documentExtensions = { ".pdf" };
            return Array.Exists(documentExtensions, ext => ext == extension);
        }

        private bool IsApkExtension(string extension)
        {
            string[] documentExtensions = { ".apk" };
            return Array.Exists(documentExtensions, ext => ext == extension);
        }

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                TreeNode node = e.Node;
                string fullpath = GetFullPath(node);
                txtDuongDan.Text = fullpath;

                Add_FileMini(fullpath);
            }
        }

        private void Add_FileMini(string fullpath)
        {
            flpFSFile.Controls.Clear();
            query = $"shell ls -a {fullpath}";
            string str = adb.adbCommand(query);

            string[] lines = str.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string line in lines)
            {
                string trimmedLine = line.Trim();
                if (trimmedLine != "." && trimmedLine != "..")
                {
                    string loai = "";
                    if (IsDirectory(trimmedLine))
                        loai = "folder";
                    else
                        loai = GetFileTypeImageKey(trimmedLine);

                    usr_FileMini usr_FileMini = new usr_FileMini(loai, trimmedLine, Path.Combine(fullpath, trimmedLine));
                    usr_FileMini.ControlClicked += flpFSFile_DoubleClick;
                    flpFSFile.Controls.Add(usr_FileMini);
                }
            }
        }

        private void btnQuayLai_Click(object sender, EventArgs e)
        {
            string fullPath = txtDuongDan.Text;
            fullPath = Path.GetDirectoryName(fullPath);

            fullPath = fullPath.Replace("\\", "/");
            txtDuongDan.Text = fullPath;
            Add_FileMini(fullPath);
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            if (txtTimKiem.Text != String.Empty)
            {
                string searchText = txtTimKiem.Text.ToLower();
                bool hasSearchText = !string.IsNullOrEmpty(searchText);
                foreach (usr_FileMini control in flpFSFile.Controls)
                {
                    bool isVisible = true;

                    if (hasSearchText)
                    {
                        isVisible = control.tenfile.ToLower().Contains(searchText);
                    }

                    control.Visible = isVisible;
                }
            }
        }

        private void txtTimKiem_TextChanged(object sender, EventArgs e)
        {
            foreach (usr_FileMini control in flpFSFile.Controls)
            {
                control.Visible = true;
            }
        }

        public void flpFSFile_DoubleClick(object sender, EventArgs e)
        {
            if (sender is usr_FileMini clickedControl)
            {
                string fullPath = clickedControl.fullpath.Replace("\\", "/");
                txtDuongDan.Text = fullPath;
                Add_FileMini(fullPath);
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            Load_TreeFull();
            txtTimKiem.Text = String.Empty;
            txtDuongDan.Text = String.Empty;
            flpFSFile.Controls.Clear();
        }
    }
}