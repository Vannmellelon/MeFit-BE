using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeFit_BE.Models.JSON
{
    public class Auth0TokenRequestBody
    {
        [JsonProperty("client_id")]
        public string ClientId { get; set; }

        [JsonProperty("client_secret")]
        public string ClientSecret { get; set; }

        [JsonProperty("audience")]
        public string Audience { get; set; }

        [JsonProperty("grant_type")]
        public string GrantType { get; } = "client_credentials";
    }
}
