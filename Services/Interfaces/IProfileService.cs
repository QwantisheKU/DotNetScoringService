using DotNetScoringService.Models;

namespace DotNetScoringService.Services.Interfaces
{
    public interface IProfileService
    {
        Task<Profile> GetProfileByUserIdAsync(int id);

        Task<Profile> EditProfileAsync(int id, Profile profile);
    }
}
