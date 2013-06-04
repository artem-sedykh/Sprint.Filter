using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Sprint.Mvc
{
    using System;
    using System.Web.Mvc;
    using Filter;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Enum | AttributeTargets.Interface | AttributeTargets.Parameter | AttributeTargets.Struct | AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    internal class FilterValueModelBinderAttribute : CustomModelBinderAttribute
    {
        public override IModelBinder GetBinder()
        {
            return new FilterValueModelBinder();
        }
    }
    
    internal class FilterValueModelBinder : DefaultModelBinder
    {
        internal static readonly ConcurrentDictionary<string, Type> ConcurrentDictionary =
            new ConcurrentDictionary<string, Type>();

        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var typeNameKey = bindingContext.ModelName + ".TypeName";

            if (!bindingContext.ValueProvider.ContainsPrefix(typeNameKey))
            {
                throw new KeyNotFoundException(
                    String.Format("Key {0} not found, add @Html.HiddenFor(x=>x.TypeName) in your FilterTemplate",
                                  typeNameKey));
            }

            var typeName = bindingContext.ValueProvider.GetValue(bindingContext.ModelName + ".TypeName").AttemptedValue;

            Type bindingType=null;

            if (ConcurrentDictionary.ContainsKey(typeName))            
                bindingType = ConcurrentDictionary[typeName];            
            else
            {
                var type = Type.GetType(typeName);
                if (type != null)
                {
                    if (type.IsValueType)
                    {
                        var nullableType = typeof(Nullable<>).MakeGenericType(type);
                        bindingType = typeof(FilterValue<>).MakeGenericType(nullableType);
                    }
                    else                    
                        bindingType = typeof(FilterValue<>).MakeGenericType(type);
                    
                    ConcurrentDictionary[typeName] = bindingType;                  
                }
            }

            if (bindingType != null)
            {
                var metaData = ModelMetadataProviders.Current.GetMetadataForType(null, bindingType);

                bindingContext.ModelMetadata = metaData;
            }
                                 
            return base.BindModel(controllerContext, bindingContext);
        }
    }
}
