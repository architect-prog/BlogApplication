using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace BlogApplication.Models
{
    public class User: IdentityUser 
    {
        public List<Post> Posts { get; set; }
    }
}
