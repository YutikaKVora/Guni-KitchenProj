using Guni_Kitchen.Models;
using Guni_Kitchen.Models.Enums;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;


namespace Guni_Kitchen.Data
{
    public class ApplicationDbContext : IdentityDbContext<MyIdentityUser, MyIdentityRole, Guid>
    {
        public DbSet<ProductCategory> Category { get; set; }
        public DbSet<Product> Products { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var sizeConverter = new ValueConverter<ProductSizes, string>(
                v => v.ToString()
                , v => (ProductSizes)Enum.Parse(typeof(ProductSizes), v));

            
            builder.Entity<ProductCategory>()
                   .Property(e => e.CreatedAt)
                   .HasDefaultValueSql("getdate()");

           
            builder.Entity<Product>()
                   .Property(e => e.Price)
                   .HasPrecision(precision: 6, scale: 2);

            builder.Entity<Product>()
                   .Property(e => e.Size)
                   .HasConversion(sizeConverter);
        }
    }   
}
