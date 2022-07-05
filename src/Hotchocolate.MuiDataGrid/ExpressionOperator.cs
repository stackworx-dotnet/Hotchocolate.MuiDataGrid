namespace Stackworx.Hotchocolate.MuiDataGrid;

using System.Text;

internal static class ExpressionOperator
{
    public static MemberExpression? GetMemberExpression(Expression? expression)
    {
        if (expression is MemberExpression memberExpression)
        {
            return memberExpression;
        }

        if (expression is LambdaExpression lambdaExpression)
        {
            switch (lambdaExpression.Body)
            {
                case MemberExpression body:
                    return body;
                case UnaryExpression unaryExpression:
                    return (MemberExpression)unaryExpression.Operand;
            }
        }

        return null;
    }
}
