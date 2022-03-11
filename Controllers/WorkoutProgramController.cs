using AutoMapper;
using MeFit_BE.Models;
using MeFit_BE.Models.Domain.Workout;
using MeFit_BE.Models.DTO.WorkoutProgram;
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
            return _mapper.Map<List<WorkoutProgramReadDTO>>(await _context.WorkoutPrograms.ToListAsync());
        }

        /// <summary>
        /// Method fetches a specific program from the database by id.
        /// </summary>
        /// <param name="id">Program id</param>
        /// <returns>Program</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<WorkoutProgramReadDTO>> Get(int id)
        {
            return _mapper.Map<WorkoutProgramReadDTO>(await GetProgramAsync(id));
        }

        /// <summary>
        /// Method adds a new program to the database.
        /// </summary>
        /// <param name="program">New program values</param>
        /// <returns>New program</returns>
        [HttpPost]
        public async Task<ActionResult<WorkoutProgramReadDTO>> Post(WorkoutProgramWriteDTO programDTO)  
        {
            WorkoutProgram domainProgram = _mapper.Map<WorkoutProgram>(programDTO);
            _context.WorkoutPrograms.Add(domainProgram);
            await _context.SaveChangesAsync();
            return CreatedAtAction("Get", new {Id = domainProgram.Id}, _mapper.Map<WorkoutProgramReadDTO>(domainProgram));
        }

        /// <summary>
        /// Method updates a workout program in the database by id;
        /// must pass in an updated workout program object
        /// </summary>
        /// <param name="id">Workout program id</param>
        /// <param name="program">Workout program with new values</param>
        /// <returns>Updated workout program</returns>
        [HttpPatch("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] WorkoutProgramEditDTO programDTO)
        {
            // Get WorkoutProgram
            var program = await GetProgramAsync(id);
            if (program == null) return NotFound();

            // Update WorkoutProgram
            if (programDTO.Name != null) program.Name = programDTO.Name;
            if (programDTO.Category != null) program.Category = programDTO.Category;

            _context.WorkoutPrograms.Update(program);
            await _context.SaveChangesAsync();

            return Ok(program);
        }

        /// <summary>
        /// Method deletes a workout program in the database by id.
        /// </summary>
        /// <param name="id">Workout program id</param>
        [HttpDelete("{id}")]
        public async Task<ActionResult<WorkoutProgram>> Delete(int id)
        {
            if (!ProgramExists(id))
                return NotFound($"Program with Id: {id} was not found");

            var program = await _context.WorkoutPrograms.FindAsync(id);

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
