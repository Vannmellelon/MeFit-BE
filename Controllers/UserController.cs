using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;
using AutoMapper;
using MeFit_BE.Models;
using MeFit_BE.Models.Domain.UserDomain;
using MeFit_BE.Models.DTO;
using MeFit_BE.Models.DTO.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MeFit_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    [ApiConventionType(typeof(DefaultApiConventions))]
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
            return _mapper.Map<List<UserReadDTO>>(await _context.Users.ToListAsync());
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
            return _mapper.Map<UserReadDTO>(await _context.Users.FindAsync(id));
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
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetUser", new { Id = user.Id }, _mapper.Map<UserReadDTO>(user));
        }

        // PATCH api/<UserController>/5
        /// <summary>
        /// Method updates a user in the database.
        /// </summary>
        /// <param name="id">User id</param>
        /// <param name="userDTO">User with updated values</param>
        /// <returns>Updated user</returns>
        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(int id, [FromBody] UserEditDTO userDTO)
        {
            //Get user.
            User user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();

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
        /// </summary>
        /// <param name="id">User id</param>
        /// <returns>No content</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
