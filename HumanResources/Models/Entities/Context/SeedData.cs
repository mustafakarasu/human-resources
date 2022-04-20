using HumanResources.Models.Entities.Concrete;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HumanResources.Models.Entities.Context
{
    public class SeedData
    {
        public static void Seed(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                ProjectContext context = serviceScope.ServiceProvider.GetService<ProjectContext>();

                context.Database.Migrate();

                if (!context.Roles.Any())
                {
                    context.Roles.AddRange(
                        new Role() { Name = "Site Manager", CreatedDate = DateTime.Now, IsActive = true },
                        new Role() { Name = "Company Manager", CreatedDate = DateTime.Now, IsActive = true },
                        new Role() { Name = "Employee", CreatedDate = DateTime.Now, IsActive = true }
                        );
                }

                context.SaveChanges();

                if (!context.Users.Any())
                {
                    context.Users.Add(
                        new User() { FirstName = "Admin", LastName = "SiteManager", CreatedDate = DateTime.Now, Email = "admin@admin.com", FirstPasswordEnter = false, HireDate = DateTime.Now, IsActive = true, RoleId = 1, Password = "123456" }
                        );
                }

                if (!context.Permissions.Any())
                {
                    context.Permissions.AddRange(
                        new Permission() { Name = "Evlilik İzni", DocumentRequired = false, CreatedDate = DateTime.Now, IsActive = true },
                        new Permission() { Name = "Cenaze İzni", DocumentRequired = true, CreatedDate = DateTime.Now, IsActive = true },
                        new Permission() { Name = "Hastalık İzni", DocumentRequired = true, CreatedDate = DateTime.Now, IsActive = true },
                        new Permission() { Name = "Ücretsiz İzin", DocumentRequired = false, CreatedDate = DateTime.Now, IsActive = true },
                        new Permission() { Name = "Babalık İzni", DocumentRequired = false, CreatedDate = DateTime.Now, IsActive = true },
                        new Permission() { Name = "Yıllık İzin", DocumentRequired = false, CreatedDate = DateTime.Now, IsActive = true },
                        new Permission() { Name = "Ücretli Doğum İzni", DocumentRequired = false, CreatedDate = DateTime.Now, IsActive = true },
                        new Permission() { Name = "Evlat Edinme İzni", DocumentRequired = false, CreatedDate = DateTime.Now, IsActive = true },
                        new Permission() { Name = "Ücretsiz Doğum İzni", DocumentRequired = false, CreatedDate = DateTime.Now, IsActive = true }
                        );
                }

                if (!context.AdvancePayments.Any())
                {
                    context.AdvancePayments.AddRange(
                        new AdvancePayment() { Name = "Eğitim Masrafı", CreatedDate = DateTime.Now, IsActive = true },
                        new AdvancePayment() { Name = "Ulaşım Masrafı", CreatedDate = DateTime.Now, IsActive = true },
                        new AdvancePayment() { Name = "Toplantı Masrafı", CreatedDate = DateTime.Now, IsActive = true },
                        new AdvancePayment() { Name = "Kişisel Talep", CreatedDate = DateTime.Now, IsActive = true }
                        );
                }

                if (!context.Expenses.Any())
                {
                    context.Expenses.AddRange(
                        new Expense() { Name = "Eğitim Gideri", CreatedDate = DateTime.Now, IsActive = true },
                        new Expense() { Name = "Toplantı Gideri", CreatedDate = DateTime.Now, IsActive = true },
                        new Expense() { Name = "Ulaşım Gideri", CreatedDate = DateTime.Now, IsActive = true },
                        new Expense() { Name = "Yemek Gideri", CreatedDate = DateTime.Now, IsActive = true }
                        );
                }

                if (!context.Statuses.Any())
                {
                    context.Statuses.AddRange(
                        new Status() { Name = "Beklemede", CreatedDate = DateTime.Now, IsActive = true },
                        new Status() { Name = "Onaylandı", CreatedDate = DateTime.Now, IsActive = true },
                        new Status() { Name = "Reddedildi", CreatedDate = DateTime.Now, IsActive = true }
                        );
                }
                //context.PersonalPermits.AddRange(
                //    new PersonalPermit() { CreatedDate = DateTime.Now, IsActive = true, PersonalID = 2, CompanyID = 1, RequestDate = DateTime.Now, StartDate = new DateTime(2022, 03, 15), EndDate = new DateTime(2022, 04, 05), PermissionID = 4, StatusID = 1 },
                //    new PersonalPermit() { CreatedDate = DateTime.Now, IsActive = true, PersonalID = 2, CompanyID = 1, RequestDate = DateTime.Now, StartDate = new DateTime(2022, 03, 14), EndDate = new DateTime(2022, 03, 19), PermissionID = 6, StatusID = 1 },
                //    new PersonalPermit() { CreatedDate = DateTime.Now, IsActive = true, PersonalID = 2, CompanyID = 1, RequestDate = DateTime.Now, StartDate = new DateTime(2022, 03, 28), EndDate = new DateTime(2022, 04, 08), PermissionID = 5, StatusID = 1 },
                //    new PersonalPermit() { CreatedDate = DateTime.Now, IsActive = true, PersonalID = 2, CompanyID = 1, RequestDate = DateTime.Now, StartDate = new DateTime(2022, 03, 07), EndDate = new DateTime(2022, 04, 08), PermissionID = 2, StatusID = 1 }
                //    );

                context.SaveChanges();
            }
        }
    }
}
