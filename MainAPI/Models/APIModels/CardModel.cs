using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace SkyConnect.API.Models.APIModels
{
    public class CardModel
    {
        [JsonProperty("code")]
        public string Code { get; set; }
        [JsonProperty("applied_time")]
        public DateTime AppliedTime { get; set; }
        [JsonProperty("provider")]
        public string Provider { get; set; }
        [JsonProperty("active")]
        public bool Active { get; set; }
        [JsonProperty("membership_id")]
        public int? MembershipId { get; set; }
        [JsonProperty("status")]
        public int Status { get; set; }

    }
}