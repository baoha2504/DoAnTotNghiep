using MTA_Mobile_Forensic.GUI.Share;
using MTA_Mobile_Forensic.Model;
using MTA_Mobile_Forensic.Support;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;


namespace MTA_Mobile_Forensic.GUI.Forensic
{
    public partial class usr_DieuTraTaiLieu : UserControl
    {
        public usr_DieuTraTaiLieu()
        {
            InitializeComponent();
            tabControl1.SelectedTabIndex = 0;
        }

        int setup1 = 0;
        int setup2 = 0;
        api api = new api();
        private List<string> tab1_pathFiles;

        private void tabControl1_SelectedTabChanged(object sender, DevComponents.DotNetBar.TabStripTabChangedEventArgs e)
        {
            try
            {
                int selectedIndex = tabControl1.SelectedTabIndex;
                if (selectedIndex == 0 && setup1 == 0)
                {
                    setup1 = 1;
                    splitContainer7.SplitterDistance = (int)(panelEx1.Width / 2.5);
                    splitContainer9.SplitterDistance = (int)(groupPanel11.Height / 3);
                }
                else if (selectedIndex == 1 && setup2 == 0)
                {
                    setup2 = 1;
                }
            }
            catch { }
        }

        private void tab1_GetFileInFolder(string folderPath)
        {
            if (tab1_pathFiles != null)
            {
                tab1_pathFiles.Clear();
            }

            if (folderPath != string.Empty)
            {
                try
                {
                    string[] docExtensions = new string[] { ".doc", ".docx", ".pdf", ".txt" };

                    // Lấy tất cả các tệp dạng âm thanh trong thư mục
                    tab1_pathFiles = Directory.GetFiles(folderPath)
                                          .Where(file => docExtensions.Contains(Path.GetExtension(file).ToLower()))
                                          .ToList();

                    var subDirectories = Directory.GetDirectories(folderPath);
                    foreach (var subDir in subDirectories)
                    {
                        var subDirFiles = Directory.GetFiles(subDir)
                                                   .Where(file => docExtensions.Contains(Path.GetExtension(file).ToLower()))
                                                   .ToList();
                        tab1_pathFiles.AddRange(subDirFiles);
                    }
                }
                catch (Exception ex)
                {
                    frm_Notification frm_Notification = new frm_Notification("error", ex.ToString());
                    frm_Notification.ShowDialog();
                }
            }
        }

        private void AddControl()
        {
            tab1_flpFile.Controls.Clear();
            for (int i = 0; i < tab1_pathFiles.Count; i++)
            {
                string extension = Path.GetExtension(tab1_pathFiles[i]);
                if (extension == ".docx" || extension == ".doc")
                {
                    usr_FileMini usr_FileMini = new usr_FileMini("word", Path.GetFileName(tab1_pathFiles[i]), tab1_pathFiles[i]);
                    usr_FileMini.ControlClicked += tab1_flpFile_Click;
                    tab1_flpFile.Controls.Add(usr_FileMini);
                }
                else if (extension == ".txt")
                {
                    usr_FileMini usr_FileMini = new usr_FileMini("txt", Path.GetFileName(tab1_pathFiles[i]), tab1_pathFiles[i]);
                    usr_FileMini.ControlClicked += tab1_flpFile_Click;
                    tab1_flpFile.Controls.Add(usr_FileMini);
                }
                else if (extension == ".pdf")
                {
                    usr_FileMini usr_FileMini = new usr_FileMini("pdf", Path.GetFileName(tab1_pathFiles[i]), tab1_pathFiles[i]);
                    usr_FileMini.ControlClicked += tab1_flpFile_Click;
                    tab1_flpFile.Controls.Add(usr_FileMini);
                }
            }
        }

        private void tab1_btnChonPath_Click(object sender, System.EventArgs e)
        {
            using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
            {
                DialogResult result = folderBrowserDialog.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(folderBrowserDialog.SelectedPath))
                {
                    string selectedFolder = folderBrowserDialog.SelectedPath;
                    tab1_txtPath.Text = selectedFolder;
                    tab1_GetFileInFolder(selectedFolder);
                    AddControl();
                }
            }
        }

