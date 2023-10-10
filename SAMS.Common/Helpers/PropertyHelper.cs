using System;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Reflection;

namespace SAMS.Common.Helpers
{
    public static class PropertyHelper
    {
        public static PropertyInfo GetPropertyInfo<T>(Expression<Func<T>> propertyExpression)
        {
            return GetPropertyInfo(propertyExpression.Body);
        }

        public static PropertyInfo GetPropertyInfo<T>(Expression<Func<T, object>> propertyExpression)
        {
            var expression = propertyExpression.Body;
            if (propertyExpression.Body is UnaryExpression)
                expression = ((UnaryExpression)expression).Operand;
            return GetPropertyInfo(expression);
        }

        public static PropertyInfo GetPropertyInfo(Expression expression)
        {
            var memberExpression = expression as MemberExpression;
            if (memberExpression == null)
                throw new ArgumentException("Cannot access expression member");
            var property = memberExpression.Member as PropertyInfo;
            if (property == null)
                throw new ArgumentException("Expression is not property");
            return (PropertyInfo)memberExpression.Member;
        }

        public static string GetPropertyName<T>(Expression<Func<T>> propertyExpression)
        {
            return GetPropertyInfo(propertyExpression).Name;
        }

        public static string GetPropertyName<T>(Expression<Func<T, object>> propertyExpression)
        {
            return GetPropertyInfo(propertyExpression).Name;
        }

        public static string GetPropertyAsMemberClass<T>(Expression<Func<T>> propertyExpression)
        {
            var propInfo = GetPropertyInfo(propertyExpression);
            return propInfo.ReflectedType.Name + "." + propInfo.Name;
        }

        public static string GetPropAsDomFormat<T>(Expression<Func<T>> propertyExpression)
        {
            var propInfo = GetPropertyInfo(propertyExpression);
            if (propInfo.ReflectedType != null)
            {
                return propInfo.ReflectedType.Name + "_" + propInfo.Name;
            }
            return propInfo.Name;
        }

        public static string GetPropertyValue<T>(T type, string propertyName)
        {
            var propertyInfo = typeof(T).GetProperty(propertyName);
            var value = propertyInfo.GetValue(type, null);
            return value.ToString();
        }

        public static object GetPropValue(object obj, string propName, char splitter = '.')
        {
            string[] nameParts = propName.Split(splitter);
            if (nameParts.Length == 1)
            {
                return obj.GetType().GetProperty(propName).GetValue(obj, null);
            }

            foreach (string part in nameParts)
            {
                if (obj == null) { return null; }

                Type type = obj.GetType();
                PropertyInfo info = type.GetProperty(part);
                if (info == null) { return null; }

                obj = info.GetValue(obj, null);
            }
            return obj;
        }

        public static string GetDisplayName(FieldInfo field)
        {
            DisplayAttribute customAttribute = CustomAttributeExtensions.GetCustomAttribute<DisplayAttribute>((MemberInfo)field, false);
            if (customAttribute != null)
            {
                string name = customAttribute.GetName();
                if (!string.IsNullOrEmpty(name))
                    return name;
            }
            return field.Name;
        }
    }

}
