using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MeFit_BE.Models.Domain.User
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
