using Auth0.AspNetCore.Authentication;
using MeFit_BE.Models;
using MeFit_BE.Models.Domain.UserDomain;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace MeFit_BE.Controllers
{
    [Route("api/login")]
    [ApiController]
    [Authorize]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    [ApiConventionType(typeof(MeFitConventions))]
    public class LoginController : ControllerBase
    {
        private readonly MeFitDbContext _context;

        public LoginController(MeFitDbContext context)
        {
            _context = context;
        }

        public class UserStatus
        {
            public bool UserExists { get; set; }
        }

        /// <summary>
        /// Checks if currently logged in user exists in database.
        /// </summary>
        /// <returns>Returns a JSON object with a boolean.</returns>
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<UserStatus>> Login()
        {
            // Frontend uses this to know where to redirect the user

            User user = await Helper.GetCurrentUser(HttpContext, _context);
            UserStatus userStatus = new();

            if (user == null)
            {
                userStatus.UserExists = false;
            }
            else
            {
                userStatus.UserExists = true;
            }

            return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(userStatus));
        }

        /*
        [HttpGet("private")]
        //[Authorize()]
        public IActionResult Private()
        {
            return Ok(new
            {
                Message = "Hello from a private endpoint! You need to be authenticated to see this."
            });
        }
        */
        /*
        public async Task Login(string returnUrl = "/")
        {

            var authenticationProperties = new LoginAuthenticationPropertiesBuilder()
                // Indicate here where Auth0 should redirect the user after a login.
                // Note that the resulting absolute Uri must be added to the
                // **Allowed Callback URLs** settings for the app.
                .WithRedirectUri(returnUrl)
                .Build();

            await HttpContext.ChallengeAsync(Auth0Constants.AuthenticationScheme, authenticationProperties);
        }

        [Authorize]
        public async Task Logout()
        {
            var authenticationProperties = new LogoutAuthenticationPropertiesBuilder()
                // Indicate here where Auth0 should redirect the user after a logout.
                // Note that the resulting absolute Uri must be added in the
                // **Allowed Logout URLs** settings for the client.
                .WithRedirectUri(Url.Action("Index", "Home"))
                .Build();

            await HttpContext.SignOutAsync(Auth0Constants.AuthenticationScheme, authenticationProperties);
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
        */
    }
}
