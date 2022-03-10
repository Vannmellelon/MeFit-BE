using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MeFit_BE.Models.Domain.Workout
{
    [Table("SubGoal")]
    public class SubGoal
    {
        public int Id { get; set; }

        public bool Achieved { get; set; }

        public int? WorkoutId { get; set; }

        public Workout Workout { get; set; }  
    }
}
