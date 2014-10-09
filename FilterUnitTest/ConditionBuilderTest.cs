using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using LinqKit;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sprint.Filter;
using Sprint.Filter.Conditions;

namespace FilterUnitTest
{
    internal class Task
    {
        public int Id { get; set; }

        public string Guid {
            get { return Id.ToString(CultureInfo.InvariantCulture); }
        }

        public string Name { get; set; }

        public IEnumerable<User> Users { get; set; }

        public IQueryable<User> QueryUsers { get; set; }
    }

    internal class User
    {
        public int Id { get; set; }

        public string Guid
        {
            get { return Id.ToString(CultureInfo.InvariantCulture); }
        }

        public string Name { get; set; }

        public decimal Salary { get; set; }

        public IEnumerable<Task> Tasks { get; set; }
    }

    [TestClass]
    public sealed class ConditionBuilderTest
    {
        internal IEnumerable<Task> Source { get; set; }

        public ConditionBuilderTest()
        {
            Source = new List<Task>
                {
                    new Task
                        {
                            Id = 1,
                            Name = "1",
                            Users = new List<User>
                                {
                                    new User {Id = 1, Salary = 200, Name = "Nancy"},
                                    new User {Id = 2, Salary = 300, Name = "Andrew"},
                                    new User {Id = 3, Salary = 400, Name = "Janet"},
                                }
                        },
                    new Task
                        {
                            Id = 2,
                            Name = "2",
                            Users = new List<User>
                                {
                                    new User {Id = 4, Salary = 500, Name = "Margaret"},
                                }
                        }
                    ,
                    new Task
                        {
                            Id = 3,
                            Name = "3",
                            Users = new List<User>
                                {
                                    new User {Id = 5, Salary = 600, Name = "Steven"},
                                    new User {Id = 5, Salary = 700, Name = "Michael"},
                                }
                        }
                    ,
                    new Task
                        {
                            Id = 3,
                            Name = "3",
                            Users = new List<User>()
                        }
                };
        }

        [TestMethod]
        public void ReferenceTypeConditionBuilder()
        {
            var filter = new ReferenceTypeFilter<User, string>();
            filter.For(x => x.Guid);

            filter.Conditions(conditions =>
                {
                    conditions["none"] = new ReferenceTypeNoneCondition<string>();
                    conditions["equal"] = new ReferenceTypeEqualCondition<string>();
                    conditions["isin"] = new ReferenceTypeIsInCondition<string>();
                    conditions["isnull"] = new ReferenceTypeIsNullCondition<string>();
                    conditions["isnotnull"] = new ReferenceTypeIsNotNullCondition<string>();
                });

            filter.ConditionBuilder((filterValue, condition, conditionKey) => {
                var expression = condition(filterValue);
                
                if(expression == null)
                    return null;

                switch(conditionKey)
                {
                    case "isnotnull":
                    {
                        return Linq.Expr<Task, bool>(x => x.Users.Any());
                    }
                    case "isnull":
                    {
                        return Linq.Expr<Task, bool>(x => !x.Users.Any());
                    }
                    default: //for equal,isin
                    {
                        return Linq.Expr<Task, bool>(x => x.Users.Any(expression));
                    }
                }
            });

            filter.Init(new FilterValue<string>
            {
                LeftValue = "1",
                ConditionKey = "equal"
            });

            var expr1 = filter.BuildExpression<User>();

            Assert.IsNull(expr1);

            var epxr2 = filter.BuildExpression<Task>();

            Assert.IsNotNull(epxr2);

            var tasksAnyUserId1 = filter.ApplyFilter(Source);

            Assert.IsTrue(tasksAnyUserId1.Count() == 1);


            filter.Init(new FilterValue<string>
            {
                ConditionKey = "isnotnull"
            });


            var tasksAnyUsers = filter.ApplyFilter(Source);

            Assert.IsTrue(tasksAnyUsers.Count() == 3);

            filter.Init(new FilterValue<string>
            {
                ConditionKey = "isnull"
            });

            var tasksNotAnyUsers = filter.ApplyFilter(Source);

            Assert.IsTrue(tasksNotAnyUsers.Count() == 1);


            filter.Init(new FilterValue<string>
            {
                ConditionKey = "isin",
                Values = new List<string> { "1", "5" }
            });

            var tasksIsInUser1User5 = filter.ApplyFilter(Source);

            Assert.IsTrue(tasksIsInUser1User5.Count() == 2);
        }

        [TestMethod]
        public void ValueTypeConditionBuilder()
        {
            var filter = new ValueTypeFilter<User, int>();
            filter.For(x => x.Id);

            filter.Conditions(conditions =>
            {
                conditions["none"] = new ValueTypeNoneCondition<int>();
                conditions["equal"] = new ValueTypeEqualCondition<int>();
                conditions["isin"] = new ValueTypeIsInCondition<int>();
                conditions["isnull"] = new ValueTypeIsNullCondition<int>();
                conditions["isnotnull"] = new ValueTypeIsNullCondition<int>();
            });

            filter.ConditionBuilder((filterValue,condition, conditionKey) =>
                {
                    var expression = condition(filterValue);
                if (expression == null)
                    return null;

                switch (conditionKey)
                {
                    case "isnotnull":
                        {
                            return Linq.Expr<Task, bool>(x => x.Users.AsQueryable().Any());
                        }
                    case "isnull":
                        {
                            return Linq.Expr<Task, bool>(x => !x.Users.AsQueryable().Any());
                        }
                    default://for equal,isin
                        {
                            return Linq.Expr<Task, bool>(x => x.Users.AsQueryable().Any(expression));
                        }
                }
            });

            filter.Init(new FilterValue<int?>
            {
                LeftValue = 1,
                ConditionKey = "equal"
            });

            var expr1 = filter.BuildExpression<User>();

            Assert.IsNull(expr1);

            var epxr2 = filter.BuildExpression<Task>();

            Assert.IsNotNull(epxr2);

            var tasksAnyUserId1 = filter.ApplyFilter(Source);

            Assert.IsTrue(tasksAnyUserId1.Count() == 1);


            filter.Init(new FilterValue<int?>
            {
                ConditionKey = "isnotnull"
            });


            var tasksAnyUsers = filter.ApplyFilter(Source);

            Assert.IsTrue(tasksAnyUsers.Count() == 3);

            filter.Init(new FilterValue<int?>
            {
                ConditionKey = "isnull"
            });

            var tasksNotAnyUsers = filter.ApplyFilter(Source);

            Assert.IsTrue(tasksNotAnyUsers.Count() == 1);


            filter.Init(new FilterValue<int?>
            {
                ConditionKey = "isin",
                Values = new List<int?> { 1, 5 }
            });

            var tasksIsInUser1User5 = filter.ApplyFilter(Source);

            Assert.IsTrue(tasksIsInUser1User5.Count() == 2);
        }
    }
}
