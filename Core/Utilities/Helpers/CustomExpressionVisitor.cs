using System.Linq.Expressions;
using System.Reflection;

namespace Core.Utilities.Helpers;

public class CustomExpressionVisitor : ExpressionVisitor
{
    public List<object> Values { get; } = new List<object>();

    protected override Expression VisitMember(MemberExpression node)
    {
        if (node.Expression is ConstantExpression constantExpression)
        {
            var value = ((FieldInfo)node.Member).GetValue(constantExpression.Value);
            Values.Add(value!);
        }

        return base.VisitMember(node);
    }
}
