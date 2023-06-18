using DotNetScoringService.Models;

namespace DotNetScoringService.Services.Interfaces
{
    public interface IFeedbackService
    {
        Task<Feedback> CreateFeedbackAsync(Feedback feedback);
    }
}
