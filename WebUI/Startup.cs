using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
using System.Threading.Tasks;
using Data.Model;
using Data.Model.Entities;
using Data.Model.Interfaces;
using Data.Model.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WebUI.Resources;

namespace WebUI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var connection = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connection));
            services.AddTransient<IEntityContext, EntityContext>();

            //// добавление сервисов Idenity
            //services.AddIdentity<User, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
            //            .AddEntityFrameworkStores<ApplicationContext>();
            // установка конфигурации подключения

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options => //CookieAuthenticationOptions
                {
                    options.LoginPath = new Microsoft.AspNetCore.Http.PathString("/Account/Login");
                });

            services.AddDistributedMemoryCache();
            services.AddHttpContextAccessor();
            services.AddSession();

            services.Configure<AppConfig>(Configuration);
            //services.AddTransient<IStringLocalizer, CustomStringLocalizer>();
            services.AddSingleton<LocalizationService>();

            services.AddLocalization(options => options.ResourcesPath = "Resources");
            services.AddControllersWithViews(mvcOptions => { mvcOptions.EnableEndpointRouting = false; })
                                    .AddViewLocalization()
                                    .AddDataAnnotationsLocalization(options =>
                                    {
                                        options.DataAnnotationLocalizerProvider = (type, factory) =>
                                        {
                                            var assemblyName = new AssemblyName(typeof(SharedResource).GetTypeInfo().Assembly.FullName);
                                            return factory.Create("SharedResource", assemblyName.Name);
                                        };
                                    });
;

            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[]
                {
                    new CultureInfo("uz-Cyrl"),
                    new CultureInfo("uz-Latn"),
                    new CultureInfo("ru")
                };

                options.DefaultRequestCulture = new RequestCulture("ru");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
                options.RequestCultureProviders.Insert(0, new CookieRequestCultureProvider());
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseAuthentication();    // подключение аутентификации
            app.UseAuthorization();

            var locOptions = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(locOptions.Value);

            app.UseSession();

            app.UseMvc(routes =>
            {
                routes.MapRoute(name: "areas", template: "{area:exists}/{controller=Pages}/{action=Index}/{id?}");
                routes.MapRoute(name: "account", template: "Account/{action?}/{id?}", defaults: new { controller = "Account", action = "Index" });
                routes.MapRoute(name: "default", template: "{controller=Home}/{action=Index}/{id?}");
            });

            //app.UseRouting();

            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapControllerRoute(
            //        name: "Areas",
            //      pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
            //    endpoints.MapControllerRoute(
            //        name: "Pages",
            //        pattern: "{controller=Pages}/{action=Index}/{page?}");
            //    endpoints.MapRazorPages();
            //});

            #region Logger
            var logFile = Configuration.GetSection("LogFile").Value;
            if (!string.IsNullOrEmpty(logFile))
            {
                var log = Path.Combine(Directory.GetCurrentDirectory(), logFile);
                if (File.Exists(log))
                {
                    File.Delete(log);
                }
                loggerFactory.AddFile(log);
                var logger = loggerFactory.CreateLogger("FileLogger");
                logger.LogInformation("Start application. {0}", DateTime.Now);
            }
            #endregion
        }
    }
}
