using HumanResources.Models.Entities.Concrete;
using HumanResources.Models.Entities.Context;
using HumanResources.Models.Reposities.Abstract;
using HumanResources.Models.Reposities.Concrete;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HumanResources
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
            services.AddControllersWithViews(config =>
            {
                var policy = new AuthorizationPolicyBuilder().RequireRole("SiteManager", "CompanyManager", "Employee").RequireAuthenticatedUser().Build();
                config.Filters.Add(new AuthorizeFilter(policy));
            }).AddRazorRuntimeCompilation().AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            services.AddDbContext<ProjectContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("HumanResourceData"));
                options.UseLazyLoadingProxies(true);
            });

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(config =>
            {
                config.LoginPath = "/Login/Login/";
            });

            services.AddTransient<IEntityRepository<User>, EFUserRepository>();
            services.AddTransient<IEntityRepository<Role>, EFRoleRepository>();
            services.AddTransient<IEntityRepository<Company>, EFCompanyRepostory>();
            services.AddTransient<IEntityRepository<Package>, EFPackageRepository>();
            services.AddTransient<IEntityRepository<PersonalPermit>, EFPermitRepository>();
            services.AddTransient<IEntityRepository<Permission>, EFPermissionRepository>();
            services.AddTransient<IEntityRepository<PersonalAdvance>, EFPersonalAdvanceRepository>();
            services.AddTransient<IEntityRepository<AdvancePayment>, EFAdvancePaymentRepository>();
            services.AddTransient<IEntityRepository<PersonalExpense>, EFPersonalExpenseRepository>();
            services.AddTransient<IEntityRepository<Expense>, EFExpenseRepository>();
            services.AddTransient<IEntityRepository<Status>, EFStatusRepository>();
            services.AddTransient<IEntityRepository<Idea>, EFIdeaRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            SeedData.Seed(app);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapAreaControllerRoute(
                    name: "Admin",
                    areaName: "Admin",
                    pattern: "Admin/{controller=Home}/{action=Index}/{id?}"
                    );

                endpoints.MapAreaControllerRoute(
                    name: "CompanyManager",
                    areaName: "CompanyManager",
                    pattern: "CompanyManager/{controller=Home}/{action=Index}/{id?}"
                    );

                endpoints.MapAreaControllerRoute(
                    name: "Employee",
                    areaName: "Employee",
                    pattern: "Employee/{controller=Home}/{action=Index}/{id?}"
                    );

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Login}/{action=Login}/{id?}");
            });
        }
    }
}
