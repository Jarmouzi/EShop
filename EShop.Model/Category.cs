using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EShop.Model
{
    public class Category : BaseModel
    {
        public string Title { get; set; }

        
        public string Handle { get; set; }
        public string? Path { get; set; }
        public Byte Level { get; set; }
        public Int64? ParentId { get; set; }
        public int? DisplayOrder { get; set; }
        public bool? Confirmed { get; set; }
        public virtual Category Parent { get; set; }
        public virtual ICollection<Filter> Filters { get; set; }
        public virtual ICollection<Product> Products { get; set; }
        public virtual ICollection<Supplier_Contract> Supplier_Contracts { get; set; }
    }
}