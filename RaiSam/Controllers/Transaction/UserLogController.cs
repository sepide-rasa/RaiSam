using RaiSam.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ext.Net;
using Ext.Net.MVC;
using Ext.Net.Utilities;
using RaiSam.Models;
using System.IO;
using Newtonsoft.Json;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace RaiSam.Controllers.Transaction
{
    public class UserLogController : Controller
    {
        //
        // GET: /UserLog/

        string IP = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_HOST"];
        public ActionResult Index(string containerId)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            UserInfo user = (UserInfo)(Session["User"]);
            var result = new Ext.Net.MVC.PartialViewResult
            {
                WrapByScriptTag = true,
                ContainerId = containerId,
                RenderMode = RenderMode.AddTo
            };
            this.GetCmp<TabPanel>(containerId).SetLastTabAsActive();
            Models.RaiSamEntities m = new RaiSamEntities();
            var CurrentDate = m.prs_GetDate().FirstOrDefault().fldTarikh;
            var fromDate = MyLib.Shamsi.Miladi2ShamsiString(m.prs_GetDate().FirstOrDefault().fldDateTime.AddDays(-2));
            result.ViewBag.CurrentDate = CurrentDate;
            result.ViewBag.fromDate = fromDate;
            return result;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetTransactionGroup()
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
                UserInfo user = (UserInfo)(Session["User"]);
                Models.RaiSamEntities m = new RaiSamEntities();
                var q = m.prs_tblTransactionGroupSelect("","",0).Select(c => new { fldId = c.fldId, fldName = c.fldName });
                return this.Store(q);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetTransactionType(int TransactionGroupId)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
                UserInfo user = (UserInfo)(Session["User"]);
                Models.RaiSamEntities m = new RaiSamEntities();
                var q = m.prs_tblTransactionTypeSelect("fldTransactionGroupId", TransactionGroupId.ToString(),0).ToList().Select(c => new { fldId = c.fldId, fldName = c.fldName });
                return this.Store(q);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetTableNames()
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
                UserInfo user = (UserInfo)(Session["User"]);
                Models.RaiSamEntities m = new RaiSamEntities();
                
                var q = m.prs_tblNameTablesSelect("","",0).Select(c => new { fldId = c.fldId, fldName = c.fldFaName, fldEnNameTables = c.fldEnNameTables });
                return this.Store(q);
            }
        }
        public ActionResult Help()
        {//باز شدن پنجره
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            else
            {
                Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
                return PartialView;
            }
        }
        public FileContentResult ShowHelpUserLog()
        {
            if (Session["User"] == null)
                return null;
            UserInfo user = (UserInfo)(Session["User"]);
            Models.RaiSamEntities m = new RaiSamEntities();
           
            var Help = m.prs_tblHelpSelect("fldId", "32",1).FirstOrDefault();
            var q = m.prs_tblFileSelect("fldId", Help.fldFilePdfId.ToString(),0).FirstOrDefault();
            return File((byte[])q.fldFile, "application/pdf");
        }
        public ActionResult VideoWinUserLog()
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
            return PartialView;
        }
        public FileContentResult DownloadFileVideoUserLog()
        {
            if (Session["User"] == null)
                return null;
            UserInfo user = (UserInfo)(Session["User"]);
            Models.RaiSamEntities m = new RaiSamEntities();
            var Help = m.prs_tblHelpSelect("fldId", "32", 1).FirstOrDefault();
            var q = m.prs_tblFileSelect("fldId", Help.fldFileVideoId.ToString(), 0).FirstOrDefault();
            MemoryStream st = new MemoryStream(q.fldFile);
            return File(st.ToArray(), MimeType.Get(q.fldPasvand), q.fldFileName + q.fldPasvand);
        }
        public ActionResult OldIndex(string containerId)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            UserInfo user = (UserInfo)(Session["User"]);
            var result = new Ext.Net.MVC.PartialViewResult
            {
                WrapByScriptTag = true,
                ContainerId = containerId,
                RenderMode = RenderMode.AddTo
            };
            this.GetCmp<TabPanel>(containerId).SetLastTabAsActive();
            Models.RaiSamEntities m = new RaiSamEntities();
            var CurrentDate = m.prs_GetDate().FirstOrDefault().fldTarikh;
            result.ViewBag.CurrentDate = CurrentDate;
            return result;
        }
        public ActionResult ShowJsonParam(string JsonParametr)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            var result = new Ext.Net.MVC.PartialViewResult();
            result.ViewBag.JsonParametr = JsonParametr;
            return result;
        }
        public ActionResult HistoryLogTable(int fldId, string tableName)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            var result = new Ext.Net.MVC.PartialViewResult();
            result.ViewBag.fldId = fldId;
            result.ViewBag.tableName = tableName;
            return result;
        }
        public ActionResult GetFieldFromJson(string JsonParametr)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            var ParamList = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(JsonParametr);
            var Keys = ParamList.Keys;
            return Json(new
            {
                Keys = Keys
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult LoadJsonData(string JsonParametr)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            var ParamList = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(JsonParametr);
            return this.Store(ParamList);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Search_Table(string NameTable, byte Type, string AzTarikh, string TaTarikh)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
                UserInfo user = (UserInfo)(Session["User"]);
                Models.RaiSamEntities m = new RaiSamEntities();
                List<string> ColName = new List<string>();
                //if (Permission.haveAccess(265))
                //{
                try
                {
                    string commandText = "declARE @NameTable varchar(100)='" + NameTable + "' ,@AzTarikh  varchar(10)='" + AzTarikh + "' ,@TaTarikh varchar(10)='" + TaTarikh + "' ,@Type tinyint="
                        + Type + " ,@Name Nvarchar(100)='', @Family Nvarchar(100)='', @CodeMeli varchar(20)='' EXEC prs_LogAllTable @NameTable,@AzTarikh,@TaTarikh,@Type,@Name,@Family,@CodeMeli";

                    string ConnectionString = ConfigurationManager.ConnectionStrings["RaiSamConnectionString"].ConnectionString;
                    SqlConnection con = new SqlConnection(ConnectionString);
                    con.Open();
                    SqlCommand com = new SqlCommand(commandText, con);
                    com.CommandTimeout = 200000;
                    var dr = com.ExecuteReader();
                    System.Data.DataTable dt = new System.Data.DataTable();
                    dt.Load(dr);
                    List<IDictionary<string, object>> Info = new List<IDictionary<string, object>>();
                    foreach (DataRow row in dt.Rows)
                    {
                        IDictionary<string, object> q = new Dictionary<string, object>();
                        foreach (DataColumn column in dt.Columns)
                        {
                            q[column.ColumnName] = row[column];
                        }
                        Info.Add(q);
                    }
                    foreach (DataColumn column in dt.Columns)
                    {
                        int value;
                        if (int.TryParse(column.ColumnName, out value))
                        {
                            
                            var st = m.prs_tblStationSelect("fldId", value.ToString(),user.UserInputId,1).FirstOrDefault();
                            ColName.Add(st.fldName);
                        }
                        else
                        {
                            ColName.Add(column.ColumnName);
                        }
                    }
                    con.Close();
                    return new JsonResult()
                    {
                        Data = new
                        {
                            Er = 0,
                            Info = Info,
                            ColName = ColName
                        },
                        MaxJsonLength = Int32.MaxValue,
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    };
                }
                catch (Exception x)
                {
                    var ErMsg = "";
                    if (x.InnerException != null)
                        ErMsg = x.InnerException.Message;
                    else
                        ErMsg = x.Message;
                    System.Data.Entity.Core.Objects.ObjectParameter ErrorId = new System.Data.Entity.Core.Objects.ObjectParameter("fldID", typeof(int));
                    var Input = m.prs_tblInputInfoSelect("CheckUser", user.UserKey, IP, false, 0).FirstOrDefault();


                    m.prs_tblErrorInsert(ErrorId, ErMsg, DateTime.Now, Input.fldId, "");

                    return Json(new
                    {
                        MsgTitle = "خطا",
                        Msg = "خطایی با شماره: " + ErrorId.Value + " رخ داده است لطفا با پشتیبانی تماس گرفته و کد خطا را اعلام فرمایید.",
                        Er = 1
                    });
                }
            }
            //}
            //else
            //{
            //    return RedirectToAction("Error", "Home", new { Code = "403" });
            //}
        }
        public ActionResult GetHistoryRec(string tableName, int fldId)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            UserInfo user = (UserInfo)(Session["User"]);
            List<string> ColName = new List<string>();
            Models.RaiSamEntities m = new RaiSamEntities();
            //if (Permission.haveAccess(265))
            //{
            try
            {
                string commandText = "declARE @NameTable varchar(100)='" + tableName + "' ,@Id  int=" + fldId + " EXEC prs_LogAllTable_Record @NameTable,@Id";
                string ConnectionString = ConfigurationManager.ConnectionStrings["RaiSamConnectionString"].ConnectionString;
                SqlConnection con = new SqlConnection(ConnectionString);
                con.Open();
                SqlCommand com = new SqlCommand(commandText, con);
                com.CommandTimeout = 200000;
                var dr = com.ExecuteReader();
                System.Data.DataTable dt = new System.Data.DataTable();
                dt.Load(dr);
                List<IDictionary<string, object>> Info = new List<IDictionary<string, object>>();
                foreach (DataRow row in dt.Rows)
                {
                    IDictionary<string, object> q = new Dictionary<string, object>();
                    foreach (DataColumn column in dt.Columns)
                    {
                        q[column.ColumnName] = row[column];
                    }
                    Info.Add(q);
                }
                foreach (DataColumn column in dt.Columns)
                {
                    ColName.Add(column.ColumnName);
                }
                con.Close();
                return new JsonResult()
                {
                    Data = new
                    {
                        Er = 0,
                        Info = Info,
                        ColName = ColName
                    },
                    MaxJsonLength = Int32.MaxValue,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            catch (Exception x)
            {
                var ErMsg = "";
                if (x.InnerException != null)
                    ErMsg = x.InnerException.Message;
                else
                    ErMsg = x.Message;
                System.Data.Entity.Core.Objects.ObjectParameter ErrorId = new System.Data.Entity.Core.Objects.ObjectParameter("fldID", typeof(int));
                var Input = m.prs_tblInputInfoSelect("CheckUser", user.UserKey, IP, false, 0).FirstOrDefault();


                m.prs_tblErrorInsert(ErrorId, ErMsg, DateTime.Now, Input.fldId, "");

                return Json(new
                {
                    MsgTitle = "خطا",
                    Msg = "خطایی با شماره: " + ErrorId.Value + " رخ داده است لطفا با پشتیبانی تماس گرفته و کد خطا را اعلام فرمایید.",
                    Er = 1
                });
            }
            //}
            //else
            //{
            //    return RedirectToAction("Error", "Home", new { Code = "403" });
            //}
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Search(StoreRequestParameters parameters, string StartDate, string EndDate, string TransactionGroupId, string TransactionTypeId,
            string TableId, string Status, string ip, string Mac, int UserSearchableId, bool Sub)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
                UserInfo user = (UserInfo)(Session["User"]);
                Models.RaiSamEntities m = new RaiSamEntities();
                try
                {
                    var Substr = "0";
                    var IDTransactionType = "";
                    var nameTable="";
                    if (Sub == true)
                    {
                        Substr = "1";
                    }
                    else
                    {
                        IDTransactionType = TransactionTypeId;
                    }
                    if (TransactionTypeId == "null")
                    {
                        IDTransactionType = "";
                    }
                    else
                    {
                        IDTransactionType = TransactionTypeId;
                    }
                    if (TableId == "null")
                    {
                        nameTable= "";
                    }
                    else
                    {
                        nameTable = TableId;
                    }
                    List<Models.prs_UserLogSelect> ListUserLog = new List<prs_UserLogSelect>();
                    
                    var SearchableUser = "";
                    if (UserSearchableId != 0)
                    {
                        SearchableUser = UserSearchableId.ToString();
                    }
                    var aztarikh = "";
                    if (StartDate != "")
                    {
                        aztarikh = StartDate.Replace("/", "");
                    }
                    var tatarikh = "";
                    if (EndDate != "")
                    {
                        tatarikh = EndDate.Replace("/", "");
                    }
                    ListUserLog = m.prs_UserLogSelect(user.UserSecondId.ToString(), aztarikh, tatarikh, TransactionGroupId, IDTransactionType, Status, SearchableUser, Substr, Mac, ip, nameTable).ToList();

                    return new JsonResult()
                    {
                        Data = new
                        {
                            Er = 0,
                            ListUserLog = ListUserLog
                        },
                        MaxJsonLength = Int32.MaxValue,
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    };
                }
                catch (Exception x)
                {
                    var ErMsg = "";
                    if (x.InnerException != null)
                        ErMsg = x.InnerException.Message;
                    else
                        ErMsg = x.Message;
                    System.Data.Entity.Core.Objects.ObjectParameter ErrorId = new System.Data.Entity.Core.Objects.ObjectParameter("fldID", typeof(int));
                    var Input = m.prs_tblInputInfoSelect("CheckUser", user.UserKey, IP, false, 0).FirstOrDefault();


                    m.prs_tblErrorInsert(ErrorId, ErMsg, DateTime.Now, Input.fldId, "");

                    return Json(new
                    {
                        MsgTitle = "خطا",
                        Msg = "خطایی با شماره: " + ErrorId.Value + " رخ داده است لطفا با پشتیبانی تماس گرفته و کد خطا را اعلام فرمایید.",
                        Er = 1
                    });
                }
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult getUser_Info(string fldNationalCode)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
                UserInfo user = (UserInfo)(Session["User"]);
                Models.RaiSamEntities m = new RaiSamEntities();
                var StationName = "";
                var PostName = "";
                var Degree = "";

                var q = m.prs_tblAshkhasSelect("fldCodeMeli", fldNationalCode, "", "", "", 0).FirstOrDefault();
               
                var q2 = m.prs_tblPersonalInfoSelect("fldId", q.fldPersonalId.ToString(),"","",0,0).FirstOrDefault();
                if (q2 != null)
                {
                    StationName = q2.fldStationName;
                    PostName = q2.fldDescPost;
                    Degree = q2.fldDegreeOfEducationName;
                }
                var image = "IA==";
                if (q.fldFileId != null)
                {
                    var f = m.prs_tblFileSelect("fldId", q.fldFileId.ToString(),1).FirstOrDefault();
                    image = Convert.ToBase64String(f.fldFile);
                }
                if (image == "IA==" || image == "ICAgICAgIA==" || image == "ICA=" || image == "IAAgAA==")
                {
                    var file = System.IO.File.ReadAllBytes(AppDomain.CurrentDomain.BaseDirectory + @"\Content\icon\no-image.jpg");
                    image = Convert.ToBase64String(file);
                }
                var fldPost = q.fldPayeName != "" ? PostName + "(" + q.fldPayeName + ")" : PostName;
                return new JsonResult()
                {
                    Data = new
                    {
                        image = image,
                        fldMobile = q.fldMobile,
                        fldCodeEnhesari = q.fldCodeEnhesari,
                        fldPersoneli_No = q.Personeli_No,
                        fldFatherName = q.fldFatherName,
                        fldPost = q.fldPayeName != "" ? PostName + "(" + q.fldPayeName + ")" : PostName,
                        fldEmploymentStatusTitle = q.fldEmploymentStatusTitle,
                        fldDegreeOfEducationName = Degree,
                        fldTitel_MaleSazemani = StationName != "" ? q.fldTitel_MaleSazemani + "(" + StationName + ")" : StationName,
                        TypeEstekhdam = q.TypeEstekhdam
                    },
                    MaxJsonLength = Int32.MaxValue,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
        }
    }
}
