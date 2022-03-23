using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MeFit_BE.Models.Domain.UserDomain
{
    [Table("Address")]
    public class Address
    {
        public int Id { get; set; }

        [StringLength(100)]
        public string Street { get; set; }

        [MaxLength(4), MinLength(4)]
        public int PostalCode { get; set; }

        [StringLength(10)]
        public string PostalPlace { get; set; }

        [StringLength(50)]
        public string Country { get; set; } 
    }
}
