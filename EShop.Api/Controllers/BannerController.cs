using EShop.IdentityService.Infrastructure.Authorizaion;
using EShop.Repository.Interface;
using EShop.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace EShop.Web.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
    //[AuthorizeApi]
    public class BannerController : ControllerBase
    {
		private readonly IBannerRepository _BannerRepository;

        public BannerController(IBannerRepository BannerRepository)
        {
            _BannerRepository = BannerRepository;
        }

        //[HttpPost("Add")]
        //public async Task<IActionResult> Insert(BannerViewModel model)
        //{
        //    try
        //    {
        //        var result = await _BannerRepository.AddAsync(model);
		//
        //        return Ok( new { Data = result.Data, Message = result.Message, Status = result.Status });
        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok( new { Message = ex.Message, Status = "server-error" });
        //    }
        //}
		//
        //[HttpPut("Update")]
        //public async Task<IActionResult> Update(BannerViewModel model)
        //{
        //    try
        //    {
        //        var result = await _BannerRepository.UpdateAsync(model);
		//
        //        return Ok( new { Data = result.Data, Message = result.Message, Status = result.Status });
        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok( new { Message = ex.Message, Status = "server-error" });
        //    }
        //}
		//
        //[HttpDelete("Delete")]
        //public async Task<IActionResult> Delete(Int64 id)
        //{
        //    try
        //    {
        //        var result = await _BannerRepository.DeleteAsync(id);
		//
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
                var result =await  _BannerRepository.GetByIdAsync(id);

                return Ok(result.Data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _BannerRepository.GetAllAsync(m => m.ExpireDate == null && m.Confirmed == true);

                return Ok(result.Data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetFiltered")]
        public async Task<IActionResult> GetAll(string? json = null)
        {
            try
            {
                var result = await _BannerRepository.GetProcedureAsync("Banner_Json", json);

                return Ok(result.Data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}