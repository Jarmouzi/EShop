using EShop.IdentityService.Infrastructure.Authorizaion;
using EShop.Repository.Interface;
using EShop.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace EShop.Web.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
    //[AuthorizeApi]
    public class ProductController : ControllerBase
    {
		private readonly IProductRepository _ProductRepository;

        public ProductController(IProductRepository ProductRepository)
        {
            _ProductRepository = ProductRepository;
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Insert(ProductViewModel model)
        {
            try
            {
                var result = await _ProductRepository.AddAsync(model);

                return Ok( new { Data = result.Data, Message = result.Message, Status = result.Status });
            }
            catch (Exception ex)
            {
                return Ok( new { Message = ex.Message, Status = "server-error" });
            }
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update(ProductViewModel model)
        {
            try
            {
                var result = await _ProductRepository.UpdateAsync(model);

                return Ok( new { Data = result.Data, Message = result.Message, Status = result.Status });
            }
            catch (Exception ex)
            {
                return Ok( new { Message = ex.Message, Status = "server-error" });
            }
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var result = await _ProductRepository.DeleteAsync(id);

                return Ok( new { Data = result.Data, Message = result.Message, Status = result.Status });
            }
            catch (Exception ex)
            {
                return Ok( new { Message = ex.Message, Status = "server-error" });
            }
        }


        [HttpGet("Get")]
        public async Task<IActionResult> Get(Guid id)
        {
            try
            {
                var result =await  _ProductRepository.GetByIdAsync(id);

                return Ok( new { Data = result.Data, Message = result.Message, Status = result.Status });
            }
            catch (Exception ex)
            {
                return Ok( new { Message = ex.Message, Status = "server-error" });
            }
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _ProductRepository.GetAllAsync();

                return Ok( new { Data = result.Data, Message = result.Message, Status = result.Status });
            }
            catch (Exception ex)
            {
                return Ok( new { Message = ex.Message, Status = "server-error" });
            }
        }

        [HttpGet("GetFiltered")]
        public async Task<IActionResult> GetAll(string? json = null)
        {
            try
            {
                var result = await _ProductRepository.GetPrecedureAsync("Product_Json", json);

                return Ok(new { Data = result.Data, Message = result.Message, Status = result.Status });
            }
            catch (Exception ex)
            {
                return Ok(new { Message = ex.Message, Status = "server-error" });
            }
        }
    }
} 