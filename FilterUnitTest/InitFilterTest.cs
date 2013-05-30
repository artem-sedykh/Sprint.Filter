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
    }
}
