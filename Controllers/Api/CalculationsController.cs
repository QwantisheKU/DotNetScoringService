using DotNetScoringService.Dto;
using DotNetScoringService.Models;
using DotNetScoringService.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DotNetScoringService.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalculationsController : ControllerBase
    {
        private readonly ICalculationService _calculationService;

        public CalculationsController(ICalculationService calculationService)
        {
            _calculationService = calculationService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCalculation(Calculation calculation)
        {
            return Ok(await _calculationService.CreateCalculationAsync(calculation));
        }
    }
}
