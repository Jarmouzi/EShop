using EShop.IdentityService.Infrastructure.Authorizaion;
using EShop.Repository.Interface;
using EShop.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace EShop.Web.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
    [AuthorizeApi]
    public class ProductVariantController : ControllerBase
    {
		private readonly IProductVariantRepository _ProductVariantRepository;

        public ProductVariantController(IProductVariantRepository ProductVariantRepository)
        {
            _ProductVariantRepository = ProductVariantRepository;
        }

        //[HttpPost("Add")]
        //public async Task<IActionResult> Insert(ProductVariantViewModel model)
        //{
        //    try
        //    {
        //        var result = await _ProductVariantRepository.AddAsync(model);
		//
        //        return Ok(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex);
        //    }
        //}
		//
        //[HttpPut("Update")]
        //public async Task<IActionResult> Update(ProductVariantViewModel model)
        //{
        //    try
        //    {
        //        var result = await _ProductVariantRepository.UpdateAsync(model);
		//
        //        return Ok(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex);
        //    }
        //}
		//
        //[HttpDelete("Delete")]
        //public async Task<IActionResult> Delete(Int64 id)
        //{
        //    try
        //    {
        //        var result = await _ProductVariantRepository.DeleteAsync(id);
		//
        //        return Ok(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex);
        //    }
        //}


        [HttpGet("Get")]
        public async Task<IActionResult> Get(Int64 id)
        {
            try
            {
                var result =await  _ProductVariantRepository.GetByIdAsync(id);

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
                var result = await _ProductVariantRepository.GetAllAsync();

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
                var result = await _ProductVariantRepository.GetProcedureAsync("ProductVariant_Json", json);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}