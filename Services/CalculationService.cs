using DotNetScoringService.Dto;
using DotNetScoringService.Models;
using DotNetScoringService.Repositories;
using DotNetScoringService.Repositories.Interfaces;
using DotNetScoringService.Services.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Security.Claims;

namespace DotNetScoringService.Services
{
    public class CalculationService: ICalculationService
    {
        private readonly ICalculationRepository _calculationRepository;
        private readonly ICalculationResultRepository _calculationResultRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpClient client = new HttpClient();

        public CalculationService(IHttpContextAccessor httpContextAccessor, ICalculationRepository calculationRepository, ICalculationResultRepository calculationResultRepository /*IUserService userService*/)
        {
            _calculationRepository = calculationRepository;
            _calculationResultRepository = calculationResultRepository;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<Calculation> GetCalculationByIdAsync(int id)
        {
            var calculation = await _calculationRepository.GetByIdAsync(id);

            if (calculation == null)
            {
                return null;
            }
            return calculation;
        }

        public async Task<Calculation> CreateCalculationAsync(Calculation calculation)
        {
            string userId = "";
            if (String.IsNullOrEmpty(calculation.UserId))
            {
                userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            }
            else
            {
                userId = calculation.UserId;
            }

            string calculationResultFromApi = "";

            calculation.DateCreated = DateTime.UtcNow;
            calculation.UserId = userId;

            await _calculationRepository.AddAsync(calculation);
            await _calculationRepository.SaveAsync();

            try
            {
                calculationResultFromApi = await CalculateCreditRiskProfile(calculation);
            }
            catch (Exception e)
            {
                return null;
            }
            

            CalculationResult calculationResult = new CalculationResult();
            calculationResult.Score = calculationResultFromApi.ToString();
            calculationResult.DateCreated = DateTime.UtcNow;
            calculationResult.CalculationId = calculation.ID;
            calculationResult.Calculation = calculation;
            await _calculationResultRepository.AddAsync(calculationResult);
            await _calculationResultRepository.SaveAsync();

            return calculation;
        }

        public async Task RemoveCalculationAsync(int id)
        {
            var calculation = await _calculationRepository.GetByIdAsync(id);
            var calculationResult = await _calculationResultRepository.GetByCalculationId(id);

            if (calculation != null && calculationResult != null)
            {
                await _calculationResultRepository.RemoveAsync(calculationResult);
                await _calculationRepository.RemoveAsync(calculation);
            }

            await _calculationResultRepository.SaveAsync();
            await _calculationRepository.SaveAsync();
        }

        public async Task<IEnumerable<Calculation>> GetAllCalculationsByUserAsync()
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var calculations = await _calculationRepository.GetAllByUserAsync(userId);
            calculations = calculations.OrderByDescending(x => x.DateCreated);
            return calculations;
        }

        public async Task<FinalResult> GetCalculationResultById(int id)
        {
            var calculationResult = await _calculationResultRepository.GetByCalculationId(id);
            var recommendations = await GetRecommendationsAsync(id);

            if (calculationResult == null)
            {
                return null;
            }

            var finalResult = new FinalResult() { calculationResult = calculationResult, recommendations = recommendations };

            return finalResult;
        }

        public async Task<IEnumerable<RecommendationModel>> GetRecommendationsAsync(int id)
        {
            List<RecommendationModel> recommendations = new List<RecommendationModel>();

            var calculation = await _calculationRepository.GetByIdAsync(id);

            // P = D x F x K, где P - платежеспособность заемщика, D – ежемесячный доход после уплаты налогов, F – срок кредита или займа в месяцах, а К – поправочный коэффициент (если D меньше $500, то К равен 0,3; если D более $500, но менее $1000, то К равен 0,4; если D больше $1000, но менее $2000, то К равен 0,5; если D больше $2000, то К равен 0,6)
            double k = 0;
            if (calculation.Income < 500) k = 0.3;
            else if (calculation.Income >= 500 && calculation.Income < 1000) k = 0.4;
            else if (calculation.Income >= 1000 && calculation.Income < 2000) k = 0.5;
            else if (calculation.Income >= 2000) k = 0.6;
            double p = Math.Round(calculation.Income, 2) * calculation.LoanTerm * k;

            // S = P/(1 + (C x (t + 1))/(2*12*100)) - optimal sum
            double s = p / (1 + (calculation.LoanInterestRate * (calculation.LoanTerm + 1)) / (2 * 12 * 100));

            int counter = 1;
            if (calculation.LoanAmount > s)
            {
                recommendations.Add(new RecommendationModel() { 
                    Number = counter, 
                    Text = $"Указанная сумма кредита или займа превышает оптимальную. Оптимальная сумма кредита или займа для вашего расчета = {Math.Round(s, 2)}$." 
                });
                counter += 1;
            }
            if (calculation.HomeOwnership == HomeOwnership.MORTGAGE)
            {
                recommendations.Add(new RecommendationModel() {
                    Number = counter, 
                    Text = "Зачастую финансовые организации отказывают в предоставлении кредита по причине имеющейся кредитной нагрузки в виде ипотеки. Рекомендуется закрывать все предыдущие кредиты или займы перед взятием новых."
                });
                counter += 1;
            }
            if (calculation.LoanDefault == LoanDefault.Y)
            {
                recommendations.Add(new RecommendationModel() { 
                    Number = counter, 
                    Text = "Наличие просроченных, даже на небольшой срок, долгов является \"красным флагом\" для финансовых организаций, особенно, банковских организаций. Рекомендуется не запаздывать и не откладывать запланированные выплаты, не имея на это жизненно-важных обоснований, даже если ранее имел место быть просроченный кредит или заём."
                });
                counter += 1;
            }
            if (calculation.LoanInterestRate > 11)
            {
                recommendations.Add(new RecommendationModel() { 
                    Number = counter, 
                    Text = "Указанная процентная ставка превышает среднее значение по рынку. Рекомендуется ознакомиться с продуктами других финансовых организаций самостоятельно или обратиться за помощью к сайтам-агрегаторам."
                });
                counter += 1;
            }
            if (calculation.LoanIntent == LoanIntent.DEBTCONSOLIDATION)
            {
                recommendations.Add(new RecommendationModel() { 
                    Number = counter, 
                    Text = "Финансовые организации в большинстве случаев не оформляют кредиты и займы для погашения предыдущих задолженностей. Рекомендуется закрывать все предыдущие кредиты или займы перед взятием новых."
                });
            }

            return recommendations;
        }

        public async Task<string> CalculateCreditRiskProfile(Calculation calculation)
        {
            var request = new Dictionary<string, string>
            {
                { "person_age", Convert.ToString(calculation.Age) },
                { "person_income", Convert.ToString(calculation.Income) },
                { "person_home_ownership", calculation.HomeOwnership.ToString() },
                { "person_emp_length", Convert.ToString(calculation.EmploymentLength) },
                { "loan_intent", calculation.LoanIntent.ToString() },
                { "loan_amnt", Convert.ToString(calculation.LoanAmount) },
                { "loan_int_rate", Convert.ToString(calculation.LoanInterestRate) },
                { "cb_person_default_on_file", calculation.LoanDefault.ToString() },
                { "cb_person_cred_hist_length", Convert.ToString(calculation.CreditHistoryLength) }
            };

            var encodedRequest = new FormUrlEncodedContent(request);

            var response = await client.PostAsync("http://localhost:8000/api/calculation", encodedRequest);
            var responseString = await response.Content.ReadAsStringAsync();
            var deserializedResponseString = JsonConvert.DeserializeObject(responseString).ToString();

            CalculationResultResponse calculationResponse = JsonConvert.DeserializeObject<CalculationResultResponse>(deserializedResponseString);
            string score = calculationResponse.Score;

            return score;

        }
    }
}
