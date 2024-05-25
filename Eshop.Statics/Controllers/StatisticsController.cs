using Microsoft.AspNetCore.Mvc;
using System.IO;
using EShop.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using EShop.IdentityService.Infrastructure.Authorizaion;

namespace Eshop.Statics.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class StatisticsController : ControllerBase
    {
        private readonly ILogger<StatisticsController> _logger;

        private readonly IWebHostEnvironment _webHostEnvironment;

        public StatisticsController(ILogger<StatisticsController> logger, IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
        }


        [HttpGet(Name = "Get")]
        public IActionResult Get(string path, int? width = null, int? height = null)
        {
            string contentRootPath = _webHostEnvironment.ContentRootPath;
            string filePath = Path.Combine(contentRootPath, "images", path);

            var extension = Path.GetExtension(filePath);
            
            //byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);

            //FileStream stream = System.IO.File.Open(filePath);
            //return File(stream, "image/jpeg");
            return PhysicalFile(filePath, extension.GetMimeType());
        }

        //public Task<IEnumerable<IHostedFile>> GetFiles(HttpContext context)
        //{
        //    string[] files = Directory.GetFiles(_rootDirectory);
        //    List<HostedFile> hostedFiles = new List<HostedFile>();
        //    foreach (string file in files)
        //    {
        //        byte[] fileBytes = File.ReadAllBytes(file);
        //        hostedFiles.Add(new HostedFile(fileBytes, Path.GetFileName(file), fileBytes.Length, fileBytes.Length, fileBytes.GetHashCode()));
        //    }
        //    return Task.FromResult(hostedFiles);
        //}

        //[AuthorizeApi]
        [HttpPost(Name = "Post")]
        public async Task<IActionResult> UploadImage([FromForm] UploadedFile uploadedFile)
        {
            string contentRootPath = _webHostEnvironment.ContentRootPath;
            var fileName = Guid.NewGuid().ToString() + "." + uploadedFile.uploadedFile.FileName.Split('.')[1];
            string filePath = Path.Combine(contentRootPath, "images", fileName);

            if (uploadedFile == null || uploadedFile.uploadedFile.Length == 0)
            {
                return NotFound();
            }

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await uploadedFile.uploadedFile.CopyToAsync(stream);
            }

            return Ok(fileName);
        }


    }

    public class UploadedFile
    {
        public IFormFile uploadedFile { get; set; }
    }
}
