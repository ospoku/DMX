using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using DMX.ViewModels;
using DMX.Data;
using DMX.Models;
using Microsoft.AspNetCore.Authorization;

namespace DMX.Controllers
{
    public class AccountController(XContext dContext, UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, SignInManager<AppUser> signinmanager) : Controller
    {
        public readonly XContext dcx = dContext;
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
            if (ModelState.IsValid)
            {
                AppUser addThisUser = new()
                {

                    UserName = addUserVM.Username,
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

            }

            else
            {
                ViewBag.Message = "User creation error";
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
        public IActionResult ManageRoles(string userId)
        {

            return ViewComponent("ManageRoles", userId);
        }
        [HttpGet]
        public IActionResult Login(LoginVM loginVM)
        {
            return View(loginVM);
        }
        [HttpPost]
        public async Task<IActionResult> LoginAsync(LoginVM loginVM)
        {


            if (ModelState.IsValid)
            {
                var user = await usm.FindByNameAsync(loginVM.Username);
                if (user == null)
                {

                };
                if (user != null)
                {
                    await sim.PasswordSignInAsync(user, loginVM.Password, false, false);

                    var userClaims = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                    userClaims.AddClaim(new Claim("Name", user.UserName));




                    userClaims.AddClaim(new Claim(ClaimTypes.Role, string.Join(",", from p in dcx.UserRoles
                                                                                    join role in dcx.Roles on p.RoleId equals role.Id
                                                                                    where p.UserId == user.Id
                                                                                    select role.Name.ToString())));




                    //string.Join(",", from p in prx.UserRoles
                    //                 join role in prx.Roles on p.RoleId equals role.Id
                    //                 where p.UserId == user.Id
                    //                 select role.Name.ToString())););
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(userClaims), new AuthenticationProperties { IsPersistent = true });

                    return RedirectToAction(nameof(HomeController.Index), "Home");
                }

            }


            return View();
        }

        [HttpGet]
        public IActionResult ManageUserRoles(string Id)
        {
            return ViewComponent("ManageUserRoles", Id);
        }
        [HttpPost]
        public async Task<IActionResult> ManageUserroles(string Id, ManageUserRolesVM model)
        {
            var user = await usm.FindByIdAsync(Id);
            var roles = await usm.GetRolesAsync(user);
            var result = await usm.RemoveFromRolesAsync(user, roles);
            result = await usm.AddToRolesAsync(user, model.UserRoles.Where(x => x.Selected).Select(y => y.RoleName));
            return RedirectToAction("ViewUserRoles");
        }


        [HttpGet]
        public IActionResult UserRoles()
        {
            return ViewComponent("UserRoles");
        }
        [HttpGet]
        public IActionResult ViewUserRoles()
        {
            return ViewComponent("ViewUserRoles");
        }
        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync();

            return RedirectToAction("Login");
        }

        [HttpGet]
        [Authorize(Roles = "SuperAdmin")]
        public IActionResult ViewRoles()
        {
            return ViewComponent("ViewRoles");
        }



        [HttpPost]
        public async Task<IActionResult> AddRole(string roleName)
        {
            if (roleName != null)
            {
                await rol.CreateAsync(new  AppRole { Name=roleName});
            }
            return RedirectToAction("ViewRoles");
        }
    }
}
