using EShop.LogService.Repository;
using EShop.Model;
using EShop.Model.TypeSafe;
using EShop.Repository.Interface;
using EShop.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.WebSockets;
using System.Security.Claims;

namespace EShop.AdminPanel.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ILogRepository _dbLog;
       
        public IEnumerable<CategoryViewModel> Categories { get; set; }
        public IEnumerable<ProductViewModel> Products { get; set; }

        public IndexModel(ILogger<IndexModel> logger
            , ILogRepository dbLog)
        {
            _logger = logger;
            _dbLog = dbLog;
        }
        public async void OnGet()
        {
            LoadModel().Wait();
            //_logger.LogInformation("Main page called at {DT}", DateTime.Now);

        }

        private async Task LoadModel()
        {
            try
            {


                //_dbLog.AddActionLogAsync(new LogService.Model.ActionLog
                //{
                //    Page = "Home",
                //    Action = "LoadModel",
                //    UserId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)),
                //    VisitLogId = Guid.Parse(User.FindFirstValue("VLI"))
                //});

            }
            catch (Exception ex)
            {
                _logger.LogError("error raised on HomePage data loading: {ex}", ex);
            }
        }
    }
}
