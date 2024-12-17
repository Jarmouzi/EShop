using EShop.IdentityService.Infrastructure.Authorizaion;
using EShop.Repository.Interface;
using EShop.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EShop.Web.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
    [AuthorizeApi]
    public class UserProfileController : ControllerBase
    {
		private readonly IUserProfileRepository _UserProfileRepository;

        public UserProfileController(IUserProfileRepository UserProfileRepository)
        {
            _UserProfileRepository = UserProfileRepository;
        }
        [HttpPost("Add")]
        public async Task<IActionResult> Insert([FromForm] UserProfileViewModel model)
        {
            try 
            { 
                var result = await _UserProfileRepository.AddAsync(model); 
                return Ok(new { Data = result.Data, Message = result.Message, Status = result.Status }); 
            }
            catch (Exception ex) 
            { 
                return Ok(new { Message = ex.Message, Status = "server-error" }); 
            }
        }
        [HttpPut("Update")]
        public async Task<IActionResult> Update(UserProfileViewModel model)
        {
            try 
            { 
                var result = await _UserProfileRepository.UpdateAsync(model); 
                return Ok(result.Data); 
            }
            catch (Exception ex) 
            {
                return BadRequest(new { errors = ex });
            }
        }
        //[HttpDelete("Delete")]
        //public async Task<IActionResult> Delete(Int64 id)
        //{try {var result = await _UserProfileRepository.DeleteAsync(id); return Ok( new { Data = result.Data, Message = result.Message, Status = result.Status }); }
        // catch (Exception ex){return Ok( new { Message = ex.Message, Status = "server-error" });}
        //}
        [HttpGet("Get")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var result = await _UserProfileRepository.GetProcedureAsync("UserProfile_Get", 
                    User.FindFirstValue(ClaimTypes.NameIdentifier));
                return Ok(result.Data);
            }
            catch (Exception ex)
            {
                return BadRequest(new { errors = ex });
            }
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _UserProfileRepository.GetAllAsync();
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
                var result = await _UserProfileRepository.GetProcedureAsync("UserProfile_Json", json);
                return Ok(new { Data = result.Data, Message = result.Message, Status = result.Status });
            }
            catch (Exception ex)
            {
                return Ok(new { Message = ex.Message, Status = "server-error" });
            }
        }
    }
}