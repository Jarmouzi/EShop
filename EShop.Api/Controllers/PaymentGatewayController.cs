using EShop.IdentityService.Infrastructure.Authorizaion;
using EShop.Repository.Interface;
using EShop.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace EShop.Web.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
    [AuthorizeApi]
    public class PaymentGatewayController : ControllerBase
    {
		private readonly IPaymentGatewayRepository _PaymentGatewayRepository;

        public PaymentGatewayController(IPaymentGatewayRepository PaymentGatewayRepository)
        {
            _PaymentGatewayRepository = PaymentGatewayRepository;
        }
        //[HttpPost("Add")]
        //public async Task<IActionResult> Insert(PaymentGatewayViewModel model)
        //{try{var result = await _PaymentGatewayRepository.AddAsync(model); return Ok( new { Data = result.Data, Message = result.Message, Status = result.Status });}
        // catch (Exception ex){return BadRequest(ex);}
        //}
        //[HttpPut("Update")]public async Task<IActionResult> Update(PaymentGatewayViewModel model)
        //{try{var result = await _PaymentGatewayRepository.UpdateAsync(model);return Ok( new { Data = result.Data, Message = result.Message, Status = result.Status });}
        // catch (Exception ex){return BadRequest(ex);}
        //}
        //[HttpDelete("Delete")]
        //public async Task<IActionResult> Delete(Int64 id)
        //{try {var result = await _PaymentGatewayRepository.DeleteAsync(id); return Ok( new { Data = result.Data, Message = result.Message, Status = result.Status }); }
        // catch (Exception ex){return BadRequest(ex);}
        //}
        [HttpGet("Get")]
        public async Task<IActionResult> Get(Int64 id)
        {
            try
            {
                var result =await  _PaymentGatewayRepository.GetByIdAsync(id);
                return Ok( new { Data = result.Data, Message = result.Message, Status = result.Status });
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
                var result = await _PaymentGatewayRepository.GetAllAsync();
                return Ok( new { Data = result.Data, Message = result.Message, Status = result.Status });
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
                var result = await _PaymentGatewayRepository.GetProcedureAsync("PaymentGateway_Json", json);
                return Ok(new { Data = result.Data, Message = result.Message, Status = result.Status });
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}