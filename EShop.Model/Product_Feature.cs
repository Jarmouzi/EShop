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
        public Int64? ProductId { get; set; }
        public Int64? FeatureId { get; set; }
        public string? Value { get; set; }
        public int? DisplayOrder { get; set; }
        public bool? Confirmed { get; set; }

        [ForeignKey("FeatureId")]
        public virtual Feature Feature { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
    }
}