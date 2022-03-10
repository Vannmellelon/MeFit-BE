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
    public class ExerciseController : ControllerBase
    {
        private readonly MeFitDbContext _context;

        public ExerciseController(MeFitDbContext context)
        {
            _context = context;
        }

        // GET: api/<ExerciseController>
        /// <summary>
        /// Fetches all Exercises from the database
        /// </summary>
        /// <returns>List of Exercises</returns>
        [HttpGet]
        public async Task<IEnumerable<Exercise>> Get()
        {
            return await _context.Exercises.ToListAsync();
        }

        /// <summary>
        /// Fetches a specific Exercise from the database by id 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Exercise</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Exercise>> Get(int id)
        {
            return await GetExerciseAsync(id);
        }

        /// <summary>
        /// Adds a new Excercise to the database
        /// NOTE: JUST IGNORE THE ID FIELD
        /// </summary>
        /// <param name="exercise"></param>
        /// <returns>Exercise</returns>
        [HttpPost]
        public async Task<Exercise> Post(Exercise exercise)
        {
            var domainExercise = new Exercise() { 
                Name = exercise.Name,
                Description = exercise.Description,
                TargetMuscleGroup = exercise.TargetMuscleGroup,
                Image = exercise.Image,
                Video = exercise.Video
            };
            _context.Exercises.Add(domainExercise);
            await _context.SaveChangesAsync();
            return domainExercise;
        }

        // PUT api/<ExerciseController>/5
        /// <summary>
        /// Updates an Exercise in the database by id;
        /// must pass in an updated Exercise object
        /// </summary>
        /// <param name="id"></param>
        /// <param name="exercise"></param>
        /// <returns>Updated Exercise</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Exercise exercise)
        {
            if (id != exercise.Id)
                return BadRequest("Invalid Exercise Id");

            if (!ExerciseExists(id))
                return NotFound($"Exercise with Id: {id} was not found");

            _context.Entry(exercise).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok($"Updated Exercise with id: {id}");
        }

        /// <summary>
        /// Deletes an Exercise in the database by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<Exercise>> Delete(int id)
        {
            if (!ExerciseExists(id))
                return NotFound($"Exercise with Id: {id} was not found");

            var exercise = await _context.Exercises.FindAsync(id);

            _context.Exercises.Remove(exercise);
            await _context.SaveChangesAsync();

            return Ok($"Deleted Exercise with Id: {id}");
        }

        private async Task<Exercise> GetExerciseAsync(int exerciseId) 
        {
            return await _context.Exercises
                .SingleOrDefaultAsync(e => e.Id == exerciseId);
        }

        private bool ExerciseExists(int id)
        {
            return _context.Exercises.Any(e => e.Id == id); 
        }
    }
}
