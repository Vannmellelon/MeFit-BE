using System.ComponentModel.DataAnnotations.Schema;

namespace MeFit_BE.Models.Domain
{
    [Table("User")]
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
    }
}
