using EShop.IdentityService.Infrastructure.Authorizaion;
using EShop.Repository.Interface;
using EShop.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace EShop.Web.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
    //[AuthorizeApi]
    public class CategoryController : ControllerBase
    {
		private readonly ICategoryRepository _CategoryRepository;

        public CategoryController(ICategoryRepository CategoryRepository)
        {
            _CategoryRepository = CategoryRepository;
        }

        //[HttpPost("Add")]
        //public async Task<IActionResult> Insert(CategoryViewModel model)
        //{
        //    try
        //    {
        //        var result = await _CategoryRepository.AddAsync(model);

        //        return Ok(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex);
        //    }
        //}

        //[HttpPut("Update")]
        //public async Task<IActionResult> Update(CategoryViewModel model)
        //{
        //    try
        //    {
        //        var result = await _CategoryRepository.UpdateAsync(model);

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
        //        var result = await _CategoryRepository.DeleteAsync(id);

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
                var result =await  _CategoryRepository.GetByIdAsync(id);

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
                var result = await _CategoryRepository.GetAllAsync();

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
                if(json == null) json = "{ \"Level1Id\": null,\"Level2Id\": null, \"Take\": 10, \"Skip\": 0}";
                var result = await _CategoryRepository.GetProcedureAsync("Category_Json", json);

                return Ok( result );
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("GetMenu")]
        public async Task<IActionResult> GetMenu()
        {
            try
            {
                var result = await _CategoryRepository.GetProcedureAsync("Menu_Json", null);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("GetCollection")]
        public async Task<IActionResult> GetCollection(string? json = null)
        {
            try
            {
                var result = await _CategoryRepository.GetProcedureAsync("Menu_Json", null);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}