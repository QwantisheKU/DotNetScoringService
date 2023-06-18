using System.ComponentModel.DataAnnotations;

namespace DotNetScoringService.Models
{
    public class CalculationResult
    {
        public int ID { get; set; }

        public string Score { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime DateCreated { get; set; }

        public int? CalculationId { get; set; }

        public Calculation? Calculation { get; set; }
    }
}
