using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RaiSam.Models
{
    public class OutputDto_Address
    {
        public int Err { get; set; }
        public string Message { get; set; }
        public string type { get; set; }
        public string exceptionCode { get; set; }
        public string exceptionMessage { get; set; }
        public string conversationId { get; set; }
        public string payloadClass { get; set; }
        public string houseNo { get; set; }
        public string floorNo { get; set; }
        public string location { get; set; }
        public string locationType { get; set; }
        public string parish { get; set; }
        public string postCode { get; set; }
        public string preAvenue { get; set; }
        public string sideFloor { get; set; }
        public string state { get; set; }
        public string townShip { get; set; }
        public string village { get; set; }
        public string zone { get; set; }
        public string buildingName { get; set; }
        public string description { get; set; }
        public string locationCode { get; set; }
    }
}