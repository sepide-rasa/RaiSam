using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RaiSam.Models
{
    public class FinancialValue
    {
        public IEnumerable<Models.prs_tblFinancialValue_HeaderSelect> Header { get; set; }
        public IEnumerable<Models.prs_tblFinancialValue_DetailSelect> Detail { get; set; }
    }
}