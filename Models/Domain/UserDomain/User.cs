using MeFit_BE.Models.Domain.GoalDomain;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MeFit_BE.Models.Domain.UserDomain
{
    [Table("User")]
    public class User
    {
        public int Id { get; set; }

        [StringLength(50)]
        public string FirstName { get; set; }

        [StringLength(50)]
        public string LastName { get; set; }

        [StringLength(50)]
        public string FitnessLevel { get; set; }

        public string? RestrictedCategories { get; set;}

        [StringLength(100)]
        [EmailAddress]
        public string Email { get; set; }

        public string? AuthId { get; set; }

        [DefaultValue(false)]
        public bool IsContributor { get; set; }

        [DefaultValue(false)]
        public bool IsAdmin { get; set; }

        public ICollection<Goal> Goals { get; set; }
    }
}
