using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MeFit_BE.Models.Domain.Workout
{
    [Table("Goal")]
    public class Goal
    {
        public int Id { get; set; }

        public DateTime EndData { get; set; }

        public bool Achieved { get; set; }

        public int? ProgramId { get; set; }

        public WorkoutProgram WorkoutProgram { get; set; }  

        public ICollection<SubGoal> SubGoals { get; set; }

    }
}
