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
        public DbSet<Brand> Brand { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Component> Component { get; set; }
        public DbSet<Feature> Feature { get; set; }
        public DbSet<Filter> Filter { get; set; }
        public DbSet<Item_Feature_Detail> Item_Feature_Detail { get; set; }
        public DbSet<Item_Feature_Details> Item_Feature_Details { get; set; }
        public DbSet<Page> Page { get; set; }
        public DbSet<Page_Item_Feature> Page_Item_Feature { get; set; }
        public DbSet<Page_Item_Supplier> Page_Item_Supplier { get; set; }
        public DbSet<PanelResource> PanelResource { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<Product_Feature> Product_Feature { get; set; }
        public DbSet<ProductInPage> ProductInPage { get; set; }
        public DbSet<Region> Region { get; set; }
        public DbSet<SaleType> SaleType { get; set; }
        public DbSet<Stock> Stock { get; set; }
        public DbSet<Supplier> Supplier { get; set; }
        public DbSet<Supplier_Contract> Supplier_Contract { get; set; }
    }
}
