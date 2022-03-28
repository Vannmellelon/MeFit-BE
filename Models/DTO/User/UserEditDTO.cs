namespace MeFit_BE.Models.DTO.User
{
    public class UserEditDTO
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; } 

        public string FitnessLevel { get; set; }

        public string RestrictedCategories { get; set; }
    }
}
