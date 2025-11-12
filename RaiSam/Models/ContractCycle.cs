using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RaiSam.Models
{
    public class ContractCycle
    {
        public int fldIdVorood { get; set; }
        public int fldContractId { get; set; }
        public int fldCharkheId { get; set; }
        public Nullable<int> DefaultOperationId { get; set; }
        public Nullable<int> DefaultOperation_CharkheEghdam { get; set; }
        public string fldNameCharkhe { get; set; }
        public int fldActionId { get; set; }
        public string fldTarikhVorood { get; set; }
        public Nullable<int> fldCount { get; set; }
        public string fldNameAction { get; set; }
        public string fldTitleHadaf { get; set; }
        public Nullable<int> fldRequestId { get; set; }
        public string fldTypeVagonName { get; set; }
        public string fldTitleContract { get; set; }
        public int fldTedad { get; set; }
        
        public string fldFullName { get; set; }
        public string fldTarikhDarkhast { get; set; }
        public Nullable<int> fldHadafId { get; set; }
    }
}