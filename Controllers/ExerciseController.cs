using AutoMapper;
using MeFit_BE.Models;
using MeFit_BE.Models.Domain.WorkoutDomain;
using MeFit_BE.Models.DTO.Exercise;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;


namespace MeFit_BE.Controllers
{
    [Route("api/exercise")]
    [ApiController]
    //[Authorize]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    [ApiConventionType(typeof(DefaultApiConventions))]
    public class ExerciseController : ControllerBase
    {
        private readonly MeFitDbContext _context;
        private readonly IMapper _mapper;

        public ExerciseController(MeFitDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/<ExerciseController>
        /// <summary>
        /// Method fetches all exercises from the database.
        /// </summary>
        /// <returns>List of Exercises</returns>
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IEnumerable<ExerciseReadDTO>> GetExercises()
        {
            return _mapper.Map<List<ExerciseReadDTO>>(await _context.Exercises.ToListAsync());
        }

        /// <summary>
        /// Method fetches a specific exercise from the database by id.
        /// </summary>
        /// <param name="id">Exercise id</param>
        /// <returns>Exercise</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ExerciseReadDTO>> GetExercise(int id)
        {
            return _mapper.Map<ExerciseReadDTO>(await GetExerciseAsync(id));
        }

        /// <summary>
        /// Method adds a new excercise to the database.
        /// </summary>
        /// <param name="exerciseDTO">New exercise</param>
        /// <returns>New exercise</returns>
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        //[Authorize(Roles = "Contributor")]
        public async Task<ActionResult<ExerciseReadDTO>> PostExercise(ExerciseWriteDTO exerciseDTO)
        {
            if (!Helper.IsContributor(HttpContext)) { return Forbid(); }

            // TODO:
            // Get id of current user via helper method for extracting authID from token, match to userId in db
            // Add UserId to new exercise

            Exercise domainExercise = _mapper.Map<Exercise>(exerciseDTO);
            _context.Exercises.Add(domainExercise);
            await _context.SaveChangesAsync();
            return _mapper.Map<ExerciseReadDTO>(domainExercise);
        }

        /// <summary>
        /// Method updates an exercise in the database by id
        /// </summary>
        /// <param name="id">Exercise id</param>
        /// <param name="exerciseDTO">Exercise with new values</param>
        /// <returns>Updated exercise</returns>
        [HttpPatch("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        //[Authorize(Roles = "Contributor")]
        public async Task<IActionResult> PatchExercise(int id, [FromBody] ExerciseEditDTO exerciseDTO)
        {
            if (!Helper.IsContributor(HttpContext)) { return Forbid(); }

            // Get Excerise
            var exercise = await GetExerciseAsync(id);
            if (exercise == null) return NotFound($"Exercise with Id: {id} was not found");

            // Update Exercise
            if (exerciseDTO.Name != null) exercise.Name = exerciseDTO.Name;
            if (exerciseDTO.Description != null) exercise.Description = exerciseDTO.Description;
            if (exerciseDTO.Image != null) exercise.Image = exerciseDTO.Image;
            if (exerciseDTO.Name != null) exercise.Video = exerciseDTO.Video;

            _context.Exercises.Update(exercise);
            await _context.SaveChangesAsync();

            return Ok(exercise);
        }

        /// <summary>
        /// Method deletes an exercise in the database by id.
        /// </summary>
        /// <param name="id">Exercise id</param>
        /// <returns>No content</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        //[Authorize(Roles = "Contributor")]
        public async Task<ActionResult> DeleteExercise(int id)
        {
            if (!Helper.IsContributor(HttpContext)) { return Forbid(); }

            if (!ExerciseExists(id))
                return NotFound($"Exercise with Id: {id} was not found");

            var exercise = await GetExerciseAsync(id);

            _context.Exercises.Remove(exercise);
            await _context.SaveChangesAsync();

            return Ok($"Deleted Exercise with Id: {id}");
        }

        /// <summary>
        /// Method used to fetch the exercise with the given id from
        /// the database.
        /// </summary>
        /// <param name="exerciseId">Exercise id</param>
        /// <returns>Exercise</returns>
        private async Task<Exercise> GetExerciseAsync(int exerciseId) 
        {
            return await _context.Exercises
                .SingleOrDefaultAsync(e => e.Id == exerciseId);
        }

        /// <summary>
        /// Method checks if there is an exercise with the given
        /// id in the database.
        /// </summary>
        /// <param name="id">Exercise id</param>
        /// <returns>Boolean</returns>
        private bool ExerciseExists(int id)
        {
            return _context.Exercises.Any(e => e.Id == id); 
        }
    }
}
