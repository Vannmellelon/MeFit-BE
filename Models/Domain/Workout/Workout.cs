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

        // TODO
        // More than one exercise(?) change to list of exercises/sets
        public int? SetId { get; set; }

        public Set Set { get; set; }

        // FK to contributor
        public int ContributorId { get; set; }
        public User.User ContributedBy { get; set; }
    }
}
