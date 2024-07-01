using AxWMPLib;
using MTA_Mobile_Forensic.Support;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MTA_Mobile_Forensic.GUI.Share
{
    public partial class frm_XemVideo : Form
    {
        public frm_XemVideo()
        {
            InitializeComponent();
        }

        public string videoPath = "";
        ffmpeg ffmpeg = new ffmpeg();
        function function = new function();

        public frm_XemVideo(string urlVideo)
        {
            InitializeComponent();

            this.Text = urlVideo;
            videoPath = urlVideo;
            axWindowsMediaPlayer1.URL = urlVideo;
            axWindowsMediaPlayer1.Ctlcontrols.play();
        }

        public int solanchup = 0;
        private async void btnChupAnh_Click(object sender, EventArgs e)
        {
            solanchup++;
            TimeSpan captureTime = GetCurrentPlaybackTime(axWindowsMediaPlayer1);
            string videoFileName = Path.GetFileNameWithoutExtension(videoPath);
            string outputDirectory = Path.Combine(Path.GetDirectoryName(videoPath), videoFileName);

            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            string outputImagePath = Path.Combine(outputDirectory, Path.GetFileName(videoFileName) + "_Capture" + solanchup.ToString() + ".png");

            // Chạy CaptureFrame trên một Task riêng
            await Task.Run(() => CaptureFrame(videoPath, captureTime, outputImagePath));
        }

        private TimeSpan GetCurrentPlaybackTime(AxWindowsMediaPlayer player)
        {
            double currentPosition = player.Ctlcontrols.currentPosition;
            return TimeSpan.FromSeconds(currentPosition);
        }

        public void CaptureFrame(string videoPath, TimeSpan timestamp, string outputImagePath)
        {
            try
            {
                string timestampString = timestamp.ToString(@"hh\:mm\:ss\.fff");
                string arguments = $"-y -ss {timestampString} -i \"{videoPath}\" -frames:v 1 \"{outputImagePath}\"";

                ffmpeg.ffmpegCommand(arguments);

                // Tạo một task để thêm usr_AnhMini vào flpAnhDaChup khi tệp tồn tại
                Task.Run(() =>
                {
                    // Kiểm tra sự tồn tại của tệp
                    while (!File.Exists(outputImagePath))
                    {
                        Thread.Sleep(100); // Đợi 100ms trước khi kiểm tra lại
                    }

                    // Sử dụng Invoke để thêm usr_AnhMini vào flpAnhDaChup trong UI thread
                    flpAnhDaChup.Invoke((MethodInvoker)delegate
                    {
                        usr_AnhMini usr_AnhMini = new usr_AnhMini(outputImagePath, Path.GetFileName(outputImagePath), function.GetLastModified(outputImagePath));
                        usr_AnhMini.ControlClicked += flpAnhDaChup_Click;
                        usr_AnhMini.checkBox.Visible = false;
                        flpAnhDaChup.Controls.Add(usr_AnhMini);
                    });
                });
            }
            catch (Exception ex)
            {
                frm_Notification frm_Notification = new frm_Notification("error", ex.ToString());
                frm_Notification.ShowDialog();
            }
        }

        private void Process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            Console.WriteLine(e.Data);
        }

        private void flpAnhDaChup_Click(object sender, EventArgs e)
        {
            if (sender is usr_AnhMini clickedControl)
            {
                if (clickedControl.linkanh != String.Empty)
                {
                    frm_XemAnh frm_XemAnh = new frm_XemAnh(clickedControl.linkanh);
                    frm_XemAnh.Show();
                }
            }
        }
    }
}
