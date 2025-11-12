using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RaiSam.Models
{
    public class Contract_Project
    {
        public IEnumerable<Models.prs_tblContract_ProjectSelect> Project { get; set; }
        public IEnumerable<Models.prs_SelectContract_Project_Detail> Detail { get; set; }
    }
}