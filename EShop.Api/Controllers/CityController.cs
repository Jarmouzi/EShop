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
        //{try{var result = await _CityRepository.AddAsync(model); return Ok(result);}
        // catch (Exception ex){return BadRequest(ex);}
        //}
        //[HttpPut("Update")]public async Task<IActionResult> Update(CityViewModel model)
        //{try{var result = await _CityRepository.UpdateAsync(model);return Ok(result);}
        // catch (Exception ex){return BadRequest(ex);}
        //}
        //[HttpDelete("Delete")]
        //public async Task<IActionResult> Delete(Int64 id)
        //{try {var result = await _CityRepository.DeleteAsync(id); return Ok(result); }
        // catch (Exception ex){return BadRequest(ex);}
        //}
        [HttpGet("Get")]
        public async Task<IActionResult> Get(Int64 id)
        {
            try
            {
                var result =await  _CityRepository.GetByIdAsync(id);
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
                var result = await _CityRepository.GetAllAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
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
                    return Ok(result);
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