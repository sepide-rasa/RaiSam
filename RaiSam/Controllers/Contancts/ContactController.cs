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
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.Reflection;

namespace RaiSam.Controllers.Contancts
{
    public class ContactController : Controller
    {
        //
        // GET: /Contact/

        string IP = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_HOST"];
        public ActionResult Index(string containerId)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            var result = new Ext.Net.MVC.PartialViewResult
            {
                WrapByScriptTag = true,
                ContainerId = containerId,
                RenderMode = RenderMode.AddTo
            };

            this.GetCmp<TabPanel>(containerId).SetLastTabAsActive();
            return result;

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetLatLng(string CityId)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            UserInfo user = (UserInfo)(Session["User"]);
            Models.RaiSamEntities m = new RaiSamEntities();
            var q = m.prs_tblShahrSelect("fldId", CityId.ToString(),0).FirstOrDefault();
            return Json(new
            {
                fldId = q.fldId,
                fldLatitude = q.fldLatitude,
                fldLongitude = q.fldLongitude
            }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult NewPerHuman(int ShakhsId, string Name, int State)
        {//باز شدن پنجره
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
            PartialView.ViewBag.Id = ShakhsId;
            PartialView.ViewBag.Name = Name;
            PartialView.ViewBag.State = State;
            return PartialView;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ReadContact_H(StoreRequestParameters parameters, int Id, int State)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
                if (Permission.haveAccess(124,"","0"))
                {
                    UserInfo user = (UserInfo)(Session["User"]);
                    Models.RaiSamEntities m = new RaiSamEntities();
                    List<prs_tblContactSelect> data = null;
                    string FieldName = "";
                    if (State == 1)
                        FieldName = "fldAshkhasId";
                    else if (State == 2)
                        FieldName = "fldMarazekTebId";
                    else if (State == 3)
                        FieldName = "fldAshkhasHoghooghiId";

                    data = m.prs_tblContactSelect(FieldName,Id.ToString(),user.UserSecondId,0).ToList();
                    //-- start paging ------------------------------------------------------------
                    int limit = parameters.Limit;

                    if ((parameters.Start + parameters.Limit) > data.Count)
                    {
                        limit = data.Count - parameters.Start;
                    }

                    List<prs_tblContactSelect> rangeData = (parameters.Start < 0 || limit < 0) ? data : data.GetRange(parameters.Start, limit);
                    //-- end paging ------------------------------------------------------------

                    return this.Store(rangeData, data.Count);
                }
                else
                {
                    return RedirectToAction("Error", "Home", new { Code = "403" });
                }
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CheckValidate(string Value, byte ContactTypeId)
        {
            if (Request.IsAjaxRequest() == false)
            {
                return Content("شما مجاز به انجام این عملیات نمی باشید.");
            }
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });

            UserInfo user = (UserInfo)(Session["User"]);
            Models.RaiSamEntities m = new RaiSamEntities();
            bool Valid = true;
            try
            {
                var q = m.prs_tblContanctTypeSelect("fldId", ContactTypeId.ToString(),1).FirstOrDefault();
                if (q.fldFormulId != null)
                {
                    var Formoul = m.prs_tblComputationFormulaSelect("fldId", q.fldFormulId.ToString(),1).FirstOrDefault();

                    string Formula = Formoul.fldFormul;
                    string[] ReferenceSplit = Formoul.fldLibrary.Split(';').Reverse().Skip(1).Reverse().ToArray();

                    ICodeCompiler loCompiler = new CSharpCodeProvider().CreateCompiler();
                    CompilerParameters loParameters = new CompilerParameters();

                    for (int i = 0; i < ReferenceSplit.Length; i++)
                    {
                        if (ReferenceSplit[i] == "System.dll" || ReferenceSplit[i] == "System.Data.dll" || ReferenceSplit[i] == "System.Xml.dll" || ReferenceSplit[i] == "System.Core.dll")
                        {
                            loParameters.ReferencedAssemblies.Add(ReferenceSplit[i]);
                        }
                        else
                        {
                            var Path = Server.MapPath(@"~\Reference\" + ReferenceSplit[i]);
                            loParameters.ReferencedAssemblies.Add(Path);
                        }
                    }

                    loParameters.GenerateInMemory = true;
                    CompilerResults loCompiled = loCompiler.CompileAssemblyFromSource(loParameters, Formula);

                    Assembly loAssembly = loCompiled.CompiledAssembly;
                    object loObject = loAssembly.CreateInstance("MyNamespace.MyClass");

                    object[] loCodeParms = new object[5];
                    loCodeParms[0] = Value;
                    loCodeParms[1] = user.UserSecondId;
                    loCodeParms[2] = user.UserInputId;
                    loCodeParms[3] = user.UserKey;
                    loCodeParms[4] = IP;

                    object loResult = loObject.GetType().InvokeMember("DynamicCode", BindingFlags.InvokeMethod, null, loObject, loCodeParms);
                    Valid = (bool)loResult;
                }
            }
            catch (Exception x)
            {
                string InnerException = "";
                if (x.InnerException != null)
                    InnerException = x.InnerException.Message;
                else
                    InnerException = x.Message;
                System.Data.Entity.Core.Objects.ObjectParameter ErrorId = new System.Data.Entity.Core.Objects.ObjectParameter("fldID", typeof(int));
                var Input = m.prs_tblInputInfoSelect("CheckUser", user.UserKey, IP, false, 0).FirstOrDefault();
                m.prs_tblErrorInsert(ErrorId, InnerException, DateTime.Now, Input.fldId, "");
                return Json(new
                {
                    MsgTitle = "خطا",
                    Msg = "خطایی با شماره: " + ErrorId.Value + " رخ داده است لطفا با پشتیبانی تماس گرفته و کد خطا را اعلام فرمایید.",
                    Er = 1,
                });
            }
            return Json(new
            {
                Er = 0,
                Valid = Valid
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetContactGroup()
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
                var q = m.prs_tblContactGroupSelect("","",0).Select(l => new { fldId = l.fldId, fldName = l.fldNameGroup });
                return this.Store(q);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveContact_TreeGroup(string TreeGroupArray1, int ContactId, int GroupId)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
                string Msg = "", MsgTitle = ""; var Er = 0;
                UserInfo user = (UserInfo)(Session["User"]);
                Models.RaiSamEntities m = new RaiSamEntities();
                List<int> TreeGroupArray = JsonConvert.DeserializeObject<List<int>>(TreeGroupArray1);
                try
                {
                    if (Permission.haveAccess(129, "tblContact_TreeGroup", null))
                    {
                        var Contact_TreeGroup = m.prs_tblContact_TreeGroupSelect("fldTreeGroupId_ContactId", GroupId.ToString(), ContactId, 0).ToList();
                        if (Contact_TreeGroup.Count > 0)
                        {
                            foreach (var item in Contact_TreeGroup)
                            {

                                m.prs_tblContact_TreeGroupDelete("Id", item.fldId);
                                Dictionary<string, object> parameters = new Dictionary<string, object>();
                                var q = m.prs_tblTreeGroupSelect("fldId", item.fldTreeGroupId.ToString(), 0, 0).FirstOrDefault();
                                parameters.Add("کد تماس", ContactId);
                                parameters.Add("عنوان گروه تماس", item.fldNameGroup);
                                parameters.Add("عنوان گره ساختار درختی", q.fldTitle);

                                string jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                                m.prs_LogDelete(user.UserInputId, "Cnt.tblContact_TreeGroup", jsonstr, true, item.fldId);
                            }
                        }

                        foreach (var item in TreeGroupArray)
                        {
                            var Tree = m.prs_tblTreeGroupSelect("fldId", item.ToString(), 0, 0).FirstOrDefault();
                            Dictionary<string, object> parameters = new Dictionary<string, object>();
                            parameters.Add("کد تماس", ContactId);
                            parameters.Add("عنوان گره ساختار درختی", Tree.fldTitle);
                            string jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                            m.prs_tblContact_TreeGroupInsert(item, ContactId, user.UserInputId, jsonstr);


                        }
                        Msg = "ذخیره با موفقیت انجام شد.";
                        MsgTitle = "دخیره موفق";
                    }
                    else
                    {
                        return Json(new
                        {
                            Msg = "شما مجاز به دسترسی نمی باشید.",
                            MsgTitle = "خطا",
                            Er = 1
                        }, JsonRequestBehavior.AllowGet);
                    }
                }
                catch (Exception x)
                {
                    string InnerException = "";
                    if (x.InnerException != null)
                        InnerException = x.InnerException.Message;
                    else
                        InnerException = x.Message;
                    System.Data.Entity.Core.Objects.ObjectParameter ErrorId = new System.Data.Entity.Core.Objects.ObjectParameter("fldID", typeof(int));
                    var Input = m.prs_tblInputInfoSelect("CheckUser", user.UserKey, IP, false, 0).FirstOrDefault();
                    m.prs_tblErrorInsert(ErrorId, InnerException, DateTime.Now, Input.fldId, "");
                    Dictionary<string, object> parameters = new Dictionary<string, object>();
                    parameters.Add("کد خطا", ErrorId.Value);
                    parameters.Add("متن خطا", "ContactGroupTree:Delete: " + InnerException);
                    string jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                    m.prs_LogInsert(user.UserInputId, "Cnt.tblContact_TreeGroup", jsonstr, false);
                    return Json(new
                    {
                        MsgTitle = "خطا",
                        Msg = "خطایی با شماره: " + ErrorId.Value + " رخ داده است لطفا با پشتیبانی تماس گرفته و کد خطا را اعلام فرمایید.",
                        Er = 1,
                    });
                }
                return Json(new
                {
                    Msg = Msg,
                    MsgTitle = MsgTitle,
                    Er = Er
                }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult Contact_TreeWin(int ContactId, string ContactName)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            var result = new Ext.Net.MVC.PartialViewResult();
            ViewData = this.ViewData;
            result.ViewBag.ContactId = ContactId;
            result.ViewBag.ContactName = ContactName;
            return result;
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

        public FileContentResult ShowHelpContact()
        {
            if (Session["User"] == null)
                return null;
            UserInfo user = (UserInfo)(Session["User"]);
            Models.RaiSamEntities m = new RaiSamEntities();
            var Help = m.prs_tblHelpSelect("fldId", "21", 1).FirstOrDefault();
            var q = m.prs_tblFileSelect("fldId", Help.fldFilePdfId.ToString(), 1).FirstOrDefault();
            return File((byte[])q.fldFile, "application/pdf");
        }
        public ActionResult VideoWinContact()
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
            return PartialView;
        }
        public FileContentResult DownloadFileVideoContact()
        {
            if (Session["User"] == null)
                return null;
            UserInfo user = (UserInfo)(Session["User"]);
            Models.RaiSamEntities m = new RaiSamEntities();
            var Help = m.prs_tblHelpSelect("fldId", "21", 1).FirstOrDefault();
            var q = m.prs_tblFileSelect("fldId", Help.fldFileVideoId.ToString(), 1).FirstOrDefault();
           
            MemoryStream st = new MemoryStream(q.fldFile);
            return File(st.ToArray(), MimeType.Get(q.fldPasvand), q.fldFileName + q.fldPasvand);
        }
        public ActionResult New(int id)
        {//باز شدن پنجره
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            else
            {
                Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
                PartialView.ViewBag.Id = id;
                return PartialView;
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetState()
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
                var q = m.prs_tblStateSelect("","",0).Select(l => new { fldId = l.fldId, fldName = l.fldName });
                return this.Store(q);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetCounty(string StateId)
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
                var q = m.prs_tblCountySelect("fldStateId", StateId,0).Select(l => new { fldId = l.fldId, fldName = l.fldName });
                return this.Store(q);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetShahr(string CountyId)
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
                var q = m.prs_tblShahrSelect("fldCountyId", CountyId,0).Select(l => new { fldId = l.fldId, fldName = l.fldName });
                return this.Store(q);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetContactType()
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
                var q = m.prs_tblContanctTypeSelect("","",0).ToList().Select(l => new { ID = l.fldId, fldName = l.fldType });
                return this.Store(q);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NodeLoadContactGroup(string nod, int? GroupId, int ContactId)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            //if (Permission.haveAccess(243))
            //{
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
                NodeCollection nodes = new Ext.Net.NodeCollection();
                UserInfo user = (UserInfo)(Session["User"]);
                Models.RaiSamEntities m = new RaiSamEntities();
                var q = m.prs_tblTreeGroupSelect("fldPId", nod, GroupId,0).ToList();
                foreach (var item in q)
                {
                    Node asyncNode = new Node();
                    asyncNode.NodeID = item.fldId.ToString();
                    asyncNode.Text = item.fldTitle;

                    
                    var contact_treegroup = m.prs_tblContact_TreeGroupSelect("fldTreeGroupId_ContactId", item.fldId.ToString(), ContactId,1).FirstOrDefault();
                    if (contact_treegroup != null)
                    {
                        asyncNode.Checked = true;
                    }
                    else
                    {
                        asyncNode.Checked = false;
                    }
                    nodes.Add(asyncNode);
                }
                return this.Store(nodes);
            }
            //}
            //else
            //{
            //    return RedirectToAction("Error", "Home", new { Code = "403" });
            //}
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(prs_tblContactSelect Contact, prs_tblAddressSelect addr, int? fldAshkhashoghooghi)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
                string Msg = "", MsgTitle = ""; var Er = 0; var Change = 0;
                UserInfo user = (UserInfo)(Session["User"]);
                Models.RaiSamEntities m = new RaiSamEntities();
                try
                {
                    if (Contact.fldId == 0)
                    {
                        //ذخیره
                        if (Permission.haveAccess(125, "tblContact", null))
                        {
                            var cont = m.prs_tblContactSelect("checkUserId", Contact.fldValue, user.UserSecondId, 0).Where(l => l.fldTypeContactId == Contact.fldTypeContactId && l.fldType == Contact.fldType).FirstOrDefault();
                            if (cont != null)
                            {
                                return Json(new
                                {
                                    Msg = "اطلاعات وارد شده تکراری است.",
                                    MsgTitle = "خطا",
                                    Er = 1
                                }, JsonRequestBehavior.AllowGet);
                            }
                            Dictionary<string, object> parameters = new Dictionary<string, object>();
                            if (!(Contact.fldAshkhasId == null || Contact.fldAshkhasId == 0))
                            {
                                var q = m.prs_tblAshkhasSelect("fldId", Contact.fldAshkhasId.ToString(), "","","", 0).FirstOrDefault();
                                parameters.Add("نام و نام خانوادگی", q.fldName + " " + q.fldFamily);
                                parameters.Add("کدملی", q.fldCodeMeli);
                            }
                            if (!(fldAshkhashoghooghi == null || fldAshkhashoghooghi == 0))
                            {
                                var q1 = m.prs_tblAshkhasHoghooghiSelect("fldId", fldAshkhashoghooghi.ToString(), 0).FirstOrDefault();
                                parameters.Add("اشخاص حقوقی", q1.fldName);
                                parameters.Add("شناسه ملی", q1.fldNationalCode);
                            }

                            parameters.Add("وضعیت", Contact.fldTyperName);
                            parameters.Add("نوع تماس", Contact.NameTypeContact);
                            parameters.Add("مقادیر", Contact.fldValue);
                            string jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                            System.Data.Entity.Core.Objects.ObjectParameter Id = new System.Data.Entity.Core.Objects.ObjectParameter("fldId", typeof(int));
                            m.prs_tblContactInsert(Id, Contact.fldTypeContactId, Contact.fldValue, Contact.fldType, user.UserSecondId, Contact.fldAshkhasId, fldAshkhashoghooghi, user.UserInputId, jsonstr);

                            if (Contact.fldTypeContactId == 1 && addr.fldLongitude != null)
                            {
                                Dictionary<string, object> parameters1 = new Dictionary<string, object>();
                                //parameters1.Add("شهر", addr.fldShahrName);
                                parameters1.Add("کد تماس", addr.fldContactId);
                                parameters1.Add("عرض جغرافیایی", addr.fldLatitude);
                                parameters1.Add("طول جغرافیایی", addr.fldLongitude);
                                string jsonstr1 = Newtonsoft.Json.JsonConvert.SerializeObject(parameters1, Newtonsoft.Json.Formatting.Indented);
                                m.prs_tblAddressInsert(addr.fldShahrId, Convert.ToInt32(Id.Value), addr.fldLatitude, addr.fldLongitude, user.UserInputId, jsonstr1);

                            }
                            Msg = "ذخیره با موفقیت انجام شد.";
                            MsgTitle = "دخیره موفق";
                        }
                        else
                        {
                            return Json(new
                            {
                                Msg = "شما مجاز به دسترسی نمی باشید.",
                                MsgTitle = "خطا",
                                Er = 1
                            }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        //ویرایش
                        if (Permission.haveAccess(126, "tblContact", Contact.fldId.ToString()))
                        {
                            var cont = m.prs_tblContactSelect("checkUserId", Contact.fldValue, user.UserSecondId, 0).Where(l => l.fldTypeContactId == Contact.fldTypeContactId && l.fldType == Contact.fldType).FirstOrDefault();
                            if (cont != null && cont.fldId != Contact.fldId)
                            {
                                return Json(new
                                {
                                    Msg = "اطلاعات وارد شده تکراری است.",
                                    MsgTitle = "خطا",
                                    Er = 1
                                }, JsonRequestBehavior.AllowGet);
                            }
                            var q1 = m.prs_tblContactUpdate(Contact.fldId, Contact.fldTypeContactId, Contact.fldValue, Contact.fldType, user.UserSecondId, Contact.fldAshkhasId, fldAshkhashoghooghi, user.UserInputId, Contact.fldTimeStamp).FirstOrDefault();


                            if (q1.flag == 1)
                            {
                                Msg = "ویرایش با موفقیت انجام شد";
                                MsgTitle = "ویرایش موفق";
                            }
                            else if (q1.flag == 0)
                            {
                                Msg = "مورد انتخاب شده قبلا تغییر کرده است لطفا منتظر بارگذاری جدید باشید";
                                MsgTitle = "هشدار";
                                Er = 1;
                                Change = 1;
                            }
                            else if (q1.flag == 2)
                            {
                                Msg = "مورد انتخاب شده قبلا حذف گردیده است لذا امکان تغییر وجود ندارد.";
                                MsgTitle = "خطا";
                                Er = 1;
                                Change = 2;
                            }


                            Dictionary<string, object> parameters = new Dictionary<string, object>();
                            if (!(fldAshkhashoghooghi == null || fldAshkhashoghooghi == 0))
                            {
                                var q = m.prs_tblAshkhasHoghooghiSelect("fldId", fldAshkhashoghooghi.ToString(), 1).FirstOrDefault();
                                parameters.Add("نام شرکت", q.fldName);
                                parameters.Add("شناسه ملی", q.fldNationalCode);
                            }
                            if (!(Contact.fldAshkhasId == null || Contact.fldAshkhasId == 0))
                            {
                                var q = m.prs_tblAshkhasSelect("fldId", Contact.fldAshkhasId.ToString(), "","","", 0).FirstOrDefault();
                                parameters.Add("نام و نام خانوادگی", q.fldName + " " + q.fldFamily);
                                parameters.Add("کدملی", q.fldCodeMeli);
                            }

                            parameters.Add("وضعیت", Contact.fldTyperName);
                            parameters.Add("نوع تماس", Contact.NameTypeContact);
                            parameters.Add("کد کاربر", Contact.fldUserId);
                            parameters.Add("مقادیر", Contact.fldValue);


                            if (Er == 1)
                            {
                                parameters.Add("متن خطا", "Contact:Save: " + Msg);
                                string jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                                m.prs_LogUpdate(user.UserInputId, "Cnt.tblContact", jsonstr, false, Contact.fldId);
                            }

                            else
                            {
                                string jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                                m.prs_LogUpdate(user.UserInputId, "Cnt.tblContact", jsonstr, true, Contact.fldId);
                            }

                            if (q1.flag == 1)
                            {
                                Msg = "ویرایش با موفقیت انجام شد";
                                MsgTitle = "ویرایش موفق";
                                if (Contact.fldTypeContactId == 1 && addr.fldLongitude != null)
                                {
                                    var ad = m.prs_tblAddressSelect("fldContactId", Contact.fldId.ToString(), 1).FirstOrDefault();
                                    if (ad != null)
                                    {
                                        addr.fldId = ad.fldId;
                                        addr.fldContactId = Contact.fldId;
                                        var output = m.prs_tblAddressUpdate(ad.fldId, addr.fldShahrId, Contact.fldId, addr.fldLatitude, addr.fldLongitude, user.UserInputId);

                                        Dictionary<string, object> parameters2 = new Dictionary<string, object>();
                                        // parameters.Add("شهر", addr.fldShahrName);
                                        parameters2.Add("کد تماس", addr.fldContactId);
                                        parameters2.Add("عرض جغرافیایی", addr.fldLatitude);
                                        parameters2.Add("طول جغرافیایی", addr.fldLongitude);
                                        string jsonstr2 = Newtonsoft.Json.JsonConvert.SerializeObject(parameters2, Newtonsoft.Json.Formatting.Indented);

                                        m.prs_LogUpdate(user.UserInputId, "Cnt.tblAddress", jsonstr2, true, addr.fldId);

                                    }
                                    else
                                    {
                                        Dictionary<string, object> parameters3 = new Dictionary<string, object>();
                                        // parameters.Add("شهر", addr.fldShahrName);
                                        parameters3.Add("کد تماس", addr.fldContactId);
                                        parameters3.Add("عرض جغرافیایی", addr.fldLatitude);
                                        parameters3.Add("طول جغرافیایی", addr.fldLongitude);
                                        string jsonstr3 = Newtonsoft.Json.JsonConvert.SerializeObject(parameters3, Newtonsoft.Json.Formatting.Indented);
                                        m.prs_tblAddressInsert(addr.fldShahrId, Contact.fldId, addr.fldLatitude, addr.fldLongitude, user.UserInputId, jsonstr3);

                                    }
                                }
                            }

                        }
                        else
                        {
                            return Json(new
                            {
                                Msg = "شما مجاز به دسترسی نمی باشید.",
                                MsgTitle = "خطا",
                                Er = 1
                            }, JsonRequestBehavior.AllowGet);
                        }

                    }
                }
                catch (Exception x)
                {
                    string InnerException = "";
                    if (x.InnerException != null)
                        InnerException = x.InnerException.Message;
                    else
                        InnerException = x.Message;
                    System.Data.Entity.Core.Objects.ObjectParameter ErrorId = new System.Data.Entity.Core.Objects.ObjectParameter("fldID", typeof(int));
                    var Input = m.prs_tblInputInfoSelect("CheckUser", user.UserKey, IP, false, 0).FirstOrDefault();
                    m.prs_tblErrorInsert(ErrorId, InnerException, DateTime.Now, Input.fldId, "");
                    Dictionary<string, object> parameters = new Dictionary<string, object>();
                    if (!(fldAshkhashoghooghi == null || fldAshkhashoghooghi == 0))
                    {
                        var q = m.prs_tblAshkhasHoghooghiSelect("fldId", fldAshkhashoghooghi.ToString(), 1).FirstOrDefault();
                        parameters.Add("نام شرکت", q.fldName);
                        parameters.Add("شناسه ملی", q.fldNationalCode);
                    }
                    if (!(Contact.fldAshkhasId == null || Contact.fldAshkhasId == 0))
                    {
                        var q = m.prs_tblAshkhasSelect("fldId", Contact.fldAshkhasId.ToString(), "","","", 0).FirstOrDefault();
                        parameters.Add("نام و نام خانوادگی", q.fldName + " " + q.fldFamily);
                        parameters.Add("کدملی", q.fldCodeMeli);
                    }

                    parameters.Add("وضعیت", Contact.fldTyperName);
                    parameters.Add("نوع تماس", Contact.NameTypeContact);
                    parameters.Add("کد کاربر", Contact.fldUserId);
                    parameters.Add("مقادیر", Contact.fldValue);
                    parameters.Add("کد خطا", ErrorId.Value);
                    parameters.Add("متن خطا", "Contact:Save: " + InnerException);
                    string jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                    if (Contact.fldId == 0)
                    {
                        m.prs_LogInsert(user.UserInputId, "Cnt.tblContact", jsonstr, false);
                    }
                    else
                    {
                        m.prs_LogUpdate(user.UserInputId, "Cnt.tblContact", jsonstr, false, Contact.fldId);
                    }

                    if (Session["savePath"] != null)
                    {
                        string physicalPath = System.IO.Path.Combine(Session["savePath"].ToString());
                        Session.Remove("savePath");
                        Session.Remove("FileName");
                        System.IO.File.Delete(physicalPath);
                    }
                    return Json(new
                    {
                        MsgTitle = "خطا",
                        Msg = "خطایی با شماره: " + ErrorId.Value + " رخ داده است لطفا با پشتیبانی تماس گرفته و کد خطا را اعلام فرمایید.",
                        Er = 1,
                        Change = Change
                    });
                }
                return Json(new
                {
                    Msg = Msg,
                    MsgTitle = MsgTitle,
                    Er = Er,
                    Change = Change
                }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, int TimeStamp)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
                string Msg = "", MsgTitle = ""; byte Er = 0; var Change = 0;
                UserInfo user = (UserInfo)(Session["User"]);
                Models.RaiSamEntities m = new RaiSamEntities();


                string jsonstr = "";
                var Contact = m.prs_tblContactSelect("fldId", id.ToString(), user.UserSecondId,0).FirstOrDefault();
                try
                {//حذف

                    //service.DeleteOpertion_Action(id, user.UserKey, IP);
                     if (Permission.haveAccess(127, "tblContact", id.ToString()))
                        {
                    var q = m.prs_CheckContact(id,user.UserSecondId).FirstOrDefault();
                    if (q.flag == true)
                    {
                        var q1= m.prs_tblContactDelete(id, TimeStamp).FirstOrDefault();
                        
                            if (q1.flag == 1)
                            {
                                Msg = "حذف با موفقیت انجام شد";
                                MsgTitle = "حذف موفق";
                            }
                            else if (q1.flag == 0)
                            {
                                Msg = "مورد انتخاب شده قبلا تغییر کرده است لطفا منتظر بارگذاری جدید باشید";
                                MsgTitle = "هشدار";
                                Er = 1;
                                Change = 1;
                            }
                            else if (q1.flag == 2)
                            {
                                Msg = "مورد انتخاب شده قبلا حذف گردیده است لذا امکان دوباره حذف وجود ندارد.";
                                MsgTitle = "خطا";
                                Er = 1;
                                Change = 2;
                            }
                            Dictionary<string, object> parameters = new Dictionary<string, object>();
                            parameters.Add("وضعیت", Contact.fldTyperName);
                            parameters.Add("نوع تماس", Contact.NameTypeContact);
                            parameters.Add("کد کاربر", Contact.fldUserId);
                            parameters.Add("مقادیر", Contact.fldValue);

                            if (Er == 1)
                            {
                                parameters.Add("متن خطا", "Contact:Delete: " + Msg);
                                jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                                m.prs_LogDelete(user.UserInputId, "Cnt.tblContact", jsonstr, false, id);
                            }

                            else
                            {
                                jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                                m.prs_LogDelete(user.UserInputId, "Cnt.tblContact", jsonstr, true, id);
                            }
                               
                        

                    }
                    else
                    {
                        Msg = "شما مجاز به دسترسی نمی باشید.";
                        MsgTitle = "خطا";
                        Er = 1;
                        Change = 1;
                    }
                        }
                     else
                     {
                         return Json(new
                         {
                             Msg = "شما مجاز به دسترسی نمی باشید.",
                             MsgTitle = "خطا",
                             Er = 1
                         }, JsonRequestBehavior.AllowGet);
                     }
                }
                catch (Exception x)
                {
                    string InnerException = "";
                    if (x.InnerException != null)
                        InnerException = x.InnerException.Message;
                    else
                        InnerException = x.Message;
                    System.Data.Entity.Core.Objects.ObjectParameter ErrorId = new System.Data.Entity.Core.Objects.ObjectParameter("fldID", typeof(int));
                    var Input = m.prs_tblInputInfoSelect("CheckUser", user.UserKey, IP, false, 0).FirstOrDefault();
                    m.prs_tblErrorInsert(ErrorId, InnerException, DateTime.Now, Input.fldId, "");
                    Dictionary<string, object> parameters = new Dictionary<string, object>();
                    parameters.Add("وضعیت", Contact.fldTyperName);
                    parameters.Add("نوع تماس", Contact.NameTypeContact);
                    parameters.Add("کد کاربر", Contact.fldUserId);
                    parameters.Add("مقادیر", Contact.fldValue);
                    parameters.Add("کد خطا",ErrorId.Value);
                    parameters.Add("متن خطا", "Contact:Delete: " + InnerException);
                    jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);

                    m.prs_LogDelete(user.UserInputId, "Cnt.tblContact", jsonstr, false, id);
                    return Json(new
                    {
                        MsgTitle = "خطا",
                        Msg = "خطایی با شماره: " + ErrorId.Value + " رخ داده است لطفا با پشتیبانی تماس گرفته و کد خطا را اعلام فرمایید.",
                        Er = 1,
                        Change = Change
                    });
                }
                return Json(new
                {
                    Msg = Msg,
                    MsgTitle = MsgTitle,
                    Er = Er,
                    Change = Change
                }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Details(int Id)
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
                var q = m.prs_tblContactSelect("fldId", Id.ToString(), user.UserSecondId, 0).FirstOrDefault();
                int? fldAshkhashoghooghi = null;
                if (q.fldTypeshakhs == 1)
                    fldAshkhashoghooghi = q.fldAshkhasId;

                int shahr = 0;
                int county = 0;
                int state = 0;
                string lat = "";
                string lng = "";
                var ad = m.prs_tblAddressSelect("fldContactId", Id.ToString(),1).FirstOrDefault();
                if (ad != null)
                {
                    shahr = ad.fldShahrId;
                    county = ad.fldCountyId;
                    state = ad.fldStateId;
                    lat = ad.fldLatitude;
                    lng = ad.fldLongitude;
                }

                return Json(new
                {
                    fldId = q.fldId,
                    fldType = q.fldType.ToString(),
                    fldTypeContactId = q.fldTypeContactId.ToString(),
                    fldTyperName = q.fldTyperName,
                    fldValue = q.fldValue,
                    fldTimeStamp = q.fldTimeStamp,
                    fldAshkhasId = q.fldAshkhasId,
                    NameTypeContact = q.NameTypeContact,
                    fldTypeshakhs = q.fldTypeshakhs.ToString(),
                    fldfamily = q.fldfamily,
                    fldname = q.fldname,
                    fldAshkhashoghooghi = fldAshkhashoghooghi,
                    shahr = shahr,
                    county = county,
                    state = state,
                    lat = lat,
                    lng = lng

                }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CheckEdit_Delete(int Id)
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
                var flag = m.prs_CheckContact(Id, user.UserSecondId).FirstOrDefault();


                return Json(new
                {
                    flag = flag.flag
                }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult Read(StoreRequestParameters parameters)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
                if (Permission.haveAccess(124,"","0"))
                {
                    var filterHeaders = new FilterHeaderConditions(this.Request.Params["filterheader"]);
                    UserInfo user = (UserInfo)(Session["User"]);
                    Models.RaiSamEntities m = new RaiSamEntities();
                    List<prs_tblContactSelect> data = null;
                    if (filterHeaders.Conditions.Count > 0)
                    {
                        string field = "";
                        string searchtext = "";
                        List<prs_tblContactSelect> data1 = null;
                        foreach (var item in filterHeaders.Conditions)
                        {
                            var ConditionValue = (Newtonsoft.Json.Linq.JValue)item.ValueProperty.Value;

                            switch (item.FilterProperty.Name)
                            {
                                case "fldId":
                                    searchtext = ConditionValue.Value.ToString();
                                    field = "fldId";
                                    break;
                                case "NameTypeContact":
                                    searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                    field = "NameTypeContact";
                                    break;
                                case "fldTyperName":
                                    searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                    field = "fldTyperName";
                                    break;
                                case "fldValue":
                                    searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                    field = "fldValue";
                                    break;
                                case "fldname":
                                    searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                    field = "fldname";
                                    break;
                                case "fldfamily":
                                    searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                    field = "fldfamily";
                                    break;
                                case "fldCodeMeli":
                                    searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                    field = "fldCodeMeli";
                                    break;
                            }
                            if (data != null)
                                data1 = m.prs_tblContactSelect(field, searchtext, user.UserSecondId,100).ToList();
                            else
                                data = m.prs_tblContactSelect(field, searchtext, user.UserSecondId, 100).ToList();
                        }
                        if (data != null && data1 != null)
                            data.Intersect(data1);
                    }
                    else
                    {
                        data = m.prs_tblContactSelect("","",user.UserSecondId,100).ToList();
                    }

                    var fc = new FilterHeaderConditions(this.Request.Params["filterheader"]);

                    //FilterConditions fc = parameters.GridFilters;

                    //-- start filtering ------------------------------------------------------------
                    if (fc != null)
                    {
                        foreach (var condition in fc.Conditions)
                        {
                            string field = condition.FilterProperty.Name;
                            var value = (Newtonsoft.Json.Linq.JValue)condition.ValueProperty.Value;

                            data.RemoveAll(
                                item =>
                                {
                                    object oValue = item.GetType().GetProperty(field).GetValue(item, null);
                                    return !(oValue.ToString().IndexOf(value.ToString(), StringComparison.OrdinalIgnoreCase) >= 0);
                                }
                            );
                        }
                    }
                    //-- end filtering ------------------------------------------------------------

                    //-- start paging ------------------------------------------------------------
                    int limit = parameters.Limit;

                    if ((parameters.Start + parameters.Limit) > data.Count)
                    {
                        limit = data.Count - parameters.Start;
                    }

                    List<prs_tblContactSelect> rangeData = (parameters.Start < 0 || limit < 0) ? data : data.GetRange(parameters.Start, limit);
                    //-- end paging ------------------------------------------------------------

                    return this.Store(rangeData, data.Count);
                }
                else
                {
                    return RedirectToAction("Error", "Home", new { Code = "403" });
                }
            }
        }
    }
}
