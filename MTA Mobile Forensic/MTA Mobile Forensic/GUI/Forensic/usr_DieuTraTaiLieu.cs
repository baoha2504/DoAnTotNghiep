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

            tab2_flpChonAnh.DragEnter += Flp_DragEnter;
            tab2_flpAnhDaChon.DragEnter += Flp_DragEnter;

            tab2_flpChonAnh.DragDrop += Flp_DragDrop;
            tab2_flpAnhDaChon.DragDrop += Flp_DragDrop;
        }

        int setup1 = 0;
        int setup2 = 0;
        int setup3 = 0;
        string linkanhOCR = "";
        api api = new api();
        function function = new function();
        private List<string> tab1_pathFiles;
        private List<string> imageFiles;
        private List<string> tab3_imageFiles;
        private List<string> imageChoisedFiles = new List<string>();
        private Image originalImage;


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
                    splitContainer1.SplitterDistance = (int)(panelEx1.Width / 2);
                    splitContainer2.SplitterDistance = (int)(panelEx1.Width / 6);
                }
                else if (selectedIndex == 2 && setup3 == 0)
                {
                    setup3 = 1;
                    splitContainer3.SplitterDistance = (int)(panelEx1.Width / 3.5);
                    splitContainer4.SplitterDistance = (int)(groupPanel8.Width / 2);
                    tab3_pictureBoxImage1.Height = (int)((splitContainer4.Height - 40) / 2);
                }
            }
            catch { }
        }

        private void panelEx1_Resize(object sender, EventArgs e)
        {
            try
            {
                splitContainer7.SplitterDistance = (int)(panelEx1.Width / 2.5);
                splitContainer9.SplitterDistance = (int)(groupPanel11.Height / 3);

                splitContainer1.SplitterDistance = (int)(panelEx1.Width / 2);
                splitContainer2.SplitterDistance = (int)(panelEx1.Width / 6);

                splitContainer3.SplitterDistance = (int)(panelEx1.Width / 3.5);
                splitContainer4.SplitterDistance = (int)(groupPanel8.Width / 2);
                tab3_pictureBoxImage1.Height = (int)((splitContainer4.Height - 40) / 2);
            }
            catch { }
        }


        #region================================  TAB1  ====================================================================
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
                    tab1_pathFiles = Directory.GetFiles(folderPath, "*.*", SearchOption.AllDirectories)
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
                            Process.Start("winword.exe", $"\"{tab1_txtPathFileDaChon.Text}\"");
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
                            Process.Start("notepad.exe", $"\"{tab1_txtPathFileDaChon.Text}\"");
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
                            Process.Start("msedge.exe", $"\"{tab1_txtPathFileDaChon.Text}\"");
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
        #endregion

        #region================================  TAB2  ====================================================================
        private void Flp_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(usr_AnhMini)))
            {
                e.Effect = DragDropEffects.Move;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void Flp_DragDrop(object sender, DragEventArgs e)
        {
            FlowLayoutPanel targetPanel = (FlowLayoutPanel)sender;
            Control draggedControl = null;

            if (e.Data.GetDataPresent(typeof(usr_AnhMini)))
            {
                draggedControl = (usr_AnhMini)e.Data.GetData(typeof(usr_AnhMini));
            }

            if (draggedControl != null)
            {
                // Remove from old panel
                FlowLayoutPanel oldPanel = (FlowLayoutPanel)draggedControl.Parent;
                oldPanel.Controls.Remove(draggedControl);

                // Add to new panel
                targetPanel.Controls.Add(draggedControl);
            }
        }

        private void tab2_GetImageInFolder(string folderPath)
        {
            if (folderPath != string.Empty)
            {
                try
                {
                    // Các đuôi file dạng ảnh
                    string[] imageExtensions = new string[] { ".jpg", ".jpeg", ".png", ".bmp", ".gif", ".tiff", ".webp" };

                    // Lấy tất cả các tệp dạng ảnh trong thư mục
                    imageFiles = Directory.GetFiles(folderPath)
                                             .Where(file => imageExtensions.Contains(Path.GetExtension(file).ToLower()))
                                             .ToList();
                    Add_To_flpChonAnh();
                }
                catch (Exception ex)
                {
                    frm_Notification frm_Notification = new frm_Notification("error", ex.ToString());
                    frm_Notification.ShowDialog();
                }
            }
        }

        private void Add_To_flpChonAnh()
        {
            tab2_flpChonAnh.Controls.Clear();

            foreach (var item in imageFiles)
            {
                usr_AnhMini usr_AnhMini = new usr_AnhMini(item, Path.GetFileName(item), function.GetLastModified(item));
                usr_AnhMini.ControlClicked += tab2_flpAnhDaChon_Click;
                //usr_AnhMini.checkBox.Visible = false;
                usr_AnhMini.checkBox.Checked = true;
                tab2_flpChonAnh.Controls.Add(usr_AnhMini);
            }
        }

        private void tab2_btnChonPath_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
            {
                DialogResult result = folderBrowserDialog.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(folderBrowserDialog.SelectedPath))
                {
                    string selectedFolder = folderBrowserDialog.SelectedPath;
                    tab2_txtPath.Text = selectedFolder;
                    tab2_GetImageInFolder(selectedFolder);
                }
            }
        }

        private void tab2_btnChonTatCa_Click(object sender, EventArgs e)
        {
            foreach (usr_AnhMini anhMini in tab2_flpAnhDaChon.Controls)
            {
                anhMini.checkBox.Checked = true;
            }
        }

        private void tab2_btnBoChonTatCa_Click(object sender, EventArgs e)
        {
            foreach (usr_AnhMini anhMini in tab2_flpAnhDaChon.Controls)
            {
                anhMini.checkBox.Checked = false;
            }
        }

        private void GetImageChoisedFiles()
        {
            if (imageChoisedFiles != null)
            {
                imageChoisedFiles.Clear();
            }

            foreach (usr_AnhMini anhMini in tab2_flpAnhDaChon.Controls)
            {
                if (anhMini.checkBox.Checked == true)
                {
                    imageChoisedFiles.Add(anhMini.linkanh);
                }
            }
        }

        private async void tab2_btnStartScan_Click(object sender, EventArgs e)
        {
            tab2_pictureBoxLoad.Visible = true;
            tab2_pictureBoxIcon.Visible = false;
            tab2_pdfViewer.Visible = false;
            tab2_txtFileScan.Text = string.Empty;
            tab2_btnStartScan.Text = "Đang scan...";

            GetImageChoisedFiles();

            string sourceDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string projectDirectory = Directory.GetParent(sourceDirectory).Parent.Parent.FullName;
            string filePath = Path.Combine(projectDirectory, "Cache");
            string pfdFileName = "ScanDocument_" + DateTime.Now.ToString("HH.mm.ss_dd.MM.yyyy") + ".pdf";
            string destinationPath = Path.Combine(filePath, pfdFileName);
            string path = await api.ScanDocument(destinationPath, imageChoisedFiles.ToArray());

            if (path != null)
            {
                tab2_pictureBoxLoad.Visible = false;
                tab2_pictureBoxIcon.Visible = true;
                tab2_pdfViewer.LoadDocument(path);
                tab2_pdfViewer.Visible = true;
                tab2_txtFileScan.Text = path;
                tab2_btnStartScan.Text = "Bắt đầu scan";
            }
            else
            {
                tab2_btnStartScan.Text = "Bắt đầu scan";
                tab2_pictureBoxLoad.Visible = true;
                MessageBox.Show("Failed to get a response from the API.");
            }
        }

        private void tab2_flpAnhDaChon_Click(object sender, EventArgs e)
        {
            if (sender is usr_AnhMini anhMini)
            {
                frm_XemAnh frm_XemAnh = new frm_XemAnh(anhMini.linkanh);
                frm_XemAnh.Show();
            }
        }

        private void OpenExplorer(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    // Mở thư mục và trỏ vào file
                    string argument = "/select, \"" + filePath + "\"";
                    Process.Start("explorer.exe", argument);
                }
                else
                {
                    MessageBox.Show("File không tồn tại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Có lỗi xảy ra: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void tab2_pictureBoxIcon_Click(object sender, EventArgs e)
        {
            OpenExplorer(tab2_txtFileScan.Text);
        }
        #endregion

        #region================================  TAB3  ====================================================================
        private void tab3_Add_To_flpChonAnh()
        {
            tab3_flpAnh.Controls.Clear();

            foreach (var item in tab3_imageFiles)
            {
                usr_AnhMini usr_AnhMini = new usr_AnhMini(item, Path.GetFileName(item), function.GetLastModified(item));
                usr_AnhMini.ControlClicked += tab3_flpAnh_Click;
                //usr_AnhMini.checkBox.Visible = false;
                usr_AnhMini.checkBox.Checked = true;
                tab3_flpAnh.Controls.Add(usr_AnhMini);
            }
        }

        private void tab3_GetImageInFolder(string folderPath)
        {
            if (folderPath != string.Empty)
            {
                try
                {
                    // Các đuôi file dạng ảnh
                    string[] imageExtensions = new string[] { ".jpg", ".jpeg", ".png", ".bmp", ".gif", ".tiff", ".webp" };

                    // Lấy tất cả các tệp dạng ảnh trong thư mục
                    tab3_imageFiles = Directory.GetFiles(folderPath)
                                             .Where(file => imageExtensions.Contains(Path.GetExtension(file).ToLower()))
                                             .ToList();
                    tab3_Add_To_flpChonAnh();
                }
                catch (Exception ex)
                {
                    frm_Notification frm_Notification = new frm_Notification("error", ex.ToString());
                    frm_Notification.ShowDialog();
                }
            }
        }

        private void tab3_ChonPath_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
            {
                DialogResult result = folderBrowserDialog.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(folderBrowserDialog.SelectedPath))
                {
                    string selectedFolder = folderBrowserDialog.SelectedPath;
                    tab3_txtPath.Text = selectedFolder;
                    tab3_GetImageInFolder(selectedFolder);
                }
            }
        }

        private void tab3_flpAnh_Click(object sender, EventArgs e)
        {
            if (sender is usr_AnhMini clickedControl)
            {
                tab3_txtPathDaChon.Text = clickedControl.linkanh;
                tab3_pictureBoxImage1.SizeMode = PictureBoxSizeMode.Zoom;
                tab3_pictureBoxImage1.Image = Image.FromFile(clickedControl.linkanh);
                tab3_pictureBoxImage2.Image = null;
                tab3_txtOCR.Text = string.Empty;
            }
        }

        private async void tab3_StartOCR_Click(object sender, EventArgs e)
        {
            tab3_StartOCR.Text = "Đang OCR...";
            tab3_pictureBoxLoad.Visible = true;
            string sourceDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string projectDirectory = Directory.GetParent(sourceDirectory).Parent.Parent.FullName;
            string filePath = Path.Combine(projectDirectory, "Cache");
            string fileName = "OCR_Image_" + DateTime.Now.ToString("HH.mm.ss_dd.MM.yyyy") + ".png";
            string outputPath = Path.Combine(filePath, fileName);
            OCR ocr = new OCR();
            ocr = await api.XuLyOCRAnh(tab3_txtPathDaChon.Text, outputPath);
            if (ocr != null)
            {
                tab3_pictureBoxImage2.SizeMode = PictureBoxSizeMode.Zoom;
                tab3_pictureBoxImage2.Image = Image.FromFile(ocr.filename);
                originalImage = Image.FromFile(ocr.filename);
                linkanhOCR = ocr.filename;
                tab3_pictureBoxLoad.Visible = false;
                tab3_txtOCR.Text = ocr.text;
                tab3_StartOCR.Text = "Bắt đầu OCR";
            }
            else
            {
                tab3_StartOCR.Text = "Bắt đầu OCR";
                linkanhOCR = string.Empty;
                tab3_pictureBoxLoad.Visible = true;
                MessageBox.Show("Failed to get a response from the API.");
            }
        }

        private void tab3_pictureBoxImage1_Click(object sender, EventArgs e)
        {
            if (tab3_txtPathDaChon.Text != string.Empty)
            {
                frm_XemAnh frm_XemAnh = new frm_XemAnh(tab3_txtPathDaChon.Text);
                frm_XemAnh.Show();
            }
        }

        private void tab3_pictureBoxImage2_Click(object sender, EventArgs e)
        {
            try
            {
                if (linkanhOCR != string.Empty)
                {
                    frm_XemAnh frm_XemAnh = new frm_XemAnh(linkanhOCR);
                    frm_XemAnh.Show();
                }
            }
            catch { }
        }

        private async void tab3_LuuAnhOCR_Click(object sender, EventArgs e)
        {
            if (linkanhOCR != string.Empty)
            {
                using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
                {
                    DialogResult result = folderBrowserDialog.ShowDialog();

                    if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(folderBrowserDialog.SelectedPath))
                    {
                        string response = await api.XoayAnhVaLuu(linkanhOCR, folderBrowserDialog.SelectedPath, (tab3_zoomTrackBarControl.Value - 90).ToString());
                        if (response != null)
                        {
                            if (response == "success")
                            {
                                MessageBox.Show($"Đã lưu file đến thư mục: {folderBrowserDialog.SelectedPath}");
                            }
                            else
                            {
                                MessageBox.Show("Lưu không thành công", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Failed to get a response from the API.");
                        }
                    }
                }
            }
        }

        public Image RotateImage(Image image, float angle)
        {
            if (image == null)
                throw new ArgumentNullException("image");

            Bitmap rotatedBmp = new Bitmap(image.Width, image.Height);
            rotatedBmp.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            // Sử dụng Graphics để vẽ ảnh xoay
            using (Graphics g = Graphics.FromImage(rotatedBmp))
            {
                // Đặt chất lượng vẽ
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

                // Dịch chuyển điểm gốc của Graphics đến trung tâm ảnh
                g.TranslateTransform((float)image.Width / 2, (float)image.Height / 2);
                // Xoay ảnh
                g.RotateTransform(angle);
                // Dịch chuyển lại điểm gốc của Graphics về vị trí ban đầu
                g.TranslateTransform(-(float)image.Width / 2, -(float)image.Height / 2);

                // Vẽ ảnh
                g.DrawImage(image, new Point(0, 0));
            }

            return rotatedBmp;
        }

        private void tab3_zoomTrackBarControl_ValueChanged(object sender, EventArgs e)
        {
            float angle = tab3_zoomTrackBarControl.Value - 90;
            tab3_pictureBoxImage2.Image = RotateImage(originalImage, angle);
        }
        #endregion
    }
}