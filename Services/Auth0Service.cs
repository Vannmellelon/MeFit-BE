using Auth0.AuthenticationApi;
using Auth0.AuthenticationApi.Models;
using MeFit_BE.Models;
using MeFit_BE.Models.Domain.UserDomain;
using MeFit_BE.Models.JSON;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MeFit_BE.Services
{
    public class Auth0Service : IAuth0Service
    {
        private readonly HttpClient _client;
        private readonly MeFitDbContext _context;

        // Auth0 Management API 
        private readonly string BASE_URL = "https://dev-o072w2hj.eu.auth0.com/api/v2/";

        public Auth0Service(HttpClient client, MeFitDbContext context)
        {
            _client = client;
            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", GetAccessTokenAsync().Result);
            _context = context;
        }

        public async Task DeleteUserAsync(string id)
        {
            var url = BASE_URL + $"users/{id}";

            await _client.DeleteAsync(url);

            User user = await _context.Users.FirstOrDefaultAsync(u => u.AuthId == id);

            if (user != null)
            {
                // Remove User in DB
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<string> GetAccessTokenAsync()
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

        public async Task UpdateUserAsync(string id, string email, string nickname)
        {
            var url = BASE_URL + $"users/{id}";

            var body = new Auth0UserBody
            {
                ClientId = "ViXbPTcrznJsmZxaEze6IdPXCZrGB4rp",
                Connection = "Username-Password-Authentication",
                Email = email,
                Name = email,
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
        }

        public async Task UpdateUserRolesAsync(string id, Auth0RoleBody body) 
        {
            var url = BASE_URL + $"users/{id}/roles";

            // delete Auth0 roles of User before assigning new ones
            var jsonRoles = JsonConvert.SerializeObject(new Auth0RoleBody("Admin"));
            var request = new HttpRequestMessage
            {
                Content = new StringContent(jsonRoles, Encoding.UTF8, "application/json"),
                Method = HttpMethod.Delete,
                RequestUri = new Uri(url)
            };
            await _client.SendAsync(request);

            // assign roles to user in Auth0 and update DB
            var json = JsonConvert.SerializeObject(body);
            await _client.PostAsync(url, new StringContent(json, Encoding.UTF8, "application/json"));

            //await UpdateDBRolesAsync(id, role);
        }

        public bool UserExists(string id)
        {
            return _context.Users.Any(u => u.AuthId == id);
        }
    }
}
