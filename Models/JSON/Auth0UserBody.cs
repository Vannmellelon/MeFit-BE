using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeFit_BE.Models.JSON
{
    public class Auth0UserBody
    {
        [JsonProperty("client_id")]
        public string ClientId { get; set; }

        [JsonProperty("connection")]
        public string Connection { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("nickname")]
        public string Nickname { get; set; }
    }
}
