using Guni_Kitchen.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guni_Kitchen.Data
{
    public class ApplicationDbContext : IdentityDbContext<MyIdentityUser, MyIdentityRole, Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Guni_Kitchen.Models.Product> Product { get; set; }
    }
}
