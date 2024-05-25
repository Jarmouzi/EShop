using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text.Encodings.Web;
using System.Security.Claims;
using EShop.LogService.Repository;
using EShop.LogService.Model;
using Azure;

namespace EShop.AdminPanel.Services
{
    public interface IRazorRenderService
    {
        Task<string> ToStringAsync<T>(string viewName, T model);
        Task<string> UploadImage(IFormFile uploadedFile);
    }
    public class RazorRenderService : IRazorRenderService
    {
        private readonly IRazorViewEngine _razorViewEngine;
        private readonly ITempDataProvider _tempDataProvider;
        private readonly IServiceProvider _serviceProvider;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IActionContextAccessor _actionContext;
        private readonly IRazorPageActivator _activator;
        private readonly ILogRepository _logRepository;
        public RazorRenderService(IRazorViewEngine razorViewEngine,
            ITempDataProvider tempDataProvider,
            IServiceProvider serviceProvider,
            IHttpContextAccessor httpContext,
            IRazorPageActivator activator,
            IActionContextAccessor actionContext,
            ILogRepository logRepository)
        {
            _razorViewEngine = razorViewEngine;
            _tempDataProvider = tempDataProvider;
            _serviceProvider = serviceProvider;

            _httpContext = httpContext;
            _actionContext = actionContext;
            _activator = activator;
            _logRepository = logRepository;
        }
        public async Task<string> ToStringAsync<T>(string pageName, T model)
        {
            try
            {
                var actionContext =
                    new ActionContext(
                        _httpContext.HttpContext,
                        _httpContext.HttpContext.GetRouteData(),
                        _actionContext.ActionContext.ActionDescriptor
                    );
                using (var sw = new StringWriter())
                {
                    var result = _razorViewEngine.FindPage(actionContext, pageName);
                    if (result.Page == null)
                    {
                        throw new ArgumentNullException($"The page {pageName} cannot be found.");
                    }
                    var view = new RazorView(_razorViewEngine,
                        _activator,
                        new List<IRazorPage>(),
                        result.Page,
                        HtmlEncoder.Default,
                        new DiagnosticListener("RazorRenderService"));
                    var viewContext = new ViewContext(
                        actionContext,
                        view,
                        new ViewDataDictionary<T>(new EmptyModelMetadataProvider(), new ModelStateDictionary())
                        {
                            Model = model
                        },
                        new TempDataDictionary(
                            _httpContext.HttpContext,
                            _tempDataProvider
                        ),
                        sw,
                        new HtmlHelperOptions()
                    );
                    var page = (result.Page);
                    page.ViewContext = viewContext;
                    _activator.Activate(page, viewContext);
                    await page.ExecuteAsync();
                    return sw.ToString();
                }
            }
            catch (Exception ex)
            {

                return ex.Message;
            }
        }
        private IRazorPage FindPage(ActionContext actionContext, string pageName)
        {
            var getPageResult = _razorViewEngine.GetPage(executingFilePath: null, pagePath: pageName);
            if (getPageResult.Page != null)
            {
                return getPageResult.Page;
            }
            var findPageResult = _razorViewEngine.FindPage(actionContext, pageName);
            if (findPageResult.Page != null)
            {
                return findPageResult.Page;
            }
            var searchedLocations = getPageResult.SearchedLocations.Concat(findPageResult.SearchedLocations);
            var errorMessage = string.Join(
                Environment.NewLine,
                new[] { $"Unable to find page '{pageName}'. The following locations were searched:" }.Concat(searchedLocations));
            throw new InvalidOperationException(errorMessage);
        }

        public async Task<string?> UploadImage(IFormFile uploadedFile)
        {
            var ServerIP = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("EShopSettings")["StaicsServer"];

            var user = _httpContext.HttpContext.User;

            using (var client = new HttpClient(new HttpClientHandler() { UseDefaultCredentials = true, PreAuthenticate = true }))
            {
                try
                {
                    client.BaseAddress = new Uri(ServerIP);

                    //var token = user.FindFirstValue("token");
                    var token = _httpContext.HttpContext.Session.GetString("_Token");

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
                    //client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);


                    var fileContent = new MultipartFormDataContent();

                    var streamContent = new StreamContent(uploadedFile.OpenReadStream());
                    streamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/jpeg");

                    fileContent.Add(streamContent, "uploadedFile", uploadedFile.FileName);

                    var response = client.PostAsync("/Statistics/", fileContent).Result;

                    if (!response.IsSuccessStatusCode)
                    {
                        await _logRepository.AddErrorLogAsync(new ErrorLog
                        {
                            Action = "UploadImage",
                            Error = "\n File Insert To " + ServerIP + " faild: " + response.Content.ReadAsStringAsync().Result,
                            Username = user.Identity?.Name ?? ""
                        });


                        return null;
                    }

                    var result = $"{ServerIP}/Statistics?path={response.Content.ReadAsStringAsync().Result}";

                    return result;
                }
                catch (Exception e)
                {
                    await _logRepository.AddErrorLogAsync(new ErrorLog
                    {
                        Action = "UploadImage",
                        Error = "\n File Insert To " + ServerIP + " raised error: " + e.Message,
                        Username = user.Identity?.Name ?? ""
                    });

                    return null;
                }
            }
        }
    }
}
