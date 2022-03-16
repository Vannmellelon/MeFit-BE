using MeFit_BE.Models.Domain.WorkoutDomain;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace MeFit_BE.Models.Domain.GoalDomain
{
    [Table("SubGoal")]
    public class SubGoal
    {
        public int Id { get; set; }

        [DefaultValue(false)]
        public bool Achieved { get; set; }

        public Goal Goal { get; set; }

        public int GoalId { get; set; }

        public Workout Workout { get; set; }

        public int WorkoutId { get; set; }
    }
}
