using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeFit_BE.Models.JSON
{
    public class Auth0RoleBody
    {
        // RoleIds from Auth0
        private readonly string admin = "rol_faU4A9kaqPd9WNXs";
        private readonly string contributor = "rol_29pfwlGC1UEEYnDZ";
        private readonly string user = "rol_zVdwxW5XMstHTCc2";

        [JsonProperty("roles")]
        public string[] Roles { get; set; }

        public Auth0RoleBody(string role)
        {
            Roles = AssignRoles(role);
        }

        private string[] AssignRoles(string role)
        {
            return role switch
            {
                "Admin" => new string[] { admin, contributor, user },
                "Contributor" => new string[] { contributor, user },
                "User" => new string[] { user },
                _ => null,
            };
        }
    }
}
