using MeFit_BE.Models.Domain.UserDomain;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace MeFit_BE.Models.Domain.Workout
{
    [Table("Workout")]
    public class Workout
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        [DefaultValue(false)]
        public bool Complete { get; set; }

        public SubGoal SubGoal { get; set; }

        public int SubGoalId { get; set; }

        public Set Set { get; set; }

        // FK to contributor
        public int ContributorId { get; set; }
        public User ContributedBy { get; set; }
    }
}
