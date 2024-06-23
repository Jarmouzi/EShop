using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Model
{
    [Table("Product_Image")]
    public class Product_Image: BaseModel
    {
        public Int64 ProductId { get; set; }
        public Int64? Product_OptionId { get; set; }
        public Int64 ImageId { get; set; }
        public bool Featured { get; set; } = false;
        public bool Confirmed { get; set; } = false;

        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }

        [ForeignKey("ImageId")]
        public virtual Image Image { get; set; }

        [ForeignKey("Product_OptionId")]
        public virtual Product_Option Product_Option { get; set; }
    }
}
