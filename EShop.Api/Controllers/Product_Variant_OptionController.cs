using EShop.IdentityService.Infrastructure.Authorizaion;
using EShop.Repository.Interface;
using EShop.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace EShop.Web.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
    [AuthorizeApi]
    public class Product_Variant_OptionController : ControllerBase
    {
		private readonly IProduct_Variant_OptionRepository _Product_Variant_OptionRepository;

        public Product_Variant_OptionController(IProduct_Variant_OptionRepository Product_Variant_OptionRepository)
        {
            _Product_Variant_OptionRepository = Product_Variant_OptionRepository;
        }
        //[HttpPost("Add")]
        //public async Task<IActionResult> Insert(Product_Variant_OptionViewModel model)
        //{
        //    try
        //    {
        //        var result = await _Product_Variant_OptionRepository.AddAsync(model);
        //        return Ok(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex);
        //    }
        //}
        //[HttpPut("Update")]
        //public async Task<IActionResult> Update(Product_Variant_OptionViewModel model)
        //{
        //    try
        //    {
        //        var result = await _Product_Variant_OptionRepository.UpdateAsync(model);
        //        return Ok(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex);
        //    }
        //}
        //[HttpDelete("Delete")]
        //public async Task<IActionResult> Delete(Int64 id)
        //{
        //    try
        //    {
        //        var result = await _Product_Variant_OptionRepository.DeleteAsync(id);
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
                var result =await  _Product_Variant_OptionRepository.GetByIdAsync(id);
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
                var result = await _Product_Variant_OptionRepository.GetAllAsync();
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
                var result = await _Product_Variant_OptionRepository.GetProcedureAsync("Product_Variant_Option_Json", json);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}