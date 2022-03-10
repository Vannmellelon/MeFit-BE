﻿using AutoMapper;
using MeFit_BE.Models;
using MeFit_BE.Models.Domain.Workout;
using MeFit_BE.Models.DTO.Goal;
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
        /// Method fetches all Goals from the database
        /// </summary>
        /// <returns>Goals</returns>
        [HttpGet]
        public async Task<IEnumerable<GoalReadDTO>> Get()
        {
            return _mapper.Map<List<GoalReadDTO>>(await _context.Goals.ToListAsync());
        }

        /// <summary>
        /// Method fetches a specific Goal from the database by id 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Exercise</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<GoalReadDTO>> Get(int id)
        {
            return _mapper.Map<GoalReadDTO>(await GetGoalAsync(id));
        }

        /// <summary>
        /// Method adds a new Goal to the database.
        /// </summary>
        /// <param name="goalDTO"></param>
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
        /// Method updates a Goal in the database by id;
        /// must pass in an updated Goal object
        /// </summary>
        /// <param name="id"></param>
        /// <param name="goalDTO"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, GoalEditDTO goalDTO)
        {
            if (id != goalDTO.Id)
                return BadRequest("Invalid Goal Id");

            if (!GoalExists(id))
                return NotFound($"Goal with Id: {id} was not found");

            var domainGoal = _mapper.Map<Goal>(goalDTO);
            _context.Entry(domainGoal).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok($"Updated Goal with id: {id}");
        }

        /// <summary>
        /// Method deletes an exercise in the database by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            if (!GoalExists(id))
                return NotFound($"Goal with Id: {id} was not found");

            var domainGoal = await GetGoalAsync(id);

            _context.Goals.Remove(domainGoal);
            await _context.SaveChangesAsync();

            return Ok($"Deleted Goal with Id: {id}");
        }

        private async Task<Goal> GetGoalAsync(int goalId)
        {
            return await _context.Goals
                .SingleOrDefaultAsync(g => g.Id == goalId);
        }

        private bool GoalExists(int id)
        {
            return _context.Goals.Any(g => g.Id == id);
        }
    }
}