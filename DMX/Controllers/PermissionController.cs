using DMX.Data;
using DMX.DataProtection;
using DMX.Helpers;
using DMX.Models;
using DMX.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Web;

namespace DMX.Controllers
{
    public class PermissionController : Controller
    {
        public readonly XContext xct;
        public readonly UserManager<AppUser> usm;
        public readonly RoleManager<AppRole> rol;
        public readonly SignInManager<AppUser> sim;
        public readonly IWebHostEnvironment env;
        public PermissionController(XContext xContext, UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, SignInManager<AppUser> signinmanager, IWebHostEnvironment environment)
        {
            usm = userManager;
            xct = xContext;
            rol = roleManager;
            sim = signinmanager;
            env = environment;
        }
        [HttpGet]
        public IActionResult AddUser()
        {
            return ViewComponent(nameof(AddUser));
        }

        public IActionResult UserManagement()
        {
            return ViewComponent(nameof(UserManagement));
        }


        [HttpPost]
        public async Task<IActionResult> AddUser(AddUserVM addUserVM)
        {



            if (ModelState.IsValid)
            {
                AppUser addThisUser = new()
                {

                    UserName = addUserVM.Username,
                    Email = addUserVM.Email,
                    PhoneNumber = addUserVM.Telephone,
                    Firstname = addUserVM.Firstname,
                    Surname = addUserVM.Surname,
                };
                IdentityResult result = await usm.CreateAsync(addThisUser, addUserVM.Password);
                if (result.Succeeded)
                {
                    AppRole applicationRole = await rol.FindByIdAsync(addUserVM.ApplicationRoleId);
                    if (applicationRole == null)
                    {
                    }
                    else
                    {
                        await usm.AddToRoleAsync(addThisUser, applicationRole.Name);

                    }
                    ViewBag.Message = "New User created";


                    string ctoken = usm.GenerateEmailConfirmationTokenAsync(addThisUser).Result;
                    string ctokenLink = Url.Action(nameof(VerifyEmail), "Account", new { userId = addThisUser.Id, token = ctoken }, Request.Scheme);

                    using (MailMessage mailMessage = new())
                    {

                        mailMessage.Subject = "EMAIL VERIFICATION";
                        mailMessage.IsBodyHtml = true;
                        mailMessage.To.Add(addThisUser.Email);
                        mailMessage.From = new MailAddress("ospoku@gmail.com");
                        string body = string.Empty;
                        using (StreamReader reader = new(env.WebRootPath + Path.DirectorySeparatorChar.ToString()
                      + "Templates"
                            + Path.DirectorySeparatorChar.ToString()
                            + "EmailTemplate"
                            + Path.DirectorySeparatorChar.ToString()
                            + "EmailConfirmationTemplate.cshtml"))
                        {
                            body = reader.ReadToEnd();

                        };
                        body = body.Replace("{UserName}", addThisUser.UserName);
                        body = body.Replace("{url}", ctokenLink);
                        mailMessage.Body = body;
                        using (SmtpClient smtp = new())
                        {

                            smtp.Host = "smtp.gmail.com";
                            smtp.Port = 587;
                            smtp.Credentials = new NetworkCredential("ospoku@gmail.com", "az36400@osp");
                            smtp.EnableSsl = true;
                            smtp.Send(mailMessage);
                        };
                    }







                    //we creating the necessary URL string:
                    string URL = "https://frog.wigal.com.gh/ismsweb/sendmsg?";

                    string from = "JHC";
                    string username = "KofiPoku";
                    string password = "Az36400@osp";
                    string to = "233244139692";
                    string messageText = "Testing JHC Message Alerts";

                    // Creating URL to send sms
                    string message = URL
                        + "username="
                        + username
                        + "&password="
                        + password
                        + "&from="
                        + from
                        + "&to="
                        + to
                        + "&service="
                        + "SMS"
                        + "&message="
                        + messageText;



                    ////we creating the necessary URL string:
                    //string URL = "https://frog.wigal.com.gh/api/v2/sendmsg";

                    //string username = "KofiPoku";
                    //string password = "Az36400@osp";

                    //string messageText = "Testing JHC Message Alerts";
                    //string destinations = "233244139692";
                    //string senderid = "JHC";
                    //// Creating URL to send sms
                    //string message = URL
                    //    + "username="
                    //    + username
                    //    + "&password="
                    //    + password
                    //    + "&senderid="
                    //    + senderid
                    //    + "&destinations="
                    //    + destinations
                    //    + "&service="
                    //    + "SMS"
                    //    + "&message="
                    //    + messageText;


                    HttpClient httpclient = new();

                    var response2 = await httpclient.SendAsync(new HttpRequestMessage(HttpMethod.Post, message));
                    if (response2.StatusCode == HttpStatusCode.OK)
                    {
                        // Do something with response. Example get content:
                        // var responseContent = await response.Content.ReadAsStringAsync ().ConfigureAwait (false);
                    }
                    TempData["Message"] = "New User Created";

                    return RedirectToAction("ViewUsers");

                }

            }

            else
            {
                TempData["ResultMessage"] = "User creation error";
            }

            return RedirectToAction("AddUser");
        }
        [HttpGet]
        public IActionResult EditUser(string Id)
        {
            return ViewComponent(nameof(EditUser), Id);
        }

        public IActionResult DetailUser(string Id)
        {
            return ViewComponent(nameof(DetailUser), Id);
        }
        public IActionResult DeleteUser()
        {
            return ViewComponent(nameof(DeleteUser));
        }
        [HttpPost]
        public async Task<IActionResult> EditUserAsync(string Id, AppUser user, EditUserVM editUserVM)
        {
            AppUser searchUser = (from u in usm.Users where u.Id == Id select u).FirstOrDefault();
            if (searchUser != null)
            {

                searchUser.Email = user.Email;
                searchUser.Firstname = user.Firstname;
                searchUser.Surname = user.Surname;
                searchUser.PhoneNumber = user.PhoneNumber;

                searchUser.UserName = user.UserName;

                IdentityResult identityResult = await usm.UpdateAsync(searchUser);
                IdentityResult result = identityResult;
                if (result.Succeeded)
                {
                    string existingRole = usm.GetRolesAsync(searchUser).Result.SingleOrDefault();
                    string existingRoleId = rol.Roles.Single(r => r.Name == existingRole).Id;
                    if (existingRoleId != editUserVM.ApplicationRoleId)
                    {
                        IdentityResult roleResult = await usm.RemoveFromRoleAsync(searchUser, existingRole);
                        if (roleResult.Succeeded)
                        {
                            AppRole applicationRole = await rol.FindByIdAsync(editUserVM.ApplicationRoleId);
                            if (applicationRole != null)
                            {
                                IdentityResult newRoleResult = await usm.AddToRoleAsync(searchUser, applicationRole.Name);
                                if (newRoleResult.Succeeded)
                                {
                                    return RedirectToAction("Users");
                                }
                            }
                        }
                    }
                }
            }


            return View("Users");
        }

        public IActionResult ViewPermissions()
        {
            return ViewComponent(nameof(ViewPermissions));
        }
        public IActionResult ManageUserRoles(string Id)
        {

            return ViewComponent(nameof(ManageUserRoles), Id);
        }

