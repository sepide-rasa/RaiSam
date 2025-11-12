using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RaiSam.Models
{
    public class UserInfo
    {
        public int UserId { get; set; }
        //public int TreeId { get; set; }
        public bool fldSetadi { get; set; }
        public int UserInputId { get; set; }
        public string UserKey { get; set; }
        public byte UserType { get; set; }
        public string UserName { get; set; }
        public int FileId { get; set; }
        public int UserSecondId { get; set; }
        public string UserKeyFirst { get; set; }
        public int FirstUserInputId { get; set; }
        public int FirstRegisterId { get; set; }
        public string Pass { get; set; }
    }
    public class MovazafInfo
    {
        public string CodeEnhesari { get; set; }
        public int PersonalInfoId { get; set; }
        public int AshkhasId { get; set; }
        public int PayeId { get; set; }
        public int PrsInputId { get; set; }
        public string UserKey { get; set; }
    }

    public class PesonInfo_Sabt
    {
        public string birthDate { get; set; }
        public string bookNo { get; set; }
        public string bookRow { get; set; }
        public string deathStatus { get; set; }
        public string family { get; set; }
        public string fatherName { get; set; }
        public string gender { get; set; }
        public string message { get; set; }
        public string name { get; set; }
        public string nationalCode { get; set; }
        public string officeCode { get; set; }
        public string shenasnameNo { get; set; }
        public string shenasnameSerial { get; set; }
        public string deathDate { get; set; }
    }
}