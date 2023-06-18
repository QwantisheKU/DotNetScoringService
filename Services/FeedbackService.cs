using DotNetScoringService.Models;
using DotNetScoringService.Repositories.Interfaces;
using DotNetScoringService.Services.Interfaces;
using System.Security.Claims;

namespace DotNetScoringService.Services
{
    public class FeedbackService : IFeedbackService
    {
        private readonly IFeedbackRepository _feedbackRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public FeedbackService(IFeedbackRepository feedbackRepository, IHttpContextAccessor httpContextAccessor)
        {
            _feedbackRepository = feedbackRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Feedback> CreateFeedbackAsync(Feedback feedback)
        {
            feedback.DateCreated = DateTime.UtcNow;
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            feedback.UserId = userId;
            var userEmail = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Email).Value;
            feedback.Email = userEmail;

            await _feedbackRepository.AddAsync(feedback);
            await _feedbackRepository.SaveAsync();

            return feedback;
        }
    }
}
