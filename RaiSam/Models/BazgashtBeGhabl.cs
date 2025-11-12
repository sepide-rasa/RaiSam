using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RaiSam.Models
{
    public class BazgashtBeGhabl
    {
        public IEnumerable<RaiSam.Models.prs_tblRegistrationFirstContractSelect> Contract { get; set; }
        public IEnumerable<RaiSam.Models.prs_SelectRequest_Action> Request_Action { get; set; }
    }
}