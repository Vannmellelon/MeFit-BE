using System.ComponentModel.DataAnnotations.Schema;
namespace MeFit_BE.Models.Domain.Workout
{
    [Table("Program")]
    public class WorkoutProgram
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Category { get; set; }

        // FK to contributor
        public int ContributorId { get; set; }
        public User.User ContributedBy { get; set; }
    }
}
