using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
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
        private readonly IMapper _mapper;

        public UsersController(UserManager<User> userManager, IdentityResultHandler identityHandler, IMapper mapper)
        {
            _userManager = userManager;
            _identityHandler = identityHandler;
            _mapper = mapper;
        }

        public IActionResult Index() => View(_userManager.Users.ToList());
        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(CreateUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = _mapper.Map<User>(model);
                
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

            EditUserViewModel model = _mapper.Map<EditUserViewModel>(user);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _userManager.FindByIdAsync(model.Id);
                
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

            ChangePasswordViewModel model = _mapper.Map<ChangePasswordViewModel>(user);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _userManager.FindByIdAsync(model.Id);
                
                IdentityResult result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                return _identityHandler
                    .SetIdentityResult(result)
                    .SetSuccessAction(() => RedirectToAction(nameof(Index)))
                    .SetFailureAction(() => View(model))
                    .AddModelErrors(ModelState)
                    .HandleIdentityResult();               
            }
            return View(model);
        }
    }
}
