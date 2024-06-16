using EShop.IdentityService.Infrastructure.Authorizaion;
using EShop.Repository.Interface;
using EShop.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace EShop.Web.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
    [AuthorizeApi]
    public class FeatureValueController : ControllerBase
    {
		private readonly IFeatureValueRepository _FeatureValueRepository;

        public FeatureValueController(IFeatureValueRepository FeatureValueRepository)
        {
            _FeatureValueRepository = FeatureValueRepository;
        }
        //[HttpPost("Add")]
        //public async Task<IActionResult> Insert(FeatureValueViewModel model)
        //{
        //    try
        //    {
        //        var result = await _FeatureValueRepository.AddAsync(model);
        //        return Ok( new { Data = result.Data, Message = result.Message, Status = result.Status });
        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok( new { Message = ex.Message, Status = "server-error" });
        //    }
        //}
        //[HttpPut("Update")]
        //public async Task<IActionResult> Update(FeatureValueViewModel model)
        //{
        //    try
        //    {
        //        var result = await _FeatureValueRepository.UpdateAsync(model);
        //        return Ok( new { Data = result.Data, Message = result.Message, Status = result.Status });
        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok( new { Message = ex.Message, Status = "server-error" });
        //    }
        //}
        //[HttpDelete("Delete")]
        //public async Task<IActionResult> Delete(Int64 id)
        //{
        //    try
        //    {
        //        var result = await _FeatureValueRepository.DeleteAsync(id);
        //        return Ok( new { Data = result.Data, Message = result.Message, Status = result.Status });
        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok( new { Message = ex.Message, Status = "server-error" });
        //    }
        //}
        [HttpGet("Get")]
        public async Task<IActionResult> Get(Int64 id)
        {
            try
            {
                var result =await  _FeatureValueRepository.GetByIdAsync(id);
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
                var result = await _FeatureValueRepository.GetAllAsync();
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
                var result = await _FeatureValueRepository.GetProcedureAsync("FeatureValue_Json", json);
                return Ok(new { Data = result.Data, Message = result.Message, Status = result.Status });
            }
            catch (Exception ex)
            {
                return Ok(new { Message = ex.Message, Status = "server-error" });
            }
        }
    }
}