using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using AutoMapper;
using MeFit_BE.Models;
using MeFit_BE.Models.Domain.UserDomain;
using MeFit_BE.Models.Domain.WorkoutDomain;
using MeFit_BE.Models.DTO.Workout;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MeFit_BE.Models.Domain;
using MeFit_BE.Models.Domain.GoalDomain;

namespace MeFit_BE.Controllers
{
    [Route("api/workout")]
    [ApiController]
    [Authorize]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    [ApiConventionType(typeof(MeFitConventions))]
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
                return Ok(_mapper.Map<WorkoutReadDTO>(_domainWorkout));
            }
        }


        /// <summary>
        /// Adds a new workout to the database. Workout must be in application/JSON format. Only
        /// a contributor can create a new workout.
        /// </summary>
        /// <param name="newWorkout">New workout object</param>
        /// <returns>New workout</returns>
        [HttpPost]
        [Authorize(Roles="Contributor")]
        public async Task<ActionResult<WorkoutReadDTO>> PostWorkout(WorkoutWriteDTO newWorkout)
        {
            if (!Helper.IsContributor(HttpContext)) return Forbid();

            // Get user id of current user.
            User user = await Helper.GetCurrentUser(HttpContext, _context);
            if (user == null) return BadRequest();

            //Check input validity
            if (!Category.IsValid(newWorkout.Category))
                return BadRequest($"Category {newWorkout.Category} is invalid.");
            if (!Difficulty.IsValid(newWorkout.Difficulty))
                return BadRequest($"Difficulty {newWorkout.Difficulty} is invalid.");

            //Add contributor to workout.
            Workout domainWorkout = _mapper.Map<Workout>(newWorkout);
            domainWorkout.ContributorId = user.Id;

            _context.Add(domainWorkout);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetWorkout), new { id = domainWorkout.Id }, _mapper.Map<WorkoutReadDTO>(domainWorkout));
        }
        

        /// <summary>
        /// Update existing workout. Requires the id of the workout.
        /// Can only be updated by the workout's contributor.
        /// </summary>
        /// <param name="id">Id of workout to update</param>
        /// <param name="updatedWorkout">Workout object with partial updates.</param>
        /// <returns>Updated workout</returns>
        [HttpPatch("{id}")]
        [Authorize(Roles = "Contributor")]
        public async Task<ActionResult<WorkoutReadDTO>> PatchWorkout(int id, WorkoutEditDTO updatedWorkout)
        {
            if (!Helper.IsContributor(HttpContext)) return Forbid();

            //Find workout and user in database
            if (!WorkoutExists(id))
            { return NotFound($"Cannot find workout with id: {id}"); }
            Workout _domainWorkout = await _context.Workouts.FindAsync(id);

            User user = await Helper.GetCurrentUser(HttpContext, _context);
            if (user == null) { return BadRequest(); }

            //Ensure that current user is the contributor of the workout.
            if (_domainWorkout.ContributorId != user.Id) return Forbid();

            //Update workout
            if (updatedWorkout.Category != null)
            {
                if (!Category.IsValid(updatedWorkout.Category))
                    return BadRequest($"Category {updatedWorkout.Category} is invalid.");
                else _domainWorkout.Category = updatedWorkout.Category;
            }
            if (updatedWorkout.Difficulty != null)
            {
                if (!Difficulty.IsValid(updatedWorkout.Difficulty))
                    return BadRequest($"Difficulty {updatedWorkout.Difficulty} is invalid.");
                else _domainWorkout.Difficulty = updatedWorkout.Difficulty;
            }
            if (updatedWorkout.Name != null) { _domainWorkout.Name = updatedWorkout.Name; }
            
            _context.Entry(_domainWorkout).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok(_mapper.Map<WorkoutReadDTO>(_domainWorkout));
        }

        /// <summary>
        /// Deletes a workout. Requires the id of the workout. Can only be deleted by the 
        /// workout's contributor.
        /// </summary>
        /// <param name="id">Workout id</param>
        /// <returns>No content</returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Contributor")]
        public async Task<ActionResult> DeleteWorkout(int id) 
        {
            if (!Helper.IsContributor(HttpContext)) return Forbid();

            //Find workout and current user in database.
            if (!WorkoutExists(id))
            {
                return NotFound($"Can not find workout with id: {id}");
            }
            Workout _domainWorkout = await _context.Workouts.FindAsync(id);
            User user = await Helper.GetCurrentUser(HttpContext, _context);
            if (user == null) return BadRequest();

            //Ensure current contributor owns the workout.
            if (_domainWorkout.ContributorId != user.Id) return Forbid();

            //Remove all sub-goals that rely on the workout before removing the workout.
            List<SubGoal> subGoals = await _context.SubGoals.Where(s => s.WorkoutId == id).ToListAsync();
            foreach (SubGoal subGoal in subGoals)
            {
                _context.SubGoals.Remove(subGoal);
            }

            _context.Remove(_domainWorkout);
            await _context.SaveChangesAsync();
            return NoContent();
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
    }
}
