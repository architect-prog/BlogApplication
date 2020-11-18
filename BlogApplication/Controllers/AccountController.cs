using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogApplication.Models;
using BlogApplication.ViewModels;
using BlogApplication.ViewModels.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization.Internal;

namespace BlogApplication.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

      

        public async Task<IActionResult> Details()
        {           
            User user = await _userManager.GetUserAsync(User);
            IList<string> userRoles = await _userManager.GetRolesAsync(user);        
                       

            UserDetailsViewModel userDetails = new UserDetailsViewModel()
            {
                Email = user.Email,
                UserName = user.UserName,
                UserRoles = userRoles
            };
          
            return View(userDetails);
        }

        public IActionResult SignUp() => View();

        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = new User() 
                {
                    UserName = model.Username,
                    Email = model.Email                   
                };

                IdentityResult result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, false);
                    return RedirectToHome();
                }
                else
                {
                    foreach (IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(model);
        }

        public IActionResult SignIn() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignIn(SignInViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, false);

                if (result.Succeeded)
                {
                    return RedirectToHome();
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Incorrect login or password!");
                }
            }          

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToHome();
        }

        private IActionResult RedirectToHome()
        {
            return RedirectToAction("Index", "Home");
        }
    }
}
