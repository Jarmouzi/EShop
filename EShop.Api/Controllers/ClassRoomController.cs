using EShop.Model.TypeSafe;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EShop.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoryController : ControllerBase
    {
        [HttpGet("GetCategory")]
        //[Authorize(Roles ="Admin")]
        //[Authorize(Roles = "Admin,Contributor")]
        //[Authorize(Roles = $"{TS.Roles.Admin},{TS.Roles.Contributor},{TS.Roles.User}")]
        //[Authorize(Policy = TS.Policies.ReadPolicy)]
        public string Get()
        {
            return "Get a Class Room";
        }

        [HttpPost("AddCategory")]
        //[Authorize(Roles = "Admin")]
        //[Authorize(Roles = $"{TS.Roles.Admin},{TS.Roles.Contributor}")]
        //[Authorize(Policy = TS.Policies.ReadAndWritePolicy)]
        public string Add()
        {
            return "Add a Class Room";
        }

        [HttpPut("UpdateCategory")]
        //[Authorize(Roles = TS.Roles.Admin)]
        //[Authorize(Policy = TS.Policies.FullControlPolicy)]
        public string Update()
        {
            return "Update a Class Room";
        }

        [HttpDelete("DeleteCategory")]
        //[Authorize(Roles = TS.Roles.Admin)]
        //[Authorize(Policy = TS.Policies.FullControlPolicy)]
        public string Delete()
        {
            return "Delete a Class Room";
        }
    }
}

