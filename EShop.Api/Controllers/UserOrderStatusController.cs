using EShop.IdentityService.Infrastructure.Authorizaion;
using EShop.Repository.Interface;
using EShop.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace EShop.Web.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
    [AuthorizeApi]
    public class UserOrderStatusController : ControllerBase
    {
		private readonly IUserOrderStatusRepository _UserOrderStatusRepository;

        public UserOrderStatusController(IUserOrderStatusRepository UserOrderStatusRepository)
        {
            _UserOrderStatusRepository = UserOrderStatusRepository;
        }
        //[HttpPost("Add")]
        //public async Task<IActionResult> Insert(UserOrderStatusViewModel model)
        //{try{var result = await _UserOrderStatusRepository.AddAsync(model); return Ok(result);}
        // catch (Exception ex){return BadRequest(ex);}
        //}
        //[HttpPut("Update")]public async Task<IActionResult> Update(UserOrderStatusViewModel model)
        //{try{var result = await _UserOrderStatusRepository.UpdateAsync(model);return Ok(result);}
        // catch (Exception ex){return BadRequest(ex);}
        //}
        //[HttpDelete("Delete")]
        //public async Task<IActionResult> Delete(Int64 id)
        //{try {var result = await _UserOrderStatusRepository.DeleteAsync(id); return Ok(result); }
        // catch (Exception ex){return BadRequest(ex);}
        //}
        [HttpGet("Get")]
        public async Task<IActionResult> Get(Int64 id)
        {
            try
            {
                var result =await  _UserOrderStatusRepository.GetByIdAsync(id);
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
                var result = await _UserOrderStatusRepository.GetAllAsync();
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
                var result = await _UserOrderStatusRepository.GetProcedureAsync("UserOrderStatus_Json", json);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}