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
    public class GoalController : ControllerBase
    {
        private readonly MeFitDbContext _context;

        public GoalController(MeFitDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Fetches all Goals from the database
        /// </summary>
        /// <returns>List og Goals</returns>
        [HttpGet]
        public async Task<IEnumerable<Goal>> Get()
        {
            return await _context.Goals.ToListAsync();
        }

        /// <summary>
        /// Fetches a specific Goal from the database by id 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Goal>> Get(int id)
        {
            return await GetGoalAsync(id);
        }

        // POST api/<GoalController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<GoalController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<GoalController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
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
