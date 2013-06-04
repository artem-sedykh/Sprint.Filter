using System.Web.Mvc;
using Sprint.Filter.Examples.Attributes;

namespace Sprint.Filter.Examples.App_Start
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new CustomHandleErrorAttribute());
        }
    }
}