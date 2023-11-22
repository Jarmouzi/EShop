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

            builder.Services.AddIdentityervices(builder.Configuration.GetConnectionString("IdentityConnection"));
            builder.Services.AddIdentityOptions();
            builder.Services.AddApplicationCookieAuth();

            builder.Services.AddApplicationServices<EShopLogContext>(builder.Configuration.GetConnectionString("LogConnection"));
            builder.Services.AddScoped<LogService.DataContext.IUnitOfWork, LogService.DataContext.UnitOfWork>();
            builder.Services.AddScoped<ILogRepository, LogRepository>();

            builder.Services.AddApplicationServices<EShopContext>(builder.Configuration.GetConnectionString("DefaultConnection"));
            builder.Services.AddScoped<DataContext.IUnitOfWork, DataContext.UnitOfWork>();


            builder.Services.AddAutoMapper(typeof(Program), typeof(CategoryProfile));

            builder.Services.AddScoped<IRepository<Category, CategoryViewModel>, Repository<Category, CategoryViewModel>>();
            builder.Services.AddScoped<IRepository<Product, ProductViewModel>, Repository<Product, ProductViewModel>>();

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