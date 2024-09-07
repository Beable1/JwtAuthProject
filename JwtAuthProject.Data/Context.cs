using JwtAuthProject.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwtAuthProject.Data
{
    public class Context : IdentityDbContext<UserApp, IdentityRole, string>
    {
        public Context(DbContextOptions options) : base(options)
        {
        }

        DbSet<Product> Products { get; set; }
        DbSet<UserRefreshToken> UserRefreshToken { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(GetType().Assembly);

            base.OnModelCreating(builder);
        }
    }
}
