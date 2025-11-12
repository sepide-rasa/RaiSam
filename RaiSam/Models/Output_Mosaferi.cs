using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RaiSam.Models
{
    public class Output_Mosaferi
    {
        
        public class Data
        {
            public string uicWagonNo { get; set; }
            public int trainNumber { get; set; }
            public string trainDate { get; set; }
            public string moveTime { get; set; }
            public string persianTrainDate { get; set; }
            public int wagonNumber { get; set; }
            public int compartmentNumber { get; set; }
            public int seatNumber { get; set; }
            public int ticketNumber { get; set; }
            public decimal seirStartStationCode { get; set; }
            public string seirStartStation { get; set; }
            public decimal seirEndStationCode { get; set; }
            public string seirEndStation { get; set; }
            public string nameCompany { get; set; }
            public decimal totalDistance { get; set; }

            

        }

        public List<Data> data = new List<Data>();
        public bool isSuccess { get; set; }
        public string message { get; set; }
        public int statusCode { get; set; }

        public int Err { get; set; }
        public string Msg { get; set; }
    }
}