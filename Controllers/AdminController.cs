using Auth0.AuthenticationApi;
using Auth0.AuthenticationApi.Models;
using MeFit_BE.Models;
using MeFit_BE.Models.Domain.UserDomain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MeFit_BE.Controllers
{
    [Route("api/admin")]
    [ApiController]
    //[Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly HttpClient _client;
        private readonly MeFitDbContext _context;

        private readonly string BASE_URL = "https://dev-o072w2hj.eu.auth0.com/api/v2/";
        private readonly string ACCESS_TOKEN =
            "eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCIsImtpZCI6Im4zajFOQkpnUEpWX3h5ckRsM3hINiJ9.eyJpc3MiOiJodHRwczovL2Rldi1vMDcydzJoai5ldS5hdXRoMC5jb20vIiwic3ViIjoiNFhEZDZBYmczQXdXUDBaZDRjb2lGMk41NDd1NGV0Z3JAY2xpZW50cyIsImF1ZCI6Imh0dHBzOi8vZGV2LW8wNzJ3MmhqLmV1LmF1dGgwLmNvbS9hcGkvdjIvIiwiaWF0IjoxNjQ4MDM2NTU5LCJleHAiOjE2NTA2Mjg1NTksImF6cCI6IjRYRGQ2QWJnM0F3V1AwWmQ0Y29pRjJONTQ3dTRldGdyIiwic2NvcGUiOiJyZWFkOmNsaWVudF9ncmFudHMgY3JlYXRlOmNsaWVudF9ncmFudHMgZGVsZXRlOmNsaWVudF9ncmFudHMgdXBkYXRlOmNsaWVudF9ncmFudHMgcmVhZDp1c2VycyB1cGRhdGU6dXNlcnMgZGVsZXRlOnVzZXJzIGNyZWF0ZTp1c2VycyByZWFkOnVzZXJzX2FwcF9tZXRhZGF0YSB1cGRhdGU6dXNlcnNfYXBwX21ldGFkYXRhIGRlbGV0ZTp1c2Vyc19hcHBfbWV0YWRhdGEgY3JlYXRlOnVzZXJzX2FwcF9tZXRhZGF0YSByZWFkOnVzZXJfY3VzdG9tX2Jsb2NrcyBjcmVhdGU6dXNlcl9jdXN0b21fYmxvY2tzIGRlbGV0ZTp1c2VyX2N1c3RvbV9ibG9ja3MgY3JlYXRlOnVzZXJfdGlja2V0cyByZWFkOmNsaWVudHMgdXBkYXRlOmNsaWVudHMgZGVsZXRlOmNsaWVudHMgY3JlYXRlOmNsaWVudHMgcmVhZDpjbGllbnRfa2V5cyB1cGRhdGU6Y2xpZW50X2tleXMgZGVsZXRlOmNsaWVudF9rZXlzIGNyZWF0ZTpjbGllbnRfa2V5cyByZWFkOmNvbm5lY3Rpb25zIHVwZGF0ZTpjb25uZWN0aW9ucyBkZWxldGU6Y29ubmVjdGlvbnMgY3JlYXRlOmNvbm5lY3Rpb25zIHJlYWQ6cmVzb3VyY2Vfc2VydmVycyB1cGRhdGU6cmVzb3VyY2Vfc2VydmVycyBkZWxldGU6cmVzb3VyY2Vfc2VydmVycyBjcmVhdGU6cmVzb3VyY2Vfc2VydmVycyByZWFkOmRldmljZV9jcmVkZW50aWFscyB1cGRhdGU6ZGV2aWNlX2NyZWRlbnRpYWxzIGRlbGV0ZTpkZXZpY2VfY3JlZGVudGlhbHMgY3JlYXRlOmRldmljZV9jcmVkZW50aWFscyByZWFkOnJ1bGVzIHVwZGF0ZTpydWxlcyBkZWxldGU6cnVsZXMgY3JlYXRlOnJ1bGVzIHJlYWQ6cnVsZXNfY29uZmlncyB1cGRhdGU6cnVsZXNfY29uZmlncyBkZWxldGU6cnVsZXNfY29uZmlncyByZWFkOmhvb2tzIHVwZGF0ZTpob29rcyBkZWxldGU6aG9va3MgY3JlYXRlOmhvb2tzIHJlYWQ6YWN0aW9ucyB1cGRhdGU6YWN0aW9ucyBkZWxldGU6YWN0aW9ucyBjcmVhdGU6YWN0aW9ucyByZWFkOmVtYWlsX3Byb3ZpZGVyIHVwZGF0ZTplbWFpbF9wcm92aWRlciBkZWxldGU6ZW1haWxfcHJvdmlkZXIgY3JlYXRlOmVtYWlsX3Byb3ZpZGVyIGJsYWNrbGlzdDp0b2tlbnMgcmVhZDpzdGF0cyByZWFkOmluc2lnaHRzIHJlYWQ6dGVuYW50X3NldHRpbmdzIHVwZGF0ZTp0ZW5hbnRfc2V0dGluZ3MgcmVhZDpsb2dzIHJlYWQ6bG9nc191c2VycyByZWFkOnNoaWVsZHMgY3JlYXRlOnNoaWVsZHMgdXBkYXRlOnNoaWVsZHMgZGVsZXRlOnNoaWVsZHMgcmVhZDphbm9tYWx5X2Jsb2NrcyBkZWxldGU6YW5vbWFseV9ibG9ja3MgdXBkYXRlOnRyaWdnZXJzIHJlYWQ6dHJpZ2dlcnMgcmVhZDpncmFudHMgZGVsZXRlOmdyYW50cyByZWFkOmd1YXJkaWFuX2ZhY3RvcnMgdXBkYXRlOmd1YXJkaWFuX2ZhY3RvcnMgcmVhZDpndWFyZGlhbl9lbnJvbGxtZW50cyBkZWxldGU6Z3VhcmRpYW5fZW5yb2xsbWVudHMgY3JlYXRlOmd1YXJkaWFuX2Vucm9sbG1lbnRfdGlja2V0cyByZWFkOnVzZXJfaWRwX3Rva2VucyBjcmVhdGU6cGFzc3dvcmRzX2NoZWNraW5nX2pvYiBkZWxldGU6cGFzc3dvcmRzX2NoZWNraW5nX2pvYiByZWFkOmN1c3RvbV9kb21haW5zIGRlbGV0ZTpjdXN0b21fZG9tYWlucyBjcmVhdGU6Y3VzdG9tX2RvbWFpbnMgdXBkYXRlOmN1c3RvbV9kb21haW5zIHJlYWQ6ZW1haWxfdGVtcGxhdGVzIGNyZWF0ZTplbWFpbF90ZW1wbGF0ZXMgdXBkYXRlOmVtYWlsX3RlbXBsYXRlcyByZWFkOm1mYV9wb2xpY2llcyB1cGRhdGU6bWZhX3BvbGljaWVzIHJlYWQ6cm9sZXMgY3JlYXRlOnJvbGVzIGRlbGV0ZTpyb2xlcyB1cGRhdGU6cm9sZXMgcmVhZDpwcm9tcHRzIHVwZGF0ZTpwcm9tcHRzIHJlYWQ6YnJhbmRpbmcgdXBkYXRlOmJyYW5kaW5nIGRlbGV0ZTpicmFuZGluZyByZWFkOmxvZ19zdHJlYW1zIGNyZWF0ZTpsb2dfc3RyZWFtcyBkZWxldGU6bG9nX3N0cmVhbXMgdXBkYXRlOmxvZ19zdHJlYW1zIGNyZWF0ZTpzaWduaW5nX2tleXMgcmVhZDpzaWduaW5nX2tleXMgdXBkYXRlOnNpZ25pbmdfa2V5cyByZWFkOmxpbWl0cyB1cGRhdGU6bGltaXRzIGNyZWF0ZTpyb2xlX21lbWJlcnMgcmVhZDpyb2xlX21lbWJlcnMgZGVsZXRlOnJvbGVfbWVtYmVycyByZWFkOmVudGl0bGVtZW50cyByZWFkOmF0dGFja19wcm90ZWN0aW9uIHVwZGF0ZTphdHRhY2tfcHJvdGVjdGlvbiByZWFkOm9yZ2FuaXphdGlvbnNfc3VtbWFyeSByZWFkOm9yZ2FuaXphdGlvbnMgdXBkYXRlOm9yZ2FuaXphdGlvbnMgY3JlYXRlOm9yZ2FuaXphdGlvbnMgZGVsZXRlOm9yZ2FuaXphdGlvbnMgY3JlYXRlOm9yZ2FuaXphdGlvbl9tZW1iZXJzIHJlYWQ6b3JnYW5pemF0aW9uX21lbWJlcnMgZGVsZXRlOm9yZ2FuaXphdGlvbl9tZW1iZXJzIGNyZWF0ZTpvcmdhbml6YXRpb25fY29ubmVjdGlvbnMgcmVhZDpvcmdhbml6YXRpb25fY29ubmVjdGlvbnMgdXBkYXRlOm9yZ2FuaXphdGlvbl9jb25uZWN0aW9ucyBkZWxldGU6b3JnYW5pemF0aW9uX2Nvbm5lY3Rpb25zIGNyZWF0ZTpvcmdhbml6YXRpb25fbWVtYmVyX3JvbGVzIHJlYWQ6b3JnYW5pemF0aW9uX21lbWJlcl9yb2xlcyBkZWxldGU6b3JnYW5pemF0aW9uX21lbWJlcl9yb2xlcyBjcmVhdGU6b3JnYW5pemF0aW9uX2ludml0YXRpb25zIHJlYWQ6b3JnYW5pemF0aW9uX2ludml0YXRpb25zIGRlbGV0ZTpvcmdhbml6YXRpb25faW52aXRhdGlvbnMiLCJndHkiOiJjbGllbnQtY3JlZGVudGlhbHMifQ.Zt3G9SqwZPD2nETJi0jkACa5RkieDU3D-soanTsKBKD4oh_0NrY29CjYtXUhxHieHP0QexF8LtHG72MUTJAxKEkxE1-YQC5lYJYeq9zt4a3Dxu9yXkvthX8SM9OcDzXVLrnzE6RZqY2vDSWb-LTPXGEa_XgjZa3b-AAMfBglnbwUYQinEbjasyzc1mUx1JMQoNcoEmC5gO9atVHfY0jfFmOg7dswbYU3o4Ft6c7xcT1giylOy82JgDZg7H7UrUedQFbazgosFrv-sF0LTEevAkdfnnFt8mhN8XLyKmJzGA-WNU5NmXp7wvsitJ49V_z2phXyaBn9scdIvRpC8fdJrA";

        public AdminController(HttpClient client, MeFitDbContext context)
        {
            _client = client;
            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", GetAccessTokenAsync().Result);
            // _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ACCESS_TOKEN);
            _context = context;
        }

        [HttpGet("auth0")]
        public async Task<IActionResult> GetToken()
        {
            var client = new AuthenticationApiClient(new Uri("https://dev-o072w2hj.eu.auth0.com/"));


            var request = new ClientCredentialsTokenRequest
            {
                Audience = "https://dev-o072w2hj.eu.auth0.com/api/v2/",
                ClientId = "4XDd6Abg3AwWP0Zd4coiF2N547u4etgr",
                ClientSecret = "5urccG3ubdhB3Q7UkMU4A8F5r5cUaeE_3L7re-wVT0Eq1PriylPu5H7mExUQRRAB"
            };

            var token = await client.GetTokenAsync(request);

            return Ok(token);
            //return Ok(GetAccessTokenAsync().Result);
        }

        private async Task<string> GetAccessTokenAsync()
        {
            var client = new AuthenticationApiClient(new Uri("https://dev-o072w2hj.eu.auth0.com/"));

            var request = new ClientCredentialsTokenRequest
            {
                Audience = "https://dev-o072w2hj.eu.auth0.com/api/v2/",
                ClientId = "4XDd6Abg3AwWP0Zd4coiF2N547u4etgr",
                ClientSecret = "5urccG3ubdhB3Q7UkMU4A8F5r5cUaeE_3L7re-wVT0Eq1PriylPu5H7mExUQRRAB"
            };

            var token = await client.GetTokenAsync(request);

            return token.AccessToken;
        }


        [HttpPatch("users/{id}")]
        public async Task<IActionResult> PatchUser(string id, string email, string nickname) 
        {
            var url = BASE_URL + $"users/{id}";

            //var body = new Auth0UserBody() { Name = "Bob" };

            var body = new Auth0UserBody()
            {
                ClientId = "ViXbPTcrznJsmZxaEze6IdPXCZrGB4rp",
                Connection = "Username-Password-Authentication",
                Email = email,
                Nickname = nickname
                //Password = password
            };

            var json = JsonConvert.SerializeObject(body);

            await _client.PatchAsync(url, new StringContent(json, Encoding.UTF8, "application/json"));

            User user = await _context.Users.FirstOrDefaultAsync(u => u.AuthId == id);

            if (user != null)
            {
                // Updates Email of User in DB
                user.Email = email;
                _context.Entry(user).State = EntityState.Modified;
                _context.SaveChanges();
            }

            return Ok(json);
        }

        [HttpPost("users/{id}/roles")]
        public async Task<IActionResult> UpdateUserRoles(string id, string role)
        {
            var url = BASE_URL + $"users/{id}/roles";

            string body = GetRoleBody(role);
            if (body == null) return BadRequest();

            var request = new HttpRequestMessage
            {
                Content = new StringContent("{ \"roles\": [ \"rol_faU4A9kaqPd9WNXs\", \"rol_29pfwlGC1UEEYnDZ\", \"rol_zVdwxW5XMstHTCc2\" ] }", Encoding.UTF8, "application/json"),
                Method = HttpMethod.Delete,
                RequestUri = new Uri(url)
            };

            await _client.SendAsync(request);

            await _client.PostAsync(url, new StringContent(body, Encoding.UTF8, "application/json"));

            await UpdateDBRolesAsync(id, role);

            return Ok();
        }

        private string GetRoleBody(string role)
        {
            return role switch
            {
                "Admin" => "{ \"roles\": [ \"rol_faU4A9kaqPd9WNXs\", \"rol_29pfwlGC1UEEYnDZ\", \"rol_zVdwxW5XMstHTCc2\" ] }",
                "Contributor" => "{ \"roles\": [ \"rol_29pfwlGC1UEEYnDZ\", \"rol_zVdwxW5XMstHTCc2\" ] }",
                "User" => "{ \"roles\": [ \"rol_zVdwxW5XMstHTCc2\" ] }",
                _ => null,
            };
        }

        private async Task UpdateDBRolesAsync(string id, string role)
        {
            //Get user from database.
            User user = await _context.Users.FirstOrDefaultAsync(u => u.AuthId == id);
            //User user = await _context.Users.FirstOrDefaultAsync(u => u.Id == 1);

            if (user != null)
            {
                // Make user an administrator or Contributor
                switch (role)
                {
                    case "Admin":
                        user.IsAdmin = true;
                        user.IsContributor = true;
                        break;
                    case "Contributor":
                        user.IsAdmin = false;
                        user.IsContributor = true;
                        break;
                    case "User":
                        user.IsAdmin = false;
                        user.IsContributor = false;
                        break;
                    default:
                        return;
                }
                _context.Entry(user).State = EntityState.Modified;
                _context.SaveChanges();
            }

        }


        public class Auth0UserBody
        {
            [JsonProperty("client_id")]
            public string ClientId { get; set; }

            [JsonProperty("connection")]
            public string Connection { get; set; }

            [JsonProperty("email")]
            public string Email { get; set; }

            [JsonProperty("nickname")]
            public string Nickname { get; set; } 

            //[JsonProperty("password")]
            //public string Password { get; set; }
        }

        /*

        [HttpPost("users/{id}/password")]
        public async Task<IActionResult> postPassword(string id, string password)
        {
            var url = BASE_URL + $"users/{id}/roles";
            // /api/v2/tickets/password-change

            //var response = await _client.PostAsync(“dbconnections / change_password”, content);

            //string body = GetRoleBody(role); 
            //if (body == null) return BadRequest();

            //await _client.PostAsync(url, new StringContent(body, Encoding.UTF8, "application/json"));

            return Ok();
        }
        */

        /*
        public class AccessTokenRequestBody
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
        */


        [HttpGet("users")]
        private async Task<IActionResult> GetUsers()
        {
            var response = await _client.GetStringAsync(BASE_URL + "users");
            return Ok(response);
        }

        [HttpGet("users/{id}")]
        private async Task<IActionResult> GetUser(string id)
        {
            var url = BASE_URL + $"users/{id}";
            var response = await _client.GetStringAsync(url);
            return Ok(response);
        }

        [HttpGet("users/{id}/roles")]
        private async Task<IActionResult> GetUserRoles(string id)
        {
            var url = BASE_URL + $"users/{id}/roles";
            var response = await _client.GetStringAsync(url);
            return Ok(response);
        }
    }
}
