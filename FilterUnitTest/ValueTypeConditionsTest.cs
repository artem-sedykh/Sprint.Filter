using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sprint.Filter;
using Sprint.Filter.Conditions;

namespace FilterUnitTest
{
    public sealed class TestSource
    {
        public int Id { get; set; }

        public int? NullableField { get; set; }

        public DateTime Date { get; set; }

        public String Description { get; set; }

        public TestSource Source { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }
    }

    internal sealed class TestIsIntersectionConditionSource
    {
        public int Begin { get; set; }

        public int End { get; set; }
    }

    [TestClass]
    public sealed class ValueTypeConditionsTest
    {
        
        [TestMethod]
        public void ValueTypeIsNullCondition()
        {
            var isNullCondition = new ValueTypeIsNullCondition<int>
            {
                Value = new FilterValue<int?>
                {
                    LeftValue = 1
                }
            };

            var expr1 = isNullCondition.For<TestSource>(x => x.NullableField);

            Assert.IsFalse(expr1.Compile()(new TestSource { NullableField = 1 }));

            Assert.IsTrue(expr1.Compile()(new TestSource { NullableField = null }));
        }

        [TestMethod]
        public void ValueTypeIsNotNullCondition()
        {
            var isNotNullCondition = new ValueTypeIsNotNullCondition<int>
            {
                Value = new FilterValue<int?>
                {
                    LeftValue = 1
                }
            };

            var expr1 = isNotNullCondition.For<TestSource>(x => x.NullableField);

            Assert.IsTrue(expr1.Compile()(new TestSource { NullableField = 1 }));

            Assert.IsFalse(expr1.Compile()(new TestSource { NullableField = null }));
        }

        [TestMethod]
        public void ValueTypeEqualCondition()
        {
            var equalCondition = new ValueTypeEqualCondition<int>();

            var expr1 = equalCondition.For<TestSource>(x => x.Id);
            Assert.IsNull(expr1);

            equalCondition.Value = new FilterValue<int?> { LeftValue = 1 };
            var expr2 = equalCondition.For<TestSource>(x => x.Id);
            Assert.IsTrue(expr2.Compile()(new TestSource { Id = 1 }));

            equalCondition.Value = new FilterValue<int?> { LeftValue = 2 };
            var expr3 = equalCondition.For<TestSource>(x => x.Id);
            Assert.IsFalse(expr3.Compile()(new TestSource { Id = 1 }));
        }

        [TestMethod]
        public void ValueTypeNotEqualCondition()
        {
            var equalCondition = new ValueTypeNotEqualCondition<int>();

            var expr1 = equalCondition.For<TestSource>(x => x.Id);
            Assert.IsNull(expr1);

            equalCondition.Value = new FilterValue<int?> { LeftValue = 1 };
            var expr2 = equalCondition.For<TestSource>(x => x.Id);
            Assert.IsFalse(expr2.Compile()(new TestSource { Id = 1 }));

            equalCondition.Value = new FilterValue<int?> { LeftValue = 2 };
            var expr3 = equalCondition.For<TestSource>(x => x.Id);
            Assert.IsTrue(expr3.Compile()(new TestSource { Id = 1 }));
        }

        [TestMethod]
        public void BetweenCondition()
        {
            var betweenCondition = new BetweenCondition<decimal>();

            var expr1 = betweenCondition.For<TestSource>(x => x.Price);

            Assert.IsNull(expr1);

            betweenCondition.Value = new FilterValue<decimal?>
            {
                LeftValue = 8M,
                RightValue = 70M
            };

            var expr2 = betweenCondition.For<TestSource>(x => x.Price);
            var expr2Func = expr2.Compile();

            Assert.IsFalse(expr2Func(new TestSource { Price = 1 }));
            Assert.IsTrue(expr2Func(new TestSource { Price = 8 }));
            Assert.IsTrue(expr2Func(new TestSource { Price = 15 }));

            betweenCondition.Value = new FilterValue<decimal?>
            {
                LeftValue = 8M
            };

            var expr3 = betweenCondition.For<TestSource>(x => x.Price);
            var expr3Func = expr3.Compile();

            Assert.IsTrue(expr3Func(new TestSource { Price = 9 }));
            Assert.IsTrue(expr3Func(new TestSource { Price = 346 }));
            Assert.IsFalse(expr3Func(new TestSource { Price = 0 }));

            betweenCondition.Value = new FilterValue<decimal?>
            {
                RightValue = 70M
            };

            var expr4 = betweenCondition.For<TestSource>(x => x.Price);
            var expr4Func = expr4.Compile();

            Assert.IsTrue(expr4Func(new TestSource { Price = 9 }));
            Assert.IsTrue(expr4Func(new TestSource { Price = -15 }));
            Assert.IsFalse(expr4Func(new TestSource { Price = 90 }));

        }

        [TestMethod]
        public void ValueTypeNoneCondition()
        {
            var noneCondition = new ValueTypeNoneCondition<int>();

            var expr1 = noneCondition.For<TestSource>(x => x.Id);

            Assert.IsNull(expr1);

            noneCondition.Value = new FilterValue<int?>
            {
                LeftValue = 123,
                RightValue = 1,
                Values = new List<int?> { 1, 2, 3, 34 }
            };

            var expr2 = noneCondition.For<TestSource>(x => x.Id);

            Assert.IsNull(expr2);


        }

        [TestMethod]
        public void ValueTypeIsInCondition()
        {
            var isInCondition = new ValueTypeIsInCondition<int>();

            var expr1 = isInCondition.For<TestSource>(x => x.Id);

            Assert.IsNull(expr1);

            isInCondition.Value = new FilterValue<int?>
            {
                Values = new List<int?> { 1, 2, 3 }
            };

            var expr2 = isInCondition.For<TestSource>(x => x.Id);
            var expr2Func = expr2.Compile();
            Assert.IsTrue(expr2Func(new TestSource { Id = 1 }));
            Assert.IsFalse(expr2Func(new TestSource { Id = 100 }));

            isInCondition.Value = new FilterValue<int?>
            {
                Values = new List<int?> { 1, 2, 3, null }
            };

            var expr3 = isInCondition.For<TestSource>(x => x.Id);

            Assert.IsNotNull(expr3);
        }

        [TestMethod]
        public void ValueTypeIsNotInCondition()
        {
            var isNotInCondition = new ValueTypeIsNotInCondition<int>();

            var expr1 = isNotInCondition.For<TestSource>(x => x.Id);

            Assert.IsNull(expr1);

            isNotInCondition.Value = new FilterValue<int?>
            {
                Values = new List<int?> { 1, 2, 3 }
            };

            var expr2 = isNotInCondition.For<TestSource>(x => x.Id);
            var expr2Func = expr2.Compile();
            Assert.IsFalse(expr2Func(new TestSource { Id = 1 }));
            Assert.IsTrue(expr2Func(new TestSource { Id = 100 }));

            isNotInCondition.Value = new FilterValue<int?>
            {
                Values = new List<int?> { 1, 2, 3, null }
            };

            var expr3 = isNotInCondition.For<TestSource>(x => x.Id);

            Assert.IsNotNull(expr3);
        }

        [TestMethod]
        public void GreaterThanCondition()
        {
            var greaterThanCondition = new GreaterThanCondition<decimal>();

            var expr1 = greaterThanCondition.For<TestSource>(x => x.Price);

            Assert.IsNull(expr1);

            greaterThanCondition.Value = new FilterValue<decimal?>
            {
                LeftValue = 1M
            };

            var expr2 = greaterThanCondition.For<TestSource>(x => x.Price);

            Assert.IsTrue(expr2.Compile()(new TestSource { Price = 2 }));

            greaterThanCondition.Value = new FilterValue<decimal?>
            {
                LeftValue = 5M
            };

            var expr3 = greaterThanCondition.For<TestSource>(x => x.Price);

            Assert.IsFalse(expr3.Compile()(new TestSource { Price = 5 }));
        }

        [TestMethod]
        public void LessThanCondition()
        {
            var lessThanCondition = new LessThanCondition<decimal>();

            var expr1 = lessThanCondition.For<TestSource>(x => x.Price);

            Assert.IsNull(expr1);

            lessThanCondition.Value = new FilterValue<decimal?>
            {
                LeftValue = 1M
            };

            var expr2 = lessThanCondition.For<TestSource>(x => x.Price);

            Assert.IsFalse(expr2.Compile()(new TestSource { Price = 2 }));

            lessThanCondition.Value = new FilterValue<decimal?>
            {
                LeftValue = 5M
            };

            var expr3 = lessThanCondition.For<TestSource>(x => x.Price);

            Assert.IsFalse(expr3.Compile()(new TestSource { Price = 5 }));

            lessThanCondition.Value = new FilterValue<decimal?>
            {
                LeftValue = 5M
            };

            var expr4 = lessThanCondition.For<TestSource>(x => x.Price);

            Assert.IsTrue(expr4.Compile()(new TestSource { Price = 4 }));
        }

        [TestMethod]
        public void GreaterThanOrEqualCondition()
        {
            var greaterThanOrEqual = new GreaterThanOrEqualCondition<decimal>();

            var expr1 = greaterThanOrEqual.For<TestSource>(x => x.Price);

            Assert.IsNull(expr1);

            greaterThanOrEqual.Value = new FilterValue<decimal?>
            {
                LeftValue = 1M
            };

            var expr2 = greaterThanOrEqual.For<TestSource>(x => x.Price);

            Assert.IsTrue(expr2.Compile()(new TestSource { Price = 2 }));

            greaterThanOrEqual.Value = new FilterValue<decimal?>
            {
                LeftValue = 5M
            };

            var expr3 = greaterThanOrEqual.For<TestSource>(x => x.Price);

            Assert.IsTrue(expr3.Compile()(new TestSource { Price = 5 }));
        }

        [TestMethod]
        public void LessThanOrEqualCondition()
        {
            var lessThanOrEqualCondition = new LessThanOrEqualCondition<decimal>();

            var expr1 = lessThanOrEqualCondition.For<TestSource>(x => x.Price);

            Assert.IsNull(expr1);

            lessThanOrEqualCondition.Value = new FilterValue<decimal?>
            {
                LeftValue = 1M
            };

            var expr2 = lessThanOrEqualCondition.For<TestSource>(x => x.Price);

            Assert.IsFalse(expr2.Compile()(new TestSource { Price = 2 }));

            lessThanOrEqualCondition.Value = new FilterValue<decimal?>
            {
                LeftValue = 5M
            };

            var expr3 = lessThanOrEqualCondition.For<TestSource>(x => x.Price);

            Assert.IsTrue(expr3.Compile()(new TestSource { Price = 5 }));

            lessThanOrEqualCondition.Value = new FilterValue<decimal?>
            {
                LeftValue = 5M
            };

            var expr4 = lessThanOrEqualCondition.For<TestSource>(x => x.Price);

            Assert.IsTrue(expr4.Compile()(new TestSource { Price = 4 }));
        }

        [TestMethod]
        public void IsIntersectionCondition()
        {
            var isIntersectionCondition = new IsIntersectionCondition<int>();

            var expr1 = isIntersectionCondition.For<TestIsIntersectionConditionSource>(x => x.Begin, x => x.End);

            Assert.IsNull(expr1);

            //try
            //{
            //    isIntersectionCondition.For<TestIsIntersectionConditionSource>(x => x.Begin);
            //}
            //catch (Exception e)
            //{
            //    Assert.IsTrue(e.GetType() == typeof(NotSupportedException));
            //}

            isIntersectionCondition.Value = new FilterValue<int?>
            {
                LeftValue = 0,
                RightValue = 10
            };

            var expr2 = isIntersectionCondition.For<TestIsIntersectionConditionSource>(x => x.Begin, x => x.End);
            Assert.IsNotNull(expr2);
            var expr2Func = expr2.Compile();
            Assert.IsTrue(expr2Func(new TestIsIntersectionConditionSource { Begin = 0, End = 10 }));
            Assert.IsFalse(expr2Func(new TestIsIntersectionConditionSource { Begin = -10, End = -1 }));

            isIntersectionCondition.Value = new FilterValue<int?>
            {
                LeftValue = -12
            };

            var expr3 = isIntersectionCondition.For<TestIsIntersectionConditionSource>(x => x.Begin, x => x.End);
            var expr3Func = expr3.Compile();
            Assert.IsTrue(expr3Func(new TestIsIntersectionConditionSource { Begin = 0, End = 10 }));
            Assert.IsFalse(expr3Func(new TestIsIntersectionConditionSource { Begin = -100, End = -15 }));

            isIntersectionCondition.Value = new FilterValue<int?>
            {
                RightValue = 15
            };

            var expr4 = isIntersectionCondition.For<TestIsIntersectionConditionSource>(x => x.Begin, x => x.End);
            var expr4Func = expr4.Compile();
            Assert.IsTrue(expr4Func(new TestIsIntersectionConditionSource { Begin = 0, End = 10 }));
            Assert.IsFalse(expr4Func(new TestIsIntersectionConditionSource { Begin = 16, End = 20 }));

        }        
    }
}
