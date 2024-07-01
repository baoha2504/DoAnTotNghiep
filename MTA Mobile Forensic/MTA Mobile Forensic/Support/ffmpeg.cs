using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTA_Mobile_Forensic.Support
{
    internal class ffmpeg
    {
        public void ffmpegCommand(string command)
        {
            try
            {
                Process ffmpegProcess = new Process();
                ffmpegProcess.StartInfo.FileName = "ffmpeg.exe";
                ffmpegProcess.StartInfo.Arguments = command;
                ffmpegProcess.StartInfo.UseShellExecute = false;
                ffmpegProcess.StartInfo.RedirectStandardOutput = true;
                ffmpegProcess.StartInfo.RedirectStandardError = true;
                ffmpegProcess.StartInfo.CreateNoWindow = true;
                ffmpegProcess.StartInfo.StandardOutputEncoding = System.Text.Encoding.UTF8;
                ffmpegProcess.StartInfo.StandardErrorEncoding = System.Text.Encoding.UTF8;
                ffmpegProcess.Start();
            }
            catch
            {
                
            }
        }

    }
}
