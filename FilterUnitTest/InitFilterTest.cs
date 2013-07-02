using System;
using System.Collections.Specialized;
using System.Linq.Expressions;
using System.Web.Mvc;
using FilterUnitTest.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sprint.Filter;
using Sprint.Filter.Conditions;

namespace FilterUnitTest
{
    [TestClass]
    public sealed class InitFilterTest
    {
        [TestMethod]
        public void ValueTypeFilterSetDefaultValue()
        {

            var filter = new ValueTypeFilter<TestSource, int>();
            filter.For(x => x.Id);
            filter.Conditions(condition =>
                {
                    condition["equal"] = new ValueTypeEqualCondition<int>();
                    condition["greaterthan"] = new GreaterThanCondition<int>();
                });


            var expr1 = filter.BuildExpression<TestSource>();

            Assert.IsNull(expr1);

            filter.SetDefaultValue(new FilterValue<int?>
                {
                    LeftValue = 10,
                    ConditionKey = "equal"
                });

            var expr2 = filter.BuildExpression<TestSource>();

            Assert.IsNotNull(expr2);

            Assert.IsTrue(expr2.Compile()(new TestSource { Id = 10 }));

            Assert.IsFalse(expr2.Compile()(new TestSource { Id = 12 }));

            filter.Init(new FilterValue<int?>
                {
                    ConditionKey = "greaterthan",
                    LeftValue = 5
                });

            var expr3 = filter.BuildExpression<TestSource>();

            Assert.IsNotNull(expr3);

            Assert.IsTrue(expr3.Compile()(new TestSource{Id = 10}));
            Assert.IsFalse(expr3.Compile()(new TestSource { Id = 4 }));
        }

        [TestMethod]
        public void ValueTypeFilterSetDefaultValue_Lazy()
        {

            var filter = new ValueTypeFilter<TestSource, int>();
            filter.For(x => x.Id);
            filter.Conditions(condition =>
            {
                condition["equal"] = new ValueTypeEqualCondition<int>();
                condition["greaterthan"] = new GreaterThanCondition<int>();
            });


            var expr1 = filter.BuildExpression<TestSource>();

            Assert.IsNull(expr1);

            filter.SetDefaultValue(() => new FilterValue<int?>
            {
                LeftValue = 10,
                ConditionKey = "equal"
            });

            var expr2 = filter.BuildExpression<TestSource>();

            Assert.IsNotNull(expr2);

            Assert.IsTrue(expr2.Compile()(new TestSource { Id = 10 }));

            Assert.IsFalse(expr2.Compile()(new TestSource { Id = 12 }));

            filter.Init(new FilterValue<int?>
            {
                ConditionKey = "greaterthan",
                LeftValue = 5
            });

            var expr3 = filter.BuildExpression<TestSource>();

            Assert.IsNotNull(expr3);

            Assert.IsTrue(expr3.Compile()(new TestSource { Id = 10 }));
            Assert.IsFalse(expr3.Compile()(new TestSource { Id = 4 }));
        }

        [TestMethod]
        public void ReferenceTypeFilterSetDefaultValue()
        {

            var filter = new ReferenceTypeFilter<TestSource, string>();
            filter.For(x => x.Name);
            filter.Conditions(condition =>
                {
                    condition["equal"] = new ReferenceTypeEqualCondition<string>();
                    condition["notequal"] = new ReferenceTypeNotEqualCondition<string>();
                });


            var expr1 = filter.BuildExpression<TestSource>();

            Assert.IsNull(expr1);

            filter.SetDefaultValue(new FilterValue<string>
            {
                LeftValue = "10",
                ConditionKey = "equal"
            });

            var expr2 = filter.BuildExpression<TestSource>();

            Assert.IsNotNull(expr2);

            Assert.IsTrue(expr2.Compile()(new TestSource { Name = "10" }));

            Assert.IsFalse(expr2.Compile()(new TestSource { Name = "12" }));

            filter.Init(new FilterValue<string>
            {
                ConditionKey = "notequal",
                LeftValue = "10"
            });

            var expr3 = filter.BuildExpression<TestSource>();

            Assert.IsNotNull(expr3);

            Assert.IsTrue(expr3.Compile()(new TestSource { Name = "12" }));
            Assert.IsFalse(expr3.Compile()(new TestSource { Name = "10" }));
        }

        [TestMethod]
        public void ReferenceTypeFilterSetDefaultValue_Lazy()
        {

            var filter = new ReferenceTypeFilter<TestSource, string>();
            filter.For(x => x.Name);
            filter.Conditions(condition =>
            {
                condition["equal"] = new ReferenceTypeEqualCondition<string>();
                condition["notequal"] = new ReferenceTypeNotEqualCondition<string>();
            });


            var expr1 = filter.BuildExpression<TestSource>();

            Assert.IsNull(expr1);

            filter.SetDefaultValue(()=>new FilterValue<string>
            {
                LeftValue = "10",
                ConditionKey = "equal"
            });

            var expr2 = filter.BuildExpression<TestSource>();

            Assert.IsNotNull(expr2);

            Assert.IsTrue(expr2.Compile()(new TestSource { Name = "10" }));

            Assert.IsFalse(expr2.Compile()(new TestSource { Name = "12" }));

            filter.Init(new FilterValue<string>
            {
                ConditionKey = "notequal",
                LeftValue = "10"
            });

            var expr3 = filter.BuildExpression<TestSource>();

            Assert.IsNotNull(expr3);

            Assert.IsTrue(expr3.Compile()(new TestSource { Name = "12" }));
            Assert.IsFalse(expr3.Compile()(new TestSource { Name = "10" }));
        }        

        [TestMethod]
        public void Init_And_BuildExpression()
        {
            var formCollection = new NameValueCollection
                {
                    {"Filters[CategoryId].Values", "1"},
                    {"Filters[CategoryId].Values", "14"},
                    {"Filters[CategoryId].Values", "5"},
                    {"Filters[CategoryId].ConditionKey", "isin"},
                    {"Filters[CategoryId].TypeName", typeof(Int32).FullName},
                
                    {"Filters[UnitPrice].LeftValue", "10"},
                    {"Filters[UnitPrice].RightValue", "2345.14"},
                    {"Filters[UnitPrice].ConditionKey", "between"},
                    {"Filters[UnitPrice].TypeName", typeof(Decimal).FullName},

                    {"Filters[Name].LeftValue", "testname"},
                    {"Filters[Name].ConditionKey", "contains"},
                    {"Filters[Name].TypeName", typeof(string).FullName}
                };

            var modelBinder = new DefaultModelBinder();

            var valueProvider = new NameValueCollectionValueProvider(formCollection, null);

            var bindingContext = new ModelBindingContext
            {
                ModelName = string.Empty,
                ValueProvider = valueProvider,
                ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(null, typeof(FilterOptions)),
            };

            var controllerContext = new ControllerContext();

            var filterOptions = modelBinder.BindModel(controllerContext, bindingContext) as IFilterOptions;

            Assert.IsNotNull(filterOptions);

            var productFilter = new ProductFilter().Init(filterOptions);

            var expression = productFilter.BuildExpression<Product>();

            Assert.IsNotNull(expression);

            var predicat = expression.Compile();         
            Assert.IsFalse(predicat(new Product{CategoryId = 24444,Name = string.Empty}));

            Assert.IsTrue(predicat(new Product
                {
                    Name = "testname",
                    CategoryId = 14,
                    UnitPrice = 23,
                }));            
        }
    }
}
