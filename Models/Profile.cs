using System.ComponentModel.DataAnnotations;

namespace DotNetScoringService.Models
{
    public class Profile
    {
        public int ID { get; set; }
        public string? Name { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        public string? ImageUrl { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime DateCreated { get; set; }

        //public int? UserId { get; set; }

        //public User? User { get; set; } = null!;
    }
}
