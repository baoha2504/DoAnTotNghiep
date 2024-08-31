using MTA_Mobile_Forensic.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTA_Mobile_Forensic.Support
{
    internal class libimobiledevice
    {
        public string idevice_idCommand(string command)
        {
            try
            {
                Process libiProcess = new Process();
                libiProcess.StartInfo.FileName = "idevice_id.exe";
                libiProcess.StartInfo.Arguments = command;
                libiProcess.StartInfo.UseShellExecute = false;
                libiProcess.StartInfo.RedirectStandardOutput = true;
                libiProcess.StartInfo.CreateNoWindow = true; // Hide the console window (use with caution)
                libiProcess.StartInfo.StandardOutputEncoding = System.Text.Encoding.UTF8; // Thiết lập mã hóa UTF-8 cho đầu ra
                libiProcess.Start();

                string output = libiProcess.StandardOutput.ReadToEnd();
                libiProcess.WaitForExit();

                return output;
            }
            catch (Exception ex)
            {
                return ("Error LIBIMOBILEDEVICE command: " + ex.ToString());
            }
        }

        public string ideviceinfoCommand(string command)
        {
            try
            {
                Process libiProcess = new Process();
                libiProcess.StartInfo.FileName = "ideviceinfo.exe";
                libiProcess.StartInfo.Arguments = command;
                libiProcess.StartInfo.UseShellExecute = false;
                libiProcess.StartInfo.RedirectStandardOutput = true;
                libiProcess.StartInfo.CreateNoWindow = true; // Hide the console window (use with caution)
                libiProcess.StartInfo.StandardOutputEncoding = System.Text.Encoding.UTF8; // Thiết lập mã hóa UTF-8 cho đầu ra
                libiProcess.Start();

                string output = libiProcess.StandardOutput.ReadToEnd();
                libiProcess.WaitForExit();

                return output;
            }
            catch (Exception ex)
            {
                return ("Error LIBIMOBILEDEVICE command: " + ex.ToString());
            }
        }

        public string ideviceinstallerCommand(string command)
        {
            try
            {
                Process libiProcess = new Process();
                libiProcess.StartInfo.FileName = "ideviceinstaller.exe";
                libiProcess.StartInfo.Arguments = command;
                libiProcess.StartInfo.UseShellExecute = false;
                libiProcess.StartInfo.RedirectStandardOutput = true;
                libiProcess.StartInfo.CreateNoWindow = true; // Hide the console window (use with caution)
                libiProcess.StartInfo.StandardOutputEncoding = System.Text.Encoding.UTF8; // Thiết lập mã hóa UTF-8 cho đầu ra
                libiProcess.Start();

                string output = libiProcess.StandardOutput.ReadToEnd();
                libiProcess.WaitForExit();

                return output;
            }
            catch (Exception ex)
            {
                return ("Error ideviceinstaller command: " + ex.ToString());
            }
        }
    }
}
