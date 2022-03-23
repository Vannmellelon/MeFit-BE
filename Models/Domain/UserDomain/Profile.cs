using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MeFit_BE.Models.Domain.UserDomain
{
    [Table("Profile")]
    public class Profile
    {
        public int Id { get; set; }

        [Range(0,1000)]
        public int Weight { get; set; }

        [Range(0, 250)]
        public int Height { get; set; }

        public string MedicalConditions { get; set; }

        public string Disabilities { get; set; }

        public User User { get; set; }

        public int UserId { get; set; }

        public int? AddressId { get; set; }

        public Address Address { get; set; }
    }
}
