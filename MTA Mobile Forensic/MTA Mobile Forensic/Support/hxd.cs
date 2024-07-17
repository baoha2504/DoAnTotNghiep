using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTA_Mobile_Forensic.Support
{
    internal class hxd
    {
        public string hxdCommand(string command)
        {
            try
            {
                Process adbProcess = new Process();
                adbProcess.StartInfo.FileName = "HxD.exe";
                adbProcess.StartInfo.Arguments = command;
                adbProcess.StartInfo.UseShellExecute = false;
                adbProcess.StartInfo.RedirectStandardOutput = true;
                adbProcess.StartInfo.CreateNoWindow = true;
                adbProcess.StartInfo.StandardOutputEncoding = System.Text.Encoding.UTF8;
                adbProcess.Start();

                string output = adbProcess.StandardOutput.ReadToEnd();
                adbProcess.WaitForExit();

                return output;
            }
            catch (Exception ex)
            {
                return ("Error HxD command: " + ex.ToString());
            }
        }
    }
}
