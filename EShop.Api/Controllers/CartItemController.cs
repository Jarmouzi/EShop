using EShop.IdentityService.Infrastructure.Authorizaion;
using EShop.Repository.Interface;
using EShop.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace EShop.Web.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
    [AuthorizeApi]
    public class CartItemController : ControllerBase
    {
		private readonly ICartItemRepository _CartItemRepository;

        public CartItemController(ICartItemRepository CartItemRepository)
        {
            _CartItemRepository = CartItemRepository;
        }
        //[HttpPost("Add")]
        //public async Task<IActionResult> Insert(CartItemViewModel model)
        //{try{var result = await _CartItemRepository.AddAsync(model); return Ok( new { Data = result.Data, Message = result.Message, Status = result.Status });}
        // catch (Exception ex){return Ok( new { Message = ex.Message, Status = "server-error" });}
        //}
        //[HttpPut("Update")]public async Task<IActionResult> Update(CartItemViewModel model)
        //{try{var result = await _CartItemRepository.UpdateAsync(model);return Ok( new { Data = result.Data, Message = result.Message, Status = result.Status });}
        // catch (Exception ex){return Ok( new { Message = ex.Message, Status = "server-error" });}
        //}
        //[HttpDelete("Delete")]
        //public async Task<IActionResult> Delete(Int64 id)
        //{try {var result = await _CartItemRepository.DeleteAsync(id); return Ok( new { Data = result.Data, Message = result.Message, Status = result.Status }); }
        // catch (Exception ex){return Ok( new { Message = ex.Message, Status = "server-error" });}
        //}
        [HttpGet("Get")]
        public async Task<IActionResult> Get(Int64 id)
        {
            try
            {
                var result =await  _CartItemRepository.GetByIdAsync(id);
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
                var result = await _CartItemRepository.GetAllAsync();
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
                var result = await _CartItemRepository.GetProcedureAsync("CartItem_Json", json);
                return Ok(new { Data = result.Data, Message = result.Message, Status = result.Status });
            }
            catch (Exception ex)
            {
                return Ok(new { Message = ex.Message, Status = "server-error" });
            }
        }
    }
}