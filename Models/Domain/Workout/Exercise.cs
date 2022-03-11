using MeFit_BE.Models.Domain.UserDomain;
using System.ComponentModel.DataAnnotations.Schema;

namespace MeFit_BE.Models.Domain.Workout
{
    [Table("Exercise")]
    public class Exercise
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string TargetMuscleGroup { get; set; }

        public string Image { get; set; }

        public string Video { get; set; }

        // FK to contributor
        public int ContributorId { get; set; }
        public User ContributedBy { get; set; }

        public Set Set { get; set; }

        public int SetId { get; set; }
    }
}
