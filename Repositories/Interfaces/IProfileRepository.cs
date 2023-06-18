using DotNetScoringService.Models;

namespace DotNetScoringService.Repositories.Interfaces
{
    public interface IProfileRepository : IGenericRepository<Profile>
    {
        Task<Profile> GetByUserIdAsync(int id);
    }
}
