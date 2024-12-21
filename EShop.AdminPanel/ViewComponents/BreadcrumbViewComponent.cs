using EShop.Model;
using EShop.Repository.Interface;
using EShop.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace EShop.AdminPanel.ViewComponents
{
    public class BreadcrumbViewComponent : ViewComponent
    {
        private readonly IRepository<PanelResource, PanelResourceViewModel> _resourceRepository;

        public BreadcrumbViewComponent(IRepository<PanelResource, PanelResourceViewModel> resourceRepository)
        {
            _resourceRepository = resourceRepository;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            //check user access
            var resources = await _resourceRepository.GetAllAsync();

            return View(resources);
        }
    }
}
