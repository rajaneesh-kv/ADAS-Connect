using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CvTracker.Web.Data;
using CvTracker.Web.Models;
using CvTracker.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CvTracker.Web.Controllers
{
    public class RegistrationsController : Controller
    {
        private const long MaxPhotoBytes = 5 * 1024 * 1024;

        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public RegistrationsController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var list = await _context.SchoolRegistrations
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
            return View(list);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var row = await _context.SchoolRegistrations.FirstOrDefaultAsync(x => x.Id == id);
            if (row == null)
            {
                return NotFound();
            }

            return View(row);
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Create()
        {
            return View(new SchoolRegistrationCreateViewModel());
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SchoolRegistrationCreateViewModel model)
        {
            if (!model.Grade.HasValue)
            {
                ModelState.AddModelError(nameof(model.Grade), "Please choose a grade.");
            }

            if (!model.DateOfBirth.HasValue)
            {
                ModelState.AddModelError(nameof(model.DateOfBirth), "Date of birth is required.");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            string photoRelativePath = null;
            if (model.Photo != null && model.Photo.Length > 0)
            {
                var ext = Path.GetExtension(model.Photo.FileName).ToLowerInvariant();
                var allowed = new[] { ".jpg", ".jpeg", ".png", ".webp", ".gif" };
                if (!allowed.Contains(ext))
                {
                    ModelState.AddModelError(nameof(model.Photo), "Photo must be JPG, PNG, WEBP, or GIF.");
                    return View(model);
                }

                if (model.Photo.Length > MaxPhotoBytes)
                {
                    ModelState.AddModelError(nameof(model.Photo), "Photo must be 5 MB or smaller.");
                    return View(model);
                }

                var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads", "balabharathi-photos");
                Directory.CreateDirectory(uploadsFolder);

                var fileName = $"{Guid.NewGuid():N}{ext}";
                var physicalPath = Path.Combine(uploadsFolder, fileName);
                await using (var stream = System.IO.File.Create(physicalPath))
                {
                    await model.Photo.CopyToAsync(stream);
                }

                photoRelativePath = $"/uploads/balabharathi-photos/{fileName}";
            }

            var dob = model.DateOfBirth.Value.Date;
            var entity = new SchoolRegistration
            {
                ChildName = model.ChildName.Trim(),
                Grade = model.Grade.Value,
                DateOfBirth = DateTime.SpecifyKind(dob, DateTimeKind.Utc),
                PhotoPath = photoRelativePath,
                FatherName = model.FatherName.Trim(),
                FatherProfession = model.FatherProfession.Trim(),
                FatherMobile = model.FatherMobile.Trim(),
                FatherEmail = model.FatherEmail.Trim().ToLowerInvariant(),
                FatherLandline = string.IsNullOrWhiteSpace(model.FatherLandline) ? null : model.FatherLandline.Trim(),
                MotherName = model.MotherName.Trim(),
                MotherProfession = model.MotherProfession.Trim(),
                MotherMobile = model.MotherMobile.Trim(),
                MotherEmail = model.MotherEmail.Trim().ToLowerInvariant(),
                MotherLandline = string.IsNullOrWhiteSpace(model.MotherLandline) ? null : model.MotherLandline.Trim(),
                AreaOfLiving = model.AreaOfLiving.Trim(),
                CreatedAt = DateTime.UtcNow
            };

            _context.SchoolRegistrations.Add(entity);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Balabharathi registration submitted successfully. Thank you.";
            return RedirectToAction(nameof(Create));
        }
    }
}
