using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DotNetScoringService.Models
{   
    public class Calculation
    {
        public int ID { get; set; }

        [Required]
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

        [JsonIgnore]
        [DataType(DataType.DateTime)]
        public DateTime DateCreated { get; set; }

        public string? UserId { get; set; }

        //public ApplicationUser? ApplicationUser { get; set; }

        //public CalculationResult? CalculationResult { get; set; }
    }

    public enum HomeOwnership
    {
        OWN,
        RENT,
        MORTGAGE,
        OTHER
    }

    public enum LoanDefault
    {
        N,
        Y
    }

    public enum LoanIntent
    {
        EDUCATION,
        MEDICAL,
        VENTURE,
        PERSONAL,
        DEBTCONSOLIDATION,
        HOMEIMPROVEMENT
    }
}
