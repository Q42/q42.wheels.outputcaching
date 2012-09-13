using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace Q42.Wheels.OutputCaching
{
  /// <summary>
  /// This outputcache extends the default OutputCache but only uses the cache if the user is NOT logged in (User.Identity.IsAuthenticated)
  /// This attribute is to be used together with <see cref="Q42.Wheels.OutputCaching.AuthenticatedOnlyDonutOutputCache"/> 
  /// to have the full cached experience (logged in with donuts and not-logged in total cache)
  /// </summary>
  public class AnonymousOnlyOutputCacheAttribute : OutputCacheAttribute
  {
    public override void OnResultExecuting(ResultExecutingContext filterContext)
    {
      var httpContext = filterContext.HttpContext;

      if (httpContext.User.Identity.IsAuthenticated)
      {
        // it's crucial not to cache Authenticated content
        Location = OutputCacheLocation.None;
      }

      // this smells a little but it works. By appending this callback we are always called last-minute to determine if we are going to use the outputcache
      httpContext.Response.Cache.AddValidationCallback(IgnoreAuthenticated, null);

      base.OnResultExecuting(filterContext);
    }

    /// <summary>
    /// This method is called each time when cached page is going to be 
    /// served and ensures that cache is ignored for authenticated users.
    /// </summary>
    /// <param name="context"></param>
    /// <param name="data"></param>
    /// <param name="validationStatus"></param>
    private void IgnoreAuthenticated(HttpContext context, object data, ref HttpValidationStatus validationStatus)
    {
      if (context.User.Identity.IsAuthenticated)
        validationStatus = HttpValidationStatus.IgnoreThisRequest;
      else
        validationStatus = HttpValidationStatus.Valid;
    }

  }
}