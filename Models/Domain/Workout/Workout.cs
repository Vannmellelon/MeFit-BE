using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MeFit_BE.Models.Domain.Workout
{
    [Table("Workout")]
    public class Workout
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public bool Complete { get; set; }

        public int? SetId { get; set; }

        public Set Set { get; set; }
    }
}
