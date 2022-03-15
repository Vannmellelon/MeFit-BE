using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.ComponentModel;

namespace MeFit_BE.Models.Domain.UserDomain
{
    [Table("Profile")]
    public class Profile
    {
        public int Id { get; set; }

        public int Weight { get; set; }

        public int Height { get; set; }

        public string MedicalConditions { get; set; }

        public string Disabilities { get; set; }

        public User User { get; set; }

        public int UserId { get; set; }

        public int? AddressId { get; set; }

        public Address Address { get; set; }
    }
}
