using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ciber.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Ciber.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterUserViewModel registerUserViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = new AppUser { UserName = registerUserViewModel.Email, Email = registerUserViewModel.Email };
                var rs = await userManager.CreateAsync(user, registerUserViewModel.Password);

                if (rs.Succeeded)
                {
                    await signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("index", "home");
                }

                foreach (var error in rs.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(registerUserViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                var rs = await signInManager.PasswordSignInAsync(loginViewModel.Email, loginViewModel.Password, false, false);

                if (rs.Succeeded)
                {
                    return RedirectToAction("index", "home");
                }

                ModelState.AddModelError("", "Invalid login");
                
            }
            return View(loginViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("index", "home");
        }
    }
}