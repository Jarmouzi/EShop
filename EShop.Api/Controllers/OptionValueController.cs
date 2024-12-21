using EShop.IdentityService.Infrastructure.Authorizaion;
using EShop.Repository.Interface;
using EShop.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace EShop.Web.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
    [AuthorizeApi]
    public class OptionValueController : ControllerBase
    {
		private readonly IOptionValueRepository _OptionValueRepository;

        public OptionValueController(IOptionValueRepository OptionValueRepository)
        {
            _OptionValueRepository = OptionValueRepository;
        }

        //[HttpPost("Add")]
        //public async Task<IActionResult> Insert(OptionValueViewModel model)
        //{
        //    try
        //    {
        //        var result = await _OptionValueRepository.AddAsync(model);
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
        //public async Task<IActionResult> Update(OptionValueViewModel model)
        //{
        //    try
        //    {
        //        var result = await _OptionValueRepository.UpdateAsync(model);
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
        //        var result = await _OptionValueRepository.DeleteAsync(id);
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
                var result =await  _OptionValueRepository.GetByIdAsync(id);

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
                var result = await _OptionValueRepository.GetAllAsync();

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
                var result = await _OptionValueRepository.GetProcedureAsync("OptionValue_Json", json);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}