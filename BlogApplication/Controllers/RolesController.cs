using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BlogApplication.Models;
using BlogApplication.Services;
using BlogApplication.ViewModels.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BlogApplication.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RolesController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;
        private readonly IdentityResultHandler _identityHandler;
        private readonly IMapper _mapper;

        public RolesController(RoleManager<IdentityRole> roleManager, UserManager<User> userManager, IdentityResultHandler identityHandler, IMapper mapper)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _identityHandler = identityHandler;
            _mapper = mapper;
        }

        public IActionResult Index() => View(_roleManager.Roles.ToList());
        public IActionResult UserList() => View(_userManager.Users.ToList());
        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                IdentityResult result = await _roleManager.CreateAsync(new IdentityRole(name));
                return _identityHandler
                    .SetIdentityResult(result)
                    .SetSuccessAction(() => RedirectToAction(nameof(Index)))
                    .SetFailureAction(() => View((object)name))
                    .AddModelErrors(ModelState)
                    .HandleIdentityResult();          
            }
            return View(name);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            IdentityRole role = await _roleManager.FindByIdAsync(id);
            if (role != null)
            {
                await _roleManager.DeleteAsync(role);
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(string userId)
        {
            User user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            IList<string> userRoles = await _userManager.GetRolesAsync(user);
            List<IdentityRole> allRoles = _roleManager.Roles.ToList();

            ChangeRoleViewModel model = _mapper.Map<ChangeRoleViewModel>(user);
            model.UserRoles = userRoles;
            model.AllRoles = allRoles;        

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string userId, List<string> roles)
        {
            User user = await _userManager.FindByIdAsync(userId);            
            IList<string> userRoles = await _userManager.GetRolesAsync(user);

            List<IdentityRole> allRoles = _roleManager.Roles.ToList();
            List<string> addedRoles = roles.Except(userRoles).ToList();
            List<string> removedRoles = userRoles.Except(roles).ToList();

            await _userManager.AddToRolesAsync(user, addedRoles);
            await _userManager.RemoveFromRolesAsync(user, removedRoles);

            return RedirectToAction(nameof(UserList));
        }     
    }
}
