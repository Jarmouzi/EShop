using EShop.Model;
using Microsoft.EntityFrameworkCore;

namespace EShop.DataContext
{
    public class EShopContext : DbContext
    {

        public EShopContext(DbContextOptions<EShopContext> options) : base(options)
        {
        } 
				public DbSet<Banner> Banner { get; set; }
				public DbSet<Collection> Collection { get; set; }
				public DbSet<Component> Component { get; set; }
				public DbSet<Feature> Feature { get; set; }
				public DbSet<FeatureValue> FeatureValue { get; set; }
				public DbSet<Filter> Filter { get; set; }
				public DbSet<GroupType> GroupType { get; set; }
				public DbSet<Image> Image { get; set; }
				public DbSet<Option> Option { get; set; }
				public DbSet<OptionValue> OptionValue { get; set; }
				public DbSet<Product_Feature> Product_Feature { get; set; }
				public DbSet<ProductVariant> ProductVariant { get; set; }
				public DbSet<Product_Group> Product_Group { get; set; }
				public DbSet<Product_Image> Product_Image { get; set; }
				public DbSet<Product_Option> Product_Option { get; set; }
				public DbSet<ProductVariant_Option> ProductVariant_Option { get; set; }
				public DbSet<ProductSeo> ProductSeo { get; set; }
				public DbSet<Region> Region { get; set; }
				public DbSet<SaleType> SaleType { get; set; }
				public DbSet<Stock> Stock { get; set; }
				public DbSet<Supplier> Supplier { get; set; }
				public DbSet<Supplier_Brand> Supplier_Brand { get; set; }
				public DbSet<Product> Product { get; set; }
				public DbSet<Supplier_Contract> Supplier_Contract { get; set; }
				public DbSet<Brand> Brand { get; set; }
				public DbSet<Cart> Cart { get; set; }
				public DbSet<CartItem> CartItem { get; set; }
				public DbSet<State> State { get; set; }
				public DbSet<City> City { get; set; }
				public DbSet<UserProfile> UserProfile { get; set; }
				public DbSet<UserAddress> UserAddress { get; set; }
				public DbSet<Category> Category { get; set; }
    }
}
