using DotNetScoringService.Data;
using DotNetScoringService.Models;
using DotNetScoringService.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DotNetScoringService.Repositories
{
    public class CalculationRepository: GenericRepository<Calculation>, ICalculationRepository
    {
        private readonly EducativeAppContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CalculationRepository(EducativeAppContext context, IHttpContextAccessor httpContextAccessor) : base(context)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IEnumerable<Calculation>> GetAllByUserAsync(string userId)
        {
            return await _context.Calculation.Where(u => u.UserId == userId).ToListAsync();
        }
    }
}
