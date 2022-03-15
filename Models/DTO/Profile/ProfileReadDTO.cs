namespace MeFit_BE.Models.DTO.Profile
{
    public class ProfileReadDTO
    {
        public int Id { get; set; }

        public int Weight { get; set; }

        public int Height { get; set; }

        public string MedicalConditions { get; set; }

        public string Disabilities { get; set; }

        public int UserId { get; set; }

        public int AddressId { get; set; }
    }
}
