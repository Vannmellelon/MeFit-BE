using System.ComponentModel.DataAnnotations.Schema;

namespace MeFit_BE.Models.Domain.Workout
{
    [Table("Workout")]
    public class Workout
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public bool Complete { get; set; }

        public int? SetId { get; set; }

        public Set Set { get; set; }

        // FK to contributor
        public int ContributorId { get; set; }
        public User.User ContributedBy { get; set; }
    }
}
