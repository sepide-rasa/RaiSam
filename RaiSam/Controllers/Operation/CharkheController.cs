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

namespace RaiSam.Controllers.Operation
{
    public class CharkheController : Controller
    {
        //
        // GET: /Charkhe/
        string IP = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_HOST"];
        UserInfo u = (UserInfo)System.Web.HttpContext.Current.Session["User"];

        public ActionResult Index(string containerId)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            ViewData.Model = new CharkheAction();
            var result = new Ext.Net.MVC.PartialViewResult
            {
                WrapByScriptTag = true,
                ContainerId = containerId,
                ViewData = ViewData,
                RenderMode = RenderMode.AddTo
            };
            this.GetCmp<TabPanel>(containerId).SetLastTabAsActive();
            return result;

        }
        public ActionResult ListAskhasCharkhe(int CharkheId, string Name)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            var result = new Ext.Net.MVC.PartialViewResult();
            result.ViewBag.CharkheId = CharkheId;
            result.ViewBag.Name = Name;
            return result;
        }
        public ActionResult CopyCharkhe(int Id, int NoeGhebelCharkheshId)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Models.RaiSamEntities m = new RaiSamEntities();

            var k = m.prs_tblCharkhe_EghdamSelect("fldCharkheId", Id.ToString(),"",0).Any();
            if (k == true)
            {
                X.Msg.Show(new MessageBoxConfig
                {
                    Buttons = MessageBox.Button.OK,
                    Icon = MessageBox.Icon.ERROR,
                    Title = "خطا",
                    Message = "برای چرخه موردنظر قبلا اقدام تعریف شده است."
                });
                DirectResult result = new DirectResult();
                return result;
            }
            else
            {
                Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
                PartialView.ViewBag.EmptyCharkheId = Id;
                PartialView.ViewBag.NoeGhebelCharkheshId = NoeGhebelCharkheshId;
                return PartialView;
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ReadAllCharkhe(int CharkheId, int NoeGhebelCharkheshId)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
                Models.RaiSamEntities m = new RaiSamEntities();

                List<prs_tblCharkheSelect> data = null;
                data = m.prs_tblCharkheSelect("ExistsCharkheEghdam", NoeGhebelCharkheshId.ToString(),u.UserSecondId,0).Where(l => l.fldId != CharkheId).ToList();
                return this.Store(data);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveCharkhe_Copy(int OldCharkheId, int NewCharkheId)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
                Models.RaiSamEntities m = new RaiSamEntities();
                string Msg = "", MsgTitle = ""; var Er = 0;
                try
                {
                    if (Permission.haveAccess(146, "tblCharkhe", null))
                    {
                        string NameCharkhe_New = "";
                        NameCharkhe_New = m.prs_tblCharkheSelect("fldId", NewCharkheId.ToString(), 0, 0).FirstOrDefault().fldName;

                        string NameCharkhe_old = "";
                        NameCharkhe_old = m.prs_tblCharkheSelect("fldId", OldCharkheId.ToString(), 0, 0).FirstOrDefault().fldName;

                        Dictionary<string, object> parameters = new Dictionary<string, object>();

                        parameters.Add("نام چرخه جدید", NameCharkhe_New);
                        parameters.Add("نام چرخه قدیم", OldCharkheId);
                        //parameters.Add("کد جدول ورود و خروج", Charkhe.fldInputId);
                        string jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);

                    prs_tblCharkheSelect Charkhe = new prs_tblCharkheSelect();
                    m.prs_CopyCharkhe(NewCharkheId, OldCharkheId, u.UserInputId, jsonstr);

                    Msg = "ذخیره با موفقیت انجام شد.";
                    MsgTitle = "ذخیره موفق";
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
                    var Input = m.prs_tblInputInfoSelect("CheckUser", u.UserKey, IP, false, 0).FirstOrDefault();
                    m.prs_tblErrorInsert(ErrorId, InnerException, DateTime.Now, Input.fldId, "");
                    return Json(new
                    {
                        MsgTitle = "خطا",
                        Msg = "خطایی با شماره: " + ErrorId.Value + " رخ داده است لطفا با پشتیبانی تماس گرفته و کد خطا را اعلام فرمایید.",
                        Er = 1
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
        public FileContentResult ShowHelpCharkhe()
        {
            if (Session["User"] == null)
                return null;
            Models.RaiSamEntities m = new RaiSamEntities();
            var Help = m.prs_tblHelpSelect("fldId", "14", 1).FirstOrDefault();

            var q = m.prs_tblFileSelect("fldId", Help.fldFilePdfId.ToString(), 1).FirstOrDefault();
            return File((byte[])q.fldFile, "application/pdf");
        }
        public ActionResult VideoWinCharkhe()
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
            return PartialView;
        }
        public FileContentResult DownloadFileVideoCharkhe()
        {
            if (Session["User"] == null)
                return null;
            Models.RaiSamEntities m = new RaiSamEntities();
            var Help = m.prs_tblHelpSelect("fldId", "14", 1).FirstOrDefault();
            var q = m.prs_tblFileSelect("fldId", Help.fldFileVideoId.ToString(), 1).FirstOrDefault();
            MemoryStream st = new MemoryStream(q.fldFile);
            return File(st.ToArray(), MimeType.Get(q.fldPasvand), q.fldFileName + q.fldPasvand);
        }
        public ActionResult New(int id)
        {//باز شدن پنجره
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
            PartialView.ViewBag.Id = id.ToString();
            return PartialView;
        }
        public ActionResult DefaulOP_Ac_CharkheEghdam(int Charkhe_EghdamId, int DefaultOP, int CharkheId)
        {//باز شدن پنجره
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
            PartialView.ViewBag.Charkhe_EghdamId = Charkhe_EghdamId;
            PartialView.ViewBag.DefaultOP = DefaultOP;
            PartialView.ViewBag.CharkheId = CharkheId.ToString();
            return PartialView;
        }
        public ActionResult CartableAssign(int CharkheEghdamId, int CartablId, string CharkheEghdamName, string CharkheName, string EghdamName)
        {//باز شدن پنجره
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
            PartialView.ViewBag.CharkheEghdamId = CharkheEghdamId;
            PartialView.ViewBag.CharkheEghdamName = CharkheEghdamName;
            PartialView.ViewBag.CharkheName = CharkheName;
            PartialView.ViewBag.EghdamName = EghdamName;
            PartialView.ViewBag.CartablId = CartablId.ToString();
            return PartialView;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveCartableAssign(string Actions1, byte Edit)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            string Msg = "", MsgTitle = ""; var Er = 0;
            Models.RaiSamEntities m = new RaiSamEntities();
            string NameCharkhe = "", Name_Actions = "", Name_Kartabl = "";
            var Id = 0;
            string jsonstr ="";
            bool CheckSave_ActionId=false;
            try
            {
                List<prs_tblEkhtesasActions_CartableSelect> Actions = JsonConvert.DeserializeObject<List<prs_tblEkhtesasActions_CartableSelect>>(Actions1);

                foreach (var item in Actions)
                {
                    NameCharkhe = item.fldNameCharkhe;
                    Name_Actions = item.fldName_Actions;
                    Name_Kartabl = item.fldName_Kartabl;
                    Id = item.fldId;

                    if (item.fldDesc == null)
                        item.fldDesc = "";

                Dictionary<string, object> parameters = new Dictionary<string, object>();
                            parameters.Add("نام چرخه", item.fldNameCharkhe);
                            parameters.Add("نام اقدام", item.fldName_Actions);
                            parameters.Add("نام کارتابل", item.fldName_Kartabl);
                            parameters.Add("ترتیب", item.fldOrder);
                            parameters.Add("توضیحات", item.fldDesc);

                    if (Edit == 0 && item.fldId == 0)
                    {
                        if (Permission.haveAccess(121, "tblEkhtesasActions_Cartable", null)) {
                            CheckSave_ActionId = m.prs_tblEkhtesasActions_CartableSelect("fldCharkhe_EghdamId", item.fldCharkhe_EghdamId.ToString(), "", "", 0).Any();

                            if (CheckSave_ActionId == false)
                            {
                                Msg = "ذخیره با موفقیت انجام شد.";
                                MsgTitle = "ذخیره موفق";
                                jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                                m.prs_tblEkhtesasActions_CartableInsert(item.fldCartableId, item.fldOrder, item.fldDesc, u.UserInputId, item.fldCharkhe_EghdamId, jsonstr);
                            }
                            else
                            {
                                Msg = "اقدام انتخاب شده تنها به یک کارتابل میتواند اختصاص داده شود";
                                MsgTitle = "خطا";
                                parameters.Add("متن خطا", "EkhtesasActions_Cartable:Save: " + Msg);
                                jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                                m.prs_LogInsert(u.UserInputId, "dbo.tblEkhtesasActions_Cartable", jsonstr, false);
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
                    else
                    {
                        if (Permission.haveAccess(122, "tblEkhtesasActions_Cartable", null)) {
                        var A = m.prs_tblEkhtesasActions_CartableSelect("fldCharkhe_EghdamId", item.fldCharkhe_EghdamId.ToString(), "", "", 0).FirstOrDefault();
                        if (A != null && A.fldId != item.fldId)
                            CheckSave_ActionId = true;

                             if (CheckSave_ActionId == false)
                             {
                                 if (item.fldDesc == null)
                                     item.fldDesc = "";
                                var s=m.prs_tblEkhtesasActions_CartableUpdate(item.fldId, item.fldCartableId, item.fldOrder, item.fldDesc, u.UserInputId, item.fldCharkhe_EghdamId,item.fldTimeStamp).FirstOrDefault();
                                if (s.flag == 1)
                                {
                                    Msg = "ویرایش با موفقیت انجام شد.";
                                    MsgTitle = "ویرایش موفق";
                                }
                                else if (s.flag == 0)
                                {
                                    Msg = "مورد انتخاب شده قبلا تغییر کرده است لطفا منتظر بارگذاری جدید باشید";
                                    MsgTitle = "هشدار";
                                    Er = 1;
                                }
                                else if (s.flag == 2)
                                {
                                    Msg = "مورد انتخاب شده قبلا حذف گردیده است لذا امکان تغییر وجود ندارد.";
                                    MsgTitle = "خطا";
                                    Er = 1;
                                }
                                var Logstatus = true;
                                if (Er == 1)
                                {
                                    parameters.Add("متن خطا", "EkhtesasActions_Cartable:Edit: " + Msg);
                                    Logstatus = false;
                                }


                                jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                                m.prs_LogUpdate(u.UserInputId, "dbo.tblEkhtesasActions_Cartable", jsonstr, Logstatus, item.fldId);
                             }
                            else
                            {
                                Msg = "اقدام انتخاب شده تنها به یک کارتابل میتواند اختصاص داده شود";
                                MsgTitle = "خطا";
                                parameters.Add("متن خطا", "EkhtesasActions_Cartable:Edit: " + Msg);
                                jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                                m.prs_LogUpdate(u.UserInputId, "dbo.tblEkhtesasActions_Cartable", jsonstr, false, item.fldId);
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
            }
            catch (Exception x)
            {
                string InnerException = "";
                if (x.InnerException != null)
                    InnerException = x.InnerException.Message;
                else
                    InnerException = x.Message;
                System.Data.Entity.Core.Objects.ObjectParameter ErrorId = new System.Data.Entity.Core.Objects.ObjectParameter("fldID", typeof(int));
                var Input = m.prs_tblInputInfoSelect("CheckUser", u.UserKey, IP, false, 0).FirstOrDefault();
                m.prs_tblErrorInsert(ErrorId, InnerException, DateTime.Now, Input.fldId, "");

                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add("نام چرخه", NameCharkhe);
                parameters.Add("نام اقدام", Name_Actions);
                parameters.Add("نام کارتابل", Name_Kartabl);
                parameters.Add("کد خطا", ErrorId.Value);
                jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                if (Edit == 0 && Id == 0)
                {
                    parameters.Add("متن خطا", "EkhtesasActions_Cartable:Save: " + InnerException);
                    jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                    m.prs_LogInsert(u.UserInputId, "dbo.tblEkhtesasActions_Cartable", jsonstr, false);
                }
                else
                {
                    parameters.Add("متن خطا", "EkhtesasActions_Cartable:Edit: " + InnerException);
                    jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                    m.prs_LogUpdate(u.UserInputId, "dbo.tblEkhtesasActions_Cartable", jsonstr, false,Id);
                }

                return Json(new
                {
                    MsgTitle = "خطا",
                    Msg = "خطایی با شماره: " + ErrorId.Value + " رخ داده است لطفا با پشتیبانی تماس گرفته و کد خطا را اعلام فرمایید.",
                    Er = 1
                });
            }
            return Json(new
            {
                Msg = Msg,
                MsgTitle = MsgTitle,
                Er = Er
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetCartable()
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
                Models.RaiSamEntities m = new RaiSamEntities();
                var q = m.prs_tblKartablSelect("","",0).ToList().Select(l => new { fldId = l.fldId, fldName = l.fldName });
                return this.Store(q);
            }
        }
        public ActionResult TaainEghdamat(int CharkheId, int NoeGhabelCharkheshId)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            var result = new Ext.Net.MVC.PartialViewResult();
            ViewData.Model = new Models.CharkheAction();
            result.ViewData = ViewData;
            result.ViewBag.CharkheId = CharkheId.ToString();
            result.ViewBag.NoeGhabelCharkheshId = NoeGhabelCharkheshId.ToString();
            return result;
        }
        public ActionResult ShowAction_CharkheEghdam(int Charkhe_EghdamId)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            var result = new Ext.Net.MVC.PartialViewResult();
            result.ViewBag.Charkhe_EghdamId = Charkhe_EghdamId.ToString();
            return result;
        }
        public ActionResult ReferralRules(string id, string Name)
        {//باز شدن پنجره
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
            PartialView.ViewBag.Id = id;
            PartialView.ViewBag.Name = Name;
            return PartialView;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetAction(string fldCharkheId)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
                Models.RaiSamEntities m = new RaiSamEntities();
                var q = m.prs_tblCharkhe_EghdamSelect("fldCharkheId", fldCharkheId,"",0).ToList().Select(l => new { ID = l.fldId, fldName = l.fldNameEghdam + "(" + l.fldActiveName + ")" });
                return this.Store(q);
            }
        }
        public ActionResult GetNextAction(string fldCharkheId, int EghdamCharkheId)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Models.RaiSamEntities m = new RaiSamEntities();
            var q = m.prs_tblCharkhe_EghdamSelect("fldCharkheId", fldCharkheId,"",0).ToList().Where(l => l.fldId != EghdamCharkheId).Select(l => new { ID = l.fldEghdamId, fldName = l.fldNameEghdam + "(" + l.fldActiveName + ")" });
            return this.Store(q);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetOpration(string Charkhe_EghdamId)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
                Models.RaiSamEntities m = new RaiSamEntities();
                var q = m.prs_tblOpertion_ActionSelect("fldCharkhe_EghdamId", Charkhe_EghdamId,"","",0).ToList().Where(l => l.fldEffective == true).Select(l => new { ID = l.fldId, fldName = l.NameOperation + "(" + l.fldFaalName + ")", OperationId = l.fldOpertionId });
                return this.Store(q);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetAllOpration_Charkhe(int CharkheId)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
                Models.RaiSamEntities m = new RaiSamEntities();
                var q = m.prs_tblOpertion_ActionSelect("fldCharkheId", CharkheId.ToString(), "", "", 0).ToList()/*.Where(l => l.fldEffective == true)*/.Select(l => new { ID = l.fldId, fldName = l.fldNameAction_Opertion });
                return this.Store(q);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetRotatingAgent()
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
                Models.RaiSamEntities m = new RaiSamEntities();
                var q = m.prs_tblAnavinGhabelCharkheshSelect("","",0).ToList().Select(l => new { ID = l.fldId, fldName = l.fldName });
                return this.Store(q);
            }
        }
        
        public ActionResult GetAllOpration()
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Models.RaiSamEntities m = new RaiSamEntities();
            var q = m.prs_tblOperationSelect("","",0,0).ToList().Where(l => l.fldSpecificShow == true).Select(l => new { ID = l.fldId, fldName = l.fldName });
            return this.Store(q);
        }
        public ActionResult Upload()
        {
            string Msg = "";
            try
            {
                if (Session["savePathCharkhe"] != null)
                {
                    string physicalPath = System.IO.Path.Combine(Session["savePathCharkhe"].ToString());
                    Session.Remove("savePathCharkhe");
                    Session.Remove("FileName");
                    System.IO.File.Delete(physicalPath);
                }
                var IsImage = FileInfoExtensions.IsImage(Request.Files[0]);
                var extension = Path.GetExtension(Request.Files[0].FileName).ToLower();
                if (IsImage == true/*extension == ".jpg" || extension == ".jpeg" || extension == ".png" || extension == ".tif"
                || extension == ".tiff" || extension == ".gif"*/)
                {
                    if (Request.Files[0].ContentLength <= 307200)
                    {
                        HttpPostedFileBase file = Request.Files[0];
                        string savePath = Server.MapPath(@"~\Uploaded\" + file.FileName);
                        file.SaveAs(savePath);
                        Session["FileName"] = Path.GetFileNameWithoutExtension(Request.Files[0].FileName);
                        Session["savePathCharkhe"] = savePath;
                        object r = new
                        {
                            success = true,
                            name = Request.Files[0].FileName,
                            Message = "فایل با موفقیت آپلود شد.",
                            IsValid = true
                        };
                        return Content(string.Format("<textarea>{0}</textarea>", JSON.Serialize(r)));
                    }
                    else
                    {
                        object r = new
                        {
                            success = true,
                            name = Request.Files[0].FileName,
                            Message = "حجم فایل انتخاب شده غیرمجاز است.",
                            IsValid = false
                        };
                        return Content(string.Format("<textarea>{0}</textarea>", JSON.Serialize(r)));
                        //X.Msg.Show(new MessageBoxConfig
                        //{
                        //    Buttons = MessageBox.Button.OK,
                        //    Icon = MessageBox.Icon.ERROR,
                        //    Title = "خطا",
                        //    Message = "حجم فایل انتخاب شده غیرمجاز است."
                        //});
                        //DirectResult result = new DirectResult();
                        //return result;
                    }
                }
                else
                {
                    object r = new
                    {
                        success = true,
                        name = Request.Files[0].FileName,
                        Message = "فایل انتخاب شده غیر مجاز است.",
                        IsValid = false
                    };
                    return Content(string.Format("<textarea>{0}</textarea>", JSON.Serialize(r)));
                    //X.Msg.Show(new MessageBoxConfig
                    //{
                    //    Buttons = MessageBox.Button.OK,
                    //    Icon = MessageBox.Icon.ERROR,
                    //    Title = "خطا",
                    //    Message = "فایل انتخاب شده غیر مجاز است."
                    //});
                    //DirectResult result = new DirectResult();
                    //return result;
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
        public ActionResult ShowPicUpload(string dc)
        {//برگرداندن عکس 
            byte[] file = null;
            MemoryStream stream = new MemoryStream(System.IO.File.ReadAllBytes(Session["savePathCharkhe"].ToString()));
            file = stream.ToArray();
            var image = Convert.ToBase64String(file);
            return Json(new { image = image });
        }
        public ActionResult DeleteSessionFile()
        {
            string Msg = "";
            if (Session["savePathCharkhe"] != null)
            {
                string physicalPath = System.IO.Path.Combine(Session["savePathCharkhe"].ToString());
                Session.Remove("savePathCharkhe");
                Session.Remove("FileName");
                System.IO.File.Delete(physicalPath);
                Msg = "حذف فایل با موفقیت انجام شد";
            }
            return Json(new
            {
                Msg = Msg
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(prs_tblCharkheSelect Charkhe, bool DeleteFileKK)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
                Models.RaiSamEntities m = new RaiSamEntities();
                string Msg = "", MsgTitle = ""; var Er = 0; byte[] file = null; string Pasvand = ""; string NameFile = ""; byte TimeStamp = 1;
                string jsonstr = ""; bool CheckRepeatedRow = false;
                try
                {
                    if (Charkhe.fldDesc == null)
                        Charkhe.fldDesc = "";
                    Dictionary<string, object> parameters = new Dictionary<string, object>();
                    parameters.Add("نام چرخه", Charkhe.fldName);
                    parameters.Add("اکشن پیش فرض", Charkhe.fldNameOpertion);
                    parameters.Add("پسوند فایل", Pasvand);
                    parameters.Add("نام فایل", NameFile);
                    parameters.Add("وضعیت", Charkhe.fldStatusName);
                    parameters.Add("توضیحات", Charkhe.fldDesc);
                    parameters.Add("عامل قابل چرخش", Charkhe.fldAnvaGhabelCharkheshName);

                    if (Charkhe.fldId == 0)
                    { 
                        if (Permission.haveAccess(116, "dbo.tblCharkhe",null))
                    {
                        //ذخیره
                        if (Session["savePathCharkhe"] != null)
                        {
                            MemoryStream stream = new MemoryStream(System.IO.File.ReadAllBytes(Session["savePathCharkhe"].ToString()));
                            Pasvand = Path.GetExtension(Session["savePathCharkhe"].ToString());
                            file = stream.ToArray();
                            NameFile = Session["FileName"].ToString();
                        }
                        else
                        {
                            Pasvand = "";
                            file = null;
                            NameFile = "";
                        }
                        if (Session["savePathCharkhe"] != null)
                        {
                            string physicalPath = System.IO.Path.Combine(Session["savePathCharkhe"].ToString());
                            Session.Remove("savePathCharkhe");
                            Session.Remove("FileName");
                            System.IO.File.Delete(physicalPath);
                        }
                      

                        if (CheckRepeatedRow == false)
                        {
                            jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);

                            m.prs_tblCharkheInsert(Charkhe.fldName, Charkhe.fldDesc, Charkhe.fldDefulteOpertionId, u.UserInputId, file, Pasvand, NameFile, Charkhe.fldStatus, jsonstr,
                           Charkhe.fldNoeGhebelCharkheshId, Charkhe.fldTypeVorood);
                            Msg = "ذخیره با موفقیت انجام شد.";
                            MsgTitle = "ذخیره موفق";

                        }
                        else
                        {
                            Msg = "نام چرخه وارده شده تکراری است.";
                            MsgTitle = "خطا";
                            Er = 1;

                            parameters.Add("متن خطا", "Charkhe:Save: " + Msg);
                            jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                            m.prs_LogInsert(u.UserInputId, "dbo.tblCharkhe", jsonstr, false);
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
                    else
                    {
                        if (Permission.haveAccess(117, "dbo.tblCharkhe", Charkhe.fldId.ToString()))
                    {

                        //ویرایش
                        if (Session["savePathCharkhe"] != null)
                        {
                            MemoryStream stream = new MemoryStream(System.IO.File.ReadAllBytes(Session["savePathCharkhe"].ToString()));
                            Pasvand = Path.GetExtension(Session["savePathCharkhe"].ToString());
                            file = stream.ToArray();
                            NameFile = Session["FileName"].ToString();
                        }
                        else
                        {
                            if (DeleteFileKK == true)
                            {
                                file = null;
                                Pasvand = "";
                                NameFile = "";
                            }
                            else
                            {
                                var Ac = m.prs_tblFileSelect("fldId", Charkhe.fldFileId.ToString(),0).FirstOrDefault();
                                if (Ac != null)
                                {
                                    file = Ac.fldFile;
                                    Pasvand = Ac.fldPasvand;
                                    NameFile = Ac.fldFileName;
                                }
                            }
                        }
                        var Ch = m.prs_tblCharkheSelect("fldName", Charkhe.fldName, 0, 0).FirstOrDefault();
                        if (Ch != null && Ch.fldId != Charkhe.fldId)
                            CheckRepeatedRow = true;

                        if (CheckRepeatedRow == true)
                        {
                            Msg = "نام چرخه وارده شده تکراری است.";
                            MsgTitle = "خطا";

                            parameters.Add("متن خطا", Msg);
                            jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                            m.prs_LogUpdate(u.UserInputId, "dbo.tblCharkhe", jsonstr, false, Charkhe.fldId);

                        }
                        else
                        {

                            var s = m.prs_tblCharkheUpdate(Charkhe.fldId, Charkhe.fldName, Charkhe.fldDesc, Charkhe.fldDefulteOpertionId,Charkhe.fldFileId,  file, Pasvand, NameFile, Charkhe.fldStatus,
                           Charkhe.fldtimeStamp, Charkhe.fldNoeGhebelCharkheshId, Charkhe.fldTypeVorood, u.UserInputId).FirstOrDefault();


                            if (s.flag == 1)
                            {
                                Msg = "ذخیره با موفقیت انجام شد.";
                                MsgTitle = "ذخیره موفق";
                            }
                            else if (s.flag == 0)
                            {
                                Msg = "مورد انتخاب شده قبلا تغییر کرده است لطفا منتظر بارگذاری جدید باشید";
                                MsgTitle = "هشدار";
                                Er = 1;
                            }
                            else if (s.flag == 2)
                            {
                                Msg = "مورد انتخاب شده قبلا حذف گردیده است لذا امکان تغییر وجود ندارد.";
                                MsgTitle = "خطا";
                                Er = 1;
                            }
                            var Logstatus = true;
                            if (Er == 1)
                            {
                                parameters.Add("متن خطا", "Charkhe:Edit: " + Msg);
                                Logstatus = false;
                            }


                            jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                            m.prs_LogUpdate(u.UserInputId, "dbo.tblCharkhe", jsonstr, Logstatus, Charkhe.fldId);
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
                    var Input = m.prs_tblInputInfoSelect("CheckUser", u.UserKey, IP, false, 0).FirstOrDefault();
                    m.prs_tblErrorInsert(ErrorId, InnerException, DateTime.Now, Input.fldId, "");
                    Dictionary<string, object> parameters = new Dictionary<string, object>();
                    parameters.Add("نام چرخه", Charkhe.fldName);
                    parameters.Add("اکشن پیش فرض", Charkhe.fldNameOpertion);
                    parameters.Add("پسوند فایل", Pasvand);
                    parameters.Add("نام فایل", NameFile);
                    parameters.Add("وضعیت", Charkhe.fldStatusName);
                    parameters.Add("توضیحات", Charkhe.fldDesc);
                    parameters.Add("عامل قابل چرخش", Charkhe.fldAnvaGhabelCharkheshName);
                    parameters.Add("کد خطا", ErrorId.Value);

                    if (Charkhe.fldId == 0)
                    {
                        parameters.Add("متن خطا", "Charkhe:save: " + InnerException);
                        jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                        m.prs_LogInsert(u.UserInputId, "dbo.tblCharkhe", jsonstr, false);
                    }
                    else
                    {
                        parameters.Add("متن خطا", "Charkhe:Edit: " + InnerException);
                        jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                        m.prs_LogUpdate(u.UserInputId, "dbo.tblCharkhe", jsonstr, false, Charkhe.fldId);
                    }
                    return Json(new
                    {
                        MsgTitle = "خطا",
                        Msg = "خطایی با شماره: " + ErrorId.Value + " رخ داده است لطفا با پشتیبانی تماس گرفته و کد خطا را اعلام فرمایید.",
                        Er = 1,
                        TimeStamp = TimeStamp
                    });
                }
                return Json(new
                {
                    Msg = Msg,
                    MsgTitle = MsgTitle,
                    Er = Er,
                    TimeStamp = TimeStamp
                }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveDefaultOP_AcforCh_Ac(int CharkheEghdam, int? OPeration_Action)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
                Models.RaiSamEntities m = new RaiSamEntities();
                string Msg = "", MsgTitle = ""; var Er = 0;
                var NameOpertion_Defulte = "";
                if (OPeration_Action != null || OPeration_Action != 0)
                {
                    var q = m.prs_tblOpertion_ActionSelect("fldId", OPeration_Action.ToString(),"","",0).FirstOrDefault();
                    
                    var NameAction = m.prs_tblActionsSelect("fldId", q.fldActionId.ToString(),0).FirstOrDefault().fldName;

                    var NameOperation = m.prs_tblOperationSelect("fldId",q.fldOpertionId.ToString(),0,0).FirstOrDefault().fldName;
                    NameOpertion_Defulte = NameAction + "_" + NameOperation;
                }

                try
                {
                     if (Permission.haveAccess(148, "dbo.tblCharkhe_Eghdam", null))
                    {
                    var output = m.prs_UpdateCharkheEghdam_Opertion(CharkheEghdam, OPeration_Action);


                    Msg = "ویرایش با موفقیت انجام شد.";
                    MsgTitle = "ویرایش موفق";
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
                    var Input = m.prs_tblInputInfoSelect("CheckUser", u.UserKey, IP, false, 0).FirstOrDefault();
                    m.prs_tblErrorInsert(ErrorId, InnerException, DateTime.Now, Input.fldId, "");
                    Dictionary<string, object> parameters = new Dictionary<string, object>();
                    parameters.Add("عنوان اکشن پیش فرض", NameOpertion_Defulte);
                    parameters.Add("کد خطا", ErrorId.Value);
                    parameters.Add("متن خطا", InnerException);
                    string jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);

                    m.prs_LogUpdate(u.UserInputId, "tblCharkhe_Eghdam", jsonstr, false, CharkheEghdam);
                    return Json(new
                    {
                        MsgTitle = "خطا",
                        Msg = "خطایی با شماره: " + ErrorId.Value + " رخ داده است لطفا با پشتیبانی تماس گرفته و کد خطا را اعلام فرمایید.",
                        Er = 1
                    });
                }
                return Json(new
                {
                    Msg = Msg,
                    MsgTitle = MsgTitle,
                    Err = Er
                }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteReferralRules(int Id, int TimeStamp)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
                string Msg = "", MsgTitle = ""; byte Er = 0; byte outTimeStamp = 1;
                string jsonstr = "";
                Models.RaiSamEntities m = new RaiSamEntities();

                var q = m.prs_tblReferralRulesSelect("fldId", Id.ToString(),"","","",0).FirstOrDefault();
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add("ترتیب", q.fldOrder);
                parameters.Add("اقدام بعدی", q.fldNameNextAction);
                parameters.Add("مقدار", q.fldValue);
                parameters.Add("نام اکشن", q.fldNameOpertion);
                parameters.Add("توضیحات", q.fldDesc);

                try
                {//حذف

                    if (Permission.haveAccess(152, "tblReferralRules", Id.ToString()))
                    {

                        var s = m.prs_tblReferralRulesDelete(Id, TimeStamp).FirstOrDefault();
                        if (s.flag == 1)
                        {
                            Msg = "حذف با موفقیت انجام شد";
                            MsgTitle = "حذف موفق";
                        }
                        else if (s.flag == 0)
                        {
                            Msg = "مورد انتخاب شده قبلا تغییر کرده است لطفا منتظر بارگذاری جدید باشید";
                            MsgTitle = "هشدار";
                            Er = 1;
                        }
                        else if (s.flag == 2)
                        {
                            Msg = "مورد انتخاب شده قبلا حذف گردیده است لذا امکان تغییر وجود ندارد.";
                            MsgTitle = "خطا";
                            Er = 1;
                        }
                        var Logstatus = true;
                        if (Er == 1)
                        {
                            parameters.Add("متن خطا", "ReferralRules:Delete: " + Msg);
                            Logstatus = false;
                        }

                        jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                        m.prs_LogDelete(u.UserInputId, "dbo.tblReferralRules", jsonstr, Logstatus, Id);

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
                    var Input = m.prs_tblInputInfoSelect("CheckUser", u.UserKey, IP, false, 0).FirstOrDefault();
                    m.prs_tblErrorInsert(ErrorId, InnerException, DateTime.Now, Input.fldId, "");
                    parameters.Add("کد خطا", ErrorId.Value);
                    parameters.Add("متن خطا", InnerException);
                    jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);

                    m.prs_LogDelete(u.UserInputId, "dbo.tblReferralRules", jsonstr, false, Id);
                    return Json(new
                    {
                        MsgTitle = "خطا",
                        Msg = "خطایی با شماره: " + ErrorId.Value + " رخ داده است لطفا با پشتیبانی تماس گرفته و کد خطا را اعلام فرمایید.",
                        Er = 1
                    });
                }
                return Json(new
                {
                    Msg = Msg,
                    MsgTitle = MsgTitle,
                    Er = Er,
                    outTimeStamp = outTimeStamp
                }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveReferralRuls(prs_tblReferralRulesSelect Rules)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
                Models.RaiSamEntities m = new RaiSamEntities();
                string Msg = "", MsgTitle = ""; var Er = 0; byte TimeStamp = 1;
                string jsonstr ="";

                try
                {
                    if (Rules.fldDesc == null)
                        Rules.fldDesc = "";

                    Dictionary<string, object> parameters = new Dictionary<string, object>();
                    parameters.Add("ترتیب", Rules.fldOrder);
                    parameters.Add("اقدام بعدی", Rules.fldNameNextAction);
                    parameters.Add("مقدار", Rules.fldValue);
                    parameters.Add("نام اکشن", Rules.fldNameOpertion);
                    parameters.Add("توضیحات", Rules.fldDesc);
                    if (Rules.fldId == 0)
                    {
                        if (Permission.haveAccess(150, "tblReferralRules", null))
                        {
                            jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);

                            m.prs_tblReferralRulesInsert(Rules.fldNextActionId, Rules.fldValue, u.UserInputId, Rules.fldDesc, Rules.fldFormulId, Rules.fldFormul, Rules.fldOrder,
                                Rules.fldCharkhe_EghdamId, Rules.fldOpertion_ActionId, jsonstr);
                            Msg = "ذخیره با موفقیت انجام شد.";
                            MsgTitle = "ذخیره موفق";
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
                        if (Permission.haveAccess(151, "tblReferralRules", Rules.fldId.ToString()))
                        {
                            var s = m.prs_tblReferralRulesUpdate(Rules.fldId, Rules.fldOrder, Rules.fldNextActionId, Rules.fldValue, Rules.fldDesc,
                                Rules.fldCharkhe_EghdamId, Rules.fldOpertion_ActionId, u.UserInputId, Rules.fldTimeStamp).FirstOrDefault();
                            if (s.flag == 1)
                            {
                                Msg = "ذخیره با موفقیت انجام شد.";
                                MsgTitle = "ذخیره موفق";
                            }
                            else if (s.flag == 0)
                            {
                                Msg = "مورد انتخاب شده قبلا تغییر کرده است لطفا منتظر بارگذاری جدید باشید";
                                MsgTitle = "هشدار";
                                Er = 1;
                            }
                            else if (s.flag == 2)
                            {
                                Msg = "مورد انتخاب شده قبلا حذف گردیده است لذا امکان تغییر وجود ندارد.";
                                MsgTitle = "خطا";
                                Er = 1;
                            }
                            var Logstatus = true;
                            if (Er == 1)
                            {
                                parameters.Add("متن خطا", "ReferralRules:Edit: " + Msg);
                                Logstatus = false;
                            }


                            jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                            m.prs_LogUpdate(u.UserInputId, "dbo.tblReferralRules", jsonstr, Logstatus, Rules.fldId);
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
                    var Input = m.prs_tblInputInfoSelect("CheckUser", u.UserKey, IP, false, 0).FirstOrDefault();
                    m.prs_tblErrorInsert(ErrorId, InnerException, DateTime.Now, Input.fldId, "");
                    Dictionary<string, object> parameters = new Dictionary<string, object>();
                    parameters.Add("ترتیب", Rules.fldOrder);
                    parameters.Add("اقدام بعدی", Rules.fldNameNextAction);
                    parameters.Add("مقدار", Rules.fldValue);
                    parameters.Add("نام اکشن", Rules.fldNameOpertion);
                    parameters.Add("توضیحات", Rules.fldDesc);
                    parameters.Add("کد خطا", ErrorId.Value);
                    if (Rules.fldId == 0)
                    {
                        parameters.Add("متن خطا", "ReferralRules:Save: " + InnerException);
                        jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                        m.prs_LogInsert(u.UserInputId, "dbo.tblReferralRules", jsonstr, false);
                    }
                    else
                    {
                        parameters.Add("متن خطا", "ReferralRules:Edit: " + InnerException);
                        jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                        m.prs_LogUpdate(u.UserInputId, "dbo.tblReferralRules", jsonstr, false, Rules.fldId);
                    }
                    return Json(new
                    {
                        MsgTitle = "خطا",
                        Msg = "خطایی با شماره: " + ErrorId.Value + " رخ داده است لطفا با پشتیبانی تماس گرفته و کد خطا را اعلام فرمایید.",
                        Er = 1
                    });

                }
                return Json(new
                {
                    Msg = Msg,
                    MsgTitle = MsgTitle,
                    Er = Er,
                    TimeStamp = TimeStamp
                }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveAction(string Actions1, int CharkheId)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
                string Msg = "", MsgTitle = ""; var Er = 0; var DiagnosisLog = 0; var Id_Log = 0; bool checkDel = false; string jsonstr = "";
                Models.RaiSamEntities m = new RaiSamEntities();
                prs_tblCharkhe_EghdamSelect Actions_K = new prs_tblCharkhe_EghdamSelect(); 
              
                var charkheName = m.prs_tblCharkheSelect("fldId", CharkheId.ToString(),u.UserSecondId,0).FirstOrDefault().fldName;
               
                try
                {
                    if (Permission.haveAccess(145, "dbo.tblCharkhe_Eghdam", null))
                    {
                    List<prs_tblCharkhe_EghdamSelect> Actions = JsonConvert.DeserializeObject<List<prs_tblCharkhe_EghdamSelect>>(Actions1);
                  
                    var SaveData = m.prs_tblCharkhe_EghdamSelect("fldCharkheId", CharkheId.ToString(),"",0).ToList();
                    string[] SaveEghdamId = new string[SaveData.Count];
                    string[] NewEghdamId = new string[Actions.Count];

                    for (int i = 0; i < SaveData.Count; i++)
                    {
                        SaveEghdamId[i] = SaveData[i].fldEghdamId.ToString();
                    }
                    for (int j = 0; j < Actions.Count; j++)
                    {
                        NewEghdamId[j] = Actions[j].fldEghdamId.ToString();
                    }
                    var DeleteRow = SaveEghdamId.Except(NewEghdamId).ToArray();
                    var InsertRow = NewEghdamId.Except(SaveEghdamId).ToArray();
                    var UpdatedRow = NewEghdamId.Intersect(SaveEghdamId).ToArray();


                    if (DeleteRow.Length != 0)
                    {
                        DiagnosisLog = 1;//حذف
                        foreach (var item in DeleteRow)
                        {
                            var Index = SaveEghdamId.ToList().IndexOf(item);

                            bool CheckDel = false;
                            var Re = m.prs_tblReferralRulesSelect("fldCharkhe_EghdamId", SaveData[Index].fldId.ToString(), "", "", "", 0).FirstOrDefault();
                            var E = m.prs_tblEkhtesasActions_CartableSelect("fldCharkhe_EghdamId", SaveData[Index].fldId.ToString(), "", "", 0).FirstOrDefault();
                          //  var Op = m.prs_tblOpertion_ActionSelect("fldCharkhe_EghdamId", SaveData[Index].fldId.ToString(), "", "", 0).FirstOrDefault();

                            if (/*Op != null ||*/ Re != null || E != null)
                                CheckDel = true;


                            Dictionary<string, object> parameters = new Dictionary<string, object>();
                            parameters.Add("نام چرخه", SaveData[Index].fldNameCharkhe);
                            parameters.Add("نام اقدام", SaveData[Index].fldNameEghdam);
                            parameters.Add("ترتیب", SaveData[Index].fldOrderId);
                            parameters.Add("توضیحات", SaveData[Index].fldDesc);
                            parameters.Add("وضعیت", SaveData[Index].fldActiveName);

                            if (CheckDel==false)
                             m.prs_tblCharkhe_EghdamDelete("fldId", SaveData[Index].fldId);
                            else
                            {
                                parameters.Add("متن خطا", "آیتم مورد نظر در جای دیگر استفاده شده است لذا مجاز به حذف نمی باشید.");
                                jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                                m.prs_LogDelete(u.UserInputId, "dbo.tblCharkhe_Eghdam", jsonstr, false, SaveData[Index].fldId);
                            }

                        }
                    }
                    if (InsertRow.Length != 0)
                    {
                        DiagnosisLog = 2;//ذخیره
                        foreach (var item in InsertRow)
                        {
                            var Index = Actions.FindIndex(l => l.fldEghdamId == Convert.ToInt32(item));

                            bool CheckRepeatedRow = false;
                            Dictionary<string, object> parameters = new Dictionary<string, object>();
                            parameters.Add("نام چرخه", Actions[Index].fldNameCharkhe);
                            parameters.Add("نام اقدام", Actions[Index].fldNameEghdam);
                            parameters.Add("ترتیب", Actions[Index].fldOrderId);
                            parameters.Add("توضیحات", Actions[Index].fldDesc);
                            parameters.Add("وضعیت", Actions[Index].fldActiveName);

                            CheckRepeatedRow = m.prs_tblCharkhe_EghdamSelect("Charkhe_Eghdam", CharkheId.ToString(), item.ToString(), 0).Any();

                            if (CheckRepeatedRow == false)
                            {
                                jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                                m.prs_tblCharkhe_EghdamInsert(CharkheId, Convert.ToInt32(item), Actions[Index].fldOrderId, "", Actions[Index].fldActive, u.UserInputId, jsonstr);
                                DiagnosisLog = 0;
                            }
                            else
                            {
                                parameters.Add("متن خطا", "اطلاعات وارد شده تکراری است");
                                jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                                m.prs_LogInsert(u.UserInputId, "dbo.tblCharkhe_Eghdam", jsonstr, false);

                            }


                        }
                    }
                    if (UpdatedRow.Length != 0)
                    {
                        DiagnosisLog = 3;//ویرایش
                        foreach (var item in UpdatedRow)
                        {
                            var Index = Actions.FindIndex(l => l.fldEghdamId == Convert.ToInt32(item));

                            bool CheckRepeatedRow = false;
                            Dictionary<string, object> parameters = new Dictionary<string, object>();
                            parameters.Add("نام چرخه", charkheName);
                            parameters.Add("نام اقدام", Actions[Index].fldNameEghdam);
                            parameters.Add("ترتیب", Actions[Index].fldOrderId);
                            parameters.Add("توضیحات", Actions[Index].fldDesc);
                            parameters.Add("وضعیت", Actions[Index].fldActiveName);

                            var Ch = m.prs_tblCharkhe_EghdamSelect("Charkhe_Eghdam", CharkheId.ToString(), item.ToString(), 0).FirstOrDefault();
                            if (Ch != null && Ch.fldId != Actions[Index].fldId)
                                CheckRepeatedRow = true;


                            if (CheckRepeatedRow == false)
                            {
                                m.prs_tblCharkhe_EghdamUpdate(Actions[Index].fldId, CharkheId, Convert.ToInt32(item), Actions[Index].fldOrderId, u.UserInputId, "", Actions[Index].fldActive);
                                DiagnosisLog = 0;
                            }
                            else
                            {
                                parameters.Add("متن خطا", "اطلاعات وارد شده تکراری است");
                                jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                                m.prs_LogUpdate(u.UserInputId, "dbo.tblCharkhe_Eghdam", jsonstr, false, Actions[Index].fldId);
                            }

                        }
                    }

                    Msg = "ذخیره موفق";
                    MsgTitle = "ذخیره با موفقیت انجام شد.";
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
                    var Input = m.prs_tblInputInfoSelect("CheckUser", u.UserKey, IP, false, 0).FirstOrDefault();
                    m.prs_tblErrorInsert(ErrorId, InnerException, DateTime.Now, Input.fldId, "");
                    Dictionary<string, object> parameters = new Dictionary<string, object>();
                    parameters.Add("کد خطا", ErrorId.Value);
                    parameters.Add("متن خطا", InnerException);
                    parameters.Add("نام چرخه", charkheName);


                    if (Id_Log != 0)
                    {
                        var q = m.prs_tblCharkhe_EghdamSelect("fldId", Id_Log.ToString(),"",0).FirstOrDefault();

                        parameters.Add("نام اقدام", q.fldNameEghdam);
                        parameters.Add("ترتیب", q.fldOrderId);
                        parameters.Add("توضیحات", q.fldDesc);
                        parameters.Add("وضعیت", q.fldActiveName);
                    }
                    if (DiagnosisLog == 1)//delete
                    {
                        jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                        m.prs_LogDelete(u.UserInputId, "dbo.tblCharkhe_Eghdam", jsonstr, false, Id_Log);
                    }
                    else if (DiagnosisLog == 2)//insert
                    {
                        jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                        m.prs_LogInsert(u.UserInputId, "dbo.tblCharkhe_Eghdam", jsonstr, false);
                    }
                    else if (DiagnosisLog == 3)//update
                    {
                        jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                        m.prs_LogUpdate(u.UserInputId, "dbo.tblCharkhe_Eghdam", jsonstr, false, Id_Log);
                    }
                    return Json(new
                    {
                        MsgTitle = "خطا",
                        Msg = "خطایی با شماره: " + ErrorId.Value + " رخ داده است لطفا با پشتیبانی تماس گرفته و کد خطا را اعلام فرمایید.",
                        Er = 1
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
                string Msg = "", MsgTitle = ""; byte Er = 0; byte outTimeStamp = 1;
                Models.RaiSamEntities m = new RaiSamEntities();
                bool CheckDel=false;

                var ee = m.prs_tblCharkhe_EghdamSelect("fldCharkheId", id.ToString(), "", 0).FirstOrDefault();
                var f = m.prs_tblFactorsAffectingValues_HeaderSelect("fldCharkheId", id.ToString(), "", 0).FirstOrDefault();
                var rules = m.prs_tblRulesSelect("fldCharkheId", id.ToString(), 0).FirstOrDefault();
                var Ekhtesas = m.prs_tblEkhtesasActions_CartableSelect("fldCharkheId", id.ToString(), "", "", 0).FirstOrDefault();

                var E = m.prs_tblEnterToCycleSelect("fldCharkheId", id.ToString(), 0).FirstOrDefault();

                var Per = m.prs_tblPermissionCharkheSelect("CheckCharkheId", id.ToString(), 0, 0).FirstOrDefault();

                var u_Per = m.prs_tblUser_PermissionCharkheSelect("fldCharkheId", id.ToString(), 0).FirstOrDefault();

                if (ee != null || E != null || f != null || rules != null || Ekhtesas != null || Per != null || u_Per != null)
                    CheckDel = true;

                string jsonstr = "";
                var q = m.prs_tblCharkheSelect("fldId", id.ToString(),u.UserSecondId,0).FirstOrDefault();
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add("نام چرخه", q.fldName);
                parameters.Add("اکشن پیش فرض", q.fldNameOpertion);
                parameters.Add("نام فایل", q.fldFileName);
                parameters.Add("وضعیت", q.fldStatusName);
                parameters.Add("توضیحات", q.fldDesc);
                parameters.Add("عامل قابل چرخش", q.fldAnvaGhabelCharkheshName);
                try
                {//حذف
                    if (Permission.haveAccess(119, "dbo.tblCharkhe", id.ToString()))
                    {
                        if (CheckDel == true)
                        {
                            Msg = "آیتم مورد نظر در جای دیگر استفاده شده است لذا مجاز به حذف نمی باشید.";
                            MsgTitle = "خطا";
                            Er = 1;
                            parameters.Add("متن خطا", Msg);
                            jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                            m.prs_LogDelete(u.UserInputId, "dbo.tblCharkhe", jsonstr, false, id);
                        }
                        else
                        {
                            var s = m.prs_tblCharkheDelete(id, TimeStamp).FirstOrDefault();
                            if (s.flag == 1)
                            {
                                Msg = "حذف با موفقیت انجام شد";
                                MsgTitle = "حذف موفق";
                            }
                            else if (s.flag == 0)
                            {
                                Msg = "مورد انتخاب شده قبلا تغییر کرده است لطفا منتظر بارگذاری جدید باشید";
                                MsgTitle = "هشدار";
                                Er = 1;
                            }
                            else if (s.flag == 2)
                            {
                                Msg = "مورد انتخاب شده قبلا حذف گردیده است لذا امکان تغییر وجود ندارد.";
                                MsgTitle = "خطا";
                                Er = 1;
                            }
                            var Logstatus = true;
                            if (Er == 1)
                            {
                                parameters.Add("متن خطا", "Charkhe:Delete: " + Msg);
                                Logstatus = false;
                            }

                            jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                            m.prs_LogDelete(u.UserInputId, "dbo.tblCharkhe", jsonstr, Logstatus, id);
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
                    var Input = m.prs_tblInputInfoSelect("CheckUser", u.UserKey, IP, false, 0).FirstOrDefault();
                    m.prs_tblErrorInsert(ErrorId, InnerException, DateTime.Now, Input.fldId, "");
                    parameters.Add("کد خطا", ErrorId.Value);
                    parameters.Add("متن خطا", InnerException);
                    jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                    m.prs_LogDelete(u.UserInputId, "dbo.tblCharkhe", jsonstr,false,id);
                    return Json(new
                    {
                        MsgTitle = "خطا",
                        Msg = "خطایی با شماره: " + ErrorId.Value + " رخ داده است لطفا با پشتیبانی تماس گرفته و کد خطا را اعلام فرمایید.",
                        Er = 1
                    });
                }
                return Json(new
                {
                    Msg = Msg,
                    MsgTitle = MsgTitle,
                    Er = Er,
                    outTimeStamp = outTimeStamp
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteFromCharkhe(int id, string Name, string fldTitle, string fldCodeEnhesari, string fldNoeEnter)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
                string Msg = "", MsgTitle = ""; byte Er = 0; byte outTimeStamp = 1;
                Models.RaiSamEntities m = new RaiSamEntities();


                string jsonstr = "";
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add("نام چرخه", Name);
                parameters.Add("کد ورود به چرخه", id);
                parameters.Add("نام", fldTitle);
                parameters.Add("کدانحصاری", fldCodeEnhesari);
                parameters.Add("نوع چرخه", fldNoeEnter);
                try
                {//حذف
                    if (Permission.haveAccess(155, "tblEnterToCycle", id.ToString()))
                    {
                        var s = m.prs_DeleteKolByEnterId(id).FirstOrDefault();
                        if (s.ErrorMessage == "")
                        {
                            Msg = "حذف با موفقیت انجام شد.";
                            MsgTitle = "حذف موفق";
                        }
                        else
                        {
                            Msg = s.ErrorMessage;
                            MsgTitle = "خطا";
                            parameters.Add("کد خطا", s.ErrorCode);
                            parameters.Add("متن خطا", s.ErrorMessage);
                            jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                            m.prs_LogDelete(u.UserInputId, "dbo.tblEnterToCycle", jsonstr, false, id);
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
                    var Input = m.prs_tblInputInfoSelect("CheckUser", u.UserKey, IP, false, 0).FirstOrDefault();
                    m.prs_tblErrorInsert(ErrorId, InnerException, DateTime.Now, Input.fldId, "");
                    parameters.Add("کد خطا", ErrorId.Value);
                    parameters.Add("متن خطا", InnerException);
                    jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                    m.prs_LogDelete(u.UserInputId, "dbo.tblEnterToCycle", jsonstr, false,id);
                    return Json(new
                    {
                        MsgTitle = "خطا",
                        Msg = "خطایی با شماره: " + ErrorId.Value + " رخ داده است لطفا با پشتیبانی تماس گرفته و کد خطا را اعلام فرمایید.",
                        Er = 1
                    });
                }
                return Json(new
                {
                    Msg = Msg,
                    MsgTitle = MsgTitle,
                    Er = Er,
                    outTimeStamp = outTimeStamp
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Details(int Id)
        {
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
                Models.RaiSamEntities m = new RaiSamEntities();
                var q = m.prs_tblCharkheSelect("fldId", Id.ToString(),u.UserSecondId,1).FirstOrDefault();
                var Iconn = "";
                var file = m.prs_tblFileSelect("fldId", q.fldFileId.ToString(),1).FirstOrDefault();
                if (file != null)
                {
                    Iconn = Convert.ToBase64String(file.fldFile);
                }
                var Status = "1";
                if (q.fldStatus == false)
                {
                    Status = "0";
                }
                var TypeVorood = "1";
                if (q.fldTypeVorood == false)
                {
                    TypeVorood = "0";
                }
                return Json(new
                {
                    fldId = q.fldId,
                    fldDefulteOpertionId = q.fldDefulteOpertionId.ToString(),
                    TypeVorood = TypeVorood,
                    fldName = q.fldName,
                    fldDesc = q.fldDesc,
                    fldFileId = q.fldFileId,
                    fldFileName = q.fldFileName,
                    Status = Status,
                    fldTimeStamp = q.fldtimeStamp,
                    fldIcon = Iconn,
                    //fldAnvaMoayenatId = q.fldAnvaMoayenatId,
                    fldNoeGhebelCharkheshId = q.fldNoeGhebelCharkheshId.ToString()
                }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DetailReferralRuls(/*int CharkheId,int ActionId, */int OprationId)
        {
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
                Models.RaiSamEntities m = new RaiSamEntities();
                var q = m.prs_tblOperationSelect("fldId", OprationId.ToString(),0,1).FirstOrDefault();
              
                return Json(new
                {
                    fldDataTypeId = q.fldDataTypeId,
                    tooltip = q.fldDescAction
                    //fldValue = "",
                    //fldNextActionId = "0",
                    //fldId = 0
                }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ReadDetail(StoreRequestParameters parameters, int CharkheId)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
                if (Permission.haveAccess(116, "tblCharkhe_Eghdam","0"))
                {
                    Models.RaiSamEntities m = new RaiSamEntities();
                    List<prs_tblCharkhe_EghdamSelect> data = null;
                    data = m.prs_tblCharkhe_EghdamSelect("fldCharkheId", CharkheId.ToString(),"",0).ToList();
                    //-- start paging ------------------------------------------------------------
                    int limit = parameters.Limit;

                    if ((parameters.Start + parameters.Limit) > data.Count)
                    {
                        limit = data.Count - parameters.Start;
                    }

                    List<prs_tblCharkhe_EghdamSelect> rangeData = (parameters.Start < 0 || limit < 0) ? data : data.GetRange(parameters.Start, limit);
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
        public ActionResult ReadOP_AC(StoreRequestParameters parameters, int Charkhe_EghdamId)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
                if (Permission.haveAccess(156, "fldCharkhe_EghdamId","0"))
                {
                    Models.RaiSamEntities m = new RaiSamEntities();
                    List<prs_tblOpertion_ActionSelect> data = null;
                    data = m.prs_tblOpertion_ActionSelect("fldCharkhe_EghdamId", Charkhe_EghdamId.ToString(),"","",0).ToList();
                    //-- start paging ------------------------------------------------------------
                    int limit = parameters.Limit;

                    if ((parameters.Start + parameters.Limit) > data.Count)
                    {
                        limit = data.Count - parameters.Start;
                    }

                    List<prs_tblOpertion_ActionSelect> rangeData = (parameters.Start < 0 || limit < 0) ? data : data.GetRange(parameters.Start, limit);
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
                if (Permission.haveAccess(116, "tblCharkhe","0"))
                {
                    Models.RaiSamEntities m = new RaiSamEntities();
                    var filterHeaders = new FilterHeaderConditions(this.Request.Params["filterheader"]);
                    UserInfo user = (UserInfo)(Session["User"]);
                    List<prs_tblCharkheSelect> data = null;
                    if (filterHeaders.Conditions.Count > 0)
                    {
                        string field = "";
                        string searchtext = "";
                        List<prs_tblCharkheSelect> data1 = null;
                        foreach (var item in filterHeaders.Conditions)
                        {
                            var ConditionValue = (Newtonsoft.Json.Linq.JValue)item.ValueProperty.Value;

                            switch (item.FilterProperty.Name)
                            {
                                case "fldId":
                                    searchtext = ConditionValue.Value.ToString();
                                    field = "fldId";
                                    break;
                                case "fldName":
                                    searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                    field = "fldName";
                                    break;
                                case "fldDesc":
                                    searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                    field = "fldDesc";
                                    break;
                                case "fldStatusName":
                                    searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                    field = "fldStatusName";
                                    break;
                                case "fldNameOpertion":
                                    searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                    field = "fldNameOpertion";
                                    break;
                                case "fldAnvaGhabelCharkheshName":
                                    searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                    field = "fldAnvaGhabelCharkheshName";
                                    break;
                                case "fldTitleMoayenat":
                                    searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                    field = "fldTitleMoayenat";
                                    break;
                            }
                            if (data != null)
                                data1 = m.prs_tblCharkheSelect(field, searchtext,u.UserSecondId,100).ToList();
                            else
                                data = m.prs_tblCharkheSelect(field, searchtext, u.UserSecondId, 100).ToList();
                        }
                        if (data != null && data1 != null)
                            data.Intersect(data1);
                    }
                    else
                    {
                        data = m.prs_tblCharkheSelect("", "", u.UserSecondId, 100).ToList();
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

                    List<prs_tblCharkheSelect> rangeData = (parameters.Start < 0 || limit < 0) ? data : data.GetRange(parameters.Start, limit);
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
        public ActionResult ReadReferralRules(StoreRequestParameters parameters, int CharkheId)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
                if (Permission.haveAccess(149, "tblReferralRules", "0"))
                {
                    var filterHeaders = new FilterHeaderConditions(this.Request.Params["filterheader"]);
                    Models.RaiSamEntities m = new RaiSamEntities();
                    List<prs_tblReferralRulesSelect> data = null;
                    if (filterHeaders.Conditions.Count > 0)
                    {
                        string field = "";
                        string searchtext = "";
                        List<prs_tblReferralRulesSelect> data1 = null;
                        foreach (var item in filterHeaders.Conditions)
                        {
                            var ConditionValue = (Newtonsoft.Json.Linq.JValue)item.ValueProperty.Value;

                            switch (item.FilterProperty.Name)
                            {
                                case "fldId":
                                    searchtext = ConditionValue.Value.ToString();
                                    field = "Charkhe_Id";
                                    break;
                                case "fldNameAction":
                                    searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                    field = "Charkhe_NameAction";
                                    break;
                                case "fldNameOpertion":
                                    searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                    field = "Charkhe_NameOpertion";
                                    break;
                                case "fldValue":
                                    searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                    field = "Charkhe_Value";
                                    break;
                                case "fldNameNextAction":
                                    searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                    field = "Charkhe_NameNextAction";
                                    break;
                                case "fldOrderName":
                                    searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                    field = "Charkhe_OrderName";
                                    break;
                                case "fldDesc":
                                    searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                    field = "Charkhe_Desc";
                                    break;

                            }
                            if (data != null)
                                data1 = m.prs_tblReferralRulesSelect(field, searchtext, CharkheId.ToString(),"","",100).ToList();
                            else
                                data = m.prs_tblReferralRulesSelect(field, searchtext, CharkheId.ToString(), "", "", 100).ToList();
                        }
                        if (data != null && data1 != null)
                            data.Intersect(data1);
                    }
                    else
                    {
                        data = m.prs_tblReferralRulesSelect("Charkhe","",CharkheId.ToString(),"","",100).ToList();
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

                    List<prs_tblReferralRulesSelect> rangeData = (parameters.Start < 0 || limit < 0) ? data : data.GetRange(parameters.Start, limit);
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
        public ActionResult ReadAction(StoreRequestParameters parameters, int NoeGhabelCharkheshId)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
                //if (Permission.haveAccess(66))
                //{
                //var filterHeaders = new FilterHeaderConditions(this.Request.Params["filterheader"]);
                Models.RaiSamEntities m = new RaiSamEntities();
                List<prs_tblActionsSelect> data = null;
                //if (filterHeaders.Conditions.Count > 0)
                //{
                //    string field = "";
                //    string searchtext = "";
                //    List<OBJ_Actions> data1 = null;
                //    foreach (var item in filterHeaders.Conditions)
                //    {
                //        var ConditionValue = (Newtonsoft.Json.Linq.JValue)item.ValueProperty.Value;

                //        switch (item.FilterProperty.Name)
                //        {
                //            case "fldId":
                //                searchtext = ConditionValue.Value.ToString();
                //                field = "fldId";
                //                break;
                //            case "fldName":
                //                searchtext = "%" + ConditionValue.Value.ToString() + "%";
                //                field = "fldName";
                //                break;
                //            case "fldDesc":
                //                searchtext = "%" + ConditionValue.Value.ToString() + "%";
                //                field = "fldDesc";
                //                break;

                //        }
                //        param.FieldName = field;
                //        param.Value = searchtext;
                //        param.Top = 100;
                //        if (data != null)
                //            data1 = service.SelectActions(param, user.UserKey, IP).ActionsList.ToList();
                //        else
                //            data = service.SelectActions(param, user.UserKey, IP).ActionsList.ToList();
                //    }
                //    if (data != null && data1 != null)
                //        data.Intersect(data1);
                //}
                //else
                //{
                data = m.prs_tblActionsSelect("fldNoeGhabelCharkheshId", NoeGhabelCharkheshId.ToString(),0).ToList();
                //}

                //var fc = new FilterHeaderConditions(this.Request.Params["filterheader"]);

                //FilterConditions fc = parameters.GridFilters;

                //-- start filtering ------------------------------------------------------------
                //if (fc != null)
                //{
                //    foreach (var condition in fc.Conditions)
                //    {
                //        string field = condition.FilterProperty.Name;
                //        var value = (Newtonsoft.Json.Linq.JValue)condition.ValueProperty.Value;

                //        data.RemoveAll(
                //            item =>
                //            {
                //                object oValue = item.GetType().GetProperty(field).GetValue(item, null);
                //                return !(oValue.ToString().IndexOf(value.ToString(), StringComparison.OrdinalIgnoreCase) >= 0);
                //            }
                //        );
                //    }
                //}
                //-- end filtering ------------------------------------------------------------

                //-- start paging ------------------------------------------------------------
                int limit = parameters.Limit;

                if ((parameters.Start + parameters.Limit) > data.Count)
                {
                    limit = data.Count - parameters.Start;
                }

                List<prs_tblActionsSelect> rangeData = (parameters.Start < 0 || limit < 0) ? data : data.GetRange(parameters.Start, limit);
                //-- end paging ------------------------------------------------------------

                return this.Store(rangeData, data.Count);
                //}
                //else
                //{
                //    return RedirectToAction("Error", "Home", new { Code = "403" });
                //}
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ReadEkhtesas(int CharkheId)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
                Models.RaiSamEntities m = new RaiSamEntities();
                List<prs_tblCharkhe_EghdamSelect> data = null;
                data = m.prs_tblCharkhe_EghdamSelect("fldCharkheId", CharkheId.ToString(),"",0).ToList();
                return this.Store(data);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ReadEkhtesasActions_Cartable(int CartablId)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
                Models.RaiSamEntities m = new RaiSamEntities();

                List<prs_tblEkhtesasActions_CartableSelect> data = null;
                data = m.prs_tblEkhtesasActions_CartableSelect("fldCartableId", CartablId.ToString(),"","",0).ToList();
                return this.Store(data);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ReadListCharkhe(StoreRequestParameters parameters, int CharkheId)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
                if (Permission.haveAccess(149,"","0"))
                {
                    var filterHeaders = new FilterHeaderConditions(this.Request.Params["filterheader"]);
                    Models.RaiSamEntities m = new RaiSamEntities();
                    List<prs_selectListEnterIdByCharkhe> data = null;
                    if (filterHeaders.Conditions.Count > 0)
                    {
                        string field = "";
                        string searchtext = "";
                        List<prs_selectListEnterIdByCharkhe> data1 = null;
                        foreach (var item in filterHeaders.Conditions)
                        {
                            var ConditionValue = (Newtonsoft.Json.Linq.JValue)item.ValueProperty.Value;

                            switch (item.FilterProperty.Name)
                            {
                                case "fldEnterId":
                                    searchtext = ConditionValue.Value.ToString();
                                    field = "fldEnterId";
                                    break;
                                case "fldTitle":
                                    searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                    field = "fldTitle";
                                    break;
                                case "fldTarikhParvande":
                                    searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                    field = "fldTarikhParvande";
                                    break;
                                case "fldNoeEnter":
                                    searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                    field = "fldNoeEnter";
                                    break;
                                case "fldCodeMeli":
                                    searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                    field = "fldCodeMeli";
                                    break;
                                case "fldCodeEnhesari":
                                    searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                    field = "fldCodeEnhesari";
                                    break;
                                case "fldYear":
                                    searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                    field = "fldYear";
                                    break;
                                case "fldTitleMoayenat":
                                    searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                    field = "fldTitleMoayenat";
                                    break;
                            }
                            if (data != null)
                                data1 = m.prs_selectListEnterIdByCharkhe(CharkheId).ToList();
                            else
                                data = m.prs_selectListEnterIdByCharkhe(CharkheId).ToList();
                        }
                        if (data != null && data1 != null)
                            data.Intersect(data1);
                    }
                    else
                    {
                        data = m.prs_selectListEnterIdByCharkhe(CharkheId).ToList();
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

                    List<prs_selectListEnterIdByCharkhe> rangeData = (parameters.Start < 0 || limit < 0) ? data : data.GetRange(parameters.Start, limit);
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
