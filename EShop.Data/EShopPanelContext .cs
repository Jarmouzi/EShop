using EShop.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.DataContext
{
    public class EShopPanelContext : DbContext
    {

        public EShopPanelContext(DbContextOptions<EShopPanelContext> options) : base(options)
        {
        }

        public DbSet<PanelResource> PanelResource { get; set; }

    }
}
