using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using DMX.ViewModels;
using DMX.Data;
using DMX.DataProtection;
using DMX.Models;
using Microsoft.AspNetCore.Authorization;
using AspNetCoreHero.ToastNotification.Abstractions;

namespace DMX.Controllers
{
    public class AccountController(XContext dContext, UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, SignInManager<AppUser> signinmanager, INotyfService  notification) : Controller
    {
        public readonly XContext dcx = dContext;
        public readonly INotyfService notyf = notification;
        public readonly UserManager<AppUser> usm = userManager;
        public readonly RoleManager<AppRole> rol = roleManager;
        public readonly SignInManager<AppUser> sim = signinmanager;

        [HttpGet]
        public IActionResult AddUser()
        {
            return ViewComponent("AddUser");
        }
        [HttpPost]
        public async Task<IActionResult> AddUser(AddUserVM addUserVM)
        {
                AppUser addThisUser = new()
                {
                    UserName = addUserVM.Username,
                    Email=addUserVM.Email,
                };
                IdentityResult result = await usm.CreateAsync(addThisUser, addUserVM.Password);
                if (result.Succeeded)
                {
                    AppRole applicationRole = await rol.FindByIdAsync(addUserVM.ApplicationRoleId);
                    if (applicationRole != null)
                    {
                        await usm.AddToRoleAsync(addThisUser, applicationRole.Name);
                    };

                    ViewBag.Message = "New User created";


                    return RedirectToAction("ViewUsers");
                }

            

  

            return RedirectToAction("AddUser");
        }
        [HttpGet]
        public IActionResult EditUser(string Id)
        {
            return ViewComponent("EditUser", Id);
        }
        public IActionResult DeleteUser()
        {
            return ViewComponent("DeleteUser");
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
                                    return RedirectToAction("ViewUsers");
                                }
                            }
                        }
                    }
                }
            }


            return View("ViewUsers");
        }

        public IActionResult ViewUsers()
        {
            return ViewComponent("ViewUsers");
        }
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login(LoginVM loginVM)
        {
            return View(loginVM);
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> LoginAsync(LoginVM loginVM)
        {


            if (ModelState.IsValid)
            {
                var user = await usm.FindByNameAsync(loginVM.Username);
               
                if (user != null)
                {
                    await sim.PasswordSignInAsync(user, loginVM.Password,true,false);

                    var userClaims = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                    userClaims.AddClaim(new Claim("Name", user.UserName));




                    //userClaims.AddClaim(new Claim(ClaimTypes.Role, string.Join(",", from p in dcx.UserRoles
                    //                                                                join role in dcx.Roles on p.RoleId equals role.Id
                    //                                                                where p.UserId == user.Id
                    //                                                                select role.Name.ToString())));

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(userClaims), new AuthenticationProperties { IsPersistent = loginVM.RememberMe });

                    return RedirectToAction(nameof(HomeController.Index), "Home");
                }

            }


            return View();
        }



        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Login");
        }
        public async Task <IActionResult>ForgetPassword()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult>UserProfile()
        {
            return ViewComponent("UserProfile");
        }
        [HttpGet]
        public async Task<IActionResult> EditProfile()
        {
            return ViewComponent("EditProfile");
        }
        [HttpPost]
        public async Task<IActionResult>EditProfile(EditProfileVM editProfileVM)
        {
            AppUser profileToEdit= (from u in usm.Users
                                where u.Id == usm.GetUserId(HttpContext.User) select u).FirstOrDefault();

           

            profileToEdit.Firstname = editProfileVM.Firstname;
            profileToEdit.Surname = editProfileVM.Surname;
            profileToEdit.PhoneNumber = editProfileVM.Telephone;
            

            await usm.UpdateAsync(profileToEdit);

            notyf.Success("Profile successfully updated",5);

            return ViewComponent("UserProfile");
        }
    }
}
