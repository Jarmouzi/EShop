using EShop.IdentityService.Infrastructure.Authorizaion;
using EShop.Repository.Interface;
using EShop.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace EShop.Web.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
    [AuthorizeApi]
    public class Supplier_ContractController : ControllerBase
    {
		private readonly ISupplier_ContractRepository _Supplier_ContractRepository;

        public Supplier_ContractController(ISupplier_ContractRepository Supplier_ContractRepository)
        {
            _Supplier_ContractRepository = Supplier_ContractRepository;
        }

        //[HttpPost("Add")]
        //public async Task<IActionResult> Insert(Supplier_ContractViewModel model)
        //{
        //    try
        //    {
        //        var result = await _Supplier_ContractRepository.AddAsync(model);
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
        //public async Task<IActionResult> Update(Supplier_ContractViewModel model)
        //{
        //    try
        //    {
        //        var result = await _Supplier_ContractRepository.UpdateAsync(model);
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
        //        var result = await _Supplier_ContractRepository.DeleteAsync(id);
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
                var result =await  _Supplier_ContractRepository.GetByIdAsync(id);

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
                var result = await _Supplier_ContractRepository.GetAllAsync();

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
                var result = await _Supplier_ContractRepository.GetProcedureAsync("Supplier_Contract_Json", json);

                return Ok(new { Data = result.Data, Message = result.Message, Status = result.Status });
            }
            catch (Exception ex)
            {
                return Ok(new { Message = ex.Message, Status = "server-error" });
            }
        }
    }
}