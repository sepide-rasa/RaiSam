using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RaiSam.Models
{
    public class Permission
    {
        public static bool haveAccess(int RolId, string nameTable, string IdRecord)
        {
            Models.RaiSamEntities m = new RaiSamEntities();
            UserInfo user = (RaiSam.Models.UserInfo)(HttpContext.Current.Session["User"]);
            string IP = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_HOST"];
            var q = m.prs_User_PermissionAppIdSelect("HaveAcces", RolId.ToString(), user.UserSecondId, user.UserSecondId, nameTable, IdRecord).Any();
            return q;
        }
        public static bool haveAccessClient(int HadafId, string NameTable)
        {
            Models.RaiSamEntities m = new RaiSamEntities();
            string IP = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_HOST"];
            UserInfo user = (RaiSam.Models.UserInfo)(HttpContext.Current.Session["User"]);
            var q = m.prs_tblPermission_ClientSelect("HaveAccess_NameTable", NameTable/*RolId.ToString()*/, HadafId, "", 0).Any();
            return q;
        }
    }
}