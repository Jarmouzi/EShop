using EShop.IdentityService.Infrastructure.Authorizaion;
using EShop.Repository.Interface;
using EShop.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace EShop.Web.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
    [AuthorizeApi]
    public class ProductVariant_OptionController : ControllerBase
    {
		private readonly IProductVariant_OptionRepository _ProductVariant_OptionRepository;

        public ProductVariant_OptionController(IProductVariant_OptionRepository ProductVariant_OptionRepository)
        {
            _ProductVariant_OptionRepository = ProductVariant_OptionRepository;
        }
        //[HttpPost("Add")]
        //public async Task<IActionResult> Insert(ProductVariant_OptionViewModel model)
        //{try{var result = await _ProductVariant_OptionRepository.AddAsync(model); return Ok(result);}
        // catch (Exception ex){return BadRequest(ex);}
        //}
        //[HttpPut("Update")]public async Task<IActionResult> Update(ProductVariant_OptionViewModel model)
        //{try{var result = await _ProductVariant_OptionRepository.UpdateAsync(model);return Ok(result);}
        // catch (Exception ex){return BadRequest(ex);}
        //}
        //[HttpDelete("Delete")]
        //public async Task<IActionResult> Delete(Int64 id)
        //{try {var result = await _ProductVariant_OptionRepository.DeleteAsync(id); return Ok(result); }
        // catch (Exception ex){return BadRequest(ex);}
        //}
        [HttpGet("Get")]
        public async Task<IActionResult> Get(Int64 id)
        {
            try
            {
                var result =await  _ProductVariant_OptionRepository.GetByIdAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _ProductVariant_OptionRepository.GetAllAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("GetFiltered")]
        public async Task<IActionResult> GetAll(string? json = null)
        {
            try
            {
                var result = await _ProductVariant_OptionRepository.GetProcedureAsync("ProductVariant_Option_Json", json);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}