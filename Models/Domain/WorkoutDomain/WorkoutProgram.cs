using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using MeFit_BE.Models.Domain.UserDomain;


namespace MeFit_BE.Models.Domain.WorkoutDomain
{
    [Table("WorkoutProgram")]
    public class WorkoutProgram
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Category { get; set; }

        // FK to contributor
        public int ContributorId { get; set; }
        public User ContributedBy { get; set; }

        public Goal Goal { get; set; }

        public int GoalId { get; set; }

        public ICollection<SubGoal> SubGoals { get; set; }
    }
}
