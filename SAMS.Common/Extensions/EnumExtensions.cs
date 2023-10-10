using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SAMS.Common.Extensions
{
    public static partial class EnumExtensions
    {
        public static string GetDescription<T>(object value)
        {
            var type = typeof(T);
            var memInfo = type.GetMember(value.ToString());
            var attributes = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (attributes != null)
            {
                return ((DescriptionAttribute)attributes[0]).Description;
            }
            return "";
        }

        public static int GetValue(this Enum value)
        {
            return Convert.ToInt32(value);
        }

        public static string DisplayName(this Enum value)
        {
            var outString = String.Empty;

            if (value != null)
            {
                Type enumType = value.GetType();
                var enumValue = Enum.GetName(enumType, value);
                if (enumValue != null)
                {
                    MemberInfo member = enumType.GetMember(enumValue)[0];

                    var attrs = member.GetCustomAttributes(typeof(DisplayAttribute), false);
                    if (!attrs.Any())
                    {
                        outString = "";
                        return outString;
                    }
                    outString = ((DisplayAttribute)attrs[0]).Name;

                    if (((DisplayAttribute)attrs[0]).ResourceType != null)
                    {
                        outString = ((DisplayAttribute)attrs[0]).GetName();
                    }
                }
            }

            return outString;
        }

        public static T GetValueFromDescription<T>(string description)
        {
            var type = typeof(T);
            if (!type.IsEnum) throw new InvalidOperationException();
            foreach (var field in type.GetFields())
            {
                var attribute = Attribute.GetCustomAttribute(field,
                    typeof(DescriptionAttribute)) as DescriptionAttribute;
                if (attribute != null)
                {
                    if (attribute.Description == description)
                        return (T)field.GetValue(null);
                }
                else
                {
                    if (field.Name == description)
                        return (T)field.GetValue(null);
                }
            }
            throw new ArgumentException("Not found.", "description");
        }
    }
}
