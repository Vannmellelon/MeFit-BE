using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using AutoMapper;
using MeFit_BE.Models;
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
    [Authorize]
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
        public async Task<ActionResult<WorkoutReadDTO>> PostWorkout(WorkoutWriteDTO newWorkout)
        {
            // Check if current user is contributor

            Workout _domainWorkout = _mapper.Map<Workout>(newWorkout);
            _context.Add(_domainWorkout);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetWorkout), new { id = _domainWorkout.Id }, _mapper.Map<WorkoutReadDTO>(_domainWorkout));
        }


        // TODO:
        // update to JSON PATCH
        // add checks for contributor
        /// <summary>
        /// Update existing workout. Requires the id of the workout.
        /// Can only be updated by contributor.
        /// </summary>
        /// <param name="id">Id of workout to update</param>
        /// <param name="updatedWorkout">Workout object with partial updates.</param>
        /// <returns>Updated workout</returns>
        [HttpPatch("{id}")]
        public async Task<ActionResult<WorkoutReadDTO>> PatchWorkout(int id, WorkoutEditDTO updatedWorkout)
        {
            // Check if current user is contributor
            // Check if current user is owner/can change stuff

            if (!WorkoutExists(id))
            {
                return NotFound($"Can not find workout with id: {id}");
            }

            Workout _domainWorkout = await _context.Workouts.FindAsync(id);
            if (_domainWorkout == null) { return NotFound(); } 

            if (updatedWorkout.Name != null) { _domainWorkout.Name = updatedWorkout.Name; }
            if (updatedWorkout.Type != null) { _domainWorkout.Type = updatedWorkout.Type; }
            
            _context.Entry(_domainWorkout).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok(_mapper.Map<WorkoutReadDTO>(_domainWorkout));
        }


        // Can only be deleted by contributor/owner
        /// <summary>
        /// Delete a workout. Requires the id of the workout.
        /// </summary>
        /// <param name="id">Workout id</param>
        /// <returns>No content</returns>
        [HttpDelete]
        public async Task<ActionResult> DeleteWorkout(int id) 
        {
            // Check if current user is contributor
            // Check if current user is owner/can change stuff

            if (!WorkoutExists(id))
            {
                return NotFound($"Can not find workout with id: {id}");
            }
            Workout _domainWorkout = await _context.Workouts.FindAsync(id);
            _context.Remove(_domainWorkout);
            await _context.SaveChangesAsync();

            return Ok($"Successfully deleted workout with id: {id}.");
        }


        // TODO private method for checking contributor access.
        // TODO add endpoints for updating sets and subgoals, or do it via PATCH


        // Check if a workout with the given id exists.
        private bool WorkoutExists(int id)
        {
            return _context.Workouts.Any(w => w.Id == id);
        }
    }
}
