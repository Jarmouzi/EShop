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
        //        return Ok(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex);
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
        //        return Ok(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex);
        //    }
        //}
		//
        //[HttpDelete("Delete")]
        //public async Task<IActionResult> Delete(Int64 id)
        //{
        //    try
        //    {
        //        var result = await _Supplier_ContractRepository.DeleteAsync(id);
		//
        //        return Ok(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex);
        //    }
        //}


        [HttpGet("Get")]
        public async Task<IActionResult> Get(Int64 id)
        {
            try
            {
                var result =await  _Supplier_ContractRepository.GetByIdAsync(id);

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
                var result = await _Supplier_ContractRepository.GetAllAsync();

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
                var result = await _Supplier_ContractRepository.GetProcedureAsync("Supplier_Contract_Json", json);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}