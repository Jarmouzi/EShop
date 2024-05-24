using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.LogService.Model
{
    public class VisitLog
    {
        public Int64 Id { get; set; }
        public string Username { get; set; }
        public string IP { get; set; }
        public string DeviceInfo { get; set; }
        public DateTime Date { get; set; }
        public DateTime ExpireDate { get; set; }
        public string Language { get; set; }
        /// <summary>
        /// region Ids added comma seperated
        /// </summary>
        public string Regions { get; set; }
    }
}
