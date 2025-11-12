using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RaiSam.Models
{
    public class UserLoginCount
    {
        public static List<LogOnUsers> userObj = new List<LogOnUsers>();

        public static void AddOnlineUser(string UserIdd, string NameFamily, string Ip, string UserNamee, string AkharinOn, string strSessionId, string DescPost, string NahiName)
        {
            LogOnUsers user = new LogOnUsers();
            if (UserLoginCount.userObj.Find(k => k.userId == UserIdd) != null)
            {
                var userRemove = (LogOnUsers)userObj.Where(item => item.userId == UserIdd).FirstOrDefault();
                userObj.Remove(userRemove);
            }
            user.userId = UserIdd;
            user.IPAdress = Ip;
            user.NameFamily = NameFamily;
            user.UserName = UserNamee;
            user.newStatus = true;
            user.AkharinOn = AkharinOn;
            user.AkharinOnMiladi = MiladiDate.GetMiladiDate(AkharinOn);
            user.sessionId = strSessionId;
            user.NahiName = NahiName;
            user.DescPost = DescPost;
            userObj.Add(user);
        }

        public static void RemoveOnlineUser(string strUserId)
        {
            var userRemove = (LogOnUsers)userObj.Where(item => item.userId == strUserId).FirstOrDefault();
            userObj.Remove(userRemove);
        }
    }
}