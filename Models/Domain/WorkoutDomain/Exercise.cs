using MeFit_BE.Models.Domain.UserDomain;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MeFit_BE.Models.Domain.WorkoutDomain
{
    [Table("Exercise")]
    public class Exercise
    {
        public int Id { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(200)]
        public string Description { get; set; }

        [MaxLength]
        public string Image { get; set; }

        [MaxLength]
        public string Video { get; set; }

        [StringLength(15)]
        public string Category { get; set; }

        public int ContributorId { get; set; }

        [ForeignKey("ContributorId")]
        public User Contributor { get; set; }

        public ICollection<Set> Sets { get; set; }
    }
}
