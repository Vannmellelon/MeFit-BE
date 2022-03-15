using AutoMapper;
using MeFit_BE.Models;
using MeFit_BE.Models.Domain.WorkoutDomain;
using MeFit_BE.Models.DTO.Goal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeFit_BE.Controllers
{
    [Route("api/goal")]
    [ApiController]
    public class GoalController : ControllerBase
    {
        private readonly MeFitDbContext _context;
        private readonly IMapper _mapper;

        public GoalController(MeFitDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Method fetches all goals from the database.
        /// </summary>
        /// <returns>All goals</returns>
        [HttpGet]
        public async Task<IEnumerable<GoalReadDTO>> GetGoals()
        {
            return _mapper.Map<List<GoalReadDTO>>(await _context.Goals.ToListAsync());
        }

        /// <summary>
        /// Method fetches a specific goal from the database by id.
        /// </summary>
        /// <param name="id">Goal id</param>
        /// <returns>Goal</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<GoalReadDTO>> GetGoal(int id)
        {
            return _mapper.Map<GoalReadDTO>(await GetGoalAsync(id));
        }

        /// <summary>
        /// Method adds a new goal to the database.
        /// </summary>
        /// <param name="goalDTO">New goal</param>
        /// <returns>New Goal</returns>
        [HttpPost]
        public async Task<GoalReadDTO> Post(GoalWriteDTO goalDTO)
        {
            var domainGoal = _mapper.Map<Goal>(goalDTO);
            _context.Goals.Add(domainGoal);
            await _context.SaveChangesAsync();
            return _mapper.Map<GoalReadDTO>(domainGoal);
        }

        /// <summary>
        /// Method updates a goal in the database by id;
        /// must pass in an updated goal object
        /// </summary>
        /// <param name="id">Goal id</param>
        /// <param name="goalDTO">New goal properties</param>
        /// <returns>Updated goal</returns>
        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(int id, GoalEditDTO goalDTO)
        {
            // Find goal in database.
            Goal goal = await GetGoalAsync(id);
            if (goal == null) return NotFound();

            // Update goal
            goal.EndData = goalDTO.EndData;
            goal.Achieved = goalDTO.Achieved;

            _context.Goals.Update(goal);
            await _context.SaveChangesAsync();

            return Ok(goal);
        }

        /// <summary>
        /// Method deletes a goal in the database by id.
        /// </summary>
        /// <param name="id">Goal id</param>
        /// <returns>No content</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            if (!GoalExists(id))
                return NotFound($"Goal with Id: {id} was not found");

            Goal domainGoal = await GetGoalAsync(id);

            _context.Goals.Remove(domainGoal);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Method adds the workoutProgram with the given id to the goal with the given id.
        /// </summary>
        /// <param name="goalId">Goal id</param>
        /// <param name="workoutProgramId">WorkoutProgram id</param>
        /// <returns>Goal</returns>
        [HttpPatch("{goalId}/workoutprograms/{workoutProgramId}")]
        public async Task<ActionResult> AddWorkoutProgramToGoal(int goalId, int workoutProgramId)
        {
            Goal domainGoal = await _context.Goals.Include(g => g.WorkoutPrograms).FirstAsync(g => g.Id == goalId);
            if (domainGoal == null) return NotFound();
            WorkoutProgram workoutProgram = await _context.WorkoutPrograms.FindAsync(workoutProgramId);
            if (workoutProgram == null) return NotFound();

            domainGoal.WorkoutPrograms.Add(workoutProgram);
            await _context.SaveChangesAsync();
            return Ok(domainGoal.WorkoutPrograms);
        }

        /// <summary>
        /// Method checks if a goal with the given id exists in the database.
        /// </summary>
        /// <param name="id">Goal id</param>
        /// <returns>Boolean</returns>
        private bool GoalExists(int id)
        {
            return _context.Goals.Any(g => g.Id == id);
        }

        /// <summary>
        /// Method gets the goal with the given id from the database.
        /// </summary>
        /// <param name="goalId">Goal id</param>
        /// <returns>Goal</returns>
        private async Task<Goal> GetGoalAsync(int goalId)
        {
            return await _context.Goals
                .SingleOrDefaultAsync(g => g.Id == goalId);
        }
    }
}
