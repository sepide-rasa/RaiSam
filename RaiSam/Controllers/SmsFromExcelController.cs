using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ext.Net;
using Ext.Net.MVC;
using Ext.Net.Utilities;
using System.IO;
using System.Text.RegularExpressions;
using RaiSam.Models;

namespace RaiSam.Controllers
{
    public class SmsFromExcelController : Controller
    {
        //
        // GET: /SmsFromExcel/
        UserInfo u = (UserInfo)System.Web.HttpContext.Current.Session["User"];
        string IP = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_HOST"];

        public ActionResult Index()
        {//باز شدن تب جدید
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            ViewData.Model = new ListSms();
            Ext.Net.MVC.PartialViewResult partialView = new Ext.Net.MVC.PartialViewResult
            {
                ViewData = this.ViewData
            };
            return partialView;
        }
        public ActionResult Upload()
        {
            string Msg = "";
            try
            {
                if (Session["savePath"] != null)
                {
                    string physicalPath = System.IO.Path.Combine(Session["savePath"].ToString());
                    Session.Remove("savePath");
                    Session.Remove("FileName");
                    System.IO.File.Delete(physicalPath);
                }
                HttpPostedFileBase file = Request.Files[0];
                string e = Path.GetExtension(file.FileName);

                if (e.ToLower() == ".csv")
                {
                    string savePath = Server.MapPath(@"~\Uploaded\" + file.FileName);
                    file.SaveAs(savePath);
                    Session["FileName"] = file.FileName;
                    Session["savePath"] = savePath;
                    object r = new
                    {
                        success = true,
                        name = Request.Files[0].FileName
                    };
                    return Content(string.Format("<textarea>{0}</textarea>", JSON.Serialize(r)));

                }
                else
                {
                    X.Msg.Show(new MessageBoxConfig
                    {
                        Buttons = MessageBox.Button.OK,
                        Icon = MessageBox.Icon.ERROR,
                        Title = "خطا",
                        Message = "فایل انتخاب شده غیر مجاز است."
                    });
                    DirectResult result = new DirectResult();
                    return result;
                }
            }
            catch (Exception x)
            {
                if (x.InnerException != null)
                    Msg = x.InnerException.Message;
                else
                    Msg = x.Message;

                X.Msg.Show(new MessageBoxConfig
                {
                    Buttons = MessageBox.Button.OK,
                    Icon = MessageBox.Icon.ERROR,
                    Title = "خطا",
                    Message = Msg
                });
                DirectResult result = new DirectResult();
                return result;
            }
        }
        public ActionResult Upload2()
        {
            string Msg = "";
            try
            {
                if (Session["savePath"] != null)
                {
                    string physicalPath = System.IO.Path.Combine(Session["savePath"].ToString());
                    Session.Remove("savePath");
                    Session.Remove("FileName");
                    System.IO.File.Delete(physicalPath);
                }
                HttpPostedFileBase file = Request.Files[1];
                string e = Path.GetExtension(file.FileName);

                //if (e.ToLower() == ".xls" || e.ToLower() == ".xlsx")
                //   {
                if (e.ToLower() == ".txt")
                {
                    string savePath = Server.MapPath(@"~\Uploaded\" + file.FileName);
                    file.SaveAs(savePath);
                    Session["FileName"] = file.FileName;
                    Session["savePath"] = savePath;
                    object r = new
                    {
                        success = true,
                        name = Request.Files[1].FileName
                    };
                    return Content(string.Format("<textarea>{0}</textarea>", JSON.Serialize(r)));

                }
                else
                {
                    X.Msg.Show(new MessageBoxConfig
                    {
                        Buttons = MessageBox.Button.OK,
                        Icon = MessageBox.Icon.ERROR,
                        Title = "خطا",
                        Message = "فایل انتخاب شده غیر مجاز است."
                    });
                    DirectResult result = new DirectResult();
                    return result;
                }
            }
            catch (Exception x)
            {
                if (x.InnerException != null)
                    Msg = x.InnerException.Message;
                else
                    Msg = x.Message;

                X.Msg.Show(new MessageBoxConfig
                {
                    Buttons = MessageBox.Button.OK,
                    Icon = MessageBox.Icon.ERROR,
                    Title = "خطا",
                    Message = Msg
                });
                DirectResult result = new DirectResult();
                return result;
            }
        }
        public ActionResult Upload3()
        {
            string Msg = "";
            try
            {
                if (Session["savePath"] != null)
                {
                    string physicalPath = System.IO.Path.Combine(Session["savePath"].ToString());
                    Session.Remove("savePath");
                    Session.Remove("FileName");
                    System.IO.File.Delete(physicalPath);
                }
                HttpPostedFileBase file = Request.Files[2];
                string e = Path.GetExtension(file.FileName);

                //if (e.ToLower() == ".xls" || e.ToLower() == ".xlsx")
                //   {
                if (e.ToLower() == ".csv")
                {
                    string savePath = Server.MapPath(@"~\Uploaded\" + file.FileName);
                    file.SaveAs(savePath);
                    Session["FileName"] = file.FileName;
                    Session["savePath"] = savePath;
                    object r = new
                    {
                        success = true,
                        name = Request.Files[2].FileName
                    };
                    return Content(string.Format("<textarea>{0}</textarea>", JSON.Serialize(r)));

                }
                else
                {
                    X.Msg.Show(new MessageBoxConfig
                    {
                        Buttons = MessageBox.Button.OK,
                        Icon = MessageBox.Icon.ERROR,
                        Title = "خطا",
                        Message = "فایل انتخاب شده غیر مجاز است."
                    });
                    DirectResult result = new DirectResult();
                    return result;
                }
            }
            catch (Exception x)
            {
                if (x.InnerException != null)
                    Msg = x.InnerException.Message;
                else
                    Msg = x.Message;

                X.Msg.Show(new MessageBoxConfig
                {
                    Buttons = MessageBox.Button.OK,
                    Icon = MessageBox.Icon.ERROR,
                    Title = "خطا",
                    Message = Msg
                });
                DirectResult result = new DirectResult();
                return result;
            }
        }
        public ActionResult ReloadCompany()
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Models.RaiSamEntities m = new RaiSamEntities();

            List<prs_ListCompany> groups = new List<prs_ListCompany>();
            prs_ListCompany V = new prs_ListCompany();

            List<string> ReadCSV = System.IO.File.ReadAllLines(Session["savePath"].ToString()).ToList();

            foreach (string csvRow in ReadCSV)
            {
                if (!string.IsNullOrEmpty(csvRow))
                {
                    foreach (string FileRec in csvRow.Split(','))
                    {
                        var FileRec1 = FileRec.Trim('"');

                        var k = m.prs_ListCompany("fldFirstRegisterUser", FileRec1.ToString(), 0, "","","").FirstOrDefault();
                        if (k != null)
                        {
                            V = new prs_ListCompany();
                            V.fldMobile = k.fldMobile;
                            V.fldMobileModirAmel = k.fldMobileModirAmel;
                            V.fldFullName = k.fldFullName;
                            V.fldName_Family = k.fldName_Family;
                            V.NameModirAmel = k.NameModirAmel;
                            V.fldFirstRegisterUser = k.fldFirstRegisterUser;
                            V.fldEmail = k.fldEmail;
                            V.fldEmailCompany = k.fldEmailCompany;
                            groups.Add(V);
                        }
                    }
                }
            }

            /*  if (Session["savePath"] != null)
                 {
                     System.Data.OleDb.OleDbConnection oconn = new System.Data.OleDb.OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Session["savePath"].ToString() + ";Extended Properties=\"Excel 8.0;HDR=YES\";");
                     oconn.Open();
                    
                         if (oconn.State == System.Data.ConnectionState.Closed)
                             oconn.Open();                 
                                     
                     try{
                         System.Data.OleDb.OleDbCommand ocmd = new System.Data.OleDb.OleDbCommand("select * from [Sheet1$]", oconn); 
                         System.Data.OleDb.OleDbDataReader odr = ocmd.ExecuteReader();

                         if (odr.HasRows)
                         {
                             while (odr.Read())
                             {
                                 var k = servic.GetListCompanyWithFilter("fldFirstRegisterUser", odr["FirstRegisterId"].ToString(), 0, "", "", "", Session["Username"].ToString(), Session["Password"].ToString(), out Err).FirstOrDefault();
                                 if (k != null)
                                 {
                                     V = new RaiTrainAdminWS.OBJ_ListCompany();
                                     V.fldMobile = k.fldMobile;
                                     V.fldMobileModirAmel = k.fldMobileModirAmel;
                                     V.fldFullName = k.fldFullName;
                                     V.fldName_Family = k.fldName_Family;
                                     V.NameModirAmel = k.NameModirAmel;
                                     V.fldFirstRegisterUser = k.fldFirstRegisterUser;
                                     V.fldEmail = k.fldEmail;
                                     V.fldEmailCompany = k.fldEmailCompany;
                                     groups.Add(V);
                                 }
                             }
                         }
                     //fstream.Close();
                     oconn.Close();
                     }
                     catch (Exception ex)
                     {
                         X.Msg.Show(new MessageBoxConfig
                         {
                             Buttons = MessageBox.Button.OK,
                             Icon = MessageBox.Icon.ERROR,
                             Title = "خطا",
                             Message = ex.Message
                         });
                         DirectResult result = new DirectResult();
                         return result;
                     }
                 }*/
            if (Session["savePath"] != null)
            {
                string physicalPath = System.IO.Path.Combine(Session["savePath"].ToString());
                Session.Remove("savePath");
                Session.Remove("FileName");
                System.IO.File.Delete(physicalPath);
            }

            return Json(groups.ToList(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult ReloadTel()
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });

            List<prs_tblSMSEmailCachSelect> groups = new List<prs_tblSMSEmailCachSelect>();
            prs_tblSMSEmailCachSelect V = new prs_tblSMSEmailCachSelect();

            List<string> ReadCSV = System.IO.File.ReadAllLines(Session["savePath"].ToString()).ToList();

            foreach (string csvRow in ReadCSV)
            {
                if (!string.IsNullOrEmpty(csvRow))
                {
                    var z = csvRow.Split(',');
                    //var FileRec1 = FileRec.Trim('"');

                    V = new prs_tblSMSEmailCachSelect();
                    V.fldNameShakhs = z[0];
                    V.fldShMobile = z[1];
                    V.fldMatn = z[2];
                    groups.Add(V);
                }
            }
            /*

            if (Session["savePath"] != null)
            {
                System.Data.OleDb.OleDbConnection oconn = new System.Data.OleDb.OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Session["savePath"].ToString() + ";Extended Properties=\"Excel 8.0;HDR=YES\";");
                oconn.Open();

                if (oconn.State == System.Data.ConnectionState.Closed)
                    oconn.Open();

                try
                {
                    System.Data.OleDb.OleDbCommand ocmd = new System.Data.OleDb.OleDbCommand("select * from [Sheet1$]", oconn);
                    System.Data.OleDb.OleDbDataReader odr = ocmd.ExecuteReader();

                    if (odr.HasRows)
                    {
                        while (odr.Read())
                        {
                               V = new RaiTrainAdminWS.OBJ_SMSEmailCach();
                            V.fldNameShakhs = odr["NameShakhs"].ToString();
                            V.fldShMobile = odr["ShMobile"].ToString();
                            V.fldMatn =odr["Matn"].ToString();
                            groups.Add(V);
                        }
                    }
                    //fstream.Close();
                    oconn.Close();
                }
                catch (Exception ex)
                {
                    X.Msg.Show(new MessageBoxConfig
                    {
                        Buttons = MessageBox.Button.OK,
                        Icon = MessageBox.Icon.ERROR,
                        Title = "خطا",
                        Message = ex.Message
                    });
                    DirectResult result = new DirectResult();
                    return result;
                }
            }*/

            if (Session["savePath"] != null)
            {
                string physicalPath = System.IO.Path.Combine(Session["savePath"].ToString());
                Session.Remove("savePath");
                Session.Remove("FileName");
                System.IO.File.Delete(physicalPath);
            }

            return Json(groups.ToList(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult ReloadTelCompany()
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });

            List<prs_tblSMSEmailCachSelect> groups = new List<prs_tblSMSEmailCachSelect>();
            prs_tblSMSEmailCachSelect V = new prs_tblSMSEmailCachSelect();

            List<string> ReadCSV = System.IO.File.ReadAllLines(Session["savePath"].ToString()).ToList();

            foreach (string csvRow in ReadCSV)
            {
                if (!string.IsNullOrEmpty(csvRow))
                {
                    //foreach (string FileRec in csvRow.Split(','))
                    //{
                    var FileRec = csvRow.Split(',');
                    //var FileRec1 = FileRec.Trim('"');

                    V = new prs_tblSMSEmailCachSelect();
                    V.fldNameShakhs = FileRec[0];
                    V.fldShMobile = FileRec[1];
                    groups.Add(V);
                    //}
                }
            }
            if (Session["savePath"] != null)
            {
                string physicalPath = System.IO.Path.Combine(Session["savePath"].ToString());
                Session.Remove("savePath");
                Session.Remove("FileName");
                System.IO.File.Delete(physicalPath);
            }

            return Json(groups.ToList(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult Save(List<prs_tblSMSEmailCachSelect> SmsArr)
        {
            string Msg = "", MsgTitle = ""; var Er = 0;
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Models.RaiSamEntities m = new RaiSamEntities();
            try
            {
                var haveSmsPanel = m.prs_tblSMSSettingSelect("", "", 1).FirstOrDefault();
                RaiSms.Service w = new RaiSms.Service();
                //SMSPanel.RasaSMSPanel_Send Sms = new SMSPanel.RasaSMSPanel_Send();  

                foreach (var item in SmsArr)
                {
                    Msg = "ارسال موفق";
                    var st = true;
                    item.fldMatn = item.fldMatn.Replace("<div>", Environment.NewLine);
                    var Matn = Regex.Replace(item.fldMatn, "<.*?>", String.Empty);
                    try
                    {
                        var returnCode = w.SendSms(haveSmsPanel.fldUserName, haveSmsPanel.fldPassword, Matn, item.fldShMobile, "0", 1, 2, null, "RailWay", null, 0, 0, "", "");

                        if (returnCode.Length < 3)
                        {
                            MsgTitle = "خطا";
                            Msg = w.ShowError(returnCode, "FA");
                            Er = 1;
                            st = false;
                        }
                        //var k = Sms.SendMessage("azad_shahrood", "azad_shahrood",new string[] { item.fldShMobile }, Matn, 1,"0");
                    }
                    catch (Exception)
                    {
                        Msg = "قطع ارتباط با سرور.";
                        st = false;
                    }

                    m.prs_tblSMSEmailCachInsert(Matn, item.fldShMobile, "", Msg, item.fldNameShakhs, false, false,  null,st, u.UserInputId);
                }


            }
            catch (Exception x)
            {
                if (x.InnerException != null)
                    Msg = x.InnerException.Message;
                else
                    Msg = x.Message;

                MsgTitle = "خطا";
                Er = 1;
            }
            return Json(new
            {
                Msg = Msg,
                MsgTitle = MsgTitle,
                Err = Er
            }, JsonRequestBehavior.AllowGet);
        }

    }
}
