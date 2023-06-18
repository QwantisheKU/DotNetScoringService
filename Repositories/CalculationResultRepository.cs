using DotNetScoringService.Data;
using DotNetScoringService.Models;
using DotNetScoringService.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DotNetScoringService.Repositories
{
    public class CalculationResultRepository : GenericRepository<CalculationResult>, ICalculationResultRepository
    {
        private readonly EducativeAppContext _context;
        public CalculationResultRepository(EducativeAppContext context) : base(context)
        {
            _context = context;
        }

        public async Task<CalculationResult> GetByCalculationId(int id)
        {
            return await _context.CalculationResult.Where(x => x.CalculationId == id).FirstAsync();
        }
    }
}
