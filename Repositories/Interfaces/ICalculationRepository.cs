using DotNetScoringService.Models;

namespace DotNetScoringService.Repositories.Interfaces
{
    public interface ICalculationRepository : IGenericRepository<Calculation>
    {
        Task<IEnumerable<Calculation>> GetAllByUserAsync(string userId);
    }
}
