using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RaiSam.Models
{
    public class LogOnUsers
    {
        public string IPAdress { get; set; }
        public string UserName { get; set; }
        public string userId { get; set; }
        public string NameFamily { get; set; }
        public bool newStatus { get; set; }
        public string AkharinOn { get; set; }
        public DateTime AkharinOnMiladi { get; set; }
        public string sessionId { get; set; }
        public string DescPost { get; set; }
        public string NahiName { get; set; }
    }

    public class MiladiDate
    {
        public static DateTime GetMiladiDate(string tarikh_saat)
        {
            var Second = 0;
            var MiladiString = MyLib.Shamsi.Shamsi2miladiString(tarikh_saat.Split(' ')[0]);
            var year = Convert.ToInt32(MiladiString.Substring(0, 4));
            var month = Convert.ToInt32(MiladiString.Substring(5, 2));
            var day = Convert.ToInt32(MiladiString.Substring(8, 2));
            var hour = Convert.ToInt32(tarikh_saat.Split(' ')[1].Split(':')[0]);
            var minute = Convert.ToInt32(tarikh_saat.Split(' ')[1].Split(':')[1]);
            if (tarikh_saat.Split(' ')[1].Split(':').Count() > 2)
            {
                Second = Convert.ToInt32(tarikh_saat.Split(' ')[1].Split(':')[2]);
            }
            return new DateTime(year, month, day, hour, minute, Second);
        }
    }
}