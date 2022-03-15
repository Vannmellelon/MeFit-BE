using AutoMapper;
using MeFit_BE.Models;
using MeFit_BE.Models.DTO.Profile;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;

namespace MeFit_BE.Controllers
{
    [Route("api/profile")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    [ApiConventionType(typeof(DefaultApiConventions))]
    public class ProfileController : Controller
    {
        private readonly MeFitDbContext _context;
        private readonly IMapper _mapper;

        public ProfileController(MeFitDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: ProfileController
        /// <summary>
        /// Method fetches all profiles from the database.
        /// </summary>
        /// <returns>List of profiles</returns>
        [HttpGet]
        public async Task<ActionResult<List<ProfileReadDTO>>> GetProfiles()
        {
            return _mapper.Map<List<ProfileReadDTO>>(await _context.Profiles.ToListAsync());
        }
        
        // GET api/<ProfileController>/5
        /// <summary>
        /// Method fetches a specific profile from the database.
        /// </summary>
        /// <param name="id">Profile id</param>
        /// <returns>Profile</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ProfileReadDTO>> GetProfile(int id)
        {
            return _mapper.Map<ProfileReadDTO>(await _context.Profiles.FindAsync(id));
        }
        
        // POST api/<ProfileController>/
        /// <summary>
        /// Method creates a new Profile.
        /// </summary>
        /// <param name="profileDTO">New profile</param>
        /// <returns>New profile</returns>
        [HttpPost]
        public async Task<ActionResult<ProfileReadDTO>> Post([FromBody] ProfileWriteDTO profileDTO)
        {
            Models.Domain.UserDomain.Profile profile = _mapper.Map<Models.Domain.UserDomain.Profile>(profileDTO);
            _context.Profiles.Add(profile);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetProfile", new { Id = profile.Id }, _mapper.Map<ProfileReadDTO>(profile));
        }

        // PATCH api/<ProfileController>/5
        /// <summary>
        /// Method updates a profile in the database.
        /// </summary>
        /// <param name="id">Profile id</param>
        /// <param name="profileDTO">Profile with updated values</param>
        /// <returns>Updated profile</returns>
        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(int id, [FromBody] ProfileEditDTO profileDTO)
        {
            //Get profile.
            Models.Domain.UserDomain.Profile profile = await _context.Profiles.FindAsync(id);
            if (profile == null) return NotFound();

            //Update profile.
            if (profileDTO.Disabilities != null) profile.Disabilities = profileDTO.Disabilities;
            if (profileDTO.Weight != 0) profile.Weight = profileDTO.Weight;
            if (profileDTO.Height != 0) profile.Height = profileDTO.Height;
            if (profileDTO.MedicalConditions != null) profile.MedicalConditions = profileDTO.MedicalConditions;

            _context.Entry(profile).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok(_mapper.Map<ProfileReadDTO>(profile));
        }

        // GET: ProfileController/Delete/5
        /// <summary>
        /// Method deletes the profile with the given id.
        /// </summary>
        /// <param name="id">Profile id</param>
        /// <returns>No content</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            Models.Domain.UserDomain.Profile profile = await _context.Profiles.FindAsync(id);
            if (profile == null) return NotFound();
            _context.Profiles.Remove(profile);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
