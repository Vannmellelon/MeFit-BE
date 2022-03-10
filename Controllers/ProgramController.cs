using MeFit_BE.Models;
using MeFit_BE.Models.Domain.Workout;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace MeFit_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProgramController : ControllerBase
    {
        private readonly MeFitDbContext _context;

        public ProgramController(MeFitDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Fetches all Programs from the database
        /// </summary>
        /// <returns>List of Programs</returns>
        [HttpGet]
        public async Task<IEnumerable<WorkoutProgram>> Get()
        {
            return await _context.WorkoutPrograms.ToListAsync();
        }

        /// <summary>
        /// Fetches a specific Program from the database by id 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Program</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<WorkoutProgram>> Get(int id)
        {
            return await GetProgramAsync(id);
        }

        /// <summary>
        /// Adds a new Program to the database
        /// NOTE: JUST IGNORE THE ID FIELD
        /// </summary>
        /// <param name="program"></param>
        /// <returns>Program</returns>
        [HttpPost]
        public async Task<WorkoutProgram> Post(WorkoutProgram program)  
        {
            var domainProgram = new WorkoutProgram() { Name = program.Name, Category = program.Category };
            _context.WorkoutPrograms.Add(domainProgram);
            await _context.SaveChangesAsync();
            return domainProgram; 
        }

        /// <summary>
        /// Updates a Program in the database by id;
        /// must pass in an updated Program object
        /// </summary>
        /// <param name="id"></param>
        /// <param name="program"></param>
        /// <returns>Updated Program</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, WorkoutProgram program)
        {
            if (id != program.Id) 
                return BadRequest("Invalid Program Id");

            if (!ProgramExists(id)) 
                return NotFound($"Program with Id: {id} was not found");

            var domainProgram = new WorkoutProgram() { Id = id, Name = program.Name, Category = program.Category };

            _context.Entry(domainProgram).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok($"Updated Program with id: {id}");
        }

        /// <summary>
        /// Deletes a Program in the database by id
        /// </summary>
        /// <param name="id"></param>
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

        private async Task<WorkoutProgram> GetProgramAsync(int programId)
        {
            return await _context.WorkoutPrograms
                .SingleOrDefaultAsync(p => p.Id == programId);
        }

        private bool ProgramExists(int id)
        {
            return _context.WorkoutPrograms.Any(p => p.Id == id);
        }
    }
}
