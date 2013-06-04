using System;
using System.Web.Mvc;

namespace Sprint.Filter.Examples.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false, Inherited = true)]
    public class DropDownListAttribute : Attribute, IMetadataAware
    {
        public string SourcePropertyName { get; set; }

        public const string Key = "__dropDownListKey";

        public string OptionLabel { get; set; }

        public string Css { get; set; }

        public DropDownListAttribute(string sourcePropertyName)
        {
            SourcePropertyName = sourcePropertyName;
        }

        public void OnMetadataCreated(ModelMetadata metadata)
        {
            metadata.AdditionalValues[Key] = new Tuple<string, string, string>(SourcePropertyName, OptionLabel, Css);
        }
    }
}