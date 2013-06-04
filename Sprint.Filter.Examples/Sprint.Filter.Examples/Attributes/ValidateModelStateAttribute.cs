using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Sprint.Filter.Examples.Attributes
{
    public class ValidateModelStateAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!filterContext.Controller.ViewData.ModelState.IsValid)
            {
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    var errors = filterContext.Controller.ViewData.ModelState.Where(m => m.Value.Errors.Any()).ToDictionary(x => x.Key, x => x.Value.Errors.Select(e => e.ErrorMessage).First());

                    filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
                    filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    filterContext.HttpContext.Response.StatusDescription = "Invalid Model State";

                    filterContext.Result = new JsonResult
                    {
                        Data = errors,
                        ContentType = "text/html",
                        ContentEncoding = System.Text.Encoding.UTF8,
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    };
                }
            }

            base.OnActionExecuting(filterContext);
        }
    }
}