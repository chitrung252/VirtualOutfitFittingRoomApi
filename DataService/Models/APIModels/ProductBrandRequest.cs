using DataService.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.ModelBinding;

namespace DataService.Models.APIModels
{
    [ModelBinder(typeof(SkyModelBinder))]
    public class ProductBrandRequest<T> : BaseRequest<T>
    {
        [DataMember(Name = "name")]
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
