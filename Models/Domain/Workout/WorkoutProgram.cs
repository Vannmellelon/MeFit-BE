using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MeFit_BE.Models.Domain.Workout
{
    [Table("Program")]
    public class WorkoutProgram
    {
        // Primary Key (Autoincremented Id)
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Category { get; set; } 
    }
}
