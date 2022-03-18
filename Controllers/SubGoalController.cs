using AutoMapper;
using MeFit_BE.Models;
using MeFit_BE.Models.Domain.GoalDomain;
using MeFit_BE.Models.DTO.SubGoal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;


namespace MeFit_BE.Controllers
{
    [Route("api/sub-goal")]
    [ApiController]
    [Authorize]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    [ApiConventionType(typeof(DefaultApiConventions))]
    public class SubGoalController : ControllerBase
    {
        private readonly MeFitDbContext _context;
        private readonly IMapper _mapper;

        public SubGoalController(MeFitDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Method fetches all SubGoals from the database.
        /// </summary>
        /// <returns>SubGoals</returns>
        [HttpGet]
        public async Task<IEnumerable<SubGoalReadDTO>> Get()
        {
            return _mapper.Map<List<SubGoalReadDTO>>(await _context.SubGoals.ToListAsync());
        }

        /// <summary>
        /// Method fetches a specific SubGoal from the database by id.
        /// </summary>
        /// <param name="id">SubGoal id</param>
        /// <returns>SubGoal</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<SubGoalReadDTO>> Get(int id)
        {
            return _mapper.Map<SubGoalReadDTO>(await GetSubGoalAsync(id));
        }

        /// <summary>
        /// Method adds a new SubGoal to the database.
        /// </summary>
        /// <param name="subGoalDTO">New SubGoal</param>
        /// <returns>New SubGoal</returns>
        [HttpPost]
        public async Task<SubGoalReadDTO> Post(SubGoalWriteDTO subGoalDTO) 
        {
            var domainSubGoal = _mapper.Map<SubGoal>(subGoalDTO);
            _context.SubGoals.Add(domainSubGoal);
            await _context.SaveChangesAsync();

            return _mapper.Map<SubGoalReadDTO>(domainSubGoal);
        }


        /// <summary>
        /// Method updates a SubGoal in the database by id
        /// </summary>
        /// <param name="id">SubGoal id</param>
        /// <param name="subGoalDTO">SubGoal with new values</param>
        /// <returns>Updated SubGoal</returns>
        [HttpPatch("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] SubGoalEditDTO subGoalDTO)
        {
            // Get Excerise
            var subGoal = await GetSubGoalAsync(id);
            if (subGoal == null) return NotFound();

            // Update SubGoal
            subGoal.Achieved = subGoalDTO.Achieved;

            _context.SubGoals.Update(subGoal);
            await _context.SaveChangesAsync();

            return Ok(subGoal);
        }

        /// <summary>
        /// Method deletes a SubGoal in the database by id.
        /// </summary>
        /// <param name="id">SubGoal id</param>
        /// <returns>No content</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            if (!SubGoalExists(id))
                return NotFound();

            var subGoal = await GetSubGoalAsync(id);

            _context.SubGoals.Remove(subGoal);
            await _context.SaveChangesAsync();

            return Ok();
        }

        /*
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="workoutId"></param>
        /// <returns>Updated SubGoal</returns>
        [HttpPatch("{id}/workout")]
        public async Task<IActionResult> Patch(int id, int workoutId) 
        {
            if (!SubGoalExists(id)) return NotFound();

            var subGoal = await GetSubGoalAsync(id);
            try
            {
                var workout = await _context.Workouts.FindAsync(workoutId);
                subGoal.Workout = workout ??
                    throw new KeyNotFoundException($"Record of Workout with id: {workoutId} does not exist");
                subGoal.WorkoutId = workoutId;
                await _context.SaveChangesAsync();
            }
            catch (KeyNotFoundException e)
            {
                Console.WriteLine(e.Message);
                return BadRequest();
            }
            return Ok(subGoal);
        }*/

        /// <summary>
        /// Method used to fetch the SubGoal with the given id from the database.
        /// </summary>
        /// <param name="subGoalId">SubGoal id</param>
        /// <returns>SubGoal</returns>
        private async Task<SubGoal> GetSubGoalAsync(int subGoalId)
        {
            return await _context.SubGoals
                .SingleOrDefaultAsync(sg => sg.Id == subGoalId);
        }

        /// <summary>
        /// Method checks if there is a SubGoal with the given id in the database.
        /// </summary>
        /// <param name="id">Exercise id</param>
        /// <returns>Boolean</returns>
        private bool SubGoalExists(int id)
        {
            return _context.SubGoals.Any(sg => sg.Id == id);
        }
    }
}
