using DataService.Models.APIModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataService.Models.APIModels
{
    public class FbRequest
    {
        [JsonProperty("fb_access_token")]
        public string FbAccessToken { get; set; }
    }
}