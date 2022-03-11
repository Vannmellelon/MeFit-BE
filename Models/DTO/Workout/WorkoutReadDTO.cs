namespace MeFit_BE.Models.DTO.Workout
{
    public class WorkoutReadDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public bool Complete { get; set; }

        public int? SetId { get; set; }

        public int? ContributorId { get; set; }
    }
}
