using DotNetScoringService.Models;

namespace DotNetScoringService.Repositories.Interfaces
{
    public interface ICalculationResultRepository : IGenericRepository<CalculationResult>
    {
        Task<CalculationResult> GetByCalculationId(int id);
    }
}
