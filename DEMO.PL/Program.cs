using AutoMapper;
using DEMO.BLL.Interfaces;
using DEMO.BLL.Repositories;
using DEMO.DAL.Contexts;
using DEMO.DAL.Models;
using DEMO.PL.MappingProfiles;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DEMO.PL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var Builder = WebApplication.CreateBuilder(args);
            #region DI Container

            Builder.Services.AddControllersWithViews();
            Builder.Services.AddDbContext<DemoMvcAppContext>(options =>
            {
                options.UseSqlServer(Builder.Configuration.GetConnectionString("DefaultConnection"));

            }); // Allow Dependancy injection

            Builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            Builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            Builder.Services.AddAutoMapper(M => M.AddProfiles(new List<Profile>() { new UserProfile(), new EmployeeProfile(), new RoleProfile() }));
            Builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
            })
                .AddEntityFrameworkStores<DemoMvcAppContext>()
                .AddDefaultTokenProviders();//Genereate tokens
            Builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(Options =>
            {
                Options.LoginPath = "Account/Login";
                Options.AccessDeniedPath = "Home/Error";
            });
            #endregion

            var app = Builder.Build();

            #region Configure Http pipeline
            if (app.Environment.IsDevelopment())
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

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Account}/{action=Login}/{id?}");
            });
            #endregion

            app.Run();
        }




    }
}
