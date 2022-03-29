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
using MeFit_BE.Models.Domain;
using MeFit_BE.Models.Domain.UserDomain;

namespace MeFit_BE.Controllers
{
    [Route("api/exercise")]
    [ApiController]
    [Authorize]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    [ApiConventionType(typeof(MeFitConventions))]
    public class ExerciseController : ControllerBase
    {
        private readonly MeFitDbContext _context;
        private readonly IMapper _mapper;

        public ExerciseController(MeFitDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        /// <summary>
        /// Method fetches all exercises from the database.
        /// </summary>
        /// <returns>List of Exercises</returns>
        [HttpGet]
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
        [Authorize(Roles ="Contributor")]
        public async Task<ActionResult<ExerciseReadDTO>> PostExercise(ExerciseWriteDTO exerciseDTO)
        {
            if (!Helper.IsContributor(HttpContext)) { return Forbid(); }

            User user = await Helper.GetCurrentUser(HttpContext, _context);
            if (user == null) return BadRequest();

            if (!Category.IsValid(exerciseDTO.Category)) 
                return BadRequest($"Category {exerciseDTO.Category} is not valid.");

            Exercise domainExercise = _mapper.Map<Exercise>(exerciseDTO);
            domainExercise.ContributorId = user.Id;
            _context.Exercises.Add(domainExercise);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetExercise), new { Id = domainExercise.Id }, _mapper.Map<ExerciseReadDTO>(domainExercise));
        }

        /// <summary>
        /// Method updates an exercise in the database by id. Can only be updated by the
        /// contributor that owns the exercise.
        /// </summary>
        /// <param name="id">Exercise id</param>
        /// <param name="exerciseDTO">Exercise with new values</param>
        /// <returns>Updated exercise</returns>
        [HttpPatch("{id}")]
        [Authorize(Roles = "Contributor")]
        public async Task<IActionResult> PatchExercise(int id, [FromBody] ExerciseEditDTO exerciseDTO)
        {
            if (!Helper.IsContributor(HttpContext)) { return Forbid("The current user is not a contributor."); }

            // Get user and exercise
            User user = await Helper.GetCurrentUser(HttpContext, _context);
            if (user == null) return BadRequest();
            Exercise exercise = await GetExerciseAsync(id);
            if (exercise == null) return NotFound($"Exercise with Id: {id} was not found");

            //Check that the current user owns the exercise.
            if (exercise.Id != user.Id) 
                return Forbid("Tried to change an exercise not owned by the current user.");

            // Update Exercise
            if (exercise.Category != null) {
                if (!Category.IsValid(exerciseDTO.Category))
                    return BadRequest($"Category {exerciseDTO.Category} is not valid.");
                else exercise.Category = exerciseDTO.Category;
            }
            if (exerciseDTO.Name != null) exercise.Name = exerciseDTO.Name;
            if (exerciseDTO.Description != null) exercise.Description = exerciseDTO.Description;
            if (exerciseDTO.Image != null) exercise.Image = exerciseDTO.Image;
            if (exerciseDTO.Video != null) exercise.Video = exerciseDTO.Video;

            _context.Exercises.Update(exercise);
            await _context.SaveChangesAsync();

            return Ok(_mapper.Map<ExerciseReadDTO>(exercise));
        }

        /// <summary>
        /// Method deletes an exercise in the database by id. Can only be deleted by the 
        /// contributor who owns the exercise.
        /// </summary>
        /// <param name="id">Exercise id</param>
        /// <returns>No content</returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Contributor")]
        public async Task<ActionResult> DeleteExercise(int id)
        {
            if (!Helper.IsContributor(HttpContext)) { return Forbid(); }

            //Get user and exercise from database.
            User user = await Helper.GetCurrentUser(HttpContext, _context);
            if (user == null) { return BadRequest(); }
            if (!ExerciseExists(id))
                return NotFound($"Exercise with Id: {id} was not found");
            Exercise exercise = await GetExerciseAsync(id);
            
            //Check that the current user owns the exercise.
            if (exercise.Id != user.Id)
                return Forbid("Tried to change an exercise not owned by the current user.");

            //Delete the sets that belong to the exercise before deleting the exercise.
            List<Set> sets = exercise.Sets.ToList();
            foreach (Set set in sets)
            {
                _context.Sets.Remove(set);
            }
            _context.Exercises.Remove(exercise);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Method used to fetch the exercise with the given id from
        /// the database.
        /// </summary>
        /// <param name="exerciseId">Exercise id</param>
        /// <returns>Exercise</returns>
        private async Task<Exercise> GetExerciseAsync(int exerciseId) 
        {
            return await _context.Exercises.Include(e => e.Sets)
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
