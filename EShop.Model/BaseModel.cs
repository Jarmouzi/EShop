using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Model
{
    public abstract class BaseModel 
    {
        public Int64 Id { get; set; }
        public Guid? ModifiedBy { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? ModifyDate { get; set; }
        public DateTime? ExpireDate { get; set; }
    }
}
