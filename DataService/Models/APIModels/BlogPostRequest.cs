using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataService.Models.APIModels
{
    public class BlogPostRequest<T> : BaseRequest<T> where T : class
    {
        [JsonProperty("start_date")]
        public DateTime? StartDate { get; set; }
        [JsonProperty("end_date")]
        public DateTime? EndDate { get; set; }
        [JsonProperty("public_at_max")]
        public DateTime? PublicAtMax { get; set; }
        [JsonProperty("public_at_min")]
        public DateTime? PublicAtMin { get; set; }
        [JsonProperty("blog_types")]
        public string BlogTypes { get; set; }
        [JsonProperty("blog_type_ids")]
        public  string BlogCateIds { get; set; }
    }
}
