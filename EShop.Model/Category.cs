using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Model
{
    public class Category: BaseModel
    {
        public string Title{ get; set;}
        public Byte Level{ get; set;}
        public Guid? ParentId{ get; set;}
        public int DisplayOrder{ get; set;}
        public bool? Confirmed{ get; set;}
    }
}
