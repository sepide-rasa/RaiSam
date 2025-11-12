using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RaiSam.Models
{
    public class OutputDto_SabtAhvalSitad
    {
        public int Err { get; set; }
        public string Message { get; set; }
        public int BirthDate { get; set; }
        public int BookNo { get; set; }
        public int BookRow { get; set; }
        public int DeathStatus { get; set; }
        public string Family { get; set; }
        public string FatherName { get; set; }
        public int Gender { get; set; }
        public string exceptionMessage { get; set; }
        public string exceptionCode { get; set; }
        public string type { get; set; }
        public string Name { get; set; }
        public long NationalCode { get; set; }
        public int OfficeCode { get; set; }
        public string officeName { get; set; }
        public int ShenasnameNo { get; set; }
        public int shenasnameSerial { get; set; }
        public string shenasnameSeri { get; set; }
        public string deathDate { get; set; }
    }
}