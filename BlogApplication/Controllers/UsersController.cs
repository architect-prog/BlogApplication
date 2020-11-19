using System;
using System.Linq;
using System.Threading.Tasks;
using BlogApplication.Models;
using BlogApplication.Services;
using BlogApplication.ViewModels.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BlogApplication.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly IdentityResultHandler _identityHandler;

        public UsersController(UserManager<User> userManager, IdentityResultHandler identityHandler)
        {
            _userManager = userManager;
            _identityHandler = identityHandler;
        }

        public IActionResult Index() => View(_userManager.Users.ToList());
        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(CreateUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = new User() 
                { 
                    Email = model.Email,
                    UserName = model.Username 
                };

                IdentityResult result = await _userManager.CreateAsync(user, model.Password);
                return _identityHandler
                    .SetIdentityResult(result)
                    .SetSuccessAction(() => RedirectToAction(nameof(Index)))
                    .SetFailureAction(() => View(model))
                    .AddModelErrors(ModelState)
                    .HandleIdentityResult();
            }
            return View(model);
        }

        public async Task<IActionResult> Edit(string id)
        {
            User user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            EditUserViewModel model = new EditUserViewModel 
            {
                Id = user.Id, 
                Email = user.Email, 
                Username = user.UserName
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _userManager.FindByIdAsync(model.Id);
                if (user != null)
                {
                    user.Email = model.Email;
                    user.UserName = model.Username;

                    IdentityResult result = await _userManager.UpdateAsync(user);
                    return _identityHandler
                        .SetIdentityResult(result)
                        .SetSuccessAction(() => RedirectToAction(nameof(Index)))
                        .SetFailureAction(() => View(model))
                        .AddModelErrors(ModelState)
                        .HandleIdentityResult();        
                }
            }

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Delete(string id)
        {
            User user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                IdentityResult result = await _userManager.DeleteAsync(user);
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> ChangePassword(string id)
        {
            User user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            ChangePasswordViewModel model = new ChangePasswordViewModel { Id = user.Id, Email = user.Email };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _userManager.FindByIdAsync(model.Id);

                if (user != null)
                {                    
                    IdentityResult result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                    return _identityHandler
                        .SetIdentityResult(result)
                        .SetSuccessAction(() => RedirectToAction(nameof(Index)))
                        .SetFailureAction(() => View(model))
                        .AddModelErrors(ModelState)
                        .HandleIdentityResult();
                }
                else
                {                    
                    ModelState.AddModelError(string.Empty, "Пользователь не найден");
                }
            }
            return View(model);
        }
    }
}
