using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.LogService.Model
{
    public class ActionLog
    {
        public ActionLog()
        {
            Date = DateTime.Now;
        }
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid VisitLogId { get; set; }
        public DateTime Date { get; set; }
        public string Page { get; set; }
        public string Action { get; set; }
        public string Parameters { get; set; }
    }
}
