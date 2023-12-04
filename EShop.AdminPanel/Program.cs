using AutoMapper;
using EShop.AutoMapper;
using EShop.DataContext;
using EShop.IdentityService.Infrastructure;
using EShop.Infrastructure;
using EShop.LogService.DataContext;
using EShop.LogService.Repository;
using EShop.Model;
using EShop.Service.Implementation;
using EShop.Service.Interface;
using EShop.ViewModel;
using Microsoft.AspNetCore.Authentication.Cookies;

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


            // Add services to the container.
            builder.Services.AddRazorPages();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapRazorPages();

            //await app.SeedDataAsync();

            app.Run();
        }
    }
}