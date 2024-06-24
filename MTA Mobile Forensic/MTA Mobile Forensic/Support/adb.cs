using System;
using System.Diagnostics;

namespace MTA_Mobile_Forensic.Support
{
    internal class adb
    {
        public string adbCommand(string command)
        {
            try
            {
                Process adbProcess = new Process();
                adbProcess.StartInfo.FileName = "adb.exe";
                adbProcess.StartInfo.Arguments = command;
                adbProcess.StartInfo.UseShellExecute = false;
                adbProcess.StartInfo.RedirectStandardOutput = true;
                adbProcess.StartInfo.CreateNoWindow = true; // Hide the console window (use with caution)
                adbProcess.StartInfo.StandardOutputEncoding = System.Text.Encoding.UTF8; // Thiết lập mã hóa UTF-8 cho đầu ra
                adbProcess.Start();

                string output = adbProcess.StandardOutput.ReadToEnd();
                adbProcess.WaitForExit();

                return output;
            }
            catch (Exception ex)
            {
                return ("Error ADB command: " + ex.ToString());
            }
        }
    }
}
