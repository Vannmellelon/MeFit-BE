using AutoMapper;
using MeFit_BE.Models;
using MeFit_BE.Models.Domain.UserDomain;
using MeFit_BE.Models.DTO.ContributorRequest;
using MeFit_BE.Models.JSON;
using MeFit_BE.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Net.Mime;
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
        /// <returns>Auth0 access token</returns>
        [HttpGet("auth0")]
        public async Task<IActionResult> GetToken()
        {
            return Ok(await _auth0Service.GetAccessTokenAsync());
        }

        /// <summary>
        /// Method Updates User using Auth0 Manegement API and updates User in DB
        /// </summary>
        /// <param name="id">User id</param>
        /// <param name="email">User email</param>
        /// <param name="nickname">User nickname</param>
        /// <returns>Ok</returns>
        [HttpPatch("users/{id}")]
        public async Task<IActionResult> PatchUser(int id, string email, string nickname) 
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user.AuthId == null) return NotFound();

            // /api/v2/tickets/password-change
            await _auth0Service.UpdateUserAsync(user.AuthId, email, nickname);
            return Ok();
        }

        /// <summary>
        /// Method Deletes User from Auth0 and DB using Auth0 Manegement API and MeFit DB Context
        /// </summary>
        /// <param name="id">User id</param>
        /// <returns>No content</returns>
        [HttpDelete("users/{id}")]
        public async Task<IActionResult> DeleteUser(int id) 
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user.AuthId == null) return NotFound();

            await _auth0Service.DeleteUserAsync(user.AuthId);
            return NoContent();
        }

        /// <summary>
        /// Method assigns roles to User using Auth0 Manegement API and updates User in DB 
        /// </summary>
        /// <param name="id">User id</param>
        /// <param name="role">User role</param>
        /// <returns>Ok</returns>
        [HttpPost("users/{id}/{role}")]
        public async Task<IActionResult> PostUserRoles(int id, string role)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user.AuthId == null) return NotFound();

            var body = new Auth0RoleBody(role);
            if (body.Roles == null) return BadRequest();

            await _auth0Service.UpdateUserRolesAsync(user.AuthId, role, body);

            return Ok();
        }

        // Methods for contributor requests

        /// <summary>
        /// Method fetches all pending contributor requests.
        /// </summary>
        /// <returns>List of contributor requests</returns>
        [HttpGet("contributor-request")]
        public async Task<IEnumerable<ContributorRequestReadDTO>> GetPendingContributorRequests()
        {
            return _mapper.Map<List<ContributorRequestReadDTO>>(await _context.ContributorRequests.ToListAsync());
        }

        /// <summary>
        /// Method deletes a contributor request
        /// </summary>
        /// <param name="id">Contributor request id</param>
        /// <returns>No content</returns>
        [HttpDelete("contributor-request/{id}")]
        public async Task<ActionResult> DeleteContributorRequest(int id)
        {
            ContributorRequest cr = await _context.ContributorRequests.FirstOrDefaultAsync(x => x.Id == id);
            _context.ContributorRequests.Remove(cr);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
