using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTA_Mobile_Forensic.Support
{
    internal class exiftool
    {
        public string exiftoolCommand(string command)
        {
            try
            {
                Process exiftoolProcess = new Process();
                exiftoolProcess.StartInfo.FileName = "exiftool";
                exiftoolProcess.StartInfo.Arguments = command;
                exiftoolProcess.StartInfo.UseShellExecute = false;
                exiftoolProcess.StartInfo.RedirectStandardOutput = true;
                exiftoolProcess.StartInfo.CreateNoWindow = true; // Hide the console window (use with caution)
                exiftoolProcess.StartInfo.StandardOutputEncoding = System.Text.Encoding.UTF8; // Thiết lập mã hóa UTF-8 cho đầu ra
                exiftoolProcess.Start();

                string output = exiftoolProcess.StandardOutput.ReadToEnd();
                exiftoolProcess.WaitForExit();

                return output;
            }
            catch (Exception ex)
            {
                return ("Error ADB command: " + ex.ToString());
            }
        }
    }
}
