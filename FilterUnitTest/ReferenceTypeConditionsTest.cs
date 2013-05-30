using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sprint.Filter;
using Sprint.Filter.Conditions;

namespace FilterUnitTest
{
    [TestClass]
    public sealed class ReferenceTypeConditionsTest
    {
        [TestMethod]
        public void ContainsCondition()
        {
            var containsCondition = new ContainsCondition();

            var expr1 = containsCondition.For<TestSource>(x => x.Name);

            Assert.IsNull(expr1);

            containsCondition.Value = new FilterValue<string> { LeftValue = "llow" };

            var expr2 = containsCondition.For<TestSource>(x => x.Name);

            Assert.IsTrue(expr2.Compile()(new TestSource { Name = "Hellow World!!!" }));

        }

        [TestMethod]
        public void EndsWithCondition()
        {
            var endsWithCondition = new EndsWithCondition();

            var expr1 = endsWithCondition.For<TestSource>(x => x.Name);

            Assert.IsNull(expr1);

            endsWithCondition.Value = new FilterValue<string>
            {
                LeftValue = "test1"
            };

            var expr2 = endsWithCondition.For<TestSource>(x => x.Name);
            Assert.IsTrue(expr2.Compile()(new TestSource { Name = "Firsttest1" }));
        }

        [TestMethod]
        public void IsNullOrEmptyCondition()
        {
            var isEmptyCondition = new IsNullOrEmptyCondition();

            var expr = isEmptyCondition.For<TestSource>(x => x.Name);

            Assert.IsTrue(expr.Compile()(new TestSource { Name = null }));

            Assert.IsTrue(expr.Compile()(new TestSource { Name = string.Empty }));

            Assert.IsFalse(expr.Compile()(new TestSource { Name = "123" }));

        }

        [TestMethod]
        public void IsNotNullOrEmptyCondition()
        {
            var isNotNullOrEmptyCondition = new IsNotNullOrEmptyCondition();

            var expr = isNotNullOrEmptyCondition.For<TestSource>(x => x.Name);

            Assert.IsFalse(expr.Compile()(new TestSource { Name = null }));

            Assert.IsFalse(expr.Compile()(new TestSource { Name = string.Empty }));

            Assert.IsTrue(expr.Compile()(new TestSource { Name = "123" }));

        }

        [TestMethod]
        public void NotContainsCondition()
        {
            var notContainsCondition = new NotContainsCondition();

            var expr1 = notContainsCondition.For<TestSource>(x => x.Name);

            Assert.IsNull(expr1);

            notContainsCondition.Value = new FilterValue<string> { LeftValue = "llow" };

            var expr2 = notContainsCondition.For<TestSource>(x => x.Name);

            Assert.IsFalse(expr2.Compile()(new TestSource { Name = "Hellow World!!!" }));

            notContainsCondition.Value = new FilterValue<string> { LeftValue = "NotContainsCondition" };

            var expr3 = notContainsCondition.For<TestSource>(x => x.Name);

            Assert.IsTrue(expr3.Compile()(new TestSource { Name = "Hellow World!!!" }));

        }

        [TestMethod]
        public void ReferenceTypeEqualCondition()
        {
            var referenceTypeEqualCondition = new ReferenceTypeEqualCondition<string>();

            var expr1 = referenceTypeEqualCondition.For<TestSource>(x => x.Name);
            Assert.IsNull(expr1);

            referenceTypeEqualCondition.Value = new FilterValue<string> { LeftValue = "345" };
            var expr2 = referenceTypeEqualCondition.For<TestSource>(x => x.Name);
            Assert.IsTrue(expr2.Compile()(new TestSource { Name = "345" }));

            referenceTypeEqualCondition.Value = new FilterValue<string> { LeftValue = "234" };
            var expr3 = referenceTypeEqualCondition.For<TestSource>(x => x.Name);
            Assert.IsFalse(expr3.Compile()(new TestSource { Name = "4" }));
        }

        [TestMethod]
        public void ReferenceTypeNotEqualCondition()
        {
            var referenceTypeNotEqualCondition = new ReferenceTypeNotEqualCondition<string>();

            var expr1 = referenceTypeNotEqualCondition.For<TestSource>(x => x.Name);
            Assert.IsNull(expr1);

            referenceTypeNotEqualCondition.Value = new FilterValue<string> { LeftValue = "345" };
            var expr2 = referenceTypeNotEqualCondition.For<TestSource>(x => x.Name);
            Assert.IsFalse(expr2.Compile()(new TestSource { Name = "345" }));

            referenceTypeNotEqualCondition.Value = new FilterValue<string> { LeftValue = "234" };
            var expr3 = referenceTypeNotEqualCondition.For<TestSource>(x => x.Name);
            Assert.IsTrue(expr3.Compile()(new TestSource { Name = "4" }));
        }

