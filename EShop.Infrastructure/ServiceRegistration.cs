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

            services.AddScoped<IPanelResourceRepository, PanelResourceRepository>();

            services.AddScoped<IBannerRepository, BannerRepository>();
            services.AddScoped<ICollectionRepository, CollectionRepository>();
            services.AddScoped<IComponentRepository, ComponentRepository>();
            services.AddScoped<IFeatureRepository, FeatureRepository>();
            services.AddScoped<IFeatureValueRepository, FeatureValueRepository>();
            services.AddScoped<IFilterRepository, FilterRepository>();
            services.AddScoped<IGroupTypeRepository, GroupTypeRepository>();
            services.AddScoped<IImageRepository, ImageRepository>();
            services.AddScoped<IOptionRepository, OptionRepository>();
            services.AddScoped<IOptionValueRepository, OptionValueRepository>();
            services.AddScoped<IProduct_FeatureRepository, Product_FeatureRepository>();
            services.AddScoped<IProductVariantRepository, ProductVariantRepository>();
            services.AddScoped<IProduct_GroupRepository, Product_GroupRepository>();
            services.AddScoped<IProduct_ImageRepository, Product_ImageRepository>();
            services.AddScoped<IProduct_OptionRepository, Product_OptionRepository>();
            services.AddScoped<IProductSeoRepository, ProductSeoRepository>();
            services.AddScoped<IRegionRepository, RegionRepository>();
            services.AddScoped<ISaleTypeRepository, SaleTypeRepository>();
            services.AddScoped<IStockRepository, StockRepository>();
            services.AddScoped<ISupplierRepository, SupplierRepository>();
            services.AddScoped<ISupplier_BrandRepository, Supplier_BrandRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ISupplier_ContractRepository, Supplier_ContractRepository>();
            services.AddScoped<IBrandRepository, BrandRepository>();
            services.AddScoped<ICartRepository, CartRepository>();
            services.AddScoped<ICartItemRepository, CartItemRepository>();
            services.AddScoped<IStateRepository, StateRepository>();
            services.AddScoped<ICityRepository, CityRepository>();
            services.AddScoped<IUserProfileRepository, UserProfileRepository>();
            services.AddScoped<IUserAddressRepository, UserAddressRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();

            return services;
        }
        public static IServiceCollection AddPanelServices(
            this IServiceCollection services,
            string? connectionStringConfigName)
        {
            services.AddApplicationServices<EShopPanelContext>(connectionStringConfigName);
            services.AddScoped<DataContext.IUnitOfWork<EShopPanelContext>, DataContext.UnitOfWork<EShopPanelContext>>();

            services.AddScoped<IRepository<PanelResource, PanelResourceViewModel>, Repository<PanelResource, PanelResourceViewModel, EShopPanelContext>>();

            services.AddScoped<ICollectionRepository, CollectionRepository>();
            services.AddScoped<IFilterRepository, FilterRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IFeatureValueRepository, FeatureValueRepository>();
            services.AddScoped<IProductVariantRepository, ProductVariantRepository>();
            services.AddScoped<IBrandRepository, BrandRepository>();
            services.AddScoped<ISupplier_ContractRepository, Supplier_ContractRepository>();
            services.AddScoped<IOptionRepository, OptionRepository>();
            services.AddScoped<IComponentRepository, ComponentRepository>();
            services.AddScoped<IFeatureRepository, FeatureRepository>();
            services.AddScoped<ISaleTypeRepository, SaleTypeRepository>();
            services.AddScoped<IProduct_GroupRepository, Product_GroupRepository>();
            services.AddScoped<ISupplier_BrandRepository, Supplier_BrandRepository>();
            services.AddScoped<ISupplierRepository, SupplierRepository>();
            services.AddScoped<IGroupTypeRepository, GroupTypeRepository>();
            services.AddScoped<IProduct_ImageRepository, Product_ImageRepository>();
            services.AddScoped<IImageRepository, ImageRepository>();
            services.AddScoped<IProduct_FeatureRepository, Product_FeatureRepository>();
            services.AddScoped<IProduct_Variant_OptionRepository, Product_Variant_OptionRepository>();
            services.AddScoped<IProductSeoRepository, ProductSeoRepository>();
            services.AddScoped<IRegionRepository, RegionRepository>();
            services.AddScoped<IProduct_OptionRepository, Product_OptionRepository>();
            services.AddScoped<IOptionValueRepository, OptionValueRepository>();
            services.AddScoped<IStockRepository, StockRepository>();
            services.AddScoped<IBannerRepository, BannerRepository>();


            return services;
        }
    }
}