using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;
using WebMatrix.WebData;
using RaiSam.Filters;
using RaiSam.Models;
using System.IO;
using System.Net.NetworkInformation;

namespace RaiSam.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    public class AccountController : Controller
    {
        //
        // GET: /Account/Login
        string IP = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_HOST"];
        [AllowAnonymous]
        public ActionResult LogOn()
        {
            if (Session["captchahave"] == null)
                Session["captchahave"] = 0;
            ViewBag.Title = "ورود به سامانه";
            ViewBag.captchahave = Convert.ToInt32(Session["captchahave"]);
            return View();
        }
        [AllowAnonymous]
        public FileContentResult generateCaptcha(string dc)
        {
            System.Drawing.FontFamily family = new System.Drawing.FontFamily("tahoma");
            //CaptchaImage img = new CaptchaImage(90, 40, family);
            CaptchaImage img = new CaptchaImage(110, 40, family);
            string text = img.CreateRandomText(5);
            //text = text.ToUpper();
            //text = text.Replace("O", "P").Replace("0", "2").Replace("1", "3").Replace("I", "M");
            img.SetText(text);
            img.GenerateImage();
            MemoryStream stream = new MemoryStream();
            img.Image.Save(stream,
            System.Drawing.Imaging.ImageFormat.Png);
            Session["captchaLogin"] = text;
            return File(stream.ToArray(), "jpg");
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult DisableUser(string UserName)
        {
            Models.RaiSamEntities m = new RaiSamEntities();
            byte Er = 0;
            try
            {
               
                var user = m.prs_tblUserSelect("fldUserName", UserName,"",0).FirstOrDefault();
                if (user != null)
                {
                  
                    m.prs_UpdateTypeUser(user.fldId,false);
                }
            }
            catch (Exception x)
            {
                System.Data.Entity.Core.Objects.ObjectParameter ErrorId = new System.Data.Entity.Core.Objects.ObjectParameter("fldID", typeof(int));
                string InnerException = "";
                if (x.InnerException != null)
                    InnerException = x.InnerException.Message;
                else
                    InnerException = x.Message;
                m.prs_tblErrorInsert(ErrorId, InnerException, DateTime.Now, null, "");
                return Json(new
                {
                    MsgTitle = "خطا",
                    Msg = "خطایی با شماره: " + ErrorId.Value + " رخ داده است لطفا با پشتیبانی تماس گرفته و کد خطا را اعلام فرمایید.",
                    Err = 1
                });
            }
            return Json(new
            {
                Er = Er
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Vorod(string UserName, string Password, string Capthalogin/*, string TypeUser, string CodeEnhesari*/)
        {
            string Msg1 = ""; var flag = false; var Er = 0; string MsgTitle = ""; string Key = Guid.NewGuid().ToString();
            byte UserType = 1;
            Models.RaiSamEntities model = new RaiSamEntities();
            UserInfo user = new UserInfo();
            try
            {
                //  Models.RaiKMEntities m = new Models.RaiKMEntities();
                //if (ModelState.IsValid)
                //{
                //if (TypeUser == "1")
                //{
                    if (Convert.ToInt32(Session["captchahave"]) > 4)
                    {

                        var Existuser = model.prs_tblUserSelect("fldUserName", UserName, "", 1).FirstOrDefault();

                        if (Existuser != null && Existuser.fldType == false)
                        {
                            Session["captchahave"] = Convert.ToInt32(Session["captchahave"]) + 1;
                            var q = model.prs_GetDate().FirstOrDefault().fldDateTime;
                            var min = Existuser.fldDate - q;
                            var m = Math.Abs(min.Minutes);
                            var min2 = 15;
                            if (m == 0)
                                m = min2;
                            else
                                m = min2 - m;
                            if (m <= 15 && m > 0)
                                return Json(new
                                {
                                    Msg = "کاربری شما غیرفعال شده است. لطفا" + m + " دقیقه دیگر مجدد امتحان نمایید.",
                                    MsgTitle = "ورود ناموفق",
                                    flag = flag,
                                    captchahave = Session["captchahave"]
                                }, JsonRequestBehavior.AllowGet);
                            else
                            {
                                
                                
                                model.prs_UpdateTypeUser(Existuser.fldId,true);
                            }
                        }
                    }
                    if (Convert.ToInt32(Session["captchahave"]) > 1)
                    {
                        if (Capthalogin == "")
                        {
                            Session["captchaLogin"] = "Error";
                            MsgTitle = "خطا";
                            Msg1 = "لطفا کد امنیتی را وارد نمایید.";
                            Er = 1;
                            Session["captchahave"] = Convert.ToInt32(Session["captchahave"]) + 1;
                            return Json(new
                            {
                                Msg = Msg1,
                                MsgTitle = MsgTitle,
                                flag = flag,
                                captchahave = Session["captchahave"]
                            }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            if (Capthalogin.ToLower() != Session["captchaLogin"].ToString().ToLower())
                            {
                                Session["captchaLogin"] = "Error";
                                MsgTitle = "خطا";
                                Msg1 = "لطفا کد امنیتی را صحیح وارد نمایید.";
                                Er = 1;
                                Session["captchahave"] = Convert.ToInt32(Session["captchahave"]) + 1;
                                return Json(new
                                {
                                    Msg = Msg1,
                                    MsgTitle = MsgTitle,
                                    flag = flag,
                                    captchahave = Session["captchahave"]
                                }, JsonRequestBehavior.AllowGet);
                            }
                        }
                    }

                   
                    var User = model.prs_tblUserSelect("checkPass", UserName, CodeDecode.ComputeSha256Hash(Password),1).FirstOrDefault();
                    if (User != null)
                    {
                        if (User.fldActive_Deactive == true)
                        {
                            if (User.fldType == false)
                            {
                                var q = model.prs_GetDate().FirstOrDefault().fldDateTime;
                                var min = User.fldDate - q;
                                var m = Math.Abs(min.Minutes);
                                var min2 = 15;
                                if (m == 0)
                                    m = min2;
                                else
                                    m = min2 - m;
                                if (m <= 15 && m > 0)
                                {
                                    return Json(new
                                    {
                                        Msg = "کاربری شما غیرفعال شده است. لطفا" + m + " دقیقه دیگر مجدد امتحان نمایید.",
                                        MsgTitle = "ورود ناموفق",
                                        flag = flag,
                                        captchahave = Session["captchahave"]
                                    }, JsonRequestBehavior.AllowGet);
                                }
                                else
                                {
                                    
                                    
                                    model.prs_UpdateTypeUser(User.fldId,true);
                                    
                                    var User1 = model.prs_tblUserSelect("checkPass", UserName, CodeDecode.ComputeSha256Hash(Password),1).FirstOrDefault();
                                    if (User1 != null)
                                    {
                                        if (User1.fldActive_Deactive == true)
                                        {
                                            if (User1.fldType == true)
                                            {
                                                var macAddr = (from nic in NetworkInterface.GetAllNetworkInterfaces()  /*where nic.OperationalStatus == OperationalStatus.Up*/   select nic.GetPhysicalAddress().ToString()).FirstOrDefault();

                                              

                                                //var IsLogin = service.CheckLogin(param).LimitationMacAddressList.FirstOrDefault();
                                                //if (IsLogin.fldMacValid == "1")
                                                //{
                                                user.UserId = User1.fldId;
                                                user.UserSecondId = User1.fldId;
                                                user.Pass = CodeDecode.ComputeSha256Hash(Password);
                                                user.UserKey = Key.ToString();
                                                user.UserKeyFirst = Key.ToString();
                                                user.UserType = 1;
                                                user.fldSetadi = User.fldSetadi;
                                                //user.TreeId = Convert.ToInt32(User.fldTreeId);
                                                user.UserName = UserName;

                                                string guid = Guid.NewGuid().ToString();
                                                Session["SFToken"] = guid;
                                                Response.Cookies.Add(new HttpCookie("SFToken", guid));
                                                Response.Cookies["SFToken"].HttpOnly = true;
                                                var date = model.prs_GetDate().FirstOrDefault().fldDateTime;

                                               

                                                System.Data.Entity.Core.Objects.ObjectParameter Id = new System.Data.Entity.Core.Objects.ObjectParameter("fldID", typeof(int));
                                                model.prs_tblInputInfoInsert(Id, IP, macAddr, 1, User1.fldId, "", Request.Browser.Browser + " Version:" + Request.Browser.Version, Key, 1, User1.fldId);
                                                
                                            
                                                
                                                var h = model.prs_tblInputInfoSelect("fldUserId", User1.fldId.ToString(),"",true,1).FirstOrDefault();
                                                user.UserInputId = Convert.ToInt32(Id.Value);
                                                user.FirstUserInputId = Convert.ToInt32(Id.Value);
                                                Session["User"] = user;

                                                if (UserLoginCount.userObj.Where(item => item.sessionId == System.Web.HttpContext.Current.Request.Cookies["ASP.NET_SessionId"].Value.ToString()).Count() > 0)
                                                    UserLoginCount.userObj.Remove(UserLoginCount.userObj.Where(item => item.sessionId == System.Web.HttpContext.Current.Request.Cookies["ASP.NET_SessionId"].Value.ToString()).FirstOrDefault());

                                                UserLoginCount.AddOnlineUser(User.fldId.ToString(), User.fldNamePersonal, IP, UserName, h.fldTarikh + " " + h.fldTime /*+ " " +*/ , System.Web.HttpContext.Current.Request.Cookies["ASP.NET_SessionId"].Value.ToString(), "", "");

                                                flag = true;
                                                MsgTitle = "ورود موفق";
                                            }
                                        }
                                    }
                                }
                                //Session["captchahave"] = Convert.ToInt32(Session["captchahave"]) + 1;

                            }
                            if (User.fldType == true)
                            {
                                var macAddr = (from nic in NetworkInterface.GetAllNetworkInterfaces()  /*where nic.OperationalStatus == OperationalStatus.Up*/   select nic.GetPhysicalAddress().ToString()).FirstOrDefault();
                               

                                //var IsLogin = service.CheckLogin(param).LimitationMacAddressList.FirstOrDefault();
                                //if (IsLogin.fldMacValid == "1")
                                //{
                                user.UserId = User.fldId;
                                user.UserSecondId = User.fldId;
                                user.Pass = CodeDecode.ComputeSha256Hash(Password);
                                user.UserKey = Key.ToString();
                                user.UserKeyFirst = Key.ToString();
                                user.fldSetadi = User.fldSetadi;
                                //user.TreeId = Convert.ToInt32(User.fldTreeId);
                                user.UserName = UserName;

                                if (User.fldFirstRigesterId != null)
                                {
                                    UserType = 2;
                                    user.FirstRegisterId = Convert.ToInt32(User.fldFirstRigesterId);
                                    Session["FristRegisterId"] =User.fldFirstRigesterId;
                                }
                                else
                                {
                                    user.FirstRegisterId = 0;
                                }
                                user.UserType = UserType;

                                string guid = Guid.NewGuid().ToString();
                                Session["SFToken"] = guid;
                                Response.Cookies.Add(new HttpCookie("SFToken", guid));
                                Response.Cookies["SFToken"].HttpOnly = true;
                                var date = model.prs_GetDate().FirstOrDefault().fldDateTime;

                                
                                System.Data.Entity.Core.Objects.ObjectParameter Id = new System.Data.Entity.Core.Objects.ObjectParameter("fldID", typeof(int));
                                model.prs_tblInputInfoInsert(Id, IP, macAddr, 1, User.fldId, "", Request.Browser.Browser + " Version:" + Request.Browser.Version, Key, 1, User.fldId);
                                
                                
                                
                                var h = model.prs_tblInputInfoSelect("fldUserId", User.fldId.ToString(),"",true,1).FirstOrDefault();
                                user.UserInputId = Convert.ToInt32(Id.Value);
                                user.FirstUserInputId = Convert.ToInt32(Id.Value);
                                Session["User"] = user;
                                if (UserLoginCount.userObj.Where(item => item.sessionId == System.Web.HttpContext.Current.Request.Cookies["ASP.NET_SessionId"].Value.ToString()).Count() > 0)
                                    UserLoginCount.userObj.Remove(UserLoginCount.userObj.Where(item => item.sessionId == System.Web.HttpContext.Current.Request.Cookies["ASP.NET_SessionId"].Value.ToString()).FirstOrDefault());

                                UserLoginCount.AddOnlineUser(User.fldId.ToString(), User.fldNamePersonal, IP, UserName, h.fldTarikh + " " + h.fldTime /*+ " " +*/ , System.Web.HttpContext.Current.Request.Cookies["ASP.NET_SessionId"].Value.ToString(), "", "");

                                flag = true;
                                MsgTitle = "ورود موفق";
                                //}
                                //else
                                //{
                                //    var msgg = "کاربر عزیز، محدودیت ورود شما به سامانه بدین ترتیب است:" + "</br></br>";
                                //    param.UserId = User.fldId;
                                //    var info = service.InfoLimitationUser(param).LimitationUserList.FirstOrDefault();
                                //    var arrayRooz = info.LimRoz.Split('،').Reverse().Skip(1);
                                //    foreach (var item in arrayRooz)
                                //    {
                                //        msgg = msgg + "روز: " + item + "</br></br>";
                                //    }
                                //    if (info.LimIP != "")
                                //    {
                                //        msgg = msgg + "<div style=text-align:left;direction:ltr;>" + "IP: " + info.LimIP.Substring(0, info.LimIP.Length - 2) + "</div></br>";
                                //    }
                                //    if (info.LimMac != "")
                                //    {
                                //        msgg = msgg + "<div style=text-align:left;direction:ltr;>" + "MAC Address: " + info.LimMac.Substring(0, info.LimMac.Length - 2) + "</div>";
                                //    }
                                //    Msg1 = msgg;
                                //    MsgTitle = "ورود ناموفق";
                                //}
                            }

                        }
                        else
                        {
                            Msg1 = "شما مجاز به دسترسی نمی باشید.";
                            MsgTitle = "ورود ناموفق";
                            //Session["captchahave"] = Convert.ToInt32(Session["captchahave"]) + 1;
                        }
                    }
                    else
                    {
                        Msg1 = "نام کاربری یا رمز عبور صحیح نمی باشد.";
                        MsgTitle = "ورود ناموفق";
                        Session["captchahave"] = Convert.ToInt32(Session["captchahave"]) + 1;
                    }
                //}
                
            }
            catch (Exception e)
            {
                throw;
            }
            return Json(new
            {
                Msg = Msg1,
                MsgTitle = MsgTitle,
                flag = flag,
                UserType = UserType,
                captchahave = Session["captchahave"]
            }, JsonRequestBehavior.AllowGet);
        }
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid && WebSecurity.Login(model.UserName, model.Password, persistCookie: model.RememberMe))
            {
                return RedirectToLocal(returnUrl);
            }

            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "The user name or password provided is incorrect.");
            return View(model);
        }

        //
        // POST: /Account/LogOff

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            WebSecurity.Logout();

            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/Register

        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                try
                {
                    WebSecurity.CreateUserAndAccount(model.UserName, model.Password);
                    WebSecurity.Login(model.UserName, model.Password);
                    return RedirectToAction("Index", "Home");
                }
                catch (MembershipCreateUserException e)
                {
                    ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // POST: /Account/Disassociate

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Disassociate(string provider, string providerUserId)
        {
            string ownerAccount = OAuthWebSecurity.GetUserName(provider, providerUserId);
            ManageMessageId? message = null;

            // Only disassociate the account if the currently logged in user is the owner
            if (ownerAccount == User.Identity.Name)
            {
                // Use a transaction to prevent the user from deleting their last login credential
                using (var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Serializable }))
                {
                    bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
                    if (hasLocalAccount || OAuthWebSecurity.GetAccountsFromUserName(User.Identity.Name).Count > 1)
                    {
                        OAuthWebSecurity.DeleteAccount(provider, providerUserId);
                        scope.Complete();
                        message = ManageMessageId.RemoveLoginSuccess;
                    }
                }
            }

            return RedirectToAction("Manage", new { Message = message });
        }

        //
        // GET: /Account/Manage

        public ActionResult Manage(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : "";
            ViewBag.HasLocalPassword = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            ViewBag.ReturnUrl = Url.Action("Manage");
            return View();
        }

        //
        // POST: /Account/Manage

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Manage(LocalPasswordModel model)
        {
            bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            ViewBag.HasLocalPassword = hasLocalAccount;
            ViewBag.ReturnUrl = Url.Action("Manage");
            if (hasLocalAccount)
            {
                if (ModelState.IsValid)
                {
                    // ChangePassword will throw an exception rather than return false in certain failure scenarios.
                    bool changePasswordSucceeded;
                    try
                    {
                        changePasswordSucceeded = WebSecurity.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword);
                    }
                    catch (Exception)
                    {
                        changePasswordSucceeded = false;
                    }

                    if (changePasswordSucceeded)
                    {
                        return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
                    }
                    else
                    {
                        ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                    }
                }
            }
            else
            {
                // User does not have a local password so remove any validation errors caused by a missing
                // OldPassword field
                ModelState state = ModelState["OldPassword"];
                if (state != null)
                {
                    state.Errors.Clear();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        WebSecurity.CreateAccount(User.Identity.Name, model.NewPassword);
                        return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
                    }
                    catch (Exception)
                    {
                        ModelState.AddModelError("", String.Format("Unable to create local account. An account with the name \"{0}\" may already exist.", User.Identity.Name));
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // POST: /Account/ExternalLogin

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            return new ExternalLoginResult(provider, Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/ExternalLoginCallback

        [AllowAnonymous]
        public ActionResult ExternalLoginCallback(string returnUrl)
        {
            AuthenticationResult result = OAuthWebSecurity.VerifyAuthentication(Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
            if (!result.IsSuccessful)
            {
                return RedirectToAction("ExternalLoginFailure");
            }

            if (OAuthWebSecurity.Login(result.Provider, result.ProviderUserId, createPersistentCookie: false))
            {
                return RedirectToLocal(returnUrl);
            }

            if (User.Identity.IsAuthenticated)
            {
                // If the current user is logged in add the new account
                OAuthWebSecurity.CreateOrUpdateAccount(result.Provider, result.ProviderUserId, User.Identity.Name);
                return RedirectToLocal(returnUrl);
            }
            else
            {
                // User is new, ask for their desired membership name
                string loginData = OAuthWebSecurity.SerializeProviderUserId(result.Provider, result.ProviderUserId);
                ViewBag.ProviderDisplayName = OAuthWebSecurity.GetOAuthClientData(result.Provider).DisplayName;
                ViewBag.ReturnUrl = returnUrl;
                return View("ExternalLoginConfirmation", new RegisterExternalLoginModel { UserName = result.UserName, ExternalLoginData = loginData });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLoginConfirmation(RegisterExternalLoginModel model, string returnUrl)
        {
            string provider = null;
            string providerUserId = null;

            if (User.Identity.IsAuthenticated || !OAuthWebSecurity.TryDeserializeProviderUserId(model.ExternalLoginData, out provider, out providerUserId))
            {
                return RedirectToAction("Manage");
            }

            if (ModelState.IsValid)
            {
                // Insert a new user into the database
                using (UsersContext db = new UsersContext())
                {
                    UserProfile user = db.UserProfiles.FirstOrDefault(u => u.UserName.ToLower() == model.UserName.ToLower());
                    // Check if user already exists
                    if (user == null)
                    {
                        // Insert name into the profile table
                        db.UserProfiles.Add(new UserProfile { UserName = model.UserName });
                        db.SaveChanges();

                        OAuthWebSecurity.CreateOrUpdateAccount(provider, providerUserId, model.UserName);
                        OAuthWebSecurity.Login(provider, providerUserId, createPersistentCookie: false);

                        return RedirectToLocal(returnUrl);
                    }
                    else
                    {
                        ModelState.AddModelError("UserName", "User name already exists. Please enter a different user name.");
                    }
                }
            }

            ViewBag.ProviderDisplayName = OAuthWebSecurity.GetOAuthClientData(provider).DisplayName;
            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // GET: /Account/ExternalLoginFailure

        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        [AllowAnonymous]
        [ChildActionOnly]
        public ActionResult ExternalLoginsList(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return PartialView("_ExternalLoginsListPartial", OAuthWebSecurity.RegisteredClientData);
        }

        [ChildActionOnly]
        public ActionResult RemoveExternalLogins()
        {
            ICollection<OAuthAccount> accounts = OAuthWebSecurity.GetAccountsFromUserName(User.Identity.Name);
            List<ExternalLogin> externalLogins = new List<ExternalLogin>();
            foreach (OAuthAccount account in accounts)
            {
                AuthenticationClientData clientData = OAuthWebSecurity.GetOAuthClientData(account.Provider);

                externalLogins.Add(new ExternalLogin
                {
                    Provider = account.Provider,
                    ProviderDisplayName = clientData.DisplayName,
                    ProviderUserId = account.ProviderUserId,
                });
            }

            ViewBag.ShowRemoveButton = externalLogins.Count > 1 || OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            return PartialView("_RemoveExternalLoginsPartial", externalLogins);
        }

        #region Helpers
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
        }

        internal class ExternalLoginResult : ActionResult
        {
            public ExternalLoginResult(string provider, string returnUrl)
            {
                Provider = provider;
                ReturnUrl = returnUrl;
            }

            public string Provider { get; private set; }
            public string ReturnUrl { get; private set; }

            public override void ExecuteResult(ControllerContext context)
            {
                OAuthWebSecurity.RequestAuthentication(Provider, ReturnUrl);
            }
        }

        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion
    }
}
