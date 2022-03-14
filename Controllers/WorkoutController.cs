using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using AutoMapper;
using MeFit_BE.Models;
using MeFit_BE.Models.Domain.WorkoutDomain;
using MeFit_BE.Models.DTO.Workout;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MeFit_BE.Controllers
{
    [Route("api/workout")]
    [ApiController]
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
        /// GET: Returns the workout with the specified id.
        /// </summary>
        /// <param name="id">Id of workout.</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<WorkoutReadDTO>> GetWorkout(int id)
        {
            if (!workoutExists(id))
            {
                return NotFound($"Can not find workout with id: {id}");
            }
            else
            {
                Workout _domainWorkout = await _context.Workouts.FindAsync(id);
                return _mapper.Map<WorkoutReadDTO>(_domainWorkout);
            }
        }

        /// <summary>
        /// POST: Create a new workout.
        /// Workout must be in application/JSON format.
        /// </summary>
        /// <param name="newWorkout">New workout object</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<WorkoutReadDTO>> PostWorkout(WorkoutWriteDTO newWorkout)
        {
            Workout _domainWorkout = _mapper.Map<Workout>(newWorkout);
            _context.Add(_domainWorkout);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetWorkout), new { id = _domainWorkout.Id }, _mapper.Map<WorkoutReadDTO>(_domainWorkout));
        }

        /// <summary>
        /// PUT: Replace a workout with a new one.
        /// Can be used to modify an existing workout.
        /// Pass id of workout to update and updated workout object.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updatedWorkout"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ActionResult<WorkoutReadDTO>> PutWorkout(int id, WorkoutEditDTO updatedWorkout)
        {
            if (!workoutExists(id))
            {
                return NotFound($"Can not find workout with id: {id}");
            }

            Workout _domainWorkout = _mapper.Map<Workout>(updatedWorkout);
            _context.Entry(_domainWorkout).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            //?
            return Ok(_mapper.Map<WorkoutReadDTO>(_domainWorkout));
        }

        // TODO:
        // update to JSON PATCH
        // add checks for contributor
        /// <summary>
        /// PATCH: Update existing workout.
        /// Can only be updated by contributor.
        /// </summary>
        /// <param name="id">Id of workout to update</param>
        /// <param name="updatedWorkout">Workout object with partial updates.</param>
        /// <returns></returns>
        [HttpPatch("{id}")]
        public async Task<ActionResult<WorkoutReadDTO>> PatchWorkout(int id, WorkoutEditDTO updatedWorkout)
        {
            // Check if current user is contributor
            // Check if current user is owner/can change stuff

            if (!workoutExists(id))
            {
                return NotFound($"Can not find workout with id: {id}");
            }

            Workout _domainWorkout = await _context.Workouts.FindAsync(id);
            if (_domainWorkout == null) { return NotFound(); } // redundant?

            if (updatedWorkout.Name != null) { _domainWorkout.Name = updatedWorkout.Name; }
            if (updatedWorkout.Type != null) { _domainWorkout.Type = updatedWorkout.Type; }
            if (updatedWorkout.Complete != null) { _domainWorkout.Complete = updatedWorkout.Complete; } // gah
            //if (updatedWorkout.SetId != null) { _domainWorkout.SetId = updatedWorkout.SetId; }
            
            _context.Entry(_domainWorkout).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok(_mapper.Map<WorkoutReadDTO>(_domainWorkout));
        }



        // DELETE: Delete existing workout.
        // Can only be deleted by contributor.
        [HttpDelete]
        public async Task<ActionResult> DeleteWorkout(int id) 
        {
            // Check if current user is contributor
            // Check if current user is owner/can change stuff

            if (!workoutExists(id))
            {
                return NotFound($"Can not find workout with id: {id}");
            }
            Workout _domainWorkout = await _context.Workouts.FindAsync(id);
            _context.Remove(_domainWorkout);
            await _context.SaveChangesAsync();

            return Ok($"Successfully deleted workout with id: {id}.");
        }

        // TODO private method for checking contributor access.

        // Check if a workout with the given id exists.
        private bool workoutExists(int id)
        {
            return _context.Workouts.Any(w => w.Id == id);
        }
    }
}
