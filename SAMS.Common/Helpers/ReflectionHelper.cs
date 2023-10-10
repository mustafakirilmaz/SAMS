using SAMS.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SAMS.Common.Helpers
{
    public static class ReflectionHelper
    {
        /// <summary>
        /// Eğer bir nesnenin child property'lerinin tüm property'leri null ise, o property'yi null yapar.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">Convert Edilecek Nesne</param>
        /// <returns>Convert Edilmiş Nesne</returns>
        public static object ConvertPropertiesToNull(object obj)
        {
            if (obj == null) return null;
            Type objType = obj.GetType();
            PropertyInfo[] properties = objType.GetProperties();
            var simpleTypes = properties.Where(t => IsSimpleType(t.PropertyType));
            var nonValueTypes = properties.Where(p => !simpleTypes.Contains(p));

            foreach (var child in nonValueTypes)
            {
                child.SetValue(obj, ConvertPropertiesToNull(child.GetValue(obj)));
            }
            return simpleTypes.All(z => z.GetValue(obj) == null) && nonValueTypes.All(z => z.GetValue(obj) == null) ? null : obj;
        }

        private static bool IsSimpleType(Type type)
        {
            return
                type.IsPrimitive ||
                new Type[] { typeof(Enum), typeof(String), typeof(Decimal), typeof(DateTime), typeof(DateTimeOffset), typeof(TimeSpan), typeof(Guid) }.Contains(type) ||
                Convert.GetTypeCode(type) != TypeCode.Object ||
                (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>) && IsSimpleType(type.GetGenericArguments()[0]));
        }

        [AttributeUsage(AttributeTargets.Property | AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
        public class PropNameAttribute : Attribute
        {
            private string _name = string.Empty;

            public PropNameAttribute(string name)
            {
                _name = name;
            }

            public string Name
            {
                get { return _name; }
            }
        }


        /// <summary>
        /// Clone object to another object
        /// </summary>
        /// <typeparam name="T">int, string etc.</typeparam>
        /// <param name="propertyExcludeList">excluding propety names with string list</param>
        /// <returns></returns>
        public static TU CloneObject<T, TU>(T original, params string[] propertyExcludeList)
        {
            TU copy = Activator.CreateInstance<TU>();
            if (original != null)
            {
                PropertyInfo[] propertyInfoList = typeof(TU).GetProperties();
                foreach (PropertyInfo propertyInfo in propertyInfoList)
                {
                    if (propertyExcludeList == null || (propertyExcludeList != null && !propertyExcludeList.Contains(propertyInfo.Name)))
                    {
                        PropertyInfo p = typeof(T).GetProperties().FirstOrDefault(d => d.Name == propertyInfo.Name);
                        if (p == null) continue;
                        if (!p.CanWrite) continue;
                        var value = p.GetValue(original, null);

                        if ((p.PropertyType.Name == "string" || p.PropertyType.Name == "String") && (propertyInfo.PropertyType.Name == "DateTime" || propertyInfo.PropertyType.Name == "Nullable`1"))
                        {
                            if (value != null && !string.IsNullOrEmpty(value.ToString()))
                            {
                                DateTime dtVal = Convert.ToString(value).ConvertStringToDateTime();
                                propertyInfo.SetValue(copy, dtVal, null);
                            }
                        }
                        else
                        {
                            try
                            {
                                propertyInfo.SetValue(copy, value, null);
                            }
                            catch (Exception)
                            {
                                propertyInfo.SetValue(copy, default(T), null);
                            }
                        }
                    }
                }
            }
            return copy;
        }

        /// <summary>
        /// Copy object to another object
        /// </summary>
        /// <typeparam name="T">int, string etc.</typeparam>
        /// <param name="propertyExcludeList">excluding propety names with string list</param>
        /// <returns></returns>
        public static TU CopyObject<T, TU>(this TU copy, T original, params string[] propertyExcludeList)
        {
            PropertyInfo[] propertyInfoList = typeof(TU).GetProperties();
            foreach (PropertyInfo propertyInfo in propertyInfoList)
            {
                if (propertyExcludeList == null || (propertyExcludeList != null && !propertyExcludeList.Contains(propertyInfo.Name)))
                {
                    PropertyInfo p = typeof(T).GetProperties().FirstOrDefault(d => d.Name == propertyInfo.Name);
                    if (p == null) continue;
                    if (!p.CanWrite) continue;
                    var value = p.GetValue(original, null);

                    if ((p.PropertyType.Name == "string" || p.PropertyType.Name == "String") && (propertyInfo.PropertyType.Name == "DateTime" || propertyInfo.PropertyType.Name == "Nullable`1"))
                    {
                        if (value != null && !string.IsNullOrEmpty(value.ToString()))
                        {
                            DateTime dtVal = Convert.ToString(value).ConvertStringToDateTime();
                            propertyInfo.SetValue(copy, dtVal, null);
                        }
                    }
                    else
                    {
                        try
                        {
                            propertyInfo.SetValue(copy, value, null);
                        }
                        catch (Exception)
                        {
                            propertyInfo.SetValue(copy, default(T), null);
                        }
                    }
                }
            }
            return copy;
        }
    }
}
