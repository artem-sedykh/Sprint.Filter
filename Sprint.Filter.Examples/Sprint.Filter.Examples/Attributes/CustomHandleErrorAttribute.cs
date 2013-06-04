using System;
using System.Web.Mvc;

namespace Sprint.Filter.Examples.Attributes
{
    public class CustomHandleErrorAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            if (!ExceptionType.IsInstanceOfType(filterContext.Exception))
            {
                return;
            }

            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                Func<Exception, object> getGlobalError = null;

                getGlobalError = e =>
                {
                    if (e == null)
                        return null;

                    var globalError = new
                    {
                        ErrorMessage = e.Message,
                        StackTrace = e.StackTrace,
                        InnerException = getGlobalError(e.InnerException)
                    };

                    return globalError;
                };

                var data = getGlobalError(filterContext.Exception);

                filterContext.Result = new JsonResult
                {
                    Data = data,
                    ContentType = "text/html",
                    ContentEncoding = System.Text.Encoding.UTF8,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };

                filterContext.ExceptionHandled = true;
                filterContext.HttpContext.Response.Clear();
                filterContext.HttpContext.Response.StatusCode = 500;

                filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
            }
            else
            {
                base.OnException(filterContext);                
            }            
        }
    }
}