using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;
using MeFit_BE.Models;
using MeFit_BE.Models.Domain.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MeFit_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    [ApiConventionType(typeof(DefaultApiConventions))]
    public class UserController : Controller
    {

        private readonly MeFitDbContext _context;

        public UserController(MeFitDbContext context)
        {
            _context = context;
        }

        // GET: UserController
        [HttpGet]
        public async Task<ActionResult<List<User>>> Get()
        {
            return await _context.Users.ToListAsync();
        }

        // GET api/<UserController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> Get(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        // POST api/<UserController>/
        [HttpPost]
        public async Task<User> PostAsync([FromBody] string username)
        {
            User user = new User() { Username = username };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            //Endre til CreatedAtAction
            return user;
        }

        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // GET: UserController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }
    }
}
