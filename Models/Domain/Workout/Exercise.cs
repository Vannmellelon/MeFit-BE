using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

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

        public Set Set { get; set; }

        public int SetId { get; set; }
    }
}
