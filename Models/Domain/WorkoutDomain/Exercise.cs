﻿using MeFit_BE.Models.Domain.UserDomain;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MeFit_BE.Models.Domain.WorkoutDomain
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

        public int ContributorId { get; set; }

        public User ContributedBy { get; set; }

        public ICollection<Set> Sets { get; set; }
    }
}
