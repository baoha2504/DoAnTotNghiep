using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTA_Mobile_Forensic.Model
{
    internal class LoginHistory_TrinhSat
    {
        public int login_history_id { get; set; }
        public DateTime date_time { get; set; }
        public string device_serial { get; set; }
        public string device_name { get; set; }
        public string pincode { get; set; }
        public int account_id { get; set; }
    }
}