        [HttpPost]
        public async Task<IActionResult> LoginAsync(LoginVM loginVM)
        {
            if (!ModelState.IsValid)
            {

                return View();


            };
            var user = await usm.FindByNameAsync(loginVM.Username);
            var result = await sim.PasswordSignInAsync(loginVM.Username, loginVM.Password, loginVM.RememberMe, false);
            if (result.Succeeded)
            {
                var userClaims = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                userClaims.AddClaim(new Claim("Name", user.UserName));
                userClaims.AddClaim(new Claim("Email", user.Email));
                userClaims.AddClaim(new Claim("Firstname", user.Firstname));
                userClaims.AddClaim(new Claim("Surname", user.Surname));


                userClaims.AddClaim(new Claim(ClaimTypes.Role, string.Join(",", from p in xct.UserRoles
                                                                                join role in xct.Roles on p.RoleId equals role.Id
                                                                                where p.UserId == user.Id
                                                                                select role.Name.ToString())));
                //string.Join(",", from p in gcx.UserRoles
                //                 join role in gcx.Roles on p.RoleId equals role.Id
                //                 where p.UserId == user.Id
                //                 select role.Name.ToString())););
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(userClaims), new AuthenticationProperties { IsPersistent = true });

                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            else
            {
                ViewBag.Message = "login error!";

                return View(loginVM);

            }

        }


        public async Task<IActionResult> VerifyEmail(string userId, string token)
        {

            var user = await usm.FindByIdAsync(userId);
            await usm.ConfirmEmailAsync(user, token);

            return BadRequest();
        }
        public async Task<IActionResult> SendEmailConfirmation(string userId, string token)
        {

            AppUser user = await usm.FindByIdAsync(userId);
            await usm.ConfirmEmailAsync(user, token);


            return BadRequest();
        }

     
        [HttpGet]
        public IActionResult ManagePermissions(string Id)
        {
            return ViewComponent(nameof(ManagePermissions),Id);

        }

        [HttpGet]
        public IActionResult ViewUserRoles()
        {
            return ViewComponent(nameof(ViewUserRoles));
        }
        [HttpGet]
        [Authorize(Roles = "SuperAdmin")]
        public IActionResult ViewRoles()
        {
            return ViewComponent(nameof(ViewRoles));
        }

        [HttpGet]
        public IActionResult AddRole()
        {
            return ViewComponent(nameof(AddRole));
        }


        [HttpPost]
        public async Task<IActionResult> AddRole(AddRoleVM addRoleVM)
        {
            AppRole appRole = new()
            {
                Rolename = addRoleVM.Name,
                Name = addRoleVM.Name,
                Description = addRoleVM.Description,
            };

            await rol.CreateAsync(appRole);

            return RedirectToAction("ViewRoles");
        }
        [HttpGet]
        public IActionResult RolePermissions(string Id)
        {
           

            return ViewComponent(nameof(RolePermissions),Id);
        }
        [HttpPost]
        public async Task<IActionResult> RolePermissions (string Id, RolePermissionVM model)
        {
            var role = await rol.FindByIdAsync(@Encryption.Decrypt(Id));
            var claims = await rol.GetClaimsAsync(role);
            foreach (var claim in claims)
            {
                await rol.RemoveClaimAsync(role, claim);
            }
            var selectedClaims = model.RoleClaims.Where(a => a.Selected).ToList();
            foreach (var claim in selectedClaims)
            {
                await rol.AddPermissionClaim(role, claim.Value);
            }
            
            return RedirectToAction("ViewRoles");
        }

        [HttpPost]
        public async Task<IActionResult> ManageUserRoles(string Id, ManageUserRolesVM model)
        {
            var user = await usm.FindByIdAsync(@Encryption.Decrypt(Id));
            var roles = await usm.GetRolesAsync(user);
            var result = await usm.RemoveFromRolesAsync(user, roles);
            result = await usm.AddToRolesAsync(user, model.UserRoles.Where(x => x.Selected).Select(y => y.RoleName));
            return RedirectToAction("ViewUserRoles");
        }
        [HttpPost]
        public async Task <IActionResult> AddPermission ()
        {

            
            return RedirectToAction(nameof(ViewPermissions));

        }
        [HttpGet]
        public IActionResult AddPermission(AddPermissionVM addPermission)
        {


            return ViewComponent(nameof(AddPermission));

        }

    }
}