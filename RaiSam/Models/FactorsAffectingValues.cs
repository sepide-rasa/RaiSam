using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RaiSam.Models
{
    public class FactorsAffectingValues
    {
        public IEnumerable<RaiSam.Models.prs_tblFactorsAffectingValues_HeaderSelect> FactorsAffectingValues_Header { get; set; }
        public IEnumerable<RaiSam.Models.prs_tblFactorsAffectingValues_DetailSelect> FactorsAffectingValues_Details { get; set; }
    }
}