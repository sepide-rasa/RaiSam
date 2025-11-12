using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RaiSam.Models
{
    public class CharkheAction
    {
        public IEnumerable<RaiSam.Models.prs_tblActionsSelect> Actions { get; set; }
        public IEnumerable<RaiSam.Models.prs_tblCharkheSelect> Charkhe { get; set; }
        public IEnumerable<RaiSam.Models.prs_tblCharkhe_EghdamSelect> Charkhe_Eghdam { get; set; }
        public IEnumerable<RaiSam.Models.prs_tblOpertion_ActionSelect> Opertion_Action { get; set; }
    }
}