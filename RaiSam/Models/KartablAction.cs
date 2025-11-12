using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RaiSam.Models
{
    public class KartablAction
    {
        public IEnumerable<RaiSam.Models.prs_tblCharkhe_EghdamSelect> Charkhe_Eghdam { get; set; }
        public IEnumerable<RaiSam.Models.prs_tblKartablSelect> Kartabl { get; set; }
        public IEnumerable<RaiSam.Models.prs_tblEkhtesasActions_CartableSelect> EkhtesasActionsns { get; set; }
    }
}