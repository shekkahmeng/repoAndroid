using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using ConferenceManagementSystem.Models;
using ConferenceManagementSystem.DataAccessLayer;
using ConferenceManagementSystem.Utilities;
using System.Web.Security;
using ConferenceManagementSystem.Filters;

namespace ConferenceManagementSystem.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private ConferenceManagementContext db = new ConferenceManagementContext();


        //GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }


        //POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")] 
        public ActionResult Login(Admin model)
        {
            try
            {
                //if login fields are empty, redirect to Login page
                if (model.Username == null || model.Password == null)
                {
                    return View(model);
                }

                else
                {
                    //get current IP
                    string ipAddress = Request.ServerVariables["REMOTE_ADDR"].ToString();

                    var admin = db.Admins.FirstOrDefault(u => u.Username == model.Username);
                    var conference = db.Conferences.FirstOrDefault(u => u.Username == model.Username);
                    var user = db.Users.FirstOrDefault(u => u.Username == model.Username);
                    if (admin != null)
                    {
                        if (Utilities.Function.LoginAsAdmin(model.Username, model.Password) == true)
                        {
                            Session["sessionLoggedInUserName"] = model.Username;
                            Session["sessionLoggedInUserId"] = admin.Id;
                            admin.LoggedIn = true;
                            FormsAuthentication.SetAuthCookie(model.Username, false);
                            return RedirectToAction("Index", "Conference");
                        }
                    }
                    else if (conference != null)
                    {
                        if (Utilities.Function.LoginAsOrganizer(model.Username, model.Password) == true)
                        {
                            Session["sessionLoggedInUserName"] = model.Username;
                            Session["sessionLoggedInUserId"] = conference.ConferenceId;
                            conference.LoggedIn = true;
                            FormsAuthentication.SetAuthCookie(model.Username, false);
                            return RedirectToAction("Menu", "Conference", new { id = conference.ConferenceId });
                        }
                    }
                    else if (User != null)
                    {
                        if (Utilities.Function.LoginAsUser(model.Username, model.Password) == true)
                        {
                            Session["sessionLoggedInUserName"] = model.Username;
                            Session["sessionLoggedInUserId"] = user.UserId;
                            user.LoggedIn = true;
                            FormsAuthentication.SetAuthCookie(model.Username, false);
                            return RedirectToAction("Index", "Main");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Incorrect username / password. Please try again.");
                    }
                }
                return View(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error! " + ex.Message);
                return RedirectToAction("Login");
            }
        }


        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            var admin = db.Admins.FirstOrDefault(u => u.Username == System.Web.HttpContext.Current.User.Identity.Name);
            var conference = db.Conferences.FirstOrDefault(u => u.Username == System.Web.HttpContext.Current.User.Identity.Name);
            var user = db.Users.FirstOrDefault(u => u.Username == System.Web.HttpContext.Current.User.Identity.Name);
            if (admin != null)
            {
                admin.LoggedIn = false;
                FormsAuthentication.SignOut();
                Session.Clear();
                return RedirectToAction("Login", "Account");
            }
            else if (conference != null)
            {
                conference.LoggedIn = false;
                FormsAuthentication.SignOut();
                Session.Clear();
                return RedirectToAction("Login", "Account");
            }
            else
            {
                user.LoggedIn = false;
                FormsAuthentication.SignOut();
                Session.Clear();
                return RedirectToAction("Login", "Account");
            }
        }

        //private bool IsAttemptValid(User existUser, string userName, string password, string ip)
        //{
        //    try
        //    {
        //        bool isValid = false;

        //        existUser.Password = Utilities.Function.Decrypt(existUser.EncryptPassword);
        //        existUser.ConfirmedPassword = existUser.Password;

        //        if (existUser.Attempts < 5)
        //        {
        //            //compare password entered
        //            if (existUser.Password == password)
        //            {
        //                //status checking
        //                //if active
        //                if (existUser.StatusId == 1)
        //                {
        //                    existUser.Attempts = 0;
        //                    existUser.LoggedInIp = ip;
        //                    existUser.LoggedIn = true;
        //                    isValid = true;
        //                }
        //                //if disabled
        //                else if (existUser.StatusId == 2)
        //                {
        //                    ModelState.AddModelError("", "Unable to log in from disallowed IP address.");
        //                }
        //                //if suspended
        //                else
        //                {
        //                    ModelState.AddModelError("", "Too many failed login attempts. Account has been locked.");
        //                }
        //            }
        //            //if fail login, increment attempts by 1
        //            else
        //            {
        //                existUser.Attempts++;
        //                ModelState.AddModelError("", "Incorrect username / password. Please try again.");
        //            }
        //        }
        //        //if fail login attempt  more than 10
        //        //user account suspended
        //        else
        //        {
        //            existUser.StatusId = 3;
        //            ModelState.AddModelError("", "Too many failed login attempts. Account has been locked.");
        //        }
        //        //update user details
        //        db.Entry(existUser).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return isValid;
        //    }
        //    catch (Exception ex)
        //    {
        //        ModelState.AddModelError("", "Error! " + ex.Message);
        //        return false;
        //    }
        //}

        //
        // GET: /Account/Register
        //    [AllowAnonymous]
        //    public ActionResult Register()
        //    {
        //        return View();
        //    }

        //    //
        //    // POST: /Account/Register
        //    [HttpPost]
        //    [AllowAnonymous]
        //    [ValidateAntiForgeryToken]
        //    public async Task<ActionResult> Register(RegisterViewModel model)
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            var user = new ApplicationUser() { UserName = model.UserName };
        //            var result = await UserManager.CreateAsync(user, model.Password);
        //            if (result.Succeeded)
        //            {
        //                await SignInAsync(user, isPersistent: false);
        //                return RedirectToAction("Index", "Home");
        //            }
        //            else
        //            {
        //                AddErrors(result);
        //            }
        //        }

        //        // If we got this far, something failed, redisplay form
        //        return View(model);
        //    }

        //    //
        //    // POST: /Account/Disassociate
        //    [HttpPost]
        //    [ValidateAntiForgeryToken]
        //    public async Task<ActionResult> Disassociate(string loginProvider, string providerKey)
        //    {
        //        ManageMessageId? message = null;
        //        IdentityResult result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
        //        if (result.Succeeded)
        //        {
        //            message = ManageMessageId.RemoveLoginSuccess;
        //        }
        //        else
        //        {
        //            message = ManageMessageId.Error;
        //        }
        //        return RedirectToAction("Manage", new { Message = message });
        //    }

        //    //
        //    // GET: /Account/Manage
        //    public ActionResult Manage(ManageMessageId? message)
        //    {
        //        ViewBag.StatusMessage =
        //            message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
        //            : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
        //            : message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
        //            : message == ManageMessageId.Error ? "An error has occurred."
        //            : "";
        //        ViewBag.HasLocalPassword = HasPassword();
        //        ViewBag.ReturnUrl = Url.Action("Manage");
        //        return View();
        //    }

        //    //
        //    // POST: /Account/Manage
        //    [HttpPost]
        //    [ValidateAntiForgeryToken]
        //    public async Task<ActionResult> Manage(ManageUserViewModel model)
        //    {
        //        bool hasPassword = HasPassword();
        //        ViewBag.HasLocalPassword = hasPassword;
        //        ViewBag.ReturnUrl = Url.Action("Manage");
        //        if (hasPassword)
        //        {
        //            if (ModelState.IsValid)
        //            {
        //                IdentityResult result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
        //                if (result.Succeeded)
        //                {
        //                    return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
        //                }
        //                else
        //                {
        //                    AddErrors(result);
        //                }
        //            }
        //        }
        //        else
        //        {
        //            // User does not have a password so remove any validation errors caused by a missing OldPassword field
        //            ModelState state = ModelState["OldPassword"];
        //            if (state != null)
        //            {
        //                state.Errors.Clear();
        //            }

        //            if (ModelState.IsValid)
        //            {
        //                IdentityResult result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
        //                if (result.Succeeded)
        //                {
        //                    return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
        //                }
        //                else
        //                {
        //                    AddErrors(result);
        //                }
        //            }
        //        }

        //        // If we got this far, something failed, redisplay form
        //        return View(model);
        //    }

        //    //
        //    // POST: /Account/ExternalLogin
        //    [HttpPost]
        //    [AllowAnonymous]
        //    [ValidateAntiForgeryToken]
        //    public ActionResult ExternalLogin(string provider, string returnUrl)
        //    {
        //        // Request a redirect to the external login provider
        //        return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        //    }

        //    //
        //    // GET: /Account/ExternalLoginCallback
        //    [AllowAnonymous]
        //    public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        //    {
        //        var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
        //        if (loginInfo == null)
        //        {
        //            return RedirectToAction("Login");
        //        }

        //        // Sign in the user with this external login provider if the user already has a login
        //        var user = await UserManager.FindAsync(loginInfo.Login);
        //        if (user != null)
        //        {
        //            await SignInAsync(user, isPersistent: false);
        //            return RedirectToLocal(returnUrl);
        //        }
        //        else
        //        {
        //            // If the user does not have an account, then prompt the user to create an account
        //            ViewBag.ReturnUrl = returnUrl;
        //            ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
        //            return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { UserName = loginInfo.DefaultUserName });
        //        }
        //    }

        //    //
        //    // POST: /Account/LinkLogin
        //    [HttpPost]
        //    [ValidateAntiForgeryToken]
        //    public ActionResult LinkLogin(string provider)
        //    {
        //        // Request a redirect to the external login provider to link a login for the current user
        //        return new ChallengeResult(provider, Url.Action("LinkLoginCallback", "Account"), User.Identity.GetUserId());
        //    }

        //    //
        //    // GET: /Account/LinkLoginCallback
        //    public async Task<ActionResult> LinkLoginCallback()
        //    {
        //        var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
        //        if (loginInfo == null)
        //        {
        //            return RedirectToAction("Manage", new { Message = ManageMessageId.Error });
        //        }
        //        var result = await UserManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);
        //        if (result.Succeeded)
        //        {
        //            return RedirectToAction("Manage");
        //        }
        //        return RedirectToAction("Manage", new { Message = ManageMessageId.Error });
        //    }

        //    //
        //    // POST: /Account/ExternalLoginConfirmation
        //    [HttpPost]
        //    [AllowAnonymous]
        //    [ValidateAntiForgeryToken]
        //    public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        //    {
        //        if (User.Identity.IsAuthenticated)
        //        {
        //            return RedirectToAction("Manage");
        //        }

        //        if (ModelState.IsValid)
        //        {
        //            // Get the information about the user from the external login provider
        //            var info = await AuthenticationManager.GetExternalLoginInfoAsync();
        //            if (info == null)
        //            {
        //                return View("ExternalLoginFailure");
        //            }
        //            var user = new ApplicationUser() { UserName = model.UserName };
        //            var result = await UserManager.CreateAsync(user);
        //            if (result.Succeeded)
        //            {
        //                result = await UserManager.AddLoginAsync(user.Id, info.Login);
        //                if (result.Succeeded)
        //                {
        //                    await SignInAsync(user, isPersistent: false);
        //                    return RedirectToLocal(returnUrl);
        //                }
        //            }
        //            AddErrors(result);
        //        }

        //        ViewBag.ReturnUrl = returnUrl;
        //        return View(model);
        //    }


        //    
        //    [AllowAnonymous]
        //    public ActionResult ExternalLoginFailure()
        //    {
        //        return View();
        //    }

        //    [ChildActionOnly]
        //    public ActionResult RemoveAccountList()
        //    {
        //        var linkedAccounts = UserManager.GetLogins(User.Identity.GetUserId());
        //        ViewBag.ShowRemoveButton = HasPassword() || linkedAccounts.Count > 1;
        //        return (ActionResult)PartialView("_RemoveAccountPartial", linkedAccounts);
        //    }

        //    protected override void Dispose(bool disposing)
        //    {
        //        if (disposing && UserManager != null)
        //        {
        //            UserManager.Dispose();
        //            UserManager = null;
        //        }
        //        base.Dispose(disposing);
        //    }

        //    #region Helpers
        //    // Used for XSRF protection when adding external logins
        //    private const string XsrfKey = "XsrfId";

        //    private IAuthenticationManager AuthenticationManager
        //    {
        //        get
        //        {
        //            return HttpContext.GetOwinContext().Authentication;
        //        }
        //    }

        //    private async Task SignInAsync(ApplicationUser user, bool isPersistent)
        //    {
        //        AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
        //        var identity = await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
        //        AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, identity);
        //    }

        //    private void AddErrors(IdentityResult result)
        //    {
        //        foreach (var error in result.Errors)
        //        {
        //            ModelState.AddModelError("", error);
        //        }
        //    }

        //    private bool HasPassword()
        //    {
        //        var user = UserManager.FindById(User.Identity.GetUserId());
        //        if (user != null)
        //        {
        //            return user.PasswordHash != null;
        //        }
        //        return false;
        //    }

        //    public enum ManageMessageId
        //    {
        //        ChangePasswordSuccess,
        //        SetPasswordSuccess,
        //        RemoveLoginSuccess,
        //        Error
        //    }

        //    private ActionResult RedirectToLocal(string returnUrl)
        //    {
        //        if (Url.IsLocalUrl(returnUrl))
        //        {
        //            return Redirect(returnUrl);
        //        }
        //        else
        //        {
        //            return RedirectToAction("Index", "Home");
        //        }
        //    }

        //    private class ChallengeResult : HttpUnauthorizedResult
        //    {
        //        public ChallengeResult(string provider, string redirectUri) : this(provider, redirectUri, null)
        //        {
        //        }

        //        public ChallengeResult(string provider, string redirectUri, string userId)
        //        {
        //            LoginProvider = provider;
        //            RedirectUri = redirectUri;
        //            UserId = userId;
        //        }

        //        public string LoginProvider { get; set; }
        //        public string RedirectUri { get; set; }
        //        public string UserId { get; set; }

        //        public override void ExecuteResult(ControllerContext context)
        //        {
        //            var properties = new AuthenticationProperties() { RedirectUri = RedirectUri };
        //            if (UserId != null)
        //            {
        //                properties.Dictionary[XsrfKey] = UserId;
        //            }
        //            context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
        //        }
        //    }
        //    #endregion
    }
}