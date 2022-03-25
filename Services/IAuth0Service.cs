using MeFit_BE.Models.Domain.UserDomain;
using MeFit_BE.Models.JSON;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeFit_BE.Services
{
    public interface IAuth0Service
    {
        public Task<String> GetAccessTokenAsync();
        public Task DeleteUserAsync(string id);  
        public Task UpdateUserAsync(string id, string email, string nickname); 
        public Task UpdateUserRolesAsync(string id, string role, Auth0RoleBody body);
        public bool UserExists(string id); 
    }
}
