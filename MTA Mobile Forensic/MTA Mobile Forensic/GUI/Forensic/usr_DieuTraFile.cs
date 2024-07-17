using MTA_Mobile_Forensic.Support;
using System;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Windows.Forms;

namespace MTA_Mobile_Forensic.GUI.Forensic
{
    public partial class usr_DieuTraFile : UserControl
    {
        string query = "";
        string cachePath = "";
        string fullFilePath = "";
        adb adb = new adb();
        hash hash = new hash();
        hxd hxd = new hxd();
        ImageList imageList;

        public usr_DieuTraFile()
        {
            InitializeComponent();

            treeView1.BeforeExpand += TreeView1_BeforeExpand;

            Load_TreeFull();
            LoadData();

            toolTip.SetToolTip(btnCopyHashMD5, "Sao chép");
            toolTip.SetToolTip(btnCopyHashSHA128, "Sao chép");
            toolTip.SetToolTip(btnCopyHashSHA256, "Sao chép");
            toolTip.SetToolTip(btnCopyHashSHA512, "Sao chép");
        }

        private void LoadData()
        {
            string sourceDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string projectDirectory = Directory.GetParent(sourceDirectory).Parent.Parent.FullName;
            string imagePath = Path.Combine(projectDirectory, "Data", "Image");
            cachePath = Path.Combine(projectDirectory, "Cache");
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
                str = "-1983762891\r\n.\r\n..\r\n.6226f7cbe59e99a90b5cef6f94f966fd\r\n.804c9a5b09dc1e99aefe17dd530290c3\r\n.Anh\r\n.Em\r\n.Video\r\n.Video Tele\r\n.a.dat\r\n.anti-third\r\n.backups\r\n.cc\r\n.fp\r\n.gs_fs0\r\n.msg_id\r\n.profig.os\r\n.sys\r\n.system_secure_policy\r\n.tdck\r\n.uxx\r\nASD\r\nAndroid\r\nCentauriOversea\r\nCounter Strike Cataclysm\r\nDCIM\r\nDocuments\r\nDownload\r\nFonts\r\nMIUI\r\nMidasOversea\r\nMovies\r\nMusic\r\nNS Vault\r\nNotifications\r\nPhoto Editor\r\nPicsArt\r\nPictures\r\nQTAudioEngine\r\nSnapseed\r\nTelegram\r\nbackup\r\nbackups\r\nbrowser\r\nbytedance\r\ncacheDocx\r\ncentauri\r\ncom.facebook.katana\r\ncom.facebook.orca\r\ncom.vng.codmvn\r\ndctp\r\ndid\r\ndownloaded_rom\r\ndubox\r\ngroup.com.vndc.app.widget\r\niciba\r\nlog_notification.txt\r\nmace_cl_compiled_program.bin\r\nmace_run\r\nmonitor\r\nmyfile\r\npuex_dfaa3cad.dat\r\nshopeeVN\r\nsnapshot\r\ntencent\r\ntxrtmp\r\nzalo";
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
                if (!IsDirectory(fullpath))
                {
                    txtDuongDan.Text = "/" + fullpath;
                }
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            Load_TreeFull();
        }

        private void txtDuongDan_TextChanged(object sender, EventArgs e)
        {
            string fullSourcePath = txtDuongDan.Text;
            string pullCommand = $"pull {fullSourcePath} \"{cachePath}\"";
            string result = adb.adbCommand(pullCommand);

            string filename = Path.GetFileName(fullSourcePath);
            fullFilePath = Path.Combine(cachePath, filename);
            txtHashMD5.Text = hash.ComputeFileMD5(fullFilePath);
            txtHashSHA128.Text = hash.ComputeFileSHA128(fullFilePath);
            txtHashSHA256.Text = hash.ComputeFileSHA256(fullFilePath);
            txtHashSHA512.Text = hash.ComputeFileSHA512(fullFilePath);

            CheckVirusTotal(filename);
        }

        private async void CheckVirusTotal(string filename)
        {
            string API_KEY = "f69e0c8d492999188d45c8a37f1fe88abdbf23c21725931386c489df7e76a4bf";
            string hash_sha256 = txtHashSHA256.Text;

            // Endpoint của VirusTotal API
            string url = $"https://www.virustotal.com/vtapi/v2/file/report?apikey={API_KEY}&resource={hash_sha256}";

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(url);

                string resultContent = await response.Content.ReadAsStringAsync();

                if (!string.IsNullOrEmpty(resultContent))
                {
                    dynamic result = Newtonsoft.Json.JsonConvert.DeserializeObject(resultContent);

                    if (result.positives != null && result.positives > 0)
                    {
                        txtThongTinFile.ForeColor = Color.Red;
                        txtThongTinFile.Text = $"File {filename} được phát hiện là độc hại!";
                    }
                    else
                    {
                        txtThongTinFile.ForeColor = Color.Green;
                        txtThongTinFile.Text = $"File {filename} không được phát hiện là độc hại.";
                    }
                }
                else
                {
                    MessageBox.Show("Không có kết quả phản hồi từ VirusTotal API.");
                }
            }
        }

        private async void LoadWeb(string link)
        {
            await webView21.EnsureCoreWebView2Async(null);
            webView21.CoreWebView2.Navigate(link);
        }

        private async void ClearWebView()
        {
            await webView21.EnsureCoreWebView2Async(null);
            webView21.CoreWebView2.Navigate("about:blank");
        }

        private void btnCheckVirus_Click(object sender, EventArgs e)
        {
            string url = "https://www.virustotal.com/gui/file/" + txtHashSHA128.Text;
            LoadWeb(url);
        }

        private void btnCheckHxD_Click(object sender, EventArgs e)
        {
            query = $"\"{fullFilePath}\"";
            string str = hxd.hxdCommand(query);
        }

        private void btnCopyHashMD5_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(txtHashMD5.Text);
        }

        private void btnCopyHashSHA128_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(txtHashSHA128.Text);
        }

        private void btnCopyHashSHA256_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(txtHashSHA256.Text);
        }

        private void btnCopyHashSHA512_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(txtHashSHA512.Text);
        }
    }
}
