using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using MeFit_BE.Models.Domain.Workout;

namespace MeFit_BE.Models.Domain.User
{
    [Table("User")]
    public class User
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public bool IsContributer { get; set; }

        public bool IsAdmin { get; set; } 

        public ICollection<WorkoutProgram>? Programs { get; set; }
        public ICollection<Workout.Workout>? Workouts { get; set; }
        public ICollection<Exercise>? Exercises { get; set; }
    }
}
