using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace EShop.Model
{
    public class Product_Option : BaseModel
    {
        public Int64? ProductId { get; set; }
        public Int64? OptionId { get; set; }
        public Int64? OptionValueId { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }

        [ForeignKey("OptionId")]
        public virtual Option Option { get; set; }

        [ForeignKey("OptionValueId")]
        public virtual OptionValue OptionValue { get; set; }
        public virtual ICollection<Product_Variant_Option> Product_Variant_Options { get; set; }
        public virtual ICollection<Product_Image> Product_Images { get; set; }
    }
}