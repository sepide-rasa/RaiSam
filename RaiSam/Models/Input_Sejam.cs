using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RaiSam.Models
{
    public class Input_Sejam
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public int? MoveTranId { get; set; }
        public int? CompanyId { get; set; }
    }
}