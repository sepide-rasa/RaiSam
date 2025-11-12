using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RaiSam.Models
{
    public class ListSms
    {
        public IEnumerable<RaiSam.Models.prs_ListCompany> ListCompany { get; set; }
        public IEnumerable<RaiSam.Models.prs_tblSMSEmailCachSelect> SMSEmailCach { get; set; }
    }
}