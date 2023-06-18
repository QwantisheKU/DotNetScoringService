using DotNetScoringService.Data;
using DotNetScoringService.Models;
using DotNetScoringService.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DotNetScoringService.Repositories
{
    public class ProfileRepository : GenericRepository<Profile>, IProfileRepository
    {
        private readonly EducativeAppContext _context;
        public ProfileRepository(EducativeAppContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Profile> GetByUserIdAsync(int id)
        {
            return null; 
                //await _context.Profile.Where(x => x.UserId == id).FirstAsync();
        }

    }
}
