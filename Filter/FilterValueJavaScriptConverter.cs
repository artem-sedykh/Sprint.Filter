namespace Sprint.Filter
{
    using System;
    using System.Collections.Generic;
    using System.Web.Script.Serialization;

    public sealed class FilterValueJavaScriptConverter : JavaScriptConverter
    {
        public override object Deserialize(IDictionary<string, object> dictionary, Type modelType, JavaScriptSerializer serializer)
        {
            var typeName = dictionary["TypeName"].ToString();
            var type = Type.GetType(typeName);

            if (type != null)
            {
                var typeAllowsNullValue = (!type.IsValueType || Nullable.GetUnderlyingType(type) != null);
                Type bindingType;
                if (typeAllowsNullValue)
                    bindingType = typeof(FilterValue<>).MakeGenericType(type);
                else
                {
                    var nullableType = typeof(Nullable<>).MakeGenericType(type);
                    bindingType = typeof(FilterValue<>).MakeGenericType(nullableType);
                }

                return serializer.ConvertToType(dictionary, bindingType);
            }

            return null;
        }

        public override IDictionary<string, object> Serialize(object obj, JavaScriptSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<Type> SupportedTypes
        {
            get { return new List<Type> { typeof(IFilterValue) }; }
        }
    }
}
