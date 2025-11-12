using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RaiSam.Models
{
    public class ProjectDetails
    {
        public IEnumerable<RaiSam.Models.prs_tblRegistrationFirstContract_HaghighiSelect> Namayande { get; set; }
        public IEnumerable<RaiSam.Models.prs_tblRegistrationFirstContract_HoghoghiSelect> Darkhast { get; set; }
    }
}