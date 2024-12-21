using EShop.IdentityService.Infrastructure.Authorizaion;
using EShop.Repository.Interface;
using EShop.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace EShop.Web.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
    [AuthorizeApi]
    public class Product_ImageController : ControllerBase
    {
		private readonly IProduct_ImageRepository _Product_ImageRepository;

        public Product_ImageController(IProduct_ImageRepository Product_ImageRepository)
        {
            _Product_ImageRepository = Product_ImageRepository;
        }

        //[HttpPost("Add")]
        //public async Task<IActionResult> Insert(Product_ImageViewModel model)
        //{
        //    try
        //    {
        //        var result = await _Product_ImageRepository.AddAsync(model);
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
        //public async Task<IActionResult> Update(Product_ImageViewModel model)
        //{
        //    try
        //    {
        //        var result = await _Product_ImageRepository.UpdateAsync(model);
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
        //        var result = await _Product_ImageRepository.DeleteAsync(id);
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
                var result =await  _Product_ImageRepository.GetByIdAsync(id);

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
                var result = await _Product_ImageRepository.GetAllAsync();

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
                var result = await _Product_ImageRepository.GetProcedureAsync("Product_Image_Json", json);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}