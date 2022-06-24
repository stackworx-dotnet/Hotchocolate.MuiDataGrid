namespace Stackworx.Hotchocolate.MuiDataGrid;

using System.Text;

internal static class ExpressionOperator
{
    public static string GetPropertyPath(Expression expr)
    {
        var path = new StringBuilder();
        MemberExpression? memberExpression = GetMemberExpression(expr);

        while (memberExpression != null)
        {
            if (path.Length > 0)
            {
                path.Insert(0, ".");
            }

            path.Insert(0, memberExpression.Member.Name);
            memberExpression = GetMemberExpression(memberExpression.Expression);
        }

        return path.ToString();
    }

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
