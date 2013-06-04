using System;
using System.Web;
using System.Web.Mvc;

namespace Sprint.Filter.Examples.Attributes
{
    public class SessionModelBinderAttribute : CustomModelBinderAttribute
    {
        private readonly string _sessionKey;

        public SessionModelBinderAttribute(string sessionKey)
        {
            _sessionKey = sessionKey;
        }

        public override IModelBinder GetBinder()
        {
            return new SessionModelBinder(_sessionKey);
        }
    }

    public class SessionModelBinder : DefaultModelBinder
    {
        private readonly string _sessionKey;

        public SessionModelBinder(string sessionKey)
        {
            _sessionKey = sessionKey;
        }

        protected override object CreateModel(ControllerContext controllerContext,
                ModelBindingContext bindingContext, Type modelType)
        {
            object model;

            if (controllerContext.HttpContext.Session != null &&
                (controllerContext.HttpContext.Session[_sessionKey] != null &&
                modelType.IsInstanceOfType(controllerContext.HttpContext.Session[_sessionKey])))
                model = controllerContext.HttpContext.Session[_sessionKey];
            else
            {
                model = base.CreateModel(controllerContext, bindingContext, modelType);
            }

            return model;
        }

        protected override void OnModelUpdated(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            base.OnModelUpdated(controllerContext, bindingContext);
            if (controllerContext.HttpContext.Session != null)
                controllerContext.HttpContext.Session[_sessionKey] = bindingContext.Model;
        }

        public static void Clear(string key)
        {
            HttpContext.Current.Session.Remove(key);
        }
    }
}