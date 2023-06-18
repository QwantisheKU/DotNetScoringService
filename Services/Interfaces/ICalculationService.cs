using DotNetScoringService.Dto;
using DotNetScoringService.Models;

namespace DotNetScoringService.Services.Interfaces
{
    public interface ICalculationService
    {
        Task<Calculation> GetCalculationByIdAsync(int id);

        Task<Calculation> CreateCalculationAsync(Calculation calculation);

        Task RemoveCalculationAsync(int id);

        Task<IEnumerable<Calculation>> GetAllCalculationsByUserAsync();

        Task<FinalResult> GetCalculationResultById(int id);
    }
}
