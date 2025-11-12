using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RaiSam.Models
{
    public class Monitorring
    {
        public IEnumerable<Models.prs_HadafListForFirstRegister> Hadaf { get; set; }
        public IEnumerable<Models.prs_tblMalek_VagonInfoSelect> Malek { get; set; }
    }
}