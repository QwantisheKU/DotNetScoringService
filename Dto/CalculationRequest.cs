using DotNetScoringService.Models;
using System.ComponentModel.DataAnnotations;

namespace DotNetScoringService.Dto
{
    public class CalculationRequest
    {
        public string Name { get; set; }

        [Required]
        public int Age { get; set; }

        [Display(Name = "Income ($)")]
        [Required]
        public double Income { get; set; }

        [Display(Name = "Home Ownership")]
        [Required]
        public HomeOwnership HomeOwnership { get; set; }

        [Display(Name = "Employment Length")]
        [Required]
        public int EmploymentLength { get; set; }

        [Display(Name = "Loan Intent")]
        [Required]
        public LoanIntent LoanIntent { get; set; }

        [Display(Name = "Loan Amount ($)")]
        [Required]
        public int LoanAmount { get; set; }

        [Display(Name = "Loan Interest Rate")]
        [Required]
        public double LoanInterestRate { get; set; }

        [Display(Name = "Loan Term (months)")]
        [Required]
        public int LoanTerm { get; set; }

        [Display(Name = "Loan Default")]
        [Required]
        public LoanDefault LoanDefault { get; set; }

        [Display(Name = "Credit History Length")]
        [Required]
        public int CreditHistoryLength { get; set; }

        public string? UserId { get; set; }
    }
}
