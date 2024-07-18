using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTA_Mobile_Forensic.Support
{
    internal class winrar
    {
        public string winrarCommand(string command)
        {
            try
            {
                Process winrarProcess = new Process();
                winrarProcess.StartInfo.FileName = "WinRAR.exe";
                winrarProcess.StartInfo.Arguments = command;
                winrarProcess.StartInfo.UseShellExecute = false;
                winrarProcess.StartInfo.RedirectStandardOutput = true;
                winrarProcess.StartInfo.CreateNoWindow = true; // Hide the console window (use with caution)
                winrarProcess.StartInfo.StandardOutputEncoding = System.Text.Encoding.UTF8; // Thiết lập mã hóa UTF-8 cho đầu ra
                winrarProcess.Start();

                string output = winrarProcess.StandardOutput.ReadToEnd();
                winrarProcess.WaitForExit();

                return output;
            }
            catch (Exception ex)
            {
                return ("Error Winrar command: " + ex.ToString());
            }
        }
    }
}
