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
    public class UserAddressController : ControllerBase
    {
        private readonly IUserAddressRepository _UserAddressRepository;

        public UserAddressController(IUserAddressRepository UserAddressRepository)
        {
            _UserAddressRepository = UserAddressRepository;
        }
        [HttpPost("Add")]
        public async Task<IActionResult> Insert(UserAddressViewModel model)
        {
            try
            {
                model.UserId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                model.ModifiedBy = model.UserId;
                var result = await _UserAddressRepository.AddAsync(model);
                return Ok(new { Data = result.Data, Message = result.Message, Status = result.Status });
            }
            catch (Exception ex)
            {
                return Ok(new { Message = ex.Message, Status = "server-error" });
            }
        }
        [HttpPut("Update")]
        public async Task<IActionResult> Update(UserAddressViewModel model)
        {
            try
            {
                model.UserId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                model.ModifiedBy = model.UserId;
                var result = await _UserAddressRepository.UpdateAsync(model);
                return Ok(new { Data = result.Data, Message = result.Message, Status = result.Status });
            }
            catch (Exception ex)
            {
                return Ok(new { Message = ex.Message, Status = "server-error" });
            }
        }
        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(Int64 id)
        {
            try
            {
                var result = await _UserAddressRepository.DeleteAsync(id);
                return Ok(new { Data = result.Data, Message = result.Message, Status = result.Status });
            }
            catch (Exception ex)
            {
                return Ok(new { Message = ex.Message, Status = "server-error" });
            }
        }
        [HttpGet("Get")]
        public async Task<IActionResult> Get(Int64 id)
        {
            try
            {
                var result = await _UserAddressRepository.GetByIdAsync(id);
                return Ok(new { Data = result.Data, Message = result.Message, Status = result.Status });
            }
            catch (Exception ex)
            {
                return Ok(new { Message = ex.Message, Status = "server-error" });
            }
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                if (Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var userId))
                {
                    var result = await _UserAddressRepository.GetAllAsync(m => m.UserId == userId);
                    return Ok(new { Data = result.Data, Message = result.Message, Status = result.Status });
                }
                return Ok(new { Data = new List<UserAddressViewModel>(), Message = "" });
            }
            catch (Exception ex)
            {
                return Ok(new { Message = ex.Message, Status = "server-error" });
            }
        }

        [HttpGet("GetFiltered")]
        public async Task<IActionResult> GetAll(string? json = null)
        {
            try
            {
                var result = await _UserAddressRepository.GetProcedureAsync("UserAddress_Json", json);
                return Ok(new { Data = result.Data, Message = result.Message, Status = result.Status });
            }
            catch (Exception ex)
            {
                return Ok(new { Message = ex.Message, Status = "server-error" });
            }
        }
    }
}