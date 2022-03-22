using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;
using AutoMapper;
using MeFit_BE.Models;
using MeFit_BE.Models.Domain.UserDomain;
using MeFit_BE.Models.DTO;
using MeFit_BE.Models.DTO.Profile;
using MeFit_BE.Models.DTO.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MeFit_BE.Controllers
{
    [Route("api/user")]
    [ApiController]
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

        // GET: UserController
        /// <summary>
        /// Method fetches all users from the database.
        /// </summary>
        /// <returns>List of users</returns>
        [HttpGet]
        public async Task<ActionResult<List<UserReadDTO>>> GetUsers()
        {
            return _mapper.Map<List<UserReadDTO>>(await _context.Users.Include(u => u.Goals).ToListAsync());
        }

        // GET api/<UserController>/5
        /// <summary>
        /// Method fetches a specific user form the database.
        /// </summary>
        /// <param name="id">User id</param>
        /// <returns>User</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<UserReadDTO>> GetUser(int id)
        {
            User user = await _context.Users.Include(u => u.Goals).FirstOrDefaultAsync(u => u.Id == id);
            if (user == null) return NotFound();
            return _mapper.Map<UserReadDTO>(user);
        }

        // POST api/<UserController>/
        /// <summary>
        /// Method creates a new user.
        /// </summary>
        /// <param name="userDTO">New user</param>
        /// <returns>New user</returns>
        [HttpPost]
        public async Task<ActionResult<UserReadDTO>> PostAsync([FromBody] UserWriteDTO userDTO)
        {
            User user = _mapper.Map<User>(userDTO);
            user.AuthId = Helper.GetExternalUserProviderId(HttpContext);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetUser", new { Id = user.Id }, _mapper.Map<UserReadDTO>(user));
        }

        // PATCH api/<UserController>/5
        /// <summary>
        /// Method updates a user in the database. 
        /// A user can only de altered by themselves.
        /// </summary>
        /// <param name="id">User id</param>
        /// <param name="userDTO">User with updated values</param>
        /// <returns>Updated user</returns>
        [HttpPatch("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Patch(int id, [FromBody] UserEditDTO userDTO)
        {
            //Get user from database.
            User user = await Helper.GetCurrentUser(HttpContext, _context);
            if (user == null) return NotFound();

            //Ensure current user is the user being changed.
            if (user.Id != id) return Forbid();

            //Update user.
            if (userDTO.Email != null) user.Email = userDTO.Email;
            if (userDTO.FirstName != null) user.FirstName = userDTO.FirstName;
            if (userDTO.LastName != null) user.LastName = userDTO.LastName;

            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok(_mapper.Map<UserReadDTO>(user));
        }

        // GET: UserController/Delete/5
        /// <summary>
        /// Method deletes the user with the given id.
        /// A user can only be deleted by themselves or an administrator.
        /// </summary>
        /// <param name="id">User id</param>
        /// <returns>No content</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete(int id)
        {
            //Get user from database.
            User user = await Helper.GetCurrentUser(HttpContext, _context);
            if (user == null) return NotFound();

            //Ensure current user is the one being deleted.
            if (user.Id != id || !Helper.IsAdmin(HttpContext)) return Forbid();

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
