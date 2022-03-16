using MeFit_BE.Models.Domain.UserDomain;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace MeFit_BE.Models.Domain.GoalDomain
{
    [Table("Goal")]
    public class Goal
    {
        public int Id { get; set; }

        public DateTime EndData { get; set; }

        [DefaultValue(false)]
        public bool Achieved { get; set; }

        public User User { get; set; }

        public int UserId { get; set; }
    }
}