        [TestMethod]
        public void StartsWithCondition()
        {
            var startsWithCondition = new StartsWithCondition();

            var expr1 = startsWithCondition.For<TestSource>(x => x.Name);

            Assert.IsNull(expr1);

            startsWithCondition.Value = new FilterValue<string>
            {
                LeftValue = "test1"
            };

            var expr2 = startsWithCondition.For<TestSource>(x => x.Name);
            Assert.IsTrue(expr2.Compile()(new TestSource { Name = "test1Data" }));

        }

        [TestMethod]
        public void ReferenceTypeIsInCondition()
        {
            var referenceTypeIsInCondition = new ReferenceTypeIsInCondition<string>();

            var expr1 = referenceTypeIsInCondition.For<TestSource>(x => x.Name);

            Assert.IsNull(expr1);

            referenceTypeIsInCondition.Value = new FilterValue<string>
            {
                Values = new List<string> { "1", "2", "3" }
            };

            var expr2 = referenceTypeIsInCondition.For<TestSource>(x => x.Name);
            var expr2Func = expr2.Compile();
            Assert.IsTrue(expr2Func(new TestSource { Name = "1" }));
            Assert.IsFalse(expr2Func(new TestSource { Name = "100" }));

            referenceTypeIsInCondition.Value = new FilterValue<string>
            {
                Values = new List<string> { "1", "2", "3", null }
            };

            var expr3 = referenceTypeIsInCondition.For<TestSource>(x => x.Name);

            Assert.IsNotNull(expr3);
        }

        [TestMethod]
        public void ReferenceTypeIsNotInCondition()
        {
            var referenceTypeIsNotInCondition = new ReferenceTypeIsNotInCondition<string>();

            var expr1 = referenceTypeIsNotInCondition.For<TestSource>(x => x.Name);

            Assert.IsNull(expr1);

            referenceTypeIsNotInCondition.Value = new FilterValue<string>
            {
                Values = new List<string> { "1", "2", "3" }
            };

            var expr2 = referenceTypeIsNotInCondition.For<TestSource>(x => x.Name);
            var expr2Func = expr2.Compile();
            Assert.IsFalse(expr2Func(new TestSource { Name = "1" }));
            Assert.IsTrue(expr2Func(new TestSource { Name = "100" }));

            referenceTypeIsNotInCondition.Value = new FilterValue<string>
            {
                Values = new List<string> { "1", "2", "3", null }
            };

            var expr3 = referenceTypeIsNotInCondition.For<TestSource>(x => x.Name);

            Assert.IsNotNull(expr3);
        }

        [TestMethod]
        public void ReferenceTypeNoneCondition()
        {
            var referenceTypeNoneCondition = new ReferenceTypeNoneCondition<string>();

            var expr1 = referenceTypeNoneCondition.For<TestSource>(x => x.Name);

            Assert.IsNull(expr1);

            referenceTypeNoneCondition.Value = new FilterValue<string>
            {
                LeftValue = "123",
                RightValue = "1",
                Values = new List<string> { "1", "2", "3", "34" }
            };

            var expr2 = referenceTypeNoneCondition.For<TestSource>(x => x.Name);

            Assert.IsNull(expr2);


        }

        [TestMethod]
        public void ReferenceTypeIsNotNullCondition()
        {
            var referenceTypeIsNotNullCondition = new ReferenceTypeIsNotNullCondition<string>
            {
                Value = new FilterValue<string>
                {
                    LeftValue ="1"
                }
            };

            var expr1 = referenceTypeIsNotNullCondition.For<TestSource>(x => x.Name);

            Assert.IsTrue(expr1.Compile()(new TestSource { Name = "1" }));

            Assert.IsFalse(expr1.Compile()(new TestSource { Name = null }));
        }

        [TestMethod]
        public void ReferenceTypeIsNullCondition()
        {
            var isNullCondition = new ReferenceTypeIsNullCondition<string>
            {
                Value = new FilterValue<string>
                {
                    LeftValue = "1"
                }
            };

            var expr1 = isNullCondition.For<TestSource>(x => x.Name);

            Assert.IsFalse(expr1.Compile()(new TestSource { Name = "1" }));

            Assert.IsTrue(expr1.Compile()(new TestSource { Name = null }));
        }
    }
}
