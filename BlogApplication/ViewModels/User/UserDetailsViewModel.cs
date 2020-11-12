using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApplication.ViewModels.User
{
    public class UserDetailsViewModel
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public IList<string> UserRoles { get; set; }
        public UserDetailsViewModel()
        {
            UserRoles = new List<string>();
        }
    }
}
