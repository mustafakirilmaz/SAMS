using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;

namespace SAMS.Common.Extensions
{
    public class ExpressionConverter : ExpressionVisitor
    {
        static readonly MethodInfo EnumDescriptionMethod = Expression.Call(
            typeof(DescriptionExtensions), nameof(DescriptionExtensions.Description), new[] { typeof(ExpressionType) },
            Expression.Constant(default(ExpressionType)))
            .Method.GetGenericMethodDefinition();

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            if (node.Method.IsGenericMethod && node.Method.GetGenericMethodDefinition() == EnumDescriptionMethod)
                return TranslateEnumDescription(Visit(node.Arguments[0]));
            return base.VisitMethodCall(node);
        }

        static Expression TranslateEnumDescription(Expression arg)
        {
            var names = Enum.GetNames(arg.Type);
            var values = Enum.GetValues(arg.Type);
            Expression result = Expression.Constant("");
            for (int i = names.Length - 1; i >= 0; i--)
            {
                var value = values.GetValue(i);
                var description = arg.Type.GetField(names[i], BindingFlags.Public | BindingFlags.Static).Description();
                // arg == value ? description : ...
                result = Expression.Condition(
                    Expression.Equal(arg, Expression.Constant(value)),
                    Expression.Constant(description),
                    result);
            }
            return result;
        }
    }

    public static class DescriptionExtensions
    {
        public static string Description<TEnum>(this TEnum source) where TEnum : struct, Enum => typeof(TEnum).GetField(source.ToString()).Description();
        public static string Description(this FieldInfo source) => source.GetCustomAttribute<DescriptionAttribute>()?.Description ?? source.Name;
    }
}
