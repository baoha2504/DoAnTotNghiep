using MTA_Mobile_Forensic.GUI.Share;
using MTA_Mobile_Forensic.Support;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MTA_Mobile_Forensic.GUI.Forensic
{
    public partial class usr_DieuTraVideo : UserControl
    {
        int setup1 = 0;
        int setup2 = 0;
        string tab1_pathImage_dachon = "";
        string tab1_pathFolder_dachon = "";
        private List<string> tab1_videoFiles = new List<string>();
        private List<string> tab1_response = new List<string>();
        private int tab1_sttVideo = -1;
        function function = new function();
        api api = new api();

        public usr_DieuTraVideo()
        {
            InitializeComponent();
        }

        private void tabControl1_SelectedTabChanged(object sender, DevComponents.DotNetBar.TabStripTabChangedEventArgs e)
        {
            try
            {
                int selectedIndex = tabControl1.SelectedTabIndex;
                if (selectedIndex == 0 && setup1 == 0)
                {
                    setup1 = 1;
                    splitContainer1.SplitterDistance = (int)(panelEx1.Width / 2.5);
                }
                else if (selectedIndex == 1 && setup2 == 0)
                {
                    setup2 = 1;
                    splitContainer2.SplitterDistance = (int)(panelEx1.Width / 2.5);
                }
            }
            catch { }
        }

        private void tab1_btnChonImage_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image files (*.jpg;*.jpeg;*.png;*.bmp)|*.jpg;*.jpeg;*.png;*.bmp|All files (*.*)|*.*";
                openFileDialog.Title = "Chọn file ảnh";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    tab1_txtPathImage.Text = openFileDialog.FileName;
                    tab1_Image.SizeMode = PictureBoxSizeMode.Zoom;
                    tab1_Image.Image = System.Drawing.Image.FromFile(openFileDialog.FileName);
                    tab1_pathImage_dachon = openFileDialog.FileName;
                    for (int i = 0; i < tab1_videoFiles.Count; i++)
                    {
                        tab1_response.Add($"Item");
                    }
                }
            }
        }

        private List<string> GetSmallestSubfolders(string rootFolderPath)
        {
            List<string> result = new List<string>();

            // Duyệt qua tất cả các thư mục con trong thư mục gốc
            foreach (string subfolder in Directory.GetDirectories(rootFolderPath, "*", SearchOption.AllDirectories))
            {
                // Kiểm tra xem thư mục này có chứa thư mục con nào không
                if (Directory.GetDirectories(subfolder).Length == 0)
                {
                    result.Add(subfolder);
                }
            }

            return result;
        }

        private void GetVideoInFolder(string folderPath)
        {
            if (tab1_videoFiles != null)
            {
                tab1_videoFiles.Clear();
            }

            if (folderPath != string.Empty)
            {
                try
                {
                    string[] videoExtensions = new string[] { ".mp4", ".avi", ".mov", ".wmv", ".flv", ".mkv", ".webm", ".m4v", ".mpeg", ".mpg" };

                    tab1_videoFiles = Directory.GetFiles(folderPath)
                                             .Where(file => videoExtensions.Contains(Path.GetExtension(file).ToLower()))
                                             .ToList();


                    List<string> smallestSubfolders = GetSmallestSubfolders(folderPath);
                    foreach (var subfolder in smallestSubfolders)
                    {
                        var subDirFiles = Directory.GetFiles(subfolder)
                            .Where(file => videoExtensions.Contains(Path.GetExtension(file).ToLower()))
                            .ToList();
                        tab1_videoFiles.AddRange(subDirFiles);
                    }

                    for (int i = 0; i < tab1_videoFiles.Count; i++)
                    {
                        try
                        {
                            usr_VideoMini usr_VideoMini = new usr_VideoMini(tab1_videoFiles[i], Path.GetFileName(tab1_videoFiles[i]), function.GetLastModified(tab1_videoFiles[i]));
                            usr_VideoMini.ControlClicked += tab1_flpDSVideo_Click;
                            tab1_flpDSVideo.Controls.Add(usr_VideoMini);
                        }
                        catch { }
                    }
                }
                catch (Exception ex)
                {
                    frm_Notification frm_Notification = new frm_Notification("error", ex.ToString());
                    frm_Notification.ShowDialog();
                }
            }
        }

        private void tab1_btnChonFolder_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
            {
                DialogResult result = folderBrowserDialog.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(folderBrowserDialog.SelectedPath))
                {
                    tab1_flpDSVideo.Controls.Clear();
                    string selectedFolder = folderBrowserDialog.SelectedPath;
                    tab1_txtPathFolder.Text = selectedFolder;
                    tab1_pathFolder_dachon = selectedFolder;
                    GetVideoInFolder(selectedFolder);
                    for (int i = 0; i < tab1_videoFiles.Count; i++)
                    {
                        tab1_response.Add($"Item");
                    }
                }
            }
        }

        private void LoadVideo(string path)
        {
            tab1_axWindowsMediaPlayer1.URL = path;
            tab1_axWindowsMediaPlayer1.Ctlcontrols.play();
        }

        private async Task TimKiemKhuonMatVideo(string path, List<string> listPath)
        {
            List<string> results = new List<string>();
            int batchSize = 10;

            for (int i = 0; i < listPath.Count; i += batchSize)
            {
                List<string> batch = listPath.Skip(i).Take(batchSize).ToList();
                string[] batchArray = batch.ToArray();

                var response = await api.TimKiemKhuonMatVideo(path, batchArray);

                if (response != null)
                {
                    results.AddRange(response);

                    for (int j = 0; j < batch.Count; j++)
                    {
                        tab1_response[tab1_sttVideo] = response[j];
                        if (response[j] != "false")
                        {
                            tab1_txtThongTinTimDuoc.ForeColor = Color.Green;
                            tab1_txtThongTinTimDuoc.Text = response[j].Replace("\n", Environment.NewLine);
                        }
                        else
                        {
                            tab1_txtThongTinTimDuoc.ForeColor = Color.Red;
                            tab1_txtThongTinTimDuoc.Text = "Không xuất hiện khuôn mặt trong video";
                        }
                    }
                    tab1_ImageLoad.Visible = false;
                }
                else
                {
                    MessageBox.Show("Failed to get a response from the API.");
                }
            }
        }

        private void tab1_flpDSVideo_Click(object sender, EventArgs e)
        {
            if (sender is usr_VideoMini clickedControl)
            {
                var controls = tab1_flpDSVideo.Controls;

                int index = controls.IndexOf(clickedControl);
                tab1_sttVideo = index;
                LoadVideo(clickedControl.linkvideo);
                tab1_txtPathVideo.Text = clickedControl.linkvideo;
            }
        }

        private async void tab1_btnTimKiemKhuonMat_Click(object sender, EventArgs e)
        {
            if (tab1_txtPathImage.Text != string.Empty || tab1_txtPathVideo.Text != string.Empty)
            {
                if (tab1_response[tab1_sttVideo] != "Item")
                {
                    if (tab1_response[tab1_sttVideo] != "false")
                    {
                        tab1_txtThongTinTimDuoc.ForeColor = Color.Green;
                        tab1_txtThongTinTimDuoc.Text = tab1_response[tab1_sttVideo].Replace("\n", Environment.NewLine);
                    }
                    else
                    {
                        tab1_txtThongTinTimDuoc.ForeColor = Color.Red;
                        tab1_txtThongTinTimDuoc.Text = "Không xuất hiện khuôn mặt trong video";
                    }
                }
                else
                {
                    tab1_txtThongTinTimDuoc.Text = string.Empty;
                    tab1_ImageLoad.Visible = true;
                    List<string> pathVideos = new List<string>();
                    pathVideos.Add(tab1_txtPathVideo.Text);
                    await TimKiemKhuonMatVideo(tab1_pathImage_dachon, pathVideos);
                }
            }
            else
            {
                MessageBox.Show("Chưa chọn ảnh hoặc video!");
            }
        }
    }
}
