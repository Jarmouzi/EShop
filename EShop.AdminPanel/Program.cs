using Autofac.Core;
using AutoMapper;
using EShop.AdminPanel.Services;
using EShop.AutoMapper;
using EShop.DataContext;
using EShop.IdentityService.Infrastructure;
using EShop.Infrastructure;
using EShop.LogService.DataContext;
using EShop.LogService.Repository;
using EShop.Model;
using EShop.Repository.Implementation;
using EShop.Repository.Interface;
using EShop.ViewModel;  
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace EShop.AdminPanel
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddAutoMapper(typeof(Program), typeof(CategoryProfile));

            builder.Services.AddIdentityervices(builder.Configuration.GetConnectionString("IdentityConnection"));
            builder.Services.AddIdentityOptions();
            builder.Services.AddApplicationCookieAuth();

            builder.Services.AddApplicationServices<EShopLogContext>(builder.Configuration.GetConnectionString("LogConnection"));
            builder.Services.AddScoped<LogService.DataContext.IUnitOfWork, LogService.DataContext.UnitOfWork>();
            builder.Services.AddScoped<ILogRepository, LogRepository>();

            builder.Services.AddEShopServices(builder.Configuration.GetConnectionString("DefaultConnection"));
            builder.Services.AddPanelServices(builder.Configuration.GetConnectionString("PanelConnection"));


            // Add services to the container.
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddTransient<IActionContextAccessor, ActionContextAccessor>();
            builder.Services.AddScoped<IRazorRenderService, RazorRenderService>();
            builder.Services.AddRazorPages();

            builder.Services.AddSession(options =>
            {
                options.Cookie.Name = ".EShopAdminPanel.Session";
                options.IdleTimeout = TimeSpan.FromHours(1);
                options.Cookie.IsEssential = true;
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSession();

            app.MapRazorPages();

            //await app.SeedDataAsync();

            app.Run();
        }
    }
}