﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InternetApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace InternetApp.Controllers
{
    public class AuthController : Controller
    {
        private UserManager<IdentityUser> _userManager;
        private SignInManager<IdentityUser> _signInManager;

        public AuthController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Register(RegisterViewModel registerModel)
        {
            if (ModelState.IsValid)
            {
                IdentityUser user = new IdentityUser { UserName = registerModel.UserName };
                IdentityResult registerResult = await _userManager.CreateAsync(user, registerModel.Password);
                if (registerResult.Succeeded)
                {
                    var signInResult = await _signInManager.PasswordSignInAsync(registerModel.UserName, registerModel.Password, false, false);
                    if (signInResult.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    return RedirectToAction("Login");
                }
            }
            return View(registerModel);
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(loginModel.UserName, loginModel.Password, false, false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            return View(loginModel);
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Index", "Articles");
        }
    }
}