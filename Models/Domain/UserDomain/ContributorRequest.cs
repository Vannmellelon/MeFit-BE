using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MeFit_BE.Models.Domain.UserDomain
{
    public class ContributorRequest
    {
        public int Id { get; set; }

        public int? RequestingUserId { get; set; }

        [ForeignKey("RequestingUserId")]
        public User RequestingUser { get; set; }
    }
}
