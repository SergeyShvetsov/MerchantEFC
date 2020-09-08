using Data.Model.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;

namespace Data.Model
{
    public class ApplicationContext : DbContext
    {
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<ProductModel> ProductModels { get; set; }
        public DbSet<ProductOption> ProductOptions { get; set; }

        public DbSet<SiteImage> SiteImages { get; set; }
        public DbSet<ProductPage> ProductPages { get; set; }
        public DbSet<ProductComment> ProductComments { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
        : base(options)
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
            //Database.Migrate();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<SiteImage>().Property(p => p.ObjImage).HasColumnType("MediumBlob");

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

        public IEnumerable<RoleType> GetAllRoles() => Enum.GetValues(typeof(RoleType)).Cast<RoleType>().Where(x => x != RoleType.Undefined).Select(v => v).ToList();
        public IEnumerable<RoleType> GetAvailableRoles(ClaimsPrincipal user)
        {
            var res = GetAllRoles();

            if (user.IsInRole("Admin")) return res;
            else if (user.IsInRole("Supervisor")) return res.Where(w => w == RoleType.Supervisor || w == RoleType.Manager || w == RoleType.Seller);
            else if (user.IsInRole("Manager")) return res.Where(w => w == RoleType.Manager || w == RoleType.Seller);
            else if (user.IsInRole("Seller")) return res.Where(w => w == RoleType.Seller);

            return new List<RoleType>();
        }

        public IEnumerable<Available> GetProductAvailability() => Enum.GetValues(typeof(Available)).Cast<Available>().Select(v => v).ToList();
        public IEnumerable<Status> GetAllStatuses() => Enum.GetValues(typeof(Status)).Cast<Status>().Select(v => v).ToList();
        public IEnumerable<Status> GetAvailableStatuses(ClaimsPrincipal user)
        {
            var res = GetAllStatuses();
            if (user.IsInRole("Admin")) return res;
            else if (user.IsInRole("Supervisor")) return res.Where(w => w != Status.Blocked);

            return new List<Status>();
        }
    }
}
