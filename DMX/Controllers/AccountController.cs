//using AspNetCoreHero.ToastNotification.Abstractions;

//using DMX.ViewModels;
//using DMXApplication.DTO;
//using DMXDomain.Models;
//using DMXInfrastructure.Data;
//using Microsoft.AspNetCore.Authentication;
//using Microsoft.AspNetCore.Authentication.Cookies;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.DataProtection;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Diagnostics.CodeAnalysis;
//using System.Net;
//using System.Net.Mail;
//using System.Security.Claims;

//namespace DMX.Controllers
//{
//    public class AccountController(XContext dContext, UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, SignInManager<AppUser> signinmanager, IWebHostEnvironment environment, INotyfService  notification, IDataProtectionProvider protectionProvider) : Controller
//    {
        
//        public readonly XContext dcx = dContext;
//        public readonly INotyfService notyf = notification;
//        public readonly UserManager<AppUser> usm = userManager;
//        public readonly RoleManager<AppRole> rol = roleManager;
//        public readonly SignInManager<AppUser> sim = signinmanager;
//        public readonly IWebHostEnvironment env = environment;
//        public readonly IDataProtector protector = protectionProvider.CreateProtector("IdProtector");

//        [HttpGet]
//        public IActionResult AddUser()
//        {
//            return ViewComponent(nameof(AddUser));
//        }

       

//        [HttpGet]
//        public IActionResult EditUser(string Id)
//        {
//            return ViewComponent(nameof(EditUser), Id);
//        }
//        public IActionResult DeleteUser()
//        {
//            return ViewComponent(nameof(DeleteUser));
//        }
  

//        public IActionResult ViewUsers()
//        {
//            return ViewComponent(nameof(ViewUsers));
//        }
//        [AllowAnonymous]
//        [HttpGet]
//        public IActionResult Login(LoginVM loginVM)
//        {
//            return View(loginVM);
//        }

//        [HttpPost]
//        [AllowAnonymous]
//        public async Task<IActionResult> LoginAsync(LoginVM loginVM)
//        {
//            if (!ModelState.IsValid)
//                return View(loginVM);

//            var result = await sim.PasswordSignInAsync(
//                loginVM.Username,
//                loginVM.Password,
//                loginVM.RememberMe,
//                lockoutOnFailure: false);
//            {

//            if (result.Succeeded)

//                return RedirectToAction(nameof(HomeController.Index), "Home");
//            }

//            ModelState.AddModelError("", "Invalid username or password");
//            return View(loginVM);
//        }




//        [HttpPost]
//        [Authorize]
//        public async Task<IActionResult> Logout()
//        {
//            await HttpContext.SignOutAsync(
//                CookieAuthenticationDefaults.AuthenticationScheme);

//            var logoutDTO = new LogoutDTO
//            {
//                Success = true,
//                Message = "Logout successful."
//            };

//            TempData["Message"] = logoutDTO.Message;

//            return RedirectToAction("Login");
//        }
//        public IActionResult ForgetPassword() => View();
//        [HttpGet]
//        public IActionResult UserProfile()
//        {
//            return ViewComponent(nameof(UserProfile));
//        }
//        [HttpGet]
//        public IActionResult EditProfile()
//        {
//            return ViewComponent(nameof(EditProfile));
//        }
//        [HttpPost]
//        public async Task<IActionResult>EditProfile(EditProfileVM editProfileVM,IFormFile? formFile)
//        {
//            //AppUser profileToEdit = (from u in usm.Users
//            //                        where u.Id == usm.GetUserId(htx.User)
//            //                         select u=>new EditProfileVM { Email = editProfileVM.Email }).FirstOrDefault();


         
//            var userId = usm.GetUserId(HttpContext.User);
//            if (userId == null)
//            {
//                return Unauthorized();
//            }
//            var profileToEdit =  await usm.FindByIdAsync(userId);

