using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MeFit_BE.Models.Domain.GoalDomain
{
    [Table("SubGoal")]
    public class SubGoal
    {
        public int Id { get; set; }

        [DefaultValue(false)]
        public bool Achieved { get; set; }
    }
}
