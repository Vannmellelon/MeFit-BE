namespace MeFit_BE.Models.DTO
{
    public class UserReadDTO
    {
        public int Id { get; set; }
        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public bool IsContributer { get; set; }
        public bool IsAdmin { get; set; }
    }
}
