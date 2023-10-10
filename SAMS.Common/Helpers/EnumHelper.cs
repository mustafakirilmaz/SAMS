using SAMS.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace SAMS.Common.Helpers
{
    public static class EnumHelper<T>
    {
        public static IList<T> GetValues(Enum value)
        {
            var enumValues = new List<T>();

            foreach (FieldInfo fi in value.GetType().GetFields(BindingFlags.Static | BindingFlags.Public))
            {
                enumValues.Add((T)Enum.Parse(value.GetType(), fi.Name, false));
            }
            return enumValues;
        }

        public static T Parse(string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

        public static IList<string> GetNames(Enum value)
        {
            return value.GetType().GetFields(BindingFlags.Static | BindingFlags.Public).Select(fi => fi.Name).ToList();
        }

        public static IList<string> GetDisplayValues(Enum value)
        {
            return GetNames(value).Select(obj => GetDisplayValue(Parse(obj))).ToList();
        }

        public static string GetDisplayValue(T value)
        {
            var fieldInfo = value.GetType().GetField(value.ToString());

            var descriptionAttributes = fieldInfo.GetCustomAttributes(
                typeof(DisplayAttribute), false) as DisplayAttribute[];

            if (descriptionAttributes == null) return string.Empty;
            return (descriptionAttributes.Length > 0) ? descriptionAttributes[0].Name : value.ToString();
        }

        public static string GetDescription(T value)
        {
            var fieldInfo = value.GetType().GetField(value.ToString());

            var descriptionAttributes = fieldInfo.GetCustomAttributes(
                typeof(DescriptionAttribute), false) as DescriptionAttribute[];

            if (descriptionAttributes == null) return string.Empty;
            return (descriptionAttributes.Length > 0) ? descriptionAttributes[0].Description : value.ToString();
        }

        public static T Clone(T source)
        {
            if (!typeof(T).IsSerializable)
            {
                throw new ArgumentException("The type must be serializable.", "source");
            }

            if (Object.ReferenceEquals(source, null))
            {
                return default(T);
            }

            IFormatter formatter = new BinaryFormatter();
            Stream stream = new MemoryStream();
            using (stream)
            {
                formatter.Serialize(stream, source);
                stream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(stream);
            }
        }

        public static List<SelectItem> GetSelectList(bool degereGoreSirala = false)
        {
            List<SelectItem> list = new List<SelectItem>();
            Type type1 = typeof(T);

            foreach (FieldInfo field in type1.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Static | BindingFlags.Public | BindingFlags.GetField))
            {
                object rawConstantValue = field.GetRawConstantValue();
                list.Add(new SelectItem()
                {
                    label = PropertyHelper.GetDisplayName(field),
                    value = rawConstantValue
                });
            }

            list = degereGoreSirala == true ? list.OrderBy(p => p.value).ToList() : list.OrderBy(p => p.label).ToList();

            return list;
        }

        public static List<IdNamePair> GetEnumListWithDescription(bool degereGoreSirala = false)
        {
            List<IdNamePair> list = new List<IdNamePair>();
            Type type1 = typeof(T);

            foreach (FieldInfo field in type1.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Static | BindingFlags.Public | BindingFlags.GetField))
            {
                object rawConstantValue = field.GetRawConstantValue();
                list.Add(new IdNamePair()
                {
                    Id = rawConstantValue.ToString(),
                    Name = PropertyHelper.GetDisplayName(field),
                    Description = GetDescription(Parse(rawConstantValue.ToString())),
                });
            }

            list = degereGoreSirala == true ? list.OrderBy(p => p.Id).ToList() : list.OrderBy(p => p.Name).ToList();

            return list;
        }

        public static IdNamePair GetEnumByDescription(string description)
        {
            var enumList = GetEnumListWithDescription();
            return enumList.Where(p => p.Description == description).FirstOrDefault();
        }

        public static List<SelectItem> GetSelectListAsDescription(bool orderByValue = false)
        {
            List<SelectItem> list = new List<SelectItem>();
            Type type1 = typeof(T);

            foreach (FieldInfo field in type1.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Static | BindingFlags.Public | BindingFlags.GetField))
            {
                object rawConstantValue = field.GetRawConstantValue();

                string description = string.Empty;
                var descriptionAttributes = field.GetCustomAttributes(
                typeof(DescriptionAttribute), false) as DescriptionAttribute[];
                if (descriptionAttributes != null && descriptionAttributes.Length > 0)
                {
                    description = descriptionAttributes[0].Description;
                }

                list.Add(new SelectItem()
                {
                    label = description,
                    value = rawConstantValue
                });
            }

            list = orderByValue == true ? list.OrderBy(p => p.value).ToList() : list.OrderBy(p => p.label).ToList();

            return list;
        }

    }

}
