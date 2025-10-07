﻿using NationalBank.BackEnd.Entities;
using NationalBank.BackEnd.Models;
using NationalBank.BackEnd.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;

namespace NationalBank.FrontEnd.Controllers
{

    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public class UserLoginController : Controller
    {
        private UserManager<User> userManager;
        private SignInManager<User> signInManager;
        private readonly RoleManager<Role> _roleManager;
        private IUserRepository _userRepository;
        public UserLoginController(UserManager<User> usrMgr, SignInManager<User> signInManager, RoleManager<Role> roleManager,IUserRepository userRepository)
        {
            userManager = usrMgr;
            this.signInManager = signInManager;
            _roleManager = roleManager; 
            _userRepository = userRepository;
        }
        public IActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            AuthenticateRequest login = new AuthenticateRequest();
            login.ReturnUrl = returnUrl;
            return View(login);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(AuthenticateRequest model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Step 1: Find user by username or email
            var user = await userManager.FindByNameAsync(model.Username);
            if (user == null)
            {
                ModelState.AddModelError(nameof(model.Username), "Login Failed: Invalid Username");
                return View(model);
            }

            // Step 2: Sign out any existing user sessions
            await signInManager.SignOutAsync();

            // Step 3: Try to sign in with password
            var result = await signInManager.PasswordSignInAsync(user, model.Password, isPersistent: false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return Redirect(model.ReturnUrl ?? "/");
            }
            else if (result.IsLockedOut)
            {
                ModelState.AddModelError("", "Account is locked out. Try again later.");
            }
            else
            {
                ModelState.AddModelError(nameof(model.Password), "Login Failed: Invalid Password");
            }

            return View(model);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Create()
        {
            var model = new RegisterModel()
            {
                Roles = await _userRepository.GetRoles()
            };
            return View(model); 
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Create(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var userExists = await userManager.FindByNameAsync(model.Username);
                if (userExists != null)
                {
                    model.Roles = await _userRepository.GetRoles();
                    TempData.AddAlert(new Alert() { AlertClass = "alert-danger", Message = "User Already Exists" });
                    return View("Create",model);
                }
                                      
                var roleExists = await _roleManager.FindByIdAsync(model.RoleId);
                if (roleExists == null)
                {
                    model.Roles = await _userRepository.GetRoles();
                    TempData.AddAlert(new Alert() { AlertClass = "alert-danger", Message = "Role Not Exists" });
                    return View("Create", model);
                }
                User user = new()
                {
                    Email = model.Email,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = model.Username,
                    RoleId = model.RoleId,
                };
                var result = await userManager.CreateAsync(user, model.Password);


                if (result.Succeeded)
                    return RedirectToAction("Login", "UserLogin", new { returnUrl = "/" });
                else
                {
                    model.Roles = await _userRepository.GetRoles();
                    foreach (IdentityError error in result.Errors)
                        ModelState.AddModelError("", error.Description);
                    return View(model); 
                }
            }
            else
            {
                model.Roles = await _userRepository.GetRoles();
                TempData.AddAlert(new Alert() { AlertClass = "alert-danger", Message = "Enter Correct values" });
                return View("Create", model);
            }
        }
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            Response.Cookies.Delete("LoginToken");
            return RedirectToAction("Login","UserLogin", new { returnUrl="/" });
        }
        public async Task<IActionResult> SessionExpired()
        {
            await signInManager.SignOutAsync();
            return View();
        }

        public IActionResult AccessDenied()
        {
            return View();
        }


    }
}
