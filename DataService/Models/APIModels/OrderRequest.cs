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
    public class OrderRequest<T> : BaseRequest<T> where T : class
    {
        public int CustomerId { get; set; }
        [DataMember(Name =("limit"))]
        [JsonProperty("limit")]
        public  Nullable<int> Limit { get; set; } = 5;
    }
}
