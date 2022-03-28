using Auth0.AuthenticationApi;
using Auth0.AuthenticationApi.Models;
using AutoMapper;
using MeFit_BE.Models;
using MeFit_BE.Models.Domain.UserDomain;
using MeFit_BE.Models.DTO.ContributorRequest;
using MeFit_BE.Models.JSON;
using MeFit_BE.Services;
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
        private readonly MeFitDbContext _context;
        private readonly IAuth0Service _auth0Service; 
        private readonly IMapper _mapper;

        public AdminController(IAuth0Service auth0Service, MeFitDbContext context, IMapper mapper)
        {
            _auth0Service = auth0Service;
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
            return Ok(await _auth0Service.GetAccessTokenAsync());
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
            await _auth0Service.UpdateUserAsync(id, email, nickname);
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
            await _auth0Service.DeleteUserAsync(id);
            return NoContent();
        }

        /// <summary>
        /// Method assigns roles to User using Auth0 Manegement API and updates User in DB 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        [HttpPost("users/{id}/roles")]
        public async Task<IActionResult> PostUserRoles(string id, string role)
        {
            var body = new Auth0RoleBody(role);
            if (body.Roles == null) return BadRequest();

            await _auth0Service.UpdateUserRolesAsync(id, role, body);

            return Ok();
        }

        // Methods for contributor requests

        /// <summary>
        /// Method fetches all pending contributor requests.
        /// </summary>
        /// <returns></returns>
        [HttpGet("contributor-request")]
        public async Task<IEnumerable<ContributorRequestReadDTO>> GetPendingContributorRequests()
        {
            return _mapper.Map<List<ContributorRequestReadDTO>>(await _context.ContributorRequests.ToListAsync());
        }

        /// <summary>
        /// Method deletes a contributor request
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("contributor-request/{id}")]
        public async Task<ActionResult> DeleteContributorRequest(int id)
        {
            ContributorRequest cr = await _context.ContributorRequests.FirstOrDefaultAsync(x => x.Id == id);
            _context.ContributorRequests.Remove(cr);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /* 
        [HttpGet("users/{id}")]
        private async Task<IActionResult> GetUser(string id)
        {
            //var url = BASE_URL + $"users/{id}";
            //var response = await _client.GetStringAsync(url);
            var response = await _auth0Service.GetAccessTokenAsync();
            return Ok(response);
        }

        [HttpGet("users/{id}/roles")]
        private async Task<IActionResult> GetUserRoles(string id)
        {
            //var url = BASE_URL + $"users/{id}/roles";
            //var response = await _client.GetStringAsync(url);
            var response = await _auth0Service.GetAccessTokenAsync();
            return Ok(response);
        }
        */
    }
}
