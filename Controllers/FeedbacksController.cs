using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DotNetScoringService.Data;
using DotNetScoringService.Models;
using DotNetScoringService.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace DotNetScoringService.Controllers
{
    [Authorize]
    public class FeedbacksController : Controller
    {
        private readonly IFeedbackService _feedbackService;

        public FeedbacksController(IFeedbackService feedbackServic)
        {
            _feedbackService = feedbackServic;
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,Email,Message,DateCreated,UserId")] Feedback feedback)
        {
            if (ModelState.IsValid)
            {
                await _feedbackService.CreateFeedbackAsync(feedback);

                return Redirect("~/Home/Index");
            }

            return View(feedback);
        }
    }
}
