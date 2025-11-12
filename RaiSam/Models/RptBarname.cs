using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RaiSam.Models
{
    public class RptBarname
    {
        public IEnumerable<Models.prs_tblMalek_VagonInfoSelect> Malek { get; set; }
        public IEnumerable<Models.prs_tblContract_ProjectSelect> Contract { get; set; }
        public IEnumerable<Models.prs_SelectVagonForRptBarname> vagon { get; set; }
        public IEnumerable<Models.prs_SelectContractDetailLastVagon> Detail { get; set; }
        public IEnumerable<Models.prs_RptKoliBarname> RptKoli { get; set; }
        public IEnumerable<Models.prs_SelectBarnameDetail> RptKoliDetail { get; set; }
        public IEnumerable<Models.prs_RptAmalKardKoliMosaferi> RptKoliMosaferi { get; set; }
        public IEnumerable<Models.prs_SelectMosaferiDetail> RptKoliDetailMosaferi { get; set; }
        public IEnumerable<Models.prs_tblContract_StationSelect> Khat { get; set; }
    }
}