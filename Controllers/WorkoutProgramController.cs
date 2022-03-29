using AutoMapper;
using MeFit_BE.Models;
using MeFit_BE.Models.Domain.UserDomain;
using MeFit_BE.Models.Domain.WorkoutDomain;
using MeFit_BE.Models.DTO.WorkoutProgram;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using MeFit_BE.Models.Domain;
using MeFit_BE.Models.DTO.Workout;
using MeFit_BE.Models.Domain.GoalDomain;

namespace MeFit_BE.Controllers
{
    [Route("api/workout-program")]
    [ApiController]
    [Authorize]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    [ApiConventionType(typeof(MeFitConventions))]
    public class WorkoutProgramController : ControllerBase
    {
        private readonly MeFitDbContext _context;
        private readonly IMapper _mapper;

        public WorkoutProgramController(MeFitDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        /// <summary>
        /// Method fetches all programs from the database.
        /// </summary>
        /// <returns>List of Programs</returns>
        [HttpGet]
        public async Task<IEnumerable<WorkoutProgramReadDTO>> GetWorkoutPrograms()
        {
            return _mapper.Map<List<WorkoutProgramReadDTO>>(await _context.WorkoutPrograms.Include(wp => wp.Workouts).ToListAsync());
        }


        /// <summary>
        /// Method fetches a specific workout program from the database by id.
        /// </summary>
        /// <param name="id">Workout program id</param>
        /// <returns>WorkoutProgram</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<WorkoutProgramReadDTO>> GetWorkoutProgram(int id)
        {
            WorkoutProgram workoutProgram = await _context.WorkoutPrograms.Include(wp => wp.Workouts).FirstOrDefaultAsync(wp => wp.Id == id);
            if (workoutProgram == null) return NotFound($"Could not find workout program with id {id}.");
            return _mapper.Map<WorkoutProgramReadDTO>(workoutProgram);
        }


        /// <summary>
        /// Method adds a new workout program to the database. Only a contributor can create new workout programs.
        /// </summary>
        /// <param name="programDTO">New workout program values</param>
        /// <returns>New workout program</returns>
        [HttpPost]
        [Authorize(Roles ="Contributor")]
        public async Task<ActionResult<WorkoutProgramReadDTO>> PostWorkoutProgram(WorkoutProgramWriteDTO programDTO)  
        {
            if (!Helper.IsContributor(HttpContext)) 
                return Forbid($"Only contributors can create new workout programs.");

            //Find current user in the database.
            User user = await Helper.GetCurrentUser(HttpContext, _context);
            if (user == null) return BadRequest();

            //Check input validity.
            if (!Category.IsValid(programDTO.Category)) 
                return BadRequest($"{programDTO.Category} is not a valid category.");
            if (!Difficulty.IsValid(programDTO.Difficulty))
                return BadRequest($"{programDTO.Difficulty} is not a valid difficulty level.");

            //Create workout program with the current user as contributor.
            WorkoutProgram domainProgram = _mapper.Map<WorkoutProgram>(programDTO);
            domainProgram.ContributorId = user.Id;

            _context.WorkoutPrograms.Add(domainProgram);
            await _context.SaveChangesAsync();
            return CreatedAtAction("Get", new {Id = domainProgram.Id}, _mapper.Map<WorkoutProgramReadDTO>(domainProgram));
        }


        /// <summary>
        /// Method updates a workout program in the database by id;
        /// must pass in an updated workout program object. Only the contributor of the 
        /// workout program can change it.
        /// </summary>
        /// <param name="id">Workout program id</param>
        /// <param name="programDTO">Workout program with new values</param>
        /// <returns>Updated workout program</returns>
        [HttpPatch("{id}")]
        [Authorize(Roles = "Contributor")]
        public async Task<IActionResult> PatchWorkoutProgram(int id, [FromBody] WorkoutProgramEditDTO programDTO)
        {
            if (!Helper.IsContributor(HttpContext)) 
                return Forbid("Only contributors can edit workout programs.");

            // Get workout program and current user from database.
            WorkoutProgram program = await GetProgramAsync(id);
            if (program == null) return NotFound($"Could not find workout program with id {id}.");
            User user = await Helper.GetCurrentUser(HttpContext, _context);
            if (user == null) return BadRequest();

            //Ensure current contributor owns the workout program.            
            if (program.ContributorId != user.Id) 
                return Forbid($"Tried to change workout program {id}, which is not owned by the current user.");

            // Update WorkoutProgram
            if (programDTO.Category != null)
            {
                if (!Category.IsValid(programDTO.Category))
                    return BadRequest($"{programDTO.Category} is not a valid category.");
                else program.Category = programDTO.Category;
            }
            if (programDTO.Difficulty != null)
            {
                if (!Difficulty.IsValid(programDTO.Difficulty))
                    return BadRequest($"{programDTO.Difficulty} is not a valid difficulty level.");
                else program.Difficulty = programDTO.Difficulty;
            }
            if (programDTO.Name != null) program.Name = programDTO.Name;

            _context.WorkoutPrograms.Update(program);
            await _context.SaveChangesAsync();

            return Ok(program);
        }


        /// <summary>
        /// Method deletes a workout program in the database by id. Only the
        /// contributor of the workout program can delete it.
        /// </summary>
        /// <param name="id">Workout program id</param>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Contributor")]
        public async Task<ActionResult<WorkoutProgram>> DeleteWorkoutProgram(int id)
        {
            if (!Helper.IsContributor(HttpContext)) 
                return Forbid("Only contributors can delete workout programs.");

            //Get workout program and current user form database.
            if (!ProgramExists(id))
                return NotFound($"Program with Id: {id} was not found");
            WorkoutProgram program = await _context.WorkoutPrograms.FindAsync(id);
            User user = await Helper.GetCurrentUser(HttpContext, _context);
            if (user == null) return BadRequest();

            //Ensure current contributor owns the workout program.
            if (program.Id != user.Id) 
                return Forbid($"Tried to delete workout program {id}, which is not owned by the current user.");

            //Remove goals that rely on the workout program before deleting the program.
            List<Goal> goals = await _context.Goals.Where(g => g.WorkoutProgramId == id).ToListAsync();
            foreach (Goal goal in goals)
            {
                _context.Goals.Remove(goal);
            }

            _context.WorkoutPrograms.Remove(program);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Method adds the workout with the given id to the given workout program.
        /// Only contributors can add a workout to a workout program.
        /// </summary>
        /// <param name="programId">WorkoutProgram id</param>
        /// <param name="workoutId">Workout id</param>
        /// <returns>WorkoutProgram</returns>
        [HttpPatch("{id}/workouts/{workoutId}")]
        public async Task<IActionResult> PatchAddWorkoutToProgram(int programId, int workoutId)
        {
            if (!Helper.IsContributor(HttpContext)) 
                return Forbid("Only contributors can add workouts to workout programs.");

            WorkoutProgram workoutPogram = await _context.WorkoutPrograms
                .Include(wp => wp.Workouts).SingleOrDefaultAsync(wp => wp.Id == programId);
            if (workoutPogram == null) return NotFound($"Could not find workout program with id {programId}.");
            Workout workout = await _context.Workouts.FindAsync(workoutId);
            if (workout == null) return NotFound($"Could not find workout with id {workoutId}.");

            workoutPogram.Workouts.Add(workout);
            _context.SaveChanges();
            return Ok(_mapper.Map<WorkoutProgramReadDTO>(workoutPogram));
        }

        /// <summary>
        /// Method returns the workout program with the given id.
        /// </summary>
        /// <param name="programId">Workout program id</param>
        /// <returns>Workout program</returns>
        private async Task<WorkoutProgram> GetProgramAsync(int programId)
        {
            return await _context.WorkoutPrograms
                .SingleOrDefaultAsync(p => p.Id == programId);
        }


        /// <summary>
        /// Method checks if a workout program with the given id exists 
        /// in the database.
        /// </summary>
        /// <param name="id">Workout program id</param>
        /// <returns>Boolean</returns>
        private bool ProgramExists(int id)
        {
            return _context.WorkoutPrograms.Any(p => p.Id == id);
        }
    }
}
