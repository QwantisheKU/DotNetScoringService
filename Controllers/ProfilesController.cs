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
    public class ProfilesController : Controller
    {
        private readonly IProfileService _profileService;

        public ProfilesController(IProfileService profileService)
        {
            _profileService = profileService;
        }

        public async Task<IActionResult> Get(int id)
        {
            var profile = await _profileService.GetProfileByUserIdAsync(id);

            return profile != null ? View(profile) : NotFound();
        }

        public async Task<IActionResult> Edit(int id)
        {
            var profile = await _profileService.GetProfileByUserIdAsync(id);

            return profile != null ? View(profile) : NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Email,DateCreated")] Profile profile)
        {
            Profile profileView = null;
            if (ModelState.IsValid)
            {
                profileView = await _profileService.EditProfileAsync(id, profile);

                return RedirectToAction("Get", new { id = profile.ID });
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
            }
            return View(profileView);
        }
    }
}
