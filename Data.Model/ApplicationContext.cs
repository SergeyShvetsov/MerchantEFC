using Data.Model.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace Data.Model
{
    public class ApplicationContext : DbContext
    {

        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Store> Stores { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
        : base(options)
        {
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.AppUser).WithMany(ur => ur.UserRoles).HasForeignKey(r => r.AppUserId);
            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.Role).WithMany(ur => ur.UserRoles).HasForeignKey(r => r.RoleId);

            //modelBuilder.Entity<UserRole>().HasKey(e => new { e.AppUserId, e.RoleId });

            //var adminRole = new Role() { Name = "Admin", DisplayName = "Administrator", RoleType = RoleType.Admin };
            //var superUserRole = new Role() { Name = "SU", DisplayName="Super User", RoleType = RoleType.Superuser };
            //var managerRole = new Role() { Name = "Mgr", DisplayName = "Manager", RoleType = RoleType.Manager };
            //var userRole = new Role() { Name = "Usr", DisplayName = "User", RoleType = RoleType.User };
            //modelBuilder.Entity<Role>().HasData(new Role[] { adminRole, superUserRole, managerRole, userRole });
            /////////////////////////////////////////////////////////////////////////////////////////////////////////


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
