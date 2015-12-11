using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using InternetStore.Models;
using InternetStore.Classes;
using System.Web.Security;

namespace InternetStore.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        //
        // GET: /Account
        [Authorize]
        [HttpGet]
        public ActionResult Index(string returnUrl)
        {
            ProfileViewModel profile = new ProfileViewModel();

            if (User.Identity.IsAuthenticated)
            {
                using (InternetStoreDBContext dbc = new InternetStoreDBContext())
                {
                    var currentUser = (from u in dbc.Users where u.Email == User.Identity.Name select u).ToList().FirstOrDefault();
                    if (currentUser != null)
                    {
                        profile.UserName = currentUser.UserName ?? "";
                        profile.FirstName = currentUser.FirstName ?? "";
                        profile.LastName = currentUser.LastName ?? "";
                        profile.Email = currentUser.Email ?? "";
                        profile.Phone = currentUser.Phone ?? "";
                        profile.Address = currentUser.Address ?? "";
                    }
                }
            }
            ViewBag.ReturnUrl = returnUrl;
            return View(profile);
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [Authorize]
        public ActionResult Index(ProfileViewModel profileInfo)
        {
            //if (ModelState.IsValid)
            //{
                using (InternetStoreDBContext dbc = new InternetStoreDBContext())
                {
                    var newUserInfo = new Classes.User();
                    var oldUserInfo = (from u in dbc.Users where u.Email == profileInfo.Email select u).ToList().FirstOrDefault();
                    if (oldUserInfo != null)
                    {
                        dbc.Users.DeleteOnSubmit(oldUserInfo);
                        dbc.SubmitChanges();
                        
                        //Currently constant fields:
                        newUserInfo.ID = oldUserInfo.ID;
                        newUserInfo.UserName = oldUserInfo.UserName;
                        newUserInfo.Password = oldUserInfo.Password;
                        //Changable fields:
                        newUserInfo.FirstName = profileInfo.FirstName;
                        newUserInfo.LastName = profileInfo.LastName;
                        newUserInfo.Phone = profileInfo.Phone;
                        newUserInfo.Address = profileInfo.Address;
                        newUserInfo.Email = profileInfo.Email;

                        dbc.Users.InsertOnSubmit(newUserInfo);
                        dbc.SubmitChanges();

                        ProfileViewModel newProfile = new ProfileViewModel();
                        newProfile.UserName = newUserInfo.UserName ?? "";
                        newProfile.FirstName = newUserInfo.FirstName ?? "";
                        newProfile.LastName = newUserInfo.LastName ?? "";
                        newProfile.Email = newUserInfo.Email ?? "";
                        newProfile.Phone = newUserInfo.Phone ?? "";
                        newProfile.Address = newUserInfo.Address ?? "";

                        return View(newProfile);
                    }
                } 
            //}
            return View(profileInfo);
        }

        //
        // GET: /Account/Login
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
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            bool result = false;
            using (InternetStoreDBContext dbc = new InternetStoreDBContext())
            {
                var userInfo = (from u in dbc.Users where u.Email == model.Email select u).ToList().FirstOrDefault();
                if (userInfo != null && userInfo.Password == model.Password)
                {
                    result = true;
                }
            }

            if (result)
            {
                FormsAuthentication.SetAuthCookie(model.Email, false);
                return Redirect(returnUrl ?? Url.Action("Index", "Home"));
            }
            else
            {
                ModelState.AddModelError("", "Invalid login attempt.");
                return View(model);
            }
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
        public ActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                using (InternetStoreDBContext dbc = new InternetStoreDBContext())
                {
                    var userInfo = (from u in dbc.Users where u.Email == model.Email select u).ToList().FirstOrDefault();
                    if (userInfo == null)
                    {
                        var user = new Classes.User()
                        {
                            UserName = model.UserName,
                            FirstName = model.FirstName,
                            LastName = model.LastName,
                            Phone = model.Phone,
                            Email = model.Email,
                            Password = model.Password
                        };

                        dbc.Users.InsertOnSubmit(user);
                        dbc.SubmitChanges();

                        FormsAuthentication.SetAuthCookie(model.Email, false);
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Email address already in use.");
                    }
                }
            }
            // If we got this far, something failed, redisplay form
            return View(model);
        }

        ////
        //// GET: /Account/ForgotPassword
        //[AllowAnonymous]
        //public ActionResult ForgotPassword()
        //{
        //    return View();
        //}

        ////
        //// POST: /Account/ForgotPassword
        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var user = await UserManager.FindByNameAsync(model.Email);
        //        if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
        //        {
        //            // Don't reveal that the user does not exist or is not confirmed
        //            return View("ForgotPasswordConfirmation");
        //        }

        //        // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
        //        // Send an email with this link
        //        // string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
        //        // var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);		
        //        // await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
        //        // return RedirectToAction("ForgotPasswordConfirmation", "Account");
        //    }

        //    // If we got this far, something failed, redisplay form
        //    return View(model);
        //}

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}