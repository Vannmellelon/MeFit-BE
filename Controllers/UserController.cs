using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using AutoMapper;
using MeFit_BE.Models;
using MeFit_BE.Models.Domain;
using MeFit_BE.Models.Domain.GoalDomain;
using MeFit_BE.Models.Domain.UserDomain;
using MeFit_BE.Models.Domain.WorkoutDomain;
using MeFit_BE.Models.DTO;
using MeFit_BE.Models.DTO.Exercise;
using MeFit_BE.Models.DTO.Profile;
using MeFit_BE.Models.DTO.User;
using MeFit_BE.Models.DTO.Workout;
using MeFit_BE.Models.DTO.WorkoutProgram;
using MeFit_BE.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MeFit_BE.Controllers
{
    [Route("api/user")]
    [ApiController]
    [Authorize]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    [ApiConventionType(typeof(MeFitConventions))]
    public class UserController : Controller
    {
        private readonly MeFitDbContext _context;
        private readonly IAuth0Service _auth0Service;
        private readonly IMapper _mapper;

        public UserController(MeFitDbContext context, IAuth0Service auth0Service, IMapper mapper)
        {
            _context = context;
            _auth0Service = auth0Service;
            _mapper = mapper;
        }

        /// <summary>
        /// Redirects to the currently logged in user.
        /// </summary>
        /// <returns>User object of currently logged in user.</returns>
        [HttpGet]
        public async Task<ActionResult<UserReadDTO>> RedirectUser()
        {
            User user = await Helper.GetCurrentUser(HttpContext, _context);
            if (user == null) return BadRequest();

            string host = HttpContext.Request.Host.ToUriComponent();
            string path = HttpContext.Request.Path.ToUriComponent();

            return Redirect($"https://{host}{path}{user.Id}");

        }

        /// <summary>
        /// Method fetches all users from the database.
        /// </summary>
        /// <returns>List of users</returns>
        [HttpGet("all-users")]
        public async Task<ActionResult<List<UserReadDTO>>> GetUsers()
        {
            return _mapper.Map<List<UserReadDTO>>(await _context.Users.Include(u => u.Goals).ToListAsync());
        }

        /// <summary>
        /// Admin only, fetches all users from the database.
        /// </summary>
        /// <returns>List of users</returns>
        [HttpGet("admin/all-users")]
        [Authorize(Roles ="Admin")]
        public async Task<ActionResult<List<UserAdminReadDTO>>> GetUsersAdmin()
        {
            return _mapper.Map<List<UserAdminReadDTO>>(await _context.Users.Include(u => u.Goals).ToListAsync());
        }


        /// <summary>
        /// Method fetches a specific user form the database.
        /// </summary>
        /// <param name="id">User id</param>
        /// <returns>User</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<UserReadDTO>> GetUser(int id)
        {
            User user = await _context.Users.Include(u => u.Goals).FirstOrDefaultAsync(u => u.Id == id);
            if (user == null) return BadRequest();

            return _mapper.Map<UserReadDTO>(user);
        }


        /// <summary>
        /// Method creates a new user.
        /// </summary>
        /// <param name="userDTO">New user</param>
        /// <returns>New user</returns>
        [HttpPost]
        public async Task<ActionResult<UserReadDTO>> PostUser([FromBody] UserWriteDTO userDTO)
        {
            // Validates custom-type parameters
            if (userDTO == null) return BadRequest();
            if (!Difficulty.IsValid(userDTO.FitnessLevel)) return BadRequest("Please enter a valid difficulty-category." + userDTO.FitnessLevel + " is not valid.");
            foreach (var res in userDTO.RestrictedCategories.Split(","))
            {
                // no category should be allowed
                if (!Category.IsValid(res)) return BadRequest("Please enter a valid exercise-category. " + res + " is not valid.");
            }

            User user = _mapper.Map<User>(userDTO);
            user.AuthId = Helper.GetExternalUserProviderId(HttpContext);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUser), new { Id = user.Id }, _mapper.Map<UserReadDTO>(user));
        }


        /// <summary>
        /// Allows a user to submit a request to become a contributor.
        /// User must already be registered in the database to submit this request.
        /// </summary>
        /// <returns></returns>
        [HttpPost("contributer-request")]
        public async Task<ActionResult> PostRequestContributor()
        {
            // Check if user is in database, submit request for contributor-status.
            User user = await Helper.GetCurrentUser(HttpContext, _context);
            if (user == null) return BadRequest();

            ContributorRequest cr = new ContributorRequest();
            cr.RequestingUser = user;

            _context.ContributorRequests.Add(cr);
            await _context.SaveChangesAsync();

            return Ok();
        }


        /// <summary>
        /// Method updates a user in the database. 
        /// A user can only de altered by themselves.
        /// </summary>
        /// <param name="id">User id</param>
        /// <param name="userDTO">User with updated values</param>
        /// <returns>Updated user</returns>
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchUser(int id, [FromBody] UserEditDTO userDTO)
        {
            // Get user from database
            User user = await Helper.GetCurrentUser(HttpContext, _context);
            if (user == null) return BadRequest();

            // Ensure current user is the user being changed
            if (user.Id != id) return Forbid();

            // Update user
            if (userDTO.FirstName != null) user.FirstName = userDTO.FirstName;
            if (userDTO.LastName != null) user.LastName = userDTO.LastName;

            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            await _auth0Service.UpdateUserAsync(user.AuthId, userDTO.Email, user.FirstName);

            return Ok(_mapper.Map<UserReadDTO>(user));
        }


        /// <summary>
        /// Method deletes the user with the given id.
        /// A user can only be deleted by themselves or an administrator.
        /// </summary>
        /// <param name="id">User id</param>
        /// <returns>No content</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            // Get current user
            User user = await Helper.GetCurrentUser(HttpContext, _context);
            if (user == null) return BadRequest();

            // Ensure current user is admin OR the one being deleted.
            if (user.Id != id && !Helper.IsAdmin(HttpContext)) return Forbid();

            //Get the user to be deleted.
            User userToBeDeleted = await _context.Users.FindAsync(id);
            if (userToBeDeleted == null) return NotFound();

            //Delete the user's goals and sub-goals.
            List<Goal> goals = await _context.Goals.Include(g => g.SubGoals)
                .Where(g => g.UserId == id).ToListAsync();
            foreach (Goal goal in goals)
            {
                List<SubGoal> subGoals = goal.SubGoals.ToList();
                foreach (SubGoal subGoal in subGoals)
                {
                    subGoal.WorkoutId = null;
                    _context.SubGoals.Remove(subGoal);
                }
                _context.Goals.Remove(goal);
            }

            if (user.IsContributor)
            {
                //Delete goals and sub-goals that rely on objects by the contributor to be deleted.
                DeleteGoalsRelatedToContributor(id);
                
                //Delete exercises and sets owned by the contributor to avoid issues with 
                //cascading deletes on the Set table.
                List<Exercise> exercises = await _context.Exercises.Include(e => e.Sets)
                    .Where(e => e.ContributorId == id).ToListAsync();
                foreach (Exercise exercise in exercises)
                {
                    List<Set> sets = exercise.Sets.ToList();
                    foreach (Set set in sets)
                    {
                        Workout workout = await _context.Workouts.Include(w => w.Sets)
                            .FirstOrDefaultAsync(w => w.Id == set.WorkoutId);
                        workout.Sets.Remove(set);
                        _context.Sets.Remove(set);
                    }
                    _context.Exercises.Remove(exercise);
                }
            }

            _context.Users.Remove(userToBeDeleted);
            await _context.SaveChangesAsync();

            // Deletes user from Auth0
            if (user.AuthId != null) 
                await _auth0Service.DeleteUserAsync(user.AuthId);

            return NoContent();
        }

        /// <summary>
        /// Method returns the profile belonging to the user with the given id.
        /// If the user does not have a profile, the method will return not found.
        /// </summary>
        /// <param name="id">User id</param>
        /// <returns>Profile</returns>
        [HttpGet("{id}/profile")]
        public async Task<ActionResult<ProfileReadDTO>> GetUserProfile(int id)
        {
            Models.Domain.UserDomain.Profile profile = await _context.Profiles.FirstOrDefaultAsync(p => p.UserId == id);
            if (profile == null) return NotFound();

            return Ok(_mapper.Map<ProfileReadDTO>(profile));
        }


        // TODO:
        // Add request to auth0 managementAPI, to sync user-info

        /// <summary>
        /// Method makes the user with the given id into an administrator.
        /// Only administrators can make a user an administrator.
        /// </summary>
        /// <param name="id">User id</param>
        /// <returns>Action result</returns>
        [HttpPatch("{id}/admin")]
        public async Task<IActionResult> MakeAdmin(int id)
        {
            if (!Helper.IsAdmin(HttpContext)) return Forbid();

            //Get user from database.
            User user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null) return NotFound();

            //Make user an administrator.
            user.IsAdmin = true;
            _context.Entry(user).State = EntityState.Modified;
            _context.SaveChanges();

            return Ok();
        }

        /// <summary>
        /// Method makes the user with the given id into a contributor.
        /// Only administrators can make a user a contributor.
        /// </summary>
        /// <param name="id">User id</param>
        /// <returns>Action result</returns>
        [HttpPatch("{id}/contributor")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> MakeContributor(int id)
        {
            if (!Helper.IsAdmin(HttpContext)) return Forbid();

            //Get user from database.
            User user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null) return NotFound();

            //Make user a contributor.
            user.IsContributor = true;
            _context.Entry(user).State = EntityState.Modified;
            _context.SaveChanges();

            return Ok();
        }
        
        /// <summary>
        /// Method fetches all workout programs owned by the contributor with the given id.
        /// </summary>
        /// <param name="id">Contributor id</param>
        /// <returns>List of workout programs.</returns>
        [HttpGet("{id}/workout-programs")]
        public async Task<ActionResult<List<WorkoutProgramReadDTO>>> GetContributedPrograms(int id)
        {
            User user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null) return NotFound($"No user with id {id} exists.");
            if (user.IsContributor == false) return BadRequest($"User with id {id} is not a contributor.");

            return Ok(_mapper.Map<List<WorkoutProgramReadDTO>>(
                await _context.WorkoutPrograms.Where(wp => wp.ContributorId == id).ToListAsync())
                );
        }
    
        /// <summary>
        /// Method returns all workouts owned by the contributor with the given id.
        /// </summary>
        /// <param name="id">Contributor id</param>
        /// <returns>List of workouts</returns>
        [HttpGet("{id}/workouts")]
        public async Task<ActionResult<List<WorkoutReadDTO>>> GetContributedWorkouts(int id)
        {
            User user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null) return NotFound($"No user with id {id} exists.");
            if (user.IsContributor == false) return BadRequest($"User with id {id} is not a contributor.");

            return Ok(_mapper.Map<List<WorkoutReadDTO>>(
                await _context.Workouts.Include(w => w.Sets).Where(wp => wp.ContributorId == id).ToListAsync())
                );
        }

        /// <summary>
        /// Method returns all exercises owned by the contributor with the given id.
        /// </summary>
        /// <param name="id">Contributor id</param>
        /// <returns>List of exercises.</returns>
        [HttpGet("{id}/exercises")]
        public async Task<ActionResult<List<ExerciseReadDTO>>> GetContributedExercises(int id)
        {
            User user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null) return NotFound($"No user with id {id} exists.");
            if (user.IsContributor == false) return BadRequest($"User with id {id} is not a contributor.");

            return Ok(_mapper.Map<List<ExerciseReadDTO>>(
                await _context.Exercises.Where(e => e.ContributorId == id).ToListAsync())
                );
        }

        /// <summary>
        /// Method deletes the goals and sub-goals that rely on workouts and
        /// workout programs made by the contributor with the given id.
        /// </summary>
        /// <param name="id">Contributor id</param>
        private void DeleteGoalsRelatedToContributor(int id)
        {
            //Get all workout programs owned by the contributor.
            List<WorkoutProgram> programs = _context.WorkoutPrograms
                .Include(wp => wp.Workouts).Where(wp => wp.ContributorId == id).ToList();


            foreach (WorkoutProgram program in programs)
            {
                //Delete sub-goals.
                List<Workout> workouts = program.Workouts.ToList();
                foreach (Workout workout in workouts)
                {
                    List<SubGoal> subgoals = _context.SubGoals.Where(s => s.WorkoutId == workout.Id).ToList();
                    foreach (SubGoal subGoal in subgoals)
                    {
                        _context.SubGoals.Remove(subGoal);
                    }
                }

                //Delete goals.
                List<Goal> goals = _context.Goals.Where(g => g.WorkoutProgramId == program.Id).ToList();
                foreach (Goal goal in goals)
                {
                    _context.Goals.Remove(goal);
                }
            }
        }
    }
}
