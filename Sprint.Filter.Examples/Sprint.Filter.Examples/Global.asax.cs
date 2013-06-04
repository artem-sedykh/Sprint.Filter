using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using AutofacContrib.CommonServiceLocator;
using Microsoft.Practices.ServiceLocation;
using Sprint.Filter.Examples.App_Start;
using Sprint.Filter.Examples.Controllers;

namespace Sprint.Filter.Examples
{
    // Примечание: Инструкции по включению классического режима IIS6 или IIS7 
    // см. по ссылке http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public ContainerBuilder ContainerBuilder { get; set; }
        public IContainer Container { get; set; }

        protected void Application_Start()
        {
            ContainerBuilder = new ContainerBuilder();
            ContainerBuilder.RegisterControllers(typeof(MultipleConditionsFilterController).Assembly);
            ContainerBuilder.RegisterModule(new AutofacConfig());

            Container = ContainerBuilder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(Container));

            ServiceLocator.SetLocatorProvider(() => new AutofacServiceLocator(Container));

            AreaRegistration.RegisterAllAreas();            

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);            
        }
    }
}