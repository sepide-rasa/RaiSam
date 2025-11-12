using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RaiSam.Models
{
    public class ApplicationPart
    {
        public int fldID { get; set; }
        public string fldTitle { get; set; }
        public Nullable<int> fldPID { get; set; }
        public Nullable<byte> fldUserType { get; set; }
        public Nullable<bool> fldTimeLimit { get; set; }
        public bool fldIsLimit { get; set; }
        public string fldTime { get; set; }
        public short fldMinute { get; set; }
    }
}