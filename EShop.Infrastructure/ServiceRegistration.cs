using AutoMapper;
using EShop.DataContext;
using EShop.Model;
using EShop.Service.Implementation;
using EShop.Service.Interface;
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

        public static IServiceCollection AddRepository<T, TViewModel>(this IServiceCollection services) where T : BaseModel where TViewModel : BaseModel
        {
            services.AddSingleton<IRepository<T, TViewModel>>(ServiceProvider =>
            {
                var unitOfWork = ServiceProvider.GetService<IUnitOfWork>();
                var mapper = ServiceProvider.GetService<IMapper>();
                return new Repository<T, TViewModel>(unitOfWork, mapper);
            });

            return services;
        }

    }
}