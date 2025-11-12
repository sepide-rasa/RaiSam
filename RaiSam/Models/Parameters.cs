using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RaiSam.Models
{
    public class Parameters
    {
        public IEnumerable<RaiSam.Models.prs_tblDegreeOfEducationSelect> DegreeEducation { get; set; }
        public IEnumerable<RaiSam.Models.prs_tblAvamelMoasser_UpgradeSelect> AvamelMoasser { get; set; }
    }
}