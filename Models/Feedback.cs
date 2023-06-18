using System.ComponentModel.DataAnnotations;

namespace DotNetScoringService.Models
{
    public class Feedback
    {
        public int ID { get; set; }

        public string Name { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        public string Message { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime DateCreated { get; set; }

        public string? UserId { get; set; }

        public ApplicationUser? ApplicationUser { get; set; }

    }
}
