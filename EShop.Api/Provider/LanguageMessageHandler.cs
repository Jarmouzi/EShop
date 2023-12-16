using EShop.Repository;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace EShop.Api.Provider
{
    public class LanguageMessageHandler : DelegatingHandler
    {

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var culture = "fa-IR";
            var tokenId = ClaimsPrincipal.Current.FindFirst("TokenId") == null ? null : ClaimsPrincipal.Current.FindFirst("TokenId").Value;
            if (tokenId != null)
            {
                //var autofacLifetimeScope = Autofac.Integration.Owin.OwinContextExtensions.GetAutofacLifetimeScope(request.GetOwinContext());
                //var log_VisitService = autofacLifetimeScope.Resolve<ILog_VisitService>();

                //var userId = Int32.Parse(ClaimsPrincipal.Current.FindFirst("Id").Value);

                //var visitLog = log_VisitService.Get(userId, tokenId);
                //if (visitLog.Status == "success")
                //    culture = visitLog.Data.Culture;
            }

            SetCulture(request, culture);

            return base.SendAsync(request, cancellationToken);
        }

        private void SetCulture(HttpRequestMessage request, string lang)
        {
            request.Headers.AcceptLanguage.Clear();
            request.Headers.AcceptLanguage.Add(new StringWithQualityHeaderValue(lang));
            Thread.CurrentThread.CurrentCulture = new CultureInfo(lang);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(lang);
        }
    }
}