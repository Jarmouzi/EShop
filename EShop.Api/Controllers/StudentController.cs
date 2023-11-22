using EShop.Model.TypeSafe;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EShop.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Policy = "CaimBasedPolicy")]
    public class ProductController : ControllerBase
    {
        [HttpGet("GetProduct")]
        //[Authorize(Policy = TS.Policies.ReadPolicy)]
        public string Get()
        {
            return "Get a Product";
        }

        [HttpPost("AddProduct")]
        //[Authorize(Policy = TS.Policies.ReadAndWritePolicy)]
        public string Add()
        {
            return "Add a Product";
        }

        [HttpPut("UpdateProduct")]
        //[Authorize(Policy = TS.Policies.FullControlPolicy)]
        public string Update()
        {
            return "Update a Product";
        }

        [HttpDelete("DeleteProduct")]
        //[Authorize(Policy = TS.Policies.FullControlPolicy)]
        public string Delete()
        {
            return "Delete a Product";
        }
    }
}
