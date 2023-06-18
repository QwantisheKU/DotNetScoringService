using DotNetScoringService.Models;

namespace DotNetScoringService.Dto
{
    public class FinalResult
    {
        public CalculationResult? calculationResult { get; set; }

        public IEnumerable<RecommendationModel>? recommendations { get; set; }
    }
}
