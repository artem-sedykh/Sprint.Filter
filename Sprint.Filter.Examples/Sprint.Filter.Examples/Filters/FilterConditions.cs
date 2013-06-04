using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sprint.Filter.Conditions;

namespace Sprint.Filter.Examples.Filters
{
    public static class FilterConditions
    {
        public static void NumericConditions<TProperty>(IDictionary<string, IValueTypeCondition<TProperty>> conditions) where TProperty : struct, IComparable, IComparable<TProperty>, IEquatable<TProperty>
        {
            conditions["none"] = new ValueTypeNoneCondition<TProperty>("None");
            conditions["equal"] = new ValueTypeEqualCondition<TProperty>("Equal");
            conditions["notequal"] = new ValueTypeNotEqualCondition<TProperty>("NotEqual");
            conditions["between"] = new BetweenCondition<TProperty>("Between");
            conditions["lessthan"] = new LessThanCondition<TProperty>("LessThan");
            conditions["greaterthan"] = new GreaterThanCondition<TProperty>("GreaterThan");
            conditions["lessthanorequal"] = new LessThanOrEqualCondition<TProperty>("LessThanOrEqual");
            conditions["greaterthanorequal"] = new GreaterThanOrEqualCondition<TProperty>("GreaterThanOrEqual");
            conditions["isnull"] = new ValueTypeIsNullCondition<TProperty>("IsNull");
            conditions["isnotnull"] = new ValueTypeIsNotNullCondition<TProperty>("IsNotNull");
        }

        public static void StringConditions(IDictionary<string, IReferenceTypeCondition<string>> conditions)
        {
            conditions["none"] = new ReferenceTypeNoneCondition<string>("None");
            conditions["equal"] = new ReferenceTypeEqualCondition<string>("Equal");
            conditions["notequal"] = new ReferenceTypeNotEqualCondition<string>("NotEqual");
            conditions["contains"] = new ContainsCondition("Contains");
            conditions["startswith"] = new StartsWithCondition("StartsWith");
            conditions["endtswith"] = new EndsWithCondition("EndsWith");
            conditions["isnullorempty"] = new IsNullOrEmptyCondition("IsNullOrEmpty");
            conditions["isnotnullorempty"] = new IsNotNullOrEmptyCondition("IsNotNullOrEmpty");
        }

        public static void MultipleValueConditions<TProperty>(IDictionary<string, IValueTypeCondition<TProperty>> conditions) where TProperty : struct, IComparable, IComparable<TProperty>, IEquatable<TProperty>
        {
            conditions["none"] = new ValueTypeNoneCondition<TProperty>("None");
            conditions["isin"] = new ValueTypeIsInCondition<TProperty>("IsIn");
            conditions["inotsin"] = new ValueTypeIsNotInCondition<TProperty>("IsNotIn");
            conditions["isnull"] = new ValueTypeIsNullCondition<TProperty>("IsNull");
            conditions["isnotnull"] = new ValueTypeIsNotNullCondition<TProperty>("IsNotNull");
        }
    }
}