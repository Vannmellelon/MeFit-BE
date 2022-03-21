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


namespace MeFit_BE.Controllers
{
    [Route("api/workout-program")]
    [ApiController]
    [Authorize]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    [ApiConventionType(typeof(DefaultApiConventions))]
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
        public async Task<IEnumerable<WorkoutProgramReadDTO>> Get()
        {
            return _mapper.Map<List<WorkoutProgramReadDTO>>(await _context.WorkoutPrograms.Include(wp => wp.Workouts).ToListAsync());
        }

        /// <summary>
        /// Method fetches a specific workout program from the database by id.
        /// </summary>
        /// <param name="id">Workout program id</param>
        /// <returns>WorkoutProgram</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<WorkoutProgramReadDTO>> Get(int id)
        {
            WorkoutProgram workoutProgram = await _context.WorkoutPrograms.Include(wp => wp.Workouts).FirstOrDefaultAsync(wp => wp.Id == id);
            if (workoutProgram == null) return NotFound();
            return _mapper.Map<WorkoutProgramReadDTO>(workoutProgram);
        }

        /// <summary>
        /// Method adds a new workout program to the database. Only a contributor can create new workout programs.
        /// </summary>
        /// <param name="programDTO">New workout program values</param>
        /// <returns>New workout program</returns>
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(403)]
        public async Task<ActionResult<WorkoutProgramReadDTO>> Post(WorkoutProgramWriteDTO programDTO)  
        {
            if (!Helper.IsContributor(HttpContext)) return Forbid();

            //Find current user in the database.
            User user = await Helper.GetCurrentUser(HttpContext, _context);
            if (user == null) return NotFound();

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
        /// workout program can delete it.
        /// </summary>
        /// <param name="id">Workout program id</param>
        /// <param name="programDTO">Workout program with new values</param>
        /// <returns>Updated workout program</returns>
        [HttpPatch("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(403)]
        public async Task<IActionResult> Put(int id, [FromBody] WorkoutProgramEditDTO programDTO)
        {
            if (!Helper.IsContributor(HttpContext)) return Forbid();

            // Get workout program and current user from database.
            WorkoutProgram program = await GetProgramAsync(id);
            User user = await Helper.GetCurrentUser(HttpContext, _context);
            if (user == null || program == null) return NotFound();

            //Ensure current contributor owns the workout program.            
            if (program.ContributorId != user.Id) return Forbid();

            // Update WorkoutProgram
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
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(403)]
        public async Task<ActionResult<WorkoutProgram>> Delete(int id)
        {
            if (Helper.IsContributor(HttpContext)) return Forbid();

            //Get workout program and current user form database.
            if (!ProgramExists(id))
                return NotFound($"Program with Id: {id} was not found");
            WorkoutProgram program = await _context.WorkoutPrograms.FindAsync(id);
            User user = await Helper.GetCurrentUser(HttpContext, _context);
            if (user == null) return NotFound();

            //Ensure current contributor owns the workout program.
            if (program.Id != user.Id) return Forbid();

            _context.WorkoutPrograms.Remove(program);
            await _context.SaveChangesAsync();
            return Ok($"Deleted Program with Id: {id}");
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