        private void tab1_flpFile_Click(object sender, EventArgs e)
        {
            if (sender is usr_FileMini fileMini)
            {
                tab1_txtPathFileDaChon.Text = fileMini.fullpath;
                string sourceDirectory = AppDomain.CurrentDomain.BaseDirectory;
                string projectDirectory = Directory.GetParent(sourceDirectory).Parent.Parent.FullName;
                string imagePath = Path.Combine(projectDirectory, "Data", "Image");
                if (fileMini.loai == "word")
                {
                    tab1_pictureBoxIcon.Visible = true;
                    tab1_pictureBoxIcon.Image = Image.FromFile(Path.Combine(imagePath, "word-24x24.png"));
                }
                else if (fileMini.loai == "txt")
                {
                    tab1_pictureBoxIcon.Visible = true;
                    tab1_pictureBoxIcon.Image = Image.FromFile(Path.Combine(imagePath, "txt-24x24.png"));
                }
                else if (fileMini.loai == "pdf")
                {
                    tab1_pictureBoxIcon.Visible = true;
                    tab1_pictureBoxIcon.Image = Image.FromFile(Path.Combine(imagePath, "pdf-24x24.png"));
                }
            }
        }

        private async void tab1_btnPhanTichFile_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (tab1_txtPathFileDaChon.Text != string.Empty)
                {
                    tab1_pictureBoxLoad.Visible = true;
                    AnalysisDocument analysis = await api.AskQuestionGroqAsync(Path.GetExtension(tab1_txtPathFileDaChon.Text), tab1_txtPathFileDaChon.Text);
                    if (analysis != null)
                    {
                        tab1_txtChuDe.Text = analysis.topic;
                        tab1_txtCamXuc.Text = analysis.sentiment;
                        tab1_txtTomTat.Text = analysis.summary;
                        tab1_pictureBoxLoad.Visible = false;
                    }
                    else
                    {
                        MessageBox.Show("Failed to get a response from the API.");
                    }
                }
                else
                {
                    MessageBox.Show("Chưa chọn file", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.ToString()}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void tab1_btnMoFile_Click(object sender, System.EventArgs e)
        {
            if (tab1_txtPathFileDaChon.Text != string.Empty)
            {
                string extension = Path.GetExtension(tab1_txtPathFileDaChon.Text);
                if (extension == ".docx" || extension == ".doc")
                {
                    try
                    {
                        if (System.IO.File.Exists(tab1_txtPathFileDaChon.Text))
                        {
                            Process.Start("winword.exe", tab1_txtPathFileDaChon.Text);
                        }
                        else
                        {
                            MessageBox.Show("File không tồn tại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi khi mở file: " + ex.Message, "Lỗi");
                    }
                }
                else if (extension == ".txt")
                {
                    try
                    {
                        if (System.IO.File.Exists(tab1_txtPathFileDaChon.Text))
                        {
                            Process.Start("notepad.exe", tab1_txtPathFileDaChon.Text);
                        }
                        else
                        {
                            MessageBox.Show("File không tồn tại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi khi mở file: " + ex.Message, "Lỗi");
                    }
                }
                else if (extension == ".pdf")
                {
                    try
                    {
                        if (System.IO.File.Exists(tab1_txtPathFileDaChon.Text))
                        {
                            Process.Start("msedge.exe", tab1_txtPathFileDaChon.Text);
                        }
                        else
                        {
                            MessageBox.Show("File không tồn tại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi khi mở file: " + ex.Message, "Lỗi");
                    }
                }
            }
            else
            {
                MessageBox.Show("Chưa chọn file", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void tab1_btnBoChonFile_Click(object sender, System.EventArgs e)
        {
            tab1_pictureBoxIcon.Visible = false;
            tab1_txtPathFileDaChon.Text = string.Empty;
            tab1_txtChuDe.Text = string.Empty;
            tab1_txtCamXuc.Text = string.Empty;
            tab1_txtTomTat.Text = string.Empty;
        }

        private void tab1_txtPathFileDaChon_TextChanged(object sender, EventArgs e)
        {
            tab1_txtChuDe.Text = string.Empty;
            tab1_txtCamXuc.Text = string.Empty;
            tab1_txtTomTat.Text = string.Empty;
        }
    }
}
