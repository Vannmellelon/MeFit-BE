using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using MeFit_BE.Models;
using MeFit_BE.Models.Domain.UserDomain;
using MeFit_BE.Models.Domain.WorkoutDomain;
using MeFit_BE.Models.DTO.Workout;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MeFit_BE.Controllers
{
    [Route("api/workout")]
    [ApiController]
    //[Authorize]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    [ApiConventionType(typeof(DefaultApiConventions))]
    public class WorkoutController : Controller
    {

        private readonly MeFitDbContext _context;
        private readonly IMapper _mapper;

        public WorkoutController(MeFitDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        /// <summary>
        /// Returns a list of all workouts in the database.
        /// </summary>
        /// <returns>Workout</returns>
        [HttpGet]
        public async Task<ActionResult<List<WorkoutReadDTO>>> GetAllWorkouts() 
        {
            return _mapper.Map <List<WorkoutReadDTO>> (await _context.Workouts.Include(w => w.Sets).ToListAsync());
        } 


        /// <summary>
        /// Returns the workout with the specified id.
        /// </summary>
        /// <param name="id">Id of workout</param>
        /// <returns>Workout</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<WorkoutReadDTO>> GetWorkout(int id)
        {
            if (!WorkoutExists(id))
            {
                return NotFound($"Can not find workout with id: {id}");
            }
            else
            {
                Workout _domainWorkout = await _context.Workouts.Include(w => w.Sets).FirstOrDefaultAsync(w => w.Id == id);
                return _mapper.Map<WorkoutReadDTO>(_domainWorkout);
            }
        }


        /// <summary>
        /// Add a new workout to the database.
        /// Workout must be in application/JSON format.
        /// </summary>
        /// <param name="newWorkout">New workout object</param>
        /// <returns>New workout</returns>
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(403)]
        public async Task<ActionResult<WorkoutReadDTO>> PostWorkout(WorkoutWriteDTO newWorkout)
        {
            if (!IsContributor()) return Forbid();

            GetExternalUserProviderId();

            Workout _domainWorkout = _mapper.Map<Workout>(newWorkout);

            // Get user id of current user.
            //User user = await _context.Users.FirstOrDefaultAsync(u => u.AuthId == GetExternalUserProviderId());

            //Add contributor to workout.
            //_domainWorkout.ContributorId = user.Id;

            _context.Add(_domainWorkout);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetWorkout), new { id = _domainWorkout.Id }, _mapper.Map<WorkoutReadDTO>(_domainWorkout));
        }

        /// <summary>
        /// Update existing workout. Requires the id of the workout.
        /// Can only be updated by contributor.
        /// </summary>
        /// <param name="id">Id of workout to update</param>
        /// <param name="updatedWorkout">Workout object with partial updates.</param>
        /// <returns>Updated workout</returns>
        [HttpPatch("{id}")]
        [ProducesResponseType(403)]
        public async Task<ActionResult<WorkoutReadDTO>> PatchWorkout(int id, WorkoutEditDTO updatedWorkout)
        {
            if (!IsContributor()) return Forbid();

            if (!WorkoutExists(id))
            {
                return NotFound($"Can not find workout with id: {id}");
            }

            Workout _domainWorkout = await _context.Workouts.FindAsync(id);
            if (_domainWorkout == null) { return NotFound(); }

            //Get id of current user.
            //User user = GetCurrentUser();

            //Ensure that current user is the contributor of the workout.
            //if (_domainWorkout.ContributorId != user.Id) return Forbid();

            if (updatedWorkout.Name != null) { _domainWorkout.Name = updatedWorkout.Name; }
            if (updatedWorkout.Type != null) { _domainWorkout.Type = updatedWorkout.Type; }
            
            _context.Entry(_domainWorkout).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok(_mapper.Map<WorkoutReadDTO>(_domainWorkout));
        }


        // Can only be deleted by contributor/owner
        /// <summary>
        /// Deletes a workout. Requires the id of the workout.
        /// </summary>
        /// <param name="id">Workout id</param>
        /// <returns>No content</returns>
        [HttpDelete]
        [ProducesResponseType(403)]
        public async Task<ActionResult> DeleteWorkout(int id) 
        {
            if (!IsContributor()) return Forbid();

            // TODO: Check if current user is owner/can change stuff

            if (!WorkoutExists(id))
            {
                return NotFound($"Can not find workout with id: {id}");
            }
            Workout _domainWorkout = await _context.Workouts.FindAsync(id);
            _context.Remove(_domainWorkout);
            await _context.SaveChangesAsync();

            return Ok($"Successfully deleted workout with id: {id}.");
        }


        /// <summary>
        /// Returns true if the current user is a contributor.
        /// </summary>
        /// <returns>Boolean</returns>
        private bool IsContributor()
        {
            return HttpContext.User.IsInRole("Contributor");
        }

        /// <summary>
        /// Returns true if there is a workout with the given id in the database.
        /// </summary>
        /// <param name="id">Workout id</param>
        /// <returns>Boolean</returns>
        private bool WorkoutExists(int id)
        {
            return _context.Workouts.Any(w => w.Id == id);
        }

        /// <summary>
        /// Returns the token id of the current user.
        /// </summary>
        /// <returns>String</returns>
        private string GetExternalUserProviderId()
        {
            return HttpContext.User.Identity.Name.ToString();
        }
        /*
        /// <summary>
        /// Gets the current user based on the token user id.
        /// </summary>
        /// <returns>User</returns>
        private User GetCurrentUser()
        {
            User user = _context.Users.FirstOrDefault(u => u.AuthId == GetExternalUserProviderId());
            return user;
        }*/
    }
}
