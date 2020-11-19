using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using BlogApplication.Models;

namespace BlogApplication.Services
{
    public class UserService
    {
        private readonly UserManager<User> _userManager;
        public UserService(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IList<string>> Roles(ClaimsPrincipal userPrincipal)
        {
            User user = await _userManager.GetUserAsync(userPrincipal);
            return await _userManager.GetRolesAsync(user);
        }

        public async Task<bool> HasRole(ClaimsPrincipal userPrincipal, string roleName)
        {
            IList<string> roles = await Roles(userPrincipal);
            return roles.Contains(roleName);
        }


    }
}
