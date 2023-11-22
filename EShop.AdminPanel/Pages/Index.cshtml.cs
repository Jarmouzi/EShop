using EShop.LogService.Repository;
using EShop.Model;
using EShop.Model.TypeSafe;
using EShop.Service.Interface;
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
        private readonly IRepository<Category, CategoryViewModel> _categoryRepository;
        private readonly IRepository<Product, ProductViewModel> _productRepository;

        public IEnumerable<CategoryViewModel> Categories { get; set; }
        public IEnumerable<ProductViewModel> Products { get; set; }

        public IndexModel(ILogger<IndexModel> logger
            , ILogRepository dbLog
            , IRepository<Category, CategoryViewModel> categoryRepository
            , IRepository<Product, ProductViewModel> productRepository)
        {
            _logger = logger;
            _dbLog = dbLog;
            _categoryRepository = categoryRepository;
            _productRepository = productRepository;
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
                var result = await _categoryRepository.GetAllAsync();

                if (result.Status == TS.Status.Success)
                {
                    Categories = result.Data;
                }

                var r = await _productRepository.GetAllAsync();

                if (r.Status == TS.Status.Success)
                {
                    Products = r.Data;
                }

                _dbLog.AddActionLogAsync(new LogService.Model.ActionLog
                {
                    Page = "Home",
                    Action = "LoadModel",
                    UserId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)),
                    VisitLogId = Guid.Parse(User.FindFirstValue("VLI"))
                });

            }
            catch (Exception ex)
            {
                _logger.LogError("error raised on HomePage data loading: {ex}", ex);
            }
        }
    }
}
