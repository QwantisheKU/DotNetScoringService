using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DotNetScoringService.Data;
using DotNetScoringService.Models;
using DotNetScoringService.Repositories;
using DotNetScoringService.Repositories.Interfaces;
using DotNetScoringService.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace DotNetScoringService.Controllers
{
    [Authorize]
    public class CalculationsController : Controller
    {
        private readonly ICalculationService _calculationService;

        public CalculationsController(ICalculationService calculationService)
        {
            _calculationService = calculationService;
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,Age,Income,HomeOwnership,EmploymentLength,LoanIntent,LoanAmount,LoanInterestRate,LoanTerm,LoanDefault,CreditHistoryLength,DateCreated")] Calculation calculation)
        {
            if (ModelState.IsValid)
            {
                await _calculationService.CreateCalculationAsync(calculation);

                return RedirectToAction("Result", new {id = calculation.ID});
            }
            
            return View(calculation);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var calculation = await _calculationService.GetCalculationByIdAsync(id);

            return calculation != null ? View(calculation) : NotFound();
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _calculationService.RemoveCalculationAsync(id);
            return Redirect("~/Home/Index");
        }

        /*private bool CalculationExists(int id)
        {
            return (_context.Calculation?.Any(e => e.ID == id)).GetValueOrDefault();
        }*/

        public async Task<IActionResult> Result(int id)
        {
            var finalResult = await _calculationService.GetCalculationResultById(id);

            return finalResult != null ? View(finalResult) : NotFound();
        }
    }
}
