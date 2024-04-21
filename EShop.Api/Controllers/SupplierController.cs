using EShop.IdentityService.Infrastructure.Authorizaion;
using EShop.Repository.Interface;
using EShop.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace EShop.Web.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
    [AuthorizeApi]
    public class SupplierController : ControllerBase
    {
		private readonly ISupplierRepository _SupplierRepository;

        public SupplierController(ISupplierRepository SupplierRepository)
        {
            _SupplierRepository = SupplierRepository;
        }

        //[HttpPost("Add")]
        //public async Task<IActionResult> Insert(SupplierViewModel model)
        //{
        //    try
        //    {
        //        var result = await _SupplierRepository.AddAsync(model);
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
        //public async Task<IActionResult> Update(SupplierViewModel model)
        //{
        //    try
        //    {
        //        var result = await _SupplierRepository.UpdateAsync(model);
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
        //        var result = await _SupplierRepository.DeleteAsync(id);
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
                var result =await  _SupplierRepository.GetByIdAsync(id);

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
                var result = await _SupplierRepository.GetAllAsync();

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
                var result = await _SupplierRepository.GetProcedureAsync("Supplier_Json", json);

                return Ok(new { Data = result.Data, Message = result.Message, Status = result.Status });
            }
            catch (Exception ex)
            {
                return Ok(new { Message = ex.Message, Status = "server-error" });
            }
        }
    }
}