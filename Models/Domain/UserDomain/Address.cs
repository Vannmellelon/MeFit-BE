using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MeFit_BE.Models.Domain.UserDomain
{
    [Table("Address")]
    public class Address
    {
        public int Id { get; set; }

        public string Street { get; set; }

        [MaxLength(4), MinLength(4)]
        public int PostalCode { get; set; }

        public string PostalPlace { get; set; }

        public string Country { get; set; } 
    }
}
