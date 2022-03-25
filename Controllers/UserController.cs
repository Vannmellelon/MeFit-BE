using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;
using AutoMapper;
using MeFit_BE.Models;
using MeFit_BE.Models.Domain;
using MeFit_BE.Models.Domain.UserDomain;
using MeFit_BE.Models.DTO;
using MeFit_BE.Models.DTO.Profile;
using MeFit_BE.Models.DTO.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MeFit_BE.Controllers
{
    [Route("api/user")]
    [ApiController]
    [Authorize]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    [ApiConventionType(typeof(MeFitConventions))]
    public class UserController : Controller
    {

        private readonly MeFitDbContext _context;
        private readonly IMapper _mapper;

        public UserController(MeFitDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Redirects to the currently logged in user.
        /// </summary>
        /// <returns>User object of currently logged in user.</returns>
        [HttpGet]
        public async Task<ActionResult<UserReadDTO>> RedirectUser()
        {
            User user = await Helper.GetCurrentUser(HttpContext, _context);
            if (user == null) return BadRequest();

            string host = HttpContext.Request.Host.ToUriComponent();
            string path = HttpContext.Request.Path.ToUriComponent();

            //HttpContext.Response.StatusCode = 303;
            //HttpContext.Response.Headers.Add("Location", $"https://{host}{path}{user.Id}");

            // Returns 302, not 303, but it's fine ^^
            return Redirect($"https://{host}{path}{user.Id}");

        }

        /// <summary>
        /// Method fetches all users from the database.
        /// </summary>
        /// <returns>List of users</returns>
        [HttpGet("all-users")]
        public async Task<ActionResult<List<UserReadDTO>>> GetUsers()
        {
            return _mapper.Map<List<UserReadDTO>>(await _context.Users.Include(u => u.Goals).ToListAsync());
        }

        /// <summary>
        /// Admin only, fetches all users from the database.
        /// </summary>
        /// <returns>List of users</returns>
        [HttpGet("admin/all-users")]
        [Authorize(Roles ="Admin")]
        public async Task<ActionResult<List<UserAdminReadDTO>>> GetUsersAdmin()
        {
            return _mapper.Map<List<UserAdminReadDTO>>(await _context.Users.Include(u => u.Goals).ToListAsync());
        }


        /// <summary>
        /// Method fetches a specific user form the database.
        /// </summary>
        /// <param name="id">User id</param>
        /// <returns>User</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<UserReadDTO>> GetUser(int id)
        {
            User user = await _context.Users.Include(u => u.Goals).FirstOrDefaultAsync(u => u.Id == id);
            if (user == null) return BadRequest();

            return _mapper.Map<UserReadDTO>(user);
        }


        /// <summary>
        /// Method creates a new user.
        /// </summary>
        /// <param name="userDTO">New user</param>
        /// <returns>New user</returns>
        [HttpPost]
        public async Task<ActionResult<UserReadDTO>> PostUser([FromBody] UserWriteDTO userDTO)
        {
            // Validates custom-type parameters
            if (userDTO == null) return BadRequest();
            if (!Difficulty.IsValid(userDTO.FitnessLevel)) return BadRequest("Please enter a valid difficulty-category." + userDTO.FitnessLevel + " is not valid.");
            foreach (var res in userDTO.RestrictedCategories.Split(","))
            {
                // no category should be allowed
                if (!Category.IsValid(res)) return BadRequest("Please enter a valid exercise-category. " + res + " is not valid.");
            }

            User user = _mapper.Map<User>(userDTO);
            user.AuthId = Helper.GetExternalUserProviderId(HttpContext);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUser), new { Id = user.Id }, _mapper.Map<UserReadDTO>(user));
        }


        /// <summary>
        /// Allows a user to submit a request to become a contributor.
        /// User must already be registered in the database to submit this request.
        /// </summary>
        /// <returns></returns>
        [HttpPost("contributer-request")]
        public async Task<ActionResult> PostRequestContributor()
        {
            // Check if user is in database, submit request for contributor-status.
            User user = await Helper.GetCurrentUser(HttpContext, _context);
            if (user == null) return BadRequest();

            ContributorRequest cr = new ContributorRequest();
            cr.RequestingUser = user;

            _context.ContributorRequests.Add(cr);
            await _context.SaveChangesAsync();

            return Ok();
        }


        /// <summary>
        /// Method updates a user in the database. 
        /// A user can only de altered by themselves.
        /// </summary>
        /// <param name="id">User id</param>
        /// <param name="userDTO">User with updated values</param>
        /// <returns>Updated user</returns>
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchUser(int id, [FromBody] UserEditDTO userDTO)
        {
            // Get user from database
            User user = await Helper.GetCurrentUser(HttpContext, _context);
            if (user == null) return BadRequest();

            // Ensure current user is the user being changed
            if (user.Id != id) return Forbid();

            // Update user
            if (userDTO.FirstName != null) user.FirstName = userDTO.FirstName;
            if (userDTO.LastName != null) user.LastName = userDTO.LastName;

            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(_mapper.Map<UserReadDTO>(user));
        }


        /// <summary>
        /// Method deletes the user with the given id.
        /// A user can only be deleted by themselves or an administrator.
        /// </summary>
        /// <param name="id">User id</param>
        /// <returns>No content</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            // Get user from database
            User user = await _context.Users.FindAsync(id);
            if (user == null) return BadRequest();

            // Ensure current user is admin OR the one being deleted
            if (user.Id != id && !Helper.IsAdmin(HttpContext)) return Forbid();

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }


        /// <summary>
        /// Method returns the profile belonging to the user with the given id.
        /// If the user does not have a profile, the method will return not found.
        /// </summary>
        /// <param name="id">User id</param>
        /// <returns>Profile</returns>
        [HttpGet("{id}/profile")]
        public async Task<ActionResult<ProfileReadDTO>> GetUserProfile(int id)
        {
            Models.Domain.UserDomain.Profile profile = await _context.Profiles.FirstOrDefaultAsync(p => p.UserId == id);
            if (profile == null) return NotFound();

            return Ok(_mapper.Map<ProfileReadDTO>(profile));
        }


        // TODO:
        // Add request to auth0 managementAPI, to sync user-info

        /// <summary>
        /// Method makes the user with the given id into an administrator.
        /// Only administrators can make a user an administrator.
        /// </summary>
        /// <param name="id">User id</param>
        /// <returns>Action result</returns>
        [HttpPatch("{id}/admin")]
        public async Task<IActionResult> MakeAdmin(int id)
        {
            if (!Helper.IsAdmin(HttpContext)) return Forbid();

            //Get user from database.
            User user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null) return NotFound();

            //Make user an administrator.
            user.IsAdmin = true;
            _context.Entry(user).State = EntityState.Modified;
            _context.SaveChanges();

            return Ok();
        }


        /// <summary>
        /// Method makes the user with the given id into a contributor.
        /// Only administrators can make a user a contributor.
        /// </summary>
        /// <param name="id">User id</param>
        /// <returns>Action result</returns>
        [HttpPatch("{id}/contributor")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> MakeContributor(int id)
        {
            if (!Helper.IsAdmin(HttpContext)) return Forbid();

            //Get user from database.
            User user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null) return NotFound();

            //Make user a contributor.
            user.IsContributor = true;
            _context.Entry(user).State = EntityState.Modified;
            _context.SaveChanges();

            return Ok();
        }
    }
}
