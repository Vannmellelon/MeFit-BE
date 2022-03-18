using Microsoft.AspNetCore.Http;

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
    }
}