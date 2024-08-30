using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTA_Mobile_Forensic.Model
{
    internal class SmsIOS
    {
        public string text { get; set; }
        public string service { get; set; }
        public string destinationcaller { get; set; }
        public string date { get; set; }
        public string dateread { get; set; }
        public string handle { get; set; }
    }
}
