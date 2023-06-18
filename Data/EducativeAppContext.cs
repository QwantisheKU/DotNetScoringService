using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DotNetScoringService.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace DotNetScoringService.Data
{
    public class EducativeAppContext : IdentityDbContext<ApplicationUser>
    {
        public EducativeAppContext (DbContextOptions<EducativeAppContext> options)
            : base(options)
        {
        }

        public DbSet<DotNetScoringService.Models.Profile> Profile { get; set; } = default!;

        public DbSet<DotNetScoringService.Models.Calculation> Calculation { get; set; } = default!;

        public DbSet<DotNetScoringService.Models.CalculationResult> CalculationResult { get; set; } = default!;

        public DbSet<DotNetScoringService.Models.Feedback> Feedback { get; set; } = default!;
    }
}
