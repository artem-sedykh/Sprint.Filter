using Autofac;
using Autofac.Integration.Mvc;
using Sprint.Filter.Examples.Models;
using Sprint.Filter.Examples.Services;
using Sprint.Filter.Examples.Services.Impl;

namespace Sprint.Filter.Examples.App_Start
{
    public class AutofacConfig : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .Register(c => new NorthwindDataContext())
                .As<NorthwindDataContext>().InstancePerHttpRequest();

            builder.RegisterType<SimpleStateService>().As<ISimpleStateService>().InstancePerHttpRequest();
            builder.RegisterType<SprintGridStateService>().As<ISprintGridStateService>().InstancePerHttpRequest();
        }
    }
}