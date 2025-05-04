using EShop.IdentityService.Infrastructure.Authorizaion;
using EShop.Repository.Interface;
using EShop.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace EShop.Web.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AuthorizeApi]
    public class UserOrderController(IUserOrderRepository userOrderRepository) : ControllerBase
    {
        [HttpPut("Update")]
        public async Task<IActionResult> Update(UserOrderViewModel model)
        {
            try
            {
                var result = await userOrderRepository.UpdateAsync(model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("Get")]
        public async Task<IActionResult> Get(string id)
        {
            try
            {
                var result = await userOrderRepository.GetAsync(m => m.Cart.Handle == id);

                if (result == null)
                    return NotFound();

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
                var result = await userOrderRepository.GetAllAsync();
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
                var result = await userOrderRepository.GetProcedureAsync("UserOrder_Json", json);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}