﻿using Auth0.AuthenticationApi;
using Auth0.AuthenticationApi.Models;
using AutoMapper;
using MeFit_BE.Models;
using MeFit_BE.Models.Domain.UserDomain;
using MeFit_BE.Models.DTO.ContributorRequest;
using MeFit_BE.Models.JSON;
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
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace MeFit_BE.Controllers
{
    [Route("api/admin")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    [ApiConventionType(typeof(MeFitConventions))]
    public class AdminController : ControllerBase
    {
        private readonly HttpClient _client;
        private readonly MeFitDbContext _context;
        private readonly IMapper _mapper;

        // Auth0 Management API 
        private readonly string BASE_URL = "https://dev-o072w2hj.eu.auth0.com/api/v2/";

        public AdminController(HttpClient client, MeFitDbContext context, IMapper mapper)
        {
            _client = client;
            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", GetAccessTokenAsync().Result);
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Method fetches Access Token for Management API
        /// </summary>
        /// <returns></returns>
        [HttpGet("auth0")]
        public async Task<IActionResult> GetToken()
        {
            return Ok(await GetAccessTokenAsync());
        }

        /// <summary>
        /// Method Updates User using Auth0 Manegement API and updates User in DB
        /// </summary>
        /// <param name="id"></param>
        /// <param name="email"></param>
        /// <param name="nickname"></param>
        /// <returns></returns>
        [HttpPatch("users/{id}")]
        public async Task<IActionResult> PatchUser(string id, string email, string nickname) 
        {
            // /api/v2/tickets/password-change
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

            return Ok();
        }

        /// <summary>
        /// Method Deletes User from Auth0 and DB using Auth0 Manegement API and MeFit DB Context
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("users/{id}")]
        public async Task<IActionResult> DeleteUser(string id) 
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

            return NoContent();
        }

        /// <summary>
        /// Method assigns roles to User using Auth0 Manegement API and updates User in DB 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        [HttpPost("users/{id}/roles")]
        public async Task<IActionResult> UpdateUserRoles(string id, string role)
        {
            var url = BASE_URL + $"users/{id}/roles";

            var body = new Auth0RoleBody(role);
            if (body.Roles == null) return BadRequest();

            // delete Auth0 roles of User before assigning new ones
            var request = new HttpRequestMessage
            {
                Content = new StringContent(GetJsonRoles(), Encoding.UTF8, "application/json"),
                // Content = new StringContent("{ \"roles\": [ \"rol_faU4A9kaqPd9WNXs\", \"rol_29pfwlGC1UEEYnDZ\", \"rol_zVdwxW5XMstHTCc2\" ] }", Encoding.UTF8, "application/json"),
                Method = HttpMethod.Delete,
                RequestUri = new Uri(url)
            };
            await _client.SendAsync(request);

            // assign roles to user in Auth0 and update DB
            var json = JsonConvert.SerializeObject(body);
            await _client.PostAsync(url, new StringContent(json, Encoding.UTF8, "application/json"));

            await UpdateDBRolesAsync(id, role);

            return Ok();
        }

        // Methods for contributor requests

        /// <summary>
        /// Get all pending contributor requests.
        /// </summary>
        /// <returns></returns>
        [HttpGet("contributer-request")]
        public async Task<IEnumerable<ContributorRequestReadDTO>> GetPendingContributorRequests()
        {
            return _mapper.Map<List<ContributorRequestReadDTO>>(await _context.ContributorRequests.ToListAsync());
        }

        /// <summary>
        /// Delete a contributor request
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("contributer-request/{id}")]
        public async Task<ActionResult> DeleteContributorRequest(int id)
        {
            ContributorRequest cr = await _context.ContributorRequests.FirstOrDefaultAsync(x => x.Id == id);
            _context.ContributorRequests.Remove(cr);
            await _context.SaveChangesAsync();

            return NoContent();
        }

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

        private static async Task<string> GetAccessTokenAsync()
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

        private async Task UpdateDBRolesAsync(string id, string role)
        {
            //Get user from database.
            User user = await _context.Users.FirstOrDefaultAsync(u => u.AuthId == id);
            //User user = await _context.Users.FirstOrDefaultAsync(u => u.Id == 1);

            if (user != null)
            {
                // Assign Auth0 Roles and make User an Administrator or Contributor in DB
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

        private static string GetJsonRoles()
        {
            var rolesBody = new Auth0RoleBody("Admin");
            return JsonConvert.SerializeObject(rolesBody); ;
        }

    }
}
