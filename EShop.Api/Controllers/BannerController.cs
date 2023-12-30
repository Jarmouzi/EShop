using EShop.IdentityService.Infrastructure.Authorizaion;
using EShop.Repository.Interface;
using EShop.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace EShop.Web.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
    [AuthorizeApi]
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
        //public async Task<IActionResult> Delete(Guid id)
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
        public async Task<IActionResult> Get(Guid id)
        {
            try
            {
                var result =await  _BannerRepository.GetByIdAsync(id);

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
                var result = await _BannerRepository.GetAllAsync();

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
                var result = await _BannerRepository.GetPrecedureAsync("Banner_Json", json);

                return Ok(new { Data = result.Data, Message = result.Message, Status = result.Status });
            }
            catch (Exception ex)
            {
                return Ok(new { Message = ex.Message, Status = "server-error" });
            }
        }
    }
}