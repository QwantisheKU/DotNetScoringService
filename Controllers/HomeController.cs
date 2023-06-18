using DotNetScoringService.Data;
using DotNetScoringService.Models;
using DotNetScoringService.Repositories;
using DotNetScoringService.Repositories.Interfaces;
using DotNetScoringService.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace DotNetScoringService.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICalculationService _calculationService;
        public HomeController(ILogger<HomeController> logger, ICalculationService calculationService)
        {
            _logger = logger;
            _calculationService = calculationService;
        }

        public async Task<IActionResult> Index()
        {
            var calculations = await _calculationService.GetAllCalculationsByUserAsync();
            return View(calculations) ?? View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}