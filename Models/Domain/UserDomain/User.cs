using MeFit_BE.Models.Domain.WorkoutDomain;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace MeFit_BE.Models.Domain.UserDomain
{
    [Table("User")]
    public class User
    {
        public int Id { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        [DefaultValue(false)]
        public bool IsContributer { get; set; }

        [DefaultValue(false)]
        public bool IsAdmin { get; set; }

        public ICollection<Goal> Goals { get; set; }
    }
}
