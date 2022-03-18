using MeFit_BE.Models;
using MeFit_BE.Models.Domain.UserDomain;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace MeFit_BE.Controllers
{
    public static class Helper
    {
        /// <summary>
        /// Method returns true if the current user is a contributor.
        /// </summary>
        /// <returns>Boolean</returns>
        public static bool IsContributor(HttpContext httpContext)
        {
           return httpContext.User.IsInRole("Contributor");
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
        public static User GetCurrentUser(HttpContext httpContext, MeFitDbContext context)
        {
            // TODO: Må legge in AuthId i database slik at denne kan brukes.
            //return context.Users.FirstOrDefault(u => u.AuthId == Helper.GetExternalUserProviderId(httpContext));
            return context.Users.FirstOrDefault(u => u.Id == 1);
        }
    }
}