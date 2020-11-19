using System.Threading.Tasks;
using AutoMapper;
using System.Collections.Generic;
using BlogApplication.Models;
using BlogApplication.ViewModels.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BlogApplication.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IMapper _userMapper;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, IMapper userMapper, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userMapper = userMapper;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Details()
        {           
            User user = await _userManager.GetUserAsync(User);           
            UserDetailsViewModel userDetails = _userMapper.Map<UserDetailsViewModel>(user);
                      
            return View(userDetails);
        }

        public IActionResult SignUp() => View();

        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = _userMapper.Map<User>(model);
                IdentityResult result = await _userManager.CreateAsync(user, model.Password);
                
                if (result.Succeeded)
                {
                    List<string> startRoles = new List<string>() { "User" };
                    await _userManager.AddToRolesAsync(user, startRoles);

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
