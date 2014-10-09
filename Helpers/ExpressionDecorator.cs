using System.Diagnostics;

namespace Sprint.Helpers
{
    using System.Linq.Expressions;

    [DebuggerDisplay("{Expression}")]
    public class LambdaExpressionDecorator<TDelegate>
    {
        public Expression<TDelegate> Expression { get; set; }

        public LambdaExpressionDecorator(Expression<TDelegate> expression)
        {
            Expression = expression;
        }

        public static implicit operator TDelegate(LambdaExpressionDecorator<TDelegate> decorator)
        {

            return (decorator != null && decorator.Expression != null)
                ? decorator.Expression.Compile()
                : default(TDelegate);
        }

        public static implicit operator Expression<TDelegate>(LambdaExpressionDecorator<TDelegate> decorator)
        {
            return decorator.Expression;
        }
    }
}
