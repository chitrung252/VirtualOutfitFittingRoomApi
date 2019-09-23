using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataService.Models.APIModels
{
    public static class Sky
    {

        public static  T TryParse<T>(this string value) where T : class
        {
            
            var settings = new JsonSerializerSettings
            {
                Error = (sender, args) => {  args.ErrorContext.Handled = true; },
                MissingMemberHandling = MissingMemberHandling.Error
            };
            if(value == null)
            {
                return null;
            }
            var a  =  JsonConvert.DeserializeObject<T>(value, settings);
            return a;
            
        }

    }

    //public static T TryParseJson<T>(this string json, string schema) where T : new()
    //{
    //    JsonSchema parsedSchema = JsonSchema.Parse(schema);
    //    JObject jObject = JObject.Parse(json);

    //    return jObject.IsValid(parsedSchema) ?
    //        JsonConvert.DeserializeObject<T>(json) : default(T);
    //}
}
