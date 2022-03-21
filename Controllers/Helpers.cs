using MeFit_BE.Models;
using MeFit_BE.Models.Domain.UserDomain;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace MeFit_BE.Controllers
{
    public static class Helper
    {
        /// <summary>
        /// Method returns true if the current user is a contributor.
        /// </summary>
        /// <param name="httpContext">HttpContext</param>
        /// <returns>Boolean</returns>
        public static bool IsContributor(HttpContext httpContext)
        {
           return httpContext.User.IsInRole("Contributor");
        }

        /// <summary>
        /// Method returns true if the current user is an administrator.
        /// </summary>
        /// <param name="httpContext">HttpContext</param>
        /// <returns>Boolean</returns>
        public static bool IsAdmin(HttpContext httpContext)
        {
            return httpContext.User.IsInRole("Admin");
        }

        /// <summary>
        /// Method returns the token id of the current user.
        /// </summary>
        /// <returns>String</returns>
        public static string GetExternalUserProviderId(HttpContext httpContext)
        {
           return httpContext.User.Identity.Name.ToString();
        }

        /// <summary>
        /// Method used to get the current user based on the token user id.
        /// </summary>
        /// <returns>User</returns>
        public static async Task<User> GetCurrentUser(HttpContext httpContext, MeFitDbContext context)
        {
            return await context.Users.FirstOrDefaultAsync(u => u.AuthId == Helper.GetExternalUserProviderId(httpContext));
        }
    }
}