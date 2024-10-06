using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTA_Mobile_Forensic.Model
{
    internal class CallRecord
    {
        public int Row { get; set; }
        public string Number { get; set; }
        public string Type { get; set; }
        public string Date { get; set; }
        public string Duration { get; set; }
    }
}
