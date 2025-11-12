using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RaiSam.Models
{
    public class OBJ_Permission
    {
        public int fldID { get; set; }
        public int fldUserGroupID { get; set; }
        public string fldApplicationPartID { get; set; }
        public int fldInputID { get; set; }
        public string fldDesc { get; set; }
        public short fldTimeLimit { get; set; }
    }
}