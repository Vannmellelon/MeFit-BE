using AutoMapper;
using MeFit_BE.Models;
using MeFit_BE.Models.Domain.GoalDomain;
using MeFit_BE.Models.Domain.UserDomain;
using MeFit_BE.Models.Domain.WorkoutDomain;
using MeFit_BE.Models.DTO.Goal;
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
    [Route("api/goal")]
    [ApiController]
    [Authorize]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    [ApiConventionType(typeof(MeFitConventions))]
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
            return _mapper.Map<List<GoalReadDTO>>(await _context.Goals.Include(g => g.SubGoals).ToListAsync());
        }


        /// <summary>
        /// Method fetches a specific goal from the database by id.
        /// </summary>
        /// <param name="id">Goal id</param>
        /// <returns>Goal</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<GoalReadDTO>> GetGoal(int id)
        {
            Goal goal = await _context.Goals.Include(g => g.SubGoals).FirstOrDefaultAsync(g => g.Id == id);
            if (goal == null) return NotFound();
            return _mapper.Map<GoalReadDTO>(goal);
        }


        /// <summary>
        /// Adds a new goal to the database.
        /// Creates necessary subgoals based on the provided workout-program.
        /// </summary>
        /// <param name="newGoal">New goal</param>
        /// <returns>New Goal</returns>
        [HttpPost]
        public async Task<ActionResult<GoalReadDTO>> PostGoal(GoalWriteDTO newGoal)
        {
            User user = await Helper.GetCurrentUser(HttpContext, _context);
            if (user == null) return BadRequest();

            // Find goal
            Goal domainGoal = _mapper.Map<Goal>(newGoal);
            domainGoal.UserId = user.Id;

            //Find workout program
            WorkoutProgram program = await _context.WorkoutPrograms.Include(wp => wp.Workouts).FirstOrDefaultAsync(wp => wp.Id == newGoal.WorkoutProgramId);
            if (program == null) return BadRequest("Cannot find workout-program with id: " + newGoal.WorkoutProgramId);
            WorkoutProgramReadDTO wpRdto = _mapper.Map<WorkoutProgramReadDTO>(program);

            //Save goal to database
            _context.Goals.Add(domainGoal);
            await _context.SaveChangesAsync();

            // Make subgoals for workouts within workout program.
            foreach (int wid in wpRdto.Workouts)
            {
                SubGoal _subGoal = new SubGoal()
                {
                    WorkoutId = wid,
                    GoalId = domainGoal.Id
                };
                _context.SubGoals.Add(_subGoal);
                await _context.SaveChangesAsync();
            }

            return CreatedAtAction(nameof(GetGoal), new { id = domainGoal.Id }, _mapper.Map<GoalReadDTO>(domainGoal));
        }


        /// <summary>
        /// Method updates a goal in the database by id;
        /// must pass in an updated goal object. Goal can only be updated by the
        /// user that owns it.
        /// </summary>
        /// <param name="id">Goal id</param>
        /// <param name="goalDTO">New goal properties</param>
        /// <returns>Updated goal</returns>
        [HttpPatch("{id}")]
        public async Task<ActionResult> Patch(int id, GoalEditDTO goalDTO)
        {
            // Find goal and current user.
            Goal goal = await GetGoalAsync(id);
            if (goal == null) return NotFound();
            User user = await Helper.GetCurrentUser(HttpContext, _context);
            if (user == null) return BadRequest();

            //Ensure the current user owns the goal.
            if (goal.UserId != user.Id) 
                return Forbid($"Current user does not own the goal with id {goal.Id}.");

            // Update goal
            goal.EndData = goalDTO.EndData;
            goal.Achieved = goalDTO.Achieved;
            _context.Goals.Update(goal);
            await _context.SaveChangesAsync();

            return Ok(goal);
        }


        /// <summary>
        /// Method deletes a goal in the database by id. 
        /// Only the user that owns the goal can delete it.
        /// </summary>
        /// <param name="id">Goal id</param>
        /// <returns>No content</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            if (!GoalExists(id))
                return NotFound($"Goal with Id: {id} was not found");
            Goal domainGoal = await GetGoalAsync(id);

            //Ensure the current user owns the goal.
            User user = await Helper.GetCurrentUser(HttpContext, _context);
            if (user == null) return BadRequest();
            if (user.Id != domainGoal.Id) 
                return Forbid($"The current user does not own the goal with id {domainGoal.Id}.");

            _context.Goals.Remove(domainGoal);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// Method adds a workout to a goal by adding it as one of the
        /// goal's sub-goal. A goal can only be altered by its owner.
        /// </summary>
        /// <param name="goalId">Goal id</param>
        /// <param name="workoutId">Workout id</param>
        /// <returns>Goal</returns>
        [HttpPatch("{id}/workouts/{workoutId}")]
        public async Task<ActionResult<GoalReadDTO>> PatchWorkoutToGoal(int goalId, int workoutId)
        {
            Goal goal = await GetGoalAsync(goalId); 
            if (goal == null) return NotFound($"Could not find goal with id {goalId}.");

            //Ensure the current user owns the goal.
            User user = await Helper.GetCurrentUser(HttpContext, _context);
            if (user == null) return BadRequest();
            if (user.Id != goal.Id)
                return Forbid($"The current user does not own the goal with id {goalId}.");

            SubGoal subGoal = new SubGoal()
            {
                WorkoutId = workoutId,
                GoalId = goal.Id
            };
            _context.SubGoals.Add(subGoal);
            await _context.SaveChangesAsync();
            return Ok(_mapper.Map<GoalReadDTO>(goal));
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
