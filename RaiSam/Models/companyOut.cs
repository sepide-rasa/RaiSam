using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RaiSam.Models
{
    public class companyOut
    {
        public int Err { get; set; }
        public string Message { get; set; }
        public string Name { get; set; }
        public string NationalCode { get; set; }
        public string Address { get; set; }
        public string PostCode { get; set; }
        public string RegisterDate { get; set; }
        public string RegisterNumber { get; set; }
        public string LastChangeDate { get; set; }
        public string EstablishmentDate { get; set; }
        public string FollowUpNo { get; set; }
        public string State { get; set; }
        public string Successful { get; set; }
    }
}