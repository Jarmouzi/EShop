using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace EShop.Model
{
    public class UserAddress: BaseModel
    { 
		public Guid? UserId { get; set; }
		public Int64? StateId { get; set; }
		public Int64? CityId { get; set; }
        public string? Title { get; set; }

        [Column(TypeName = "nvarchar(max)")]
		public string? Address { get; set; }
        public string? ReceiverPhoneNumber { get; set; }
        public string? ReceiverName { get; set; }
        public int? Number { get; set; }
        public string? Unit { get; set; }
        public string? PostalCode { get; set; }
        public bool? IsDefault { get; set; }
        public Double? Latitude { get; set; }
        public Double? Longtitude { get; set; }
    }
}