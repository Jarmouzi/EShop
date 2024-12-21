using EShop.IdentityService.Infrastructure.Authorizaion;
using EShop.Model.TypeSafe;
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

        //[HttpPost("Add")]
        //public async Task<IActionResult> Insert(ProductViewModel model)
        //{
        //    try
        //    {
        //        var result = await _ProductRepository.AddAsync(model);
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
        //public async Task<IActionResult> Update(ProductViewModel model)
        //{
        //    try
        //    {
        //        var result = await _ProductRepository.UpdateAsync(model);
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
        //        var result = await _ProductRepository.DeleteAsync(id);
        //
        //        return Ok(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex);
        //    }
        //}


        [HttpGet("Get")]
        public async Task<IActionResult> Get(string id)
        {
            try
            {
                var result = await _ProductRepository.GetProcedureAsync("Product_GetByHandle", id);

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
                var result = await _ProductRepository.GetProcedureAsync("Product_Handles");

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("GetFiltered")]
        public async Task<IActionResult> GetAll(string? variables = null)
        {
            try
            {
                var result = await _ProductRepository.GetProcedureAsync("Product_Json", variables);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("GetCollectionProducts")]
        public async Task<IActionResult> GetCollectionProducts(string? cn = null)
        {
            try
            {
                var result = await _ProductRepository.GetProcedureAsync("Product_GetByCollection", cn);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}