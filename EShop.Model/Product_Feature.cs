using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace EShop.Model
{
    public class Product_Feature : BaseModel
    {
        public Guid? featureId { get; set; }
        public string? Value { get; set; }
        public Guid? ItemId { get; set; }
        public bool? Active { get; set; }
        public bool? IsMain { get; set; }
        public string? ComponentName { get; set; }
        public int? DisplayOrder { get; set; }
        public bool? Confirmed { get; set; }
        public string? Title { get; set; }

        [ForeignKey("featureId")]
        public virtual Feature Feature { get; set; }

        [ForeignKey("ItemId")]
        public virtual Product Product { get; set; }
        public virtual ICollection<Item_Feature_Detail> Item_Feature_Details { get; set; }
        //public virtual ICollection<ProductInPage> ProductInPages { get; set; }
    }
}