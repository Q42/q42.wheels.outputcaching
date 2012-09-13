using System;
using System.Web;
using System.Web.Mvc;

namespace Q42.Wheels.OutputCaching
{
  /// <summary>
  /// This RequireHttps attribute is to be used in conjunction with OutputCaching because 
  /// otherwise the HTTPS result is output cached and also returned for HTTP requests.
  /// </summary>
  /// <remarks>
  /// http://stackoverflow.com/a/5604830/106909
  /// </remarks>
  [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
  public class OutputCacheCompatibleRequireHttpsAttribute : RequireHttpsAttribute
  {
    protected virtual bool AuthorizeCore(HttpContextBase httpContext)
    {
      return httpContext.Request.IsSecureConnection;
    }

    public override void OnAuthorization(AuthorizationContext filterContext)
    {
      if (!AuthorizeCore(filterContext.HttpContext))
      {
        this.HandleNonHttpsRequest(filterContext);
      }
      else
      {
        var cache = filterContext.HttpContext.Response.Cache;
        cache.SetProxyMaxAge(new TimeSpan(0L));
        cache.AddValidationCallback(this.CacheValidateHandler, null);
      }
    }

    private void CacheValidateHandler(HttpContext context, object data, ref HttpValidationStatus validationStatus)
    {
      validationStatus = this.OnCacheAuthorization(new HttpContextWrapper(context));
    }

    protected virtual HttpValidationStatus OnCacheAuthorization(HttpContextBase httpContext)
    {
      if (!AuthorizeCore(httpContext))
      {
        return HttpValidationStatus.IgnoreThisRequest;
      }
      return HttpValidationStatus.Valid;
    }
  }
}