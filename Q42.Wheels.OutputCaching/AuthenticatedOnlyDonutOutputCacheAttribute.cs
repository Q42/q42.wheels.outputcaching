using DevTrends.MvcDonutCaching;

namespace Q42.Wheels.OutputCaching
{
  /// <summary>
  /// This DonutOutputCache only uses cache if HttpContext.User.Identity.IsAuthenticated
  /// </summary>
  public class AuthenticatedOnlyDonutOutputCacheAttribute : DonutOutputCacheAttribute
  {
    public override void OnActionExecuted(System.Web.Mvc.ActionExecutedContext filterContext)
    {
      if (filterContext.HttpContext.User.Identity.IsAuthenticated)
        base.OnActionExecuted(filterContext);
    }

    public override void OnActionExecuting(System.Web.Mvc.ActionExecutingContext filterContext)
    {
      if (filterContext.HttpContext.User.Identity.IsAuthenticated)
        base.OnActionExecuting(filterContext);
    }

    public override void OnResultExecuted(System.Web.Mvc.ResultExecutedContext filterContext)
    {
      if (filterContext.HttpContext.User.Identity.IsAuthenticated)
        base.OnResultExecuted(filterContext);
    }

    public override void OnResultExecuting(System.Web.Mvc.ResultExecutingContext filterContext)
    {
      if (filterContext.HttpContext.User.Identity.IsAuthenticated)
        base.OnResultExecuting(filterContext);
    }
  }
}