using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTA_Mobile_Forensic.Support
{
    internal class abe
    {
        public string abeCommand(string command)
        {
            try
            {
                Process abeProcess = new Process();
                abeProcess.StartInfo.FileName = "java";
                abeProcess.StartInfo.Arguments = $"-jar \"C:\\tools\\abe\\abe.jar\" {command}";
                abeProcess.StartInfo.UseShellExecute = false;
                abeProcess.StartInfo.RedirectStandardOutput = true;
                abeProcess.StartInfo.CreateNoWindow = true; // Hide the console window (use with caution)
                abeProcess.StartInfo.StandardOutputEncoding = System.Text.Encoding.UTF8; // Thiết lập mã hóa UTF-8 cho đầu ra
                abeProcess.Start();

                string output = abeProcess.StandardOutput.ReadToEnd();
                abeProcess.WaitForExit();

                return output;
            }
            catch (Exception ex)
            {
                return ("Error abe command: " + ex.ToString());
            }
        }
    }
}
