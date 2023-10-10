using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Org.BouncyCastle.Asn1.Cms;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Linq;
using SAMS.Common.Helpers;

namespace SAMS.Common
{
    public class EnumTypesSchemaFilter : ISchemaFilter
    {
        private readonly XDocument _xmlComments;

        public EnumTypesSchemaFilter(string xmlPath)
        {
            if (File.Exists(xmlPath))
            {
                _xmlComments = XDocument.Load(xmlPath);
            }
        }

        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (context.Type.IsEnum)
            {
                schema.Enum.Clear();
                var enumNames = Enum.GetNames(context.Type).ToList();
                enumNames.ForEach(name => schema.Enum.Add(new OpenApiString($"{name}")));

                schema.Type = "string";
                schema.Format = string.Empty;
            }
        }
        //public void Apply(OpenApiSchema model, SchemaFilterContext context)
        //{
        //    //if (context.Type.IsEnum)
        //    //{
        //    //    model.Enum.Clear();
        //    //    foreach (string enumName in Enum.GetNames(context.Type))
        //    //    {
        //    //        System.Reflection.MemberInfo memberInfo = context.Type.GetMember(enumName).FirstOrDefault(m => m.DeclaringType == context.Type);
        //    //        EnumMemberAttribute enumMemberAttribute = memberInfo == null
        //    //         ? null
        //    //         : memberInfo.GetCustomAttributes(typeof(EnumMemberAttribute), false).OfType<EnumMemberAttribute>().FirstOrDefault();
        //    //        string label = enumMemberAttribute == null || string.IsNullOrWhiteSpace(enumMemberAttribute.Value)
        //    //         ? enumName
        //    //         : enumMemberAttribute.Value;
        //    //        model.Enum.Add(new OpenApiString(label));
        //    //    }
        //    //}

        //    if (context.Type.IsEnum)
        //    {
        //        model.Enum.Clear();
        //        Enum.GetNames(context.Type)
        //            .ToList()
        //            .ForEach(name => model.Enum.Add(new OpenApiString($"{Convert.ToInt64(Enum.Parse(context.Type, name))} = {name}")));
        //    }
        //}
    }
}

