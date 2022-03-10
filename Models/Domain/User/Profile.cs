using System;
using MeFit_BE.Models.Domain.Workout;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MeFit_BE.Models.Domain.User
{
    [Table("Profile")]
    public class Profile
    {
        public int Id { get; set; }

        public int Weight { get; set; }

        public int Height { get; set; }

        public string MedicalConditions { get; set; }

        public string Disabilities { get; set; }

        public int UserId { get; set; }

        public int? AddressId { get; set; }

        public Address Address { get; set; }

        public ICollection<Goal> Goals { get; set; } 

        public ICollection<WorkoutProgram> WorkoutPrograms { get; set; } 

        public ICollection<Workout.Workout> Workouts { get; set; }
    }
}
