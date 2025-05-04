using EShop.IdentityService.Infrastructure.Authorizaion;
using EShop.Repository.Interface;
using EShop.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Security.Claims;

namespace EShop.Web.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[AuthorizeApi]
    public class CartController : ControllerBase
    {
        private readonly ICartRepository _CartRepository;

        public CartController(ICartRepository CartRepository)
        {
            _CartRepository = CartRepository;
        }
        [HttpGet("Add")]
        public async Task<IActionResult> Add()
        {
            try
            {
                CartViewModel model = new CartViewModel();

                if (User.Identity != null && User.Identity.IsAuthenticated)// User.Identities.Any())//m => m.Name == "IsAuthenticated"))
                {
                    model.ModifiedBy = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                }

                var result = await _CartRepository.AddAsync(model);

                if(result != null && result.Handle != null)
                    return await Get(result.Handle);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [HttpGet("Update")]
        public async Task<IActionResult> Update(string? json = null)
        {
            try
            {
                var result = await _CartRepository.GetProcedureAsync("Cart_Update", json);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("AddToCart")]
        public async Task<IActionResult> AddToCart(string? json = null)
        {
            try
            {
                var result = await _CartRepository.GetProcedureAsync("Cart_AddTo", json);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [HttpGet("RemoveFromCart")]
        public async Task<IActionResult> RemoveFromCart(string? json = null)
        {
            try
            {
                var result = await _CartRepository.GetProcedureAsync("Cart_RemoveFrom", json);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex); 
            }
        }
        //[HttpPut("Update")]
        //public async Task<IActionResult> Update(CartViewModel model)
        //{
        //    try { var result = await _CartRepository.UpdateAsync(model); return Ok(result); }
        //    catch (Exception ex) { return BadRequest(ex); }
        //}
        //[HttpDelete("Delete")]
        //public async Task<IActionResult> Delete(Int64 id)
        //{
        //    try { var result = await _CartRepository.DeleteAsync(id); return Ok(result); }
        //    catch (Exception ex) { return BadRequest(ex); }
        //}

        [HttpGet("Get")]
        public async Task<IActionResult> Get(string id)
        {
            try
            {
                string json = $"{{ \"id\": \"{id}\"}}";

                if (User.Identity != null && User.Identity.IsAuthenticated)
                {
                    JObject jsonObject = JObject.Parse(json);

                    jsonObject["UserId"] = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)).ToString();

                    json = jsonObject.ToString(Formatting.None);
                }

                var result = await _CartRepository.GetProcedureAsync("Cart_Get", json);

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
                var result = await _CartRepository.GetProcedureAsync("Cart_GetAll", User.FindFirstValue(ClaimTypes.NameIdentifier));
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        //[HttpGet("GetFiltered")]
        //public async Task<IActionResult> GetAll(string? json = null)
        //{
        //    try
        //    {
        //        var result = await _CartRepository.GetProcedureAsync("Cart_Json", json);
        //        return Ok(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex);
        //    }
        //}
    }
}