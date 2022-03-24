using System.Collections.Generic;

namespace MeFit_BE.Models.DTO.User
{
    public class UserWriteDTO
    {
        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string FitnessLevel { get; set; }

        public string RestrictedCategories { get; set; }
    }
}
