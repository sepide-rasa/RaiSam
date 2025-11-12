using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RaiSam.Models
{
    public class SpecificCartablePermissionTree
    {
        public int fldNodeId { get; set; }
        public string fldNodeName { get; set; }
        public int fldCartablId { get; set; }
        public Nullable<byte> fldNodeType { get; set; }
        public Nullable<bool> fldAccess { get; set; }
        public bool fldEffective { get; set; }
        public bool fldStatus { get; set; }
        public bool fldShowSpecific { get; set; }
        public bool fldShowGeneral { get; set; }
    }
}