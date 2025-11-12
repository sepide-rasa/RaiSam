using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RaiSam.Models
{
    public class Limitation
    {
        public IEnumerable<RaiSam.Models.prs_tblLimitationTimeSelect> LimitTime { get; set; }
        public IEnumerable<RaiSam.Models.prs_tblLimitationMacAddressSelect> LimitMac { get; set; }
        public IEnumerable<RaiSam.Models.prs_tblLimitationIPSelect> LimitIP { get; set; }
    }
}