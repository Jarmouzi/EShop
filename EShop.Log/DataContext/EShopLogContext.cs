using EShop.LogService.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.LogService.DBContext
{
    public class EShopLogContext: DbContext
    {
        public DbSet<VisitLog> VisitLog { get; set; }
        public DbSet<ActionLog> ActionLog { get; set; }
    }
}
