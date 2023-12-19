using AutoMapper;
using EShop.DataContext;
using EShop.Model;
using EShop.Repository.Implementation;
using EShop.Repository.Interface;
using EShop.ViewModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Principal;

namespace EShop.Infrastructure
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddApplicationServices<TContext>(
            this IServiceCollection services,
            string? connectionStringConfigName) where TContext : DbContext
        {
            services.AddDbContext<TContext>(options =>
            {
                options.UseSqlServer(connectionStringConfigName);
            });
            return services;
        }
        public static IServiceCollection AddApiServices<TContext>(
            this IServiceCollection services,
            string? connectionStringConfigName) where TContext : DbContext
        {
            services.AddDbContext<TContext>(options =>
            {
                options.UseSqlServer(connectionStringConfigName);
            });
            services.AddScoped<DataContext.IUnitOfWork<TContext>, DataContext.UnitOfWork<TContext>>();
            return services;
        }
        public static IServiceCollection AddEShopServices(
            this IServiceCollection services,
            string? connectionStringConfigName)
        {
            services.AddApplicationServices<EShopContext>(connectionStringConfigName);
            services.AddScoped<DataContext.IUnitOfWork<EShopContext>, DataContext.UnitOfWork<EShopContext>>();

            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IRepository<Product, ProductViewModel>, Repository<Product, ProductViewModel, EShopContext>>();
            services.AddScoped<IRegionRepository, RegionRepository>();

            return services;
        }
        public static IServiceCollection AddPanelServices(
            this IServiceCollection services,
            string? connectionStringConfigName)
        {
            services.AddApplicationServices<EShopPanelContext>(connectionStringConfigName);
            services.AddScoped<DataContext.IUnitOfWork<EShopPanelContext>, DataContext.UnitOfWork<EShopPanelContext>>();

            services.AddScoped<IRepository<PanelResource, PanelResourceViewModel>, Repository<PanelResource, PanelResourceViewModel, EShopPanelContext>>();

            return services;
        }
    }
}