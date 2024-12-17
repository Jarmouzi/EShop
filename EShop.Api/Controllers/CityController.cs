using EShop.IdentityService.Infrastructure.Authorizaion;
using EShop.Repository.Interface;
using EShop.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace EShop.Web.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
    [AuthorizeApi]
    public class CityController : ControllerBase
    {
		private readonly ICityRepository _CityRepository;

        public CityController(ICityRepository CityRepository)
        {
            _CityRepository = CityRepository;
        }
        //[HttpPost("Add")]
        //public async Task<IActionResult> Insert(CityViewModel model)
        //{try{var result = await _CityRepository.AddAsync(model); return Ok( new { Data = result.Data, Message = result.Message, Status = result.Status });}
        // catch (Exception ex){return Ok( new { Message = ex.Message, Status = "server-error" });}
        //}
        //[HttpPut("Update")]public async Task<IActionResult> Update(CityViewModel model)
        //{try{var result = await _CityRepository.UpdateAsync(model);return Ok( new { Data = result.Data, Message = result.Message, Status = result.Status });}
        // catch (Exception ex){return Ok( new { Message = ex.Message, Status = "server-error" });}
        //}
        //[HttpDelete("Delete")]
        //public async Task<IActionResult> Delete(Int64 id)
        //{try {var result = await _CityRepository.DeleteAsync(id); return Ok( new { Data = result.Data, Message = result.Message, Status = result.Status }); }
        // catch (Exception ex){return Ok( new { Message = ex.Message, Status = "server-error" });}
        //}
        [HttpGet("Get")]
        public async Task<IActionResult> Get(Int64 id)
        {
            try
            {
                var result =await  _CityRepository.GetByIdAsync(id);
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
                var result = await _CityRepository.GetAllAsync();
                return Ok( new { Data = result.Data, Message = result.Message, Status = result.Status });
            }
            catch (Exception ex)
            {
                return Ok( new { Message = ex.Message, Status = "server-error" });
            }
        }

        [HttpGet("GetFiltered")]
        public async Task<IActionResult> GetAll(string? sId = null)
        {
            try
            {
                if (long.TryParse(sId, out long stateId))
                {
                    var result = await _CityRepository.GetAllItemAsync(m => m.StateId == stateId);
                    return Ok(new { Data = result.Data, Message = result.Message, Status = result.Status });
                }
                return Ok(new { Data = new List<SelectItemViewModel>(), Message = "", Status = 200 });

            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}