using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sprint.Filter;

namespace FilterUnitTest
{
    [TestClass]
    public class FilterValueModelBinderTest
    {
        [TestMethod]
        public void KeyNotFoundException()
        {
            var filterValueModelBinder = ModelBinders.Binders.GetBinder(typeof(IFilterValue));

            var formCollection = new NameValueCollection
                {
                    {"foo.ConditionKey", "euqal"},
                    {"foo.LeftValue", "1"},
                    {"foo.RightValue", "12"},                    
                    {"foo.TypeName", typeof(Int32).FullName}
                };

            var valueProvider = new NameValueCollectionValueProvider(formCollection, null);

            var bindingContext = new ModelBindingContext
            {
                ModelName = "foo",
                ValueProvider = valueProvider
            };

            var controllerContext = new ControllerContext();

            try
            {
                filterValueModelBinder.BindModel(controllerContext, bindingContext);
            }
            catch (Exception e)
            {
                Assert.IsTrue(e.GetType() == typeof(KeyNotFoundException));
            }
        }

        [TestMethod]
        public void BindingReferenceType()
        {

            var stringFullName = typeof(string).FullName;
            var filterValueModelBinder = ModelBinders.Binders.GetBinder(typeof(IFilterValue));

            var formCollection = new NameValueCollection
                {
                    {"foo.ConditionKey", "euqal"},
                    {"foo.LeftValue", "1"},
                    {"foo.RightValue", "12"},
                    {"foo.Values","one"},
                    {"foo.Values","two"},
                    {"foo.Values","three"},
                    {"foo.TypeName", stringFullName}
                };

            var valueProvider = new NameValueCollectionValueProvider(formCollection, null);

            var bindingContext = new ModelBindingContext
            {
                ModelName = "foo",
                ValueProvider = valueProvider
            };

            var controllerContext = new ControllerContext();

            var value = filterValueModelBinder.BindModel(controllerContext, bindingContext) as IFilterValue<string>;

            Assert.IsNotNull(value);

            Assert.IsTrue(value.LeftValue == "1");

            Assert.IsTrue(value.RightValue == "12");

            Assert.IsTrue(value.ConditionKey == "euqal");

            Assert.IsTrue(value.TypeName == stringFullName);

            Assert.IsTrue(value.Values.Any());
            Assert.IsTrue(value.Values.Contains("one"));
            Assert.IsTrue(value.Values.Contains("two"));
            Assert.IsTrue(value.Values.Contains("three"));


        }

        [TestMethod]
        public void BindingValueType()
        {
            var int32FullName = typeof(Int32).FullName;
            var filterValueModelBinder = ModelBinders.Binders.GetBinder(typeof(IFilterValue));

            var formCollection = new NameValueCollection
                {
                    {"foo.ConditionKey", "euqal"},
                    {"foo.LeftValue", "1"},
                    {"foo.RightValue", "12"},
                    {"foo.Values","1"},
                    {"foo.Values","2"},
                    {"foo.Values","3"},
                    {"foo.TypeName", int32FullName}
                };

            var valueProvider = new NameValueCollectionValueProvider(formCollection, null);

            var bindingContext = new ModelBindingContext
            {
                ModelName = "foo",
                ValueProvider = valueProvider
            };

            var controllerContext = new ControllerContext();

            var value = filterValueModelBinder.BindModel(controllerContext, bindingContext) as IFilterValue<int?>;

            Assert.IsNotNull(value);

            Assert.IsTrue(value.LeftValue == 1);

            Assert.IsTrue(value.RightValue == 12);

            Assert.IsTrue(value.ConditionKey == "euqal");

            Assert.IsTrue(value.TypeName == int32FullName);

            Assert.IsTrue(value.Values.Any());
            Assert.IsTrue(value.Values.Contains(1));
            Assert.IsTrue(value.Values.Contains(2));
            Assert.IsTrue(value.Values.Contains(3));
        }

        [TestMethod]
        public void BindingFilterOptions()
        {            
            var formCollection = new NameValueCollection
                {
                    {"foo.LoadFilterId","10"},
                    {"foo.FilterType","FilterType.Fast"},

                    {"foo.Filters[filter1].Values", "10"},
                    {"foo.Filters[filter1].ConditionKey", "equal"},
                    {"foo.Filters[filter1].TypeName", typeof(Int32).FullName},
                
                    {"foo.Filters[filter2].LeftValue", "2345.14"},
                    {"foo.Filters[filter2].ConditionKey", "equal"},
                    {"foo.Filters[filter2].TypeName", typeof(Decimal).FullName},

                    {"foo.Filters[filter3].LeftValue", "testname"},
                    {"foo.Filters[filter3].ConditionKey", "equal"},
                    {"foo.Filters[filter3].TypeName", typeof(string).FullName}
                };

            var modelBinder = new DefaultModelBinder();

            var valueProvider = new NameValueCollectionValueProvider(formCollection, null);

            var bindingContext = new ModelBindingContext
            {
                ModelName = "foo",
                ValueProvider = valueProvider,
                ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(null, typeof(FilterOptions)),
            };

            var controllerContext = new ControllerContext();

            var filterOptions = modelBinder.BindModel(controllerContext, bindingContext) as IFilterOptions;

            Assert.IsNotNull(filterOptions);

            Assert.IsTrue(filterOptions.Filters.Any());

            Assert.IsTrue(filterOptions.Filters.ContainsKey("filter1"));
            var filter1 = filterOptions.Filters["filter1"] as IFilterValue<int?>;
            Assert.IsNotNull(filter1);
            Assert.IsTrue(filter1.Values.Contains(10));
            Assert.IsTrue(filter1.ConditionKey == "equal");

            Assert.IsTrue(filterOptions.Filters.ContainsKey("filter2"));
            var filter2 = filterOptions.Filters["filter2"] as IFilterValue<decimal?>;
            Assert.IsNotNull(filter2);
            Assert.IsTrue(filter2.LeftValue == 2345.14M);
            Assert.IsTrue(filter2.ConditionKey == "equal");

            Assert.IsTrue(filterOptions.Filters.ContainsKey("filter3"));
            var filter3 = filterOptions.Filters["filter3"] as IFilterValue<string>;
            Assert.IsNotNull(filter3);
            Assert.IsTrue(filter3.LeftValue == "testname");
            Assert.IsTrue(filter3.ConditionKey == "equal");            
        }        
    }
}