//            if (profileToEdit == null)
//            {
//                return NotFound();
//            }
//            profileToEdit.Firstname = editProfileVM.Firstname;
//            profileToEdit.Lastname = editProfileVM.Lastname;
//            profileToEdit.PhoneNumber = editProfileVM.Telephone;

//            if (formFile != null)
//            {
//                using var memoryStream = new MemoryStream();


//                await formFile.CopyToAsync(memoryStream);


//                profileToEdit.Picture = memoryStream.ToArray();
//            }

//            await usm.UpdateAsync(profileToEdit);

//            notyf.Success("Profile successfully updated",5);

//            return  RedirectToActionPermanent("Login");
//        }

//        [HttpPost]
//        public async Task<IActionResult> DeletePhoto(string Id)
//        {
//            var userId = protector.Unprotect(Id);

//            var photoToDelete = await usm.FindByIdAsync(userId);

//            if (photoToDelete == null)
//            {
//                return NotFound();
//            }

//            photoToDelete.Picture = null;

//            await usm.UpdateAsync(photoToDelete);

//            notyf.Success("Photo successfully deleted", 5);

//            return RedirectToAction("UserProfile");
//        }

//        [AllowAnonymous]
//        public IActionResult Splash()
//        {
//            return View();
//        }
//        public IActionResult AccessDenied()
//        {
//            return View();
//        }
//        [HttpGet]
//        [AllowAnonymous]
//        public async Task<IActionResult> VerifyEmail(string userId, string token)
//        {
//            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
//            {
//                TempData["Error"] = "Invalid verification link.";
//                return RedirectToAction("Login");
//            }

//            var user = await usm.FindByIdAsync(userId);
//            if (user == null)
//            {
//                TempData["Error"] = "User not found.";
//                return RedirectToAction("Login");
//            }

//            var result = await usm.ConfirmEmailAsync(user, token);
//            if (result.Succeeded)
//            {
//                TempData["Message"] = "Email successfully verified! You can now login.";
//                return RedirectToAction("Login");
//            }
//            else
//            {
//                TempData["Error"] = "Email verification failed.";
//                return RedirectToAction("Login");
//            }
//        }

//        [HttpPost]
//        [AllowAnonymous]
//        public async Task<IActionResult> ResendVerificationEmail(string email)
//        {
//            if (string.IsNullOrEmpty(email))
//            {
//                TempData["Error"] = "Please provide a valid email address.";
//                return RedirectToAction("Login");
//            }

//            var user = await usm.FindByEmailAsync(email);
//            if (user == null)
//            {
//                TempData["Error"] = "No user found with this email.";
//                return RedirectToAction("Login");
//            }

//            if (user.EmailConfirmed)
//            {
//                TempData["Message"] = "Email is already confirmed. You can log in.";
//                return RedirectToAction("Login");
//            }

//            try
//            {
//                // Generate token and confirmation link
//                var token = await usm.GenerateEmailConfirmationTokenAsync(user);
//                var confirmationLink = Url.Action(
//                    nameof(VerifyEmail),
//                    "Account",
//                    new { userId = user.Id, token },
//                    Request.Scheme);

//                // Read email template
//                string body = await System.IO.File.ReadAllTextAsync(
//                    Path.Combine(env.WebRootPath, "Templates", "EmailTemplate", "EmailConfirmationTemplate.cshtml"));
//                body = body.Replace("{UserName}", user.UserName).Replace("{url}", confirmationLink);

//                // Send email
//                using var mailMessage = new MailMessage();
//                mailMessage.Subject = "Email Verification";
//                mailMessage.IsBodyHtml = true;
//                mailMessage.To.Add(user.Email);
//                mailMessage.From = new MailAddress("ospoku@gmail.com");
//                mailMessage.Body = body;

//                using var smtp = new SmtpClient("smtp.gmail.com", 587)
//                {
//                    Credentials = new NetworkCredential("ospoku@gmail.com", "az36400@osp"),
//                    EnableSsl = true
//                };

//                await smtp.SendMailAsync(mailMessage);

//                TempData["Message"] = "Verification email resent successfully. Please check your inbox.";
//            }
//            catch (SmtpException)
//            {
//                TempData["Error"] = "Unable to send email. Please check your internet connection and try again.";
//            }

//            return RedirectToAction("Login");
//        }

//    }
//}



using AspNetCoreHero.ToastNotification.Abstractions;

using DMX.ViewModels;
using DMXApplication.DTO;
using DMXDomain.Models;
using DMXInfrastructure.Data;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace DMX.Controllers
{
    public class AccountController(
        XContext dContext,
        UserManager<AppUser> userManager,
        RoleManager<AppRole> roleManager,
        SignInManager<AppUser> signinmanager,
        IWebHostEnvironment environment,
        INotyfService notification,
        IDataProtectionProvider protectionProvider,
        ILogger<AccountController> logger) : Controller
    {
        public readonly XContext dcx = dContext;
        public readonly INotyfService notyf = notification;
        public readonly UserManager<AppUser> usm = userManager;
        public readonly RoleManager<AppRole> rol = roleManager;
        public readonly SignInManager<AppUser> sim = signinmanager;
        public readonly IWebHostEnvironment env = environment;

        public readonly IDataProtector protector =
            protectionProvider.CreateProtector("IdProtector");

        private readonly ILogger<AccountController> log = logger;


        // ADD USER
        [HttpGet]
        public IActionResult AddUser()
        {
            return ViewComponent(nameof(AddUser));
        }


        // EDIT USER
        [HttpGet]
        public IActionResult EditUser(string Id)
        {
            return ViewComponent(nameof(EditUser), Id);
        }


        // DELETE USER
        public IActionResult DeleteUser()
        {
            return ViewComponent(nameof(DeleteUser));
        }


        // VIEW USERS
        public IActionResult ViewUsers()
        {
            return ViewComponent(nameof(ViewUsers));
        }


        // LOGIN
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login(LoginVM loginVM)
        {
            return View(loginVM);
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginAsync(LoginVM loginVM)
        {
            if (!ModelState.IsValid)
            {
                log.LogWarning(
                    "Login attempted with invalid form data.");

                return View(loginVM);
            }

            try
            {
                log.LogInformation(
                    "Login attempt initiated for username {Username}.",
                    loginVM.Username);

                var result = await sim.PasswordSignInAsync(
                    loginVM.Username,
                    loginVM.Password,
                    loginVM.RememberMe,
                    lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    log.LogInformation(
                        "User {Username} successfully logged in.",
                        loginVM.Username);

                    return RedirectToAction(
                        nameof(HomeController.Index), "Home");
                }

                log.LogWarning(
                    "Login failed for username {Username}.",
                    loginVM.Username);

                ModelState.AddModelError(
                    "", "Invalid username or password");

                return View(loginVM);
            }
            catch (Exception ex)
            {
                log.LogError(
                    ex,
                    "Unexpected error during login for username {Username}.",
                    loginVM.Username);

                ModelState.AddModelError(
                    "",
                    "An unexpected error occurred. Please try again.");

                return View(loginVM);
            }
        }


        // LOGOUT
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            try
            {
                var userId = usm.GetUserId(User);

                await HttpContext.SignOutAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme);

                log.LogInformation(
                    "User {UserId} successfully logged out.",
                    userId);

                TempData["Message"] = "Logout successful.";

                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                log.LogError(
                    ex,
                    "An error occurred while logging out user {UserId}.",
                    usm.GetUserId(User));

                return RedirectToAction("Error", "Home");
            }
        }


        // FORGET PASSWORD
        [AllowAnonymous]
        [HttpGet]
        public IActionResult ForgetPassword()
        {
            return View();
        }


        // USER PROFILE
        [HttpGet]
        public IActionResult UserProfile()
        {
            return ViewComponent(nameof(UserProfile));
        }


        // EDIT PROFILE
        [HttpGet]
        public IActionResult EditProfile()
        {
            return ViewComponent(nameof(EditProfile));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile(
            EditProfileVM editProfileVM,
            IFormFile? formFile)
        {
            if (!ModelState.IsValid)
                return View(editProfileVM);

            try
            {
                var userId = usm.GetUserId(HttpContext.User);

                if (string.IsNullOrEmpty(userId))
                {
                    log.LogWarning(
                        "Unauthenticated user attempted to edit a profile.");

                    return Unauthorized();
                }

                var profileToEdit =
                    await usm.FindByIdAsync(userId);

                if (profileToEdit == null)
                {
                    log.LogWarning(
                        "Profile update failed. User {UserId} was not found.",
                        userId);

                    return NotFound();
                }

                profileToEdit.Firstname = editProfileVM.Firstname;
                profileToEdit.Lastname = editProfileVM.Lastname;
                profileToEdit.PhoneNumber = editProfileVM.Telephone;

                if (formFile != null)
                {
                    using var memoryStream = new MemoryStream();

                    await formFile.CopyToAsync(memoryStream);

                    profileToEdit.Picture = memoryStream.ToArray();
                }

                var result = await usm.UpdateAsync(profileToEdit);

                if (!result.Succeeded)
                {
                    log.LogWarning(
                        "Profile update failed for user {UserId}. Identity errors: {Errors}",
                        userId,
                        string.Join("; ",
                            result.Errors.Select(e => e.Description)));

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(
                            "", error.Description);
                    }

                    return View(editProfileVM);
                }

                log.LogInformation(
                    "Profile successfully updated for user {UserId}.",
                    userId);

                notyf.Success(
                    "Profile successfully updated", 5);

                return RedirectToAction("UserProfile");
            }
            catch (Exception ex)
            {
                log.LogError(
                    ex,
                    "An unexpected error occurred while updating a user profile.");

                notyf.Error(
                    "Unable to update profile.", 5);

                return RedirectToAction("Error", "Home");
            }
        }


        // DELETE PHOTO
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePhoto(string Id)
        {
            try
            {
                if (string.IsNullOrEmpty(Id))
                    return BadRequest();

                string userId;

                try
                {
                    userId = protector.Unprotect(Id);
                }
                catch (Exception ex)
                {
                    log.LogWarning(
                        ex,
                        "Invalid protected user ID supplied for photo deletion.");

                    return BadRequest();
                }

                var photoToDelete =
                    await usm.FindByIdAsync(userId);

                if (photoToDelete == null)
                {
                    log.LogWarning(
                        "Photo deletion failed. User {UserId} was not found.",
                        userId);

                    return NotFound();
                }

                photoToDelete.Picture = null;

                var result = await usm.UpdateAsync(photoToDelete);

                if (!result.Succeeded)
                {
                    log.LogWarning(
                        "Photo deletion failed for user {UserId}. Identity errors: {Errors}",
                        userId,
                        string.Join("; ",
                            result.Errors.Select(e => e.Description)));

                    notyf.Error(
                        "Unable to delete photo.", 5);

                    return RedirectToAction("UserProfile");
                }

                log.LogInformation(
                    "Profile photo successfully deleted for user {UserId}.",
                    userId);

                notyf.Success(
                    "Photo successfully deleted", 5);

                return RedirectToAction("UserProfile");
            }
            catch (Exception ex)
            {
                log.LogError(
                    ex,
                    "An unexpected error occurred while deleting a profile photo.");

                notyf.Error(
                    "An error occurred while deleting the photo.", 5);

                return RedirectToAction("Error", "Home");
            }
        }


        // SPLASH
        [AllowAnonymous]
        public IActionResult Splash()
        {
            return View();
        }


        // ACCESS DENIED
        public IActionResult AccessDenied()
        {
            return View();
        }


        // VERIFY EMAIL
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> VerifyEmail(
            string userId,
            string token)
        {
            if (string.IsNullOrEmpty(userId) ||
                string.IsNullOrEmpty(token))
            {
                log.LogWarning(
                    "Email verification attempted with missing parameters.");

                TempData["Error"] =
                    "Invalid verification link.";

                return RedirectToAction("Login");
            }

            try
            {
                var user = await usm.FindByIdAsync(userId);

                if (user == null)
                {
                    log.LogWarning(
                        "Email verification failed. User {UserId} was not found.",
                        userId);

                    TempData["Error"] = "User not found.";

                    return RedirectToAction("Login");
                }

                var result =
                    await usm.ConfirmEmailAsync(user, token);

                if (result.Succeeded)
                {
                    log.LogInformation(
                        "Email successfully verified for user {UserId}.",
                        userId);

                    TempData["Message"] =
                        "Email successfully verified! You can now login.";
                }
                else
                {
                    log.LogWarning(
                        "Email verification failed for user {UserId}. Errors: {Errors}",
                        userId,
                        string.Join("; ",
                            result.Errors.Select(e => e.Description)));

                    TempData["Error"] =
                        "Email verification failed.";
                }

                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                log.LogError(
                    ex,
                    "An unexpected error occurred while verifying email for user {UserId}.",
                    userId);

                TempData["Error"] =
                    "An error occurred during email verification.";

                return RedirectToAction("Login");
            }
        }


        // RESEND VERIFICATION EMAIL
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResendVerificationEmail(
            string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                TempData["Error"] =
                    "Please provide a valid email address.";

                return RedirectToAction("Login");
            }

            try
            {
                var user = await usm.FindByEmailAsync(email);

                if (user == null)
                {
                    log.LogWarning(
                        "Verification email request received for an unknown email address.");

                    TempData["Error"] =
                        "No user found with this email.";

                    return RedirectToAction("Login");
                }

                if (user.EmailConfirmed)
                {
                    TempData["Message"] =
                        "Email is already confirmed. You can log in.";

                    return RedirectToAction("Login");
                }

                log.LogInformation(
                    "Generating email verification token for user {UserId}.",
                    user.Id);

                var token =
                    await usm.GenerateEmailConfirmationTokenAsync(user);

                var confirmationLink = Url.Action(
                    nameof(VerifyEmail),
                    "Account",
                    new { userId = user.Id, token },
                    Request.Scheme);

                string templatePath = Path.Combine(
                    env.WebRootPath,
                    "Templates",
                    "EmailTemplate",
                    "EmailConfirmationTemplate.cshtml");

                string body =
                    await System.IO.File.ReadAllTextAsync(templatePath);

                body = body
                    .Replace("{UserName}", user.UserName)
                    .Replace("{url}", confirmationLink);

                using var mailMessage = new MailMessage();

                mailMessage.Subject = "Email Verification";
                mailMessage.IsBodyHtml = true;
                mailMessage.To.Add(user.Email);

                mailMessage.From =
                    new MailAddress("ospoku@gmail.com");

                mailMessage.Body = body;

                using var smtp =
                    new SmtpClient("smtp.gmail.com", 587)
                    {
                        Credentials = new NetworkCredential(
                            "ospoku@gmail.com",
                            Environment.GetEnvironmentVariable(
                                "DMX_SMTP_PASSWORD")),

                        EnableSsl = true
                    };

                await smtp.SendMailAsync(mailMessage);

                log.LogInformation(
                    "Verification email successfully sent to user {UserId}.",
                    user.Id);

                TempData["Message"] =
                    "Verification email resent successfully. Please check your inbox.";
            }
            catch (SmtpException ex)
            {
                log.LogError(
                    ex,
                    "SMTP failure while sending verification email.");

                TempData["Error"] =
                    "Unable to send email. Please try again.";
            }
            catch (Exception ex)
            {
                log.LogError(
                    ex,
                    "Unexpected error while resending verification email.");

                TempData["Error"] =
                    "An unexpected error occurred. Please try again.";
            }

            return RedirectToAction("Login");
        }
    }
}

