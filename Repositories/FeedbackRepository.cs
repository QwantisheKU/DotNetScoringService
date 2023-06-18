using DotNetScoringService.Data;
using DotNetScoringService.Migrations;
using DotNetScoringService.Models;
using DotNetScoringService.Repositories.Interfaces;

namespace DotNetScoringService.Repositories
{
    public class FeedbackRepository: GenericRepository<Feedback>, IFeedbackRepository
    {
        public FeedbackRepository(EducativeAppContext context) : base(context) { }
    }
}
