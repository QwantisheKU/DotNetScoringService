using DotNetScoringService.Models;
using DotNetScoringService.Repositories;
using DotNetScoringService.Repositories.Interfaces;
using DotNetScoringService.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DotNetScoringService.Services
{
    public class ProfileService: IProfileService
    {
        private readonly IProfileRepository _profileRepository;
        public ProfileService(IProfileRepository profileRepository)
        {
            _profileRepository = profileRepository;
        }

        public async Task<Profile> GetProfileByUserIdAsync(int id)
        {
            var profile = await _profileRepository.GetByIdAsync(id);

            if (profile == null)
            {
                return null;
            }

            return profile;
        }

        public async Task<Profile> EditProfileAsync(int id, Profile profile)
        {

            /*var profileByUser = await _profileRepository.GetByUserIdAsync(id);

            if (profileByUser == null)
            {
                return null;
            }*/

            try
            {
                profile.DateCreated = DateTime.UtcNow;
                profile.Email = "odargov@gmail.com";
                //profile.UserId = 29;

                await _profileRepository.UpdateAsync(profile);
                await _profileRepository.SaveAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ProfileExists(id))
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }

            return profile;
        }

        private async Task<bool> ProfileExists(int id)
        {
            var user = await _profileRepository.GetByIdAsync(id);
            return user != null ? true : false;
        }
    }
}
