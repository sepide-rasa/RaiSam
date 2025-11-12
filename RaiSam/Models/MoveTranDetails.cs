using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RaiSam.Models
{
    public class MoveTranDetails
    {
        public class Data
        {
            public int MoveTranId { get; set; }
            public string Company { get; set; }
            public string TranCompany { get; set; }
            public int TranNo { get; set; }
            public string MoveDate { get; set; }
            public string MoveTime { get; set; }
            public string SourceStation { get; set; }
            public string TargetStation { get; set; }
            public int PelakNo { get; set; }
            public int SalonNo { get; set; }
            public string WagonRating { get; set; }
        }

        public List<Data> data = new List<Data>();
        public bool isSuccess { get; set; }
        public int statusCode { get; set; }
        public string message { get; set; }

        public int Err { get; set; }
        public string Msg { get; set; }
        
    }
}