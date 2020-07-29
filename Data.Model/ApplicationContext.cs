using Data.Model.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace Data.Model
{
    public class ApplicationContext : DbContext
    {

        public DbSet<City> Cities { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<ProductComment> ProductComments { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<ProductModel> ProductModels { get; set; }
        public DbSet<ProductOption> ProductOptions { get; set; }


        public ApplicationContext(DbContextOptions<ApplicationContext> options)
        : base(options)
        {
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<UserRole>()
            //    .HasOne(ur => ur.AppUser).WithMany(ur => ur.UserRoles).HasForeignKey(r => r.AppUserId);
            //modelBuilder.Entity<UserRole>()
            //    .HasOne(ur => ur.Role).WithMany(ur => ur.UserRoles).HasForeignKey(r => r.RoleId);



            //var store = new Store
            //{
            //    StoreCode = "test",
            //    StoreName = "Test Store"
            //};
            //modelBuilder.Entity<Store>().HasData(store);

            //var admin = new AppUser()
            //{
            //    FirstName = "Admin",
            //    LastName = "Administrator",
            //    EmailAddress = "admin@fake.com",
            //    UserName = "admin",
            //    Password = "admin",
            //    Store = store
            //};
            //modelBuilder.Entity<AppUser>().HasData(admin);



            //modelBuilder.Entity<UserRole>().HasData(new UserRole() { AppUserId = admin.Id, RoleId = adminRole.RoleId });

        }
    }
}
