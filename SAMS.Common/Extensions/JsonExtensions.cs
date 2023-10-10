using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using System;
using System.Collections.Generic;
using System.Text;

namespace SAMS.Common.Extensions
{
    public static class JsonExtensions
    {
        //TODO: Jschema yerine alternatif bul
        //public static T TryParseJson<T>(this string json, string schema) where T : new()
        //{
        //    JSchema parsedSchema = JSchema.Parse(schema);
        //    JObject jObject = JObject.Parse(json);

        //    return jObject.IsValid(parsedSchema) ?
        //        JsonConvert.DeserializeObject<T>(json) : default(T);
        //}

        public static bool IsNull(this JObject obj)
        {
            bool isNull = false;
            isNull = obj == null || obj.Type == JTokenType.Null;
            return isNull;
        }

        public static bool IsNull(this JToken obj)
        {
            bool isNull = false;
            isNull = obj == null || obj.Type == JTokenType.Null;
            return isNull;
        }
        public static bool IsValidJson(this string strInput)
        {
            strInput = strInput.Trim();
            if ((strInput.StartsWith("{") && strInput.EndsWith("}")) || //For object
                (strInput.StartsWith("[") && strInput.EndsWith("]"))) //For array
            {
                try
                {
                    var obj = JToken.Parse(strInput);
                    return true;
                }
                catch (JsonReaderException)
                {
                    //Exception in parsing json 
                    return false;
                }
                catch (Exception) //some other exception
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }

}
