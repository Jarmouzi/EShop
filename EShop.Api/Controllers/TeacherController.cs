using EShop.IdentityService.Infrastructure.Authorizaion;
using EShop.Model.TypeSafe;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EShop.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[CustomAuthorize]
    [Authorize(Policy = TS.Policies.GenericPolicy)]
    public class SupplierController : ControllerBase
    {
        [HttpGet("GetSupplier")]
        public string Get()
        {
            return "Get a Supplier";
        }

        [HttpPost("AddSupplier")]
        public string Add()
        {
            return "Add a Supplier";
        }

        [HttpPut("UpdateSupplier")]
        public string Update()
        {
            return "Update a Supplier";
        }

        [HttpDelete("DeleteSupplier")]
        public string Delete()
        {
            return "Delete a Supplier";
        }
    }
}
