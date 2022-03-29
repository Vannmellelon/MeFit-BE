﻿using AutoMapper;
using MeFit_BE.Models;
using MeFit_BE.Models.Domain.GoalDomain;
using MeFit_BE.Models.Domain.UserDomain;
using MeFit_BE.Models.DTO.SubGoal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    [ApiConventionType(typeof(MeFitConventions))]
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
        public async Task<IEnumerable<SubGoalReadDTO>> GetSubgoal()
        {
            return _mapper.Map<List<SubGoalReadDTO>>(await _context.SubGoals.ToListAsync());
        }


        /// <summary>
        /// Method fetches a specific SubGoal from the database by id.
        /// </summary>
        /// <param name="id">SubGoal id</param>
        /// <returns>SubGoal</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<SubGoalReadDTO>> GetSubGoal(int id)
        {
            return _mapper.Map<SubGoalReadDTO>(await GetSubGoalAsync(id));
        }


        /// <summary>
        /// Method adds a new SubGoal to the database.
        /// </summary>
        /// <param name="subGoalDTO">New SubGoal</param>
        /// <returns>New SubGoal</returns>
        [HttpPost]
        public async Task<ActionResult<SubGoalReadDTO>> PostSubGoal(SubGoalWriteDTO subGoalDTO) 
        {
            SubGoal domainSubGoal = _mapper.Map<SubGoal>(subGoalDTO);
            _context.SubGoals.Add(domainSubGoal);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSubGoal), new {Id = domainSubGoal.Id }, _mapper.Map<SubGoalReadDTO>(domainSubGoal));
        }


        /// <summary>
        /// Method updates a SubGoal in the database by id. A sub-goal can only be changed 
        /// by the person who owns the goal the sub-goal is part of.
        /// </summary>
        /// <param name="id">SubGoal id</param>
        /// <param name="subGoalDTO">SubGoal with new values</param>
        /// <returns>Updated SubGoal</returns>
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchSubGoal(int id, [FromBody] SubGoalEditDTO subGoalDTO)
        {
            // Get sub-goal
            SubGoal subGoal = await GetSubGoalAsync(id);
            if (subGoal == null) return NotFound($"Could not find sub-goal with id {id}.");

            //Ensure current user owns the sub-goal.
            Goal goal = await _context.Goals.FindAsync(subGoal.GoalId);
            User user = await Helper.GetCurrentUser(HttpContext, _context);
            if (user == null) return BadRequest();
            if (goal.UserId != user.Id) return Forbid($"The current user does not own the sub-goal with id {id}.");

            // Update SubGoal
            subGoal.Achieved = subGoalDTO.Achieved;

            _context.SubGoals.Update(subGoal);
            await _context.SaveChangesAsync();

            return Ok(_mapper.Map<SubGoalReadDTO>(subGoal));
        }


        /// <summary>
        /// Method deletes a SubGoal in the database by id. A sub-goal can only be 
        /// deleted by its owner.
        /// </summary>
        /// <param name="id">SubGoal id</param>
        /// <returns>No content</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteSubGoal(int id)
        {
            if (!SubGoalExists(id))
                return NotFound($"Could not find sub-goal with id {id}");
            SubGoal subGoal = await GetSubGoalAsync(id);

            //Ensure the current user owns the sub-goal.
            Goal goal = await _context.Goals.FindAsync(subGoal.GoalId);
            User user = await Helper.GetCurrentUser(HttpContext, _context);
            if (user == null) return BadRequest();
            if (goal.UserId != user.Id) return Forbid($"The current user does not own the sub-goal with id {id}.");

            _context.SubGoals.Remove(subGoal);
            await _context.SaveChangesAsync();

            return NoContent();
        }


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
