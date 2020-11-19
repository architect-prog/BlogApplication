using BlogApplication.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApplication.Repository
{
    public class UserContext: IdentityDbContext<User>
    {
        public DbSet<Post> Posts { get; set; }
        public UserContext(DbContextOptions<UserContext> options)
            : base(options)
        {

        }
    }
}
