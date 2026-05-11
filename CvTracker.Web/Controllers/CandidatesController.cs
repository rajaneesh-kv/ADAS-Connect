using System;
using System.Collections.Generic;
using System.Globalization;
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
    [Authorize]
    public class CandidatesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public CandidatesController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        [HttpGet]
        public async Task<IActionResult> Index(
            string search,
            string name,
            string email,
            string phone,
            string dateFrom,
            string dateTo,
            CandidateStatus? status)
        {
            var query = _context.Candidates.Where(x => !x.IsDeleted);

            if (!string.IsNullOrWhiteSpace(search))
            {
                var term = search.Trim().ToLowerInvariant();
                query = query.Where(x =>
                    x.Name.ToLower().Contains(term) ||
                    x.Email.ToLower().Contains(term) ||
                    x.Phone.ToLower().Contains(term));
            }

            if (!string.IsNullOrWhiteSpace(name))
            {
                var n = name.Trim().ToLowerInvariant();
                query = query.Where(x => x.Name.ToLower().Contains(n));
            }

            if (!string.IsNullOrWhiteSpace(email))
            {
                var e = email.Trim().ToLowerInvariant();
                query = query.Where(x => x.Email.ToLower().Contains(e));
            }

            if (!string.IsNullOrWhiteSpace(phone))
            {
                var p = phone.Trim().ToLowerInvariant();
                query = query.Where(x => x.Phone.ToLower().Contains(p));
            }

            if (TryParseDateUtcStart(dateFrom, out var fromUtc))
            {
                query = query.Where(x => x.CreatedAt >= fromUtc);
            }

            if (TryParseDateUtcEndExclusive(dateTo, out var toExclusive))
            {
                query = query.Where(x => x.CreatedAt < toExclusive);
            }

            if (status.HasValue)
            {
                query = query.Where(x => x.Status == status.Value);
            }

            var model = new CandidatesIndexViewModel
            {
                Search = search,
                Name = name,
                Email = email,
                Phone = phone,
                DateFrom = dateFrom,
                DateTo = dateTo,
                Status = status,
                Candidates = await query
                    .OrderByDescending(x => x.CreatedAt)
                    .ToListAsync()
            };

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> HistoryLookup(
            string search,
            string name,
            string email,
            string phone,
            string historyFrom,
            string historyTo)
        {
            var query = _context.Candidates.Where(x => !x.IsDeleted);

            if (!string.IsNullOrWhiteSpace(search))
            {
                var term = search.Trim().ToLowerInvariant();
                query = query.Where(x =>
                    x.Name.ToLower().Contains(term) ||
                    x.Email.ToLower().Contains(term) ||
                    x.Phone.ToLower().Contains(term));
            }

            if (!string.IsNullOrWhiteSpace(name))
            {
                var n = name.Trim().ToLowerInvariant();
                query = query.Where(x => x.Name.ToLower().Contains(n));
            }

            if (!string.IsNullOrWhiteSpace(email))
            {
                var e = email.Trim().ToLowerInvariant();
                query = query.Where(x => x.Email.ToLower().Contains(e));
            }

            if (!string.IsNullOrWhiteSpace(phone))
            {
                var p = phone.Trim().ToLowerInvariant();
                query = query.Where(x => x.Phone.ToLower().Contains(p));
            }

            var hasHistFrom = TryParseDateUtcStart(historyFrom, out var histFromUtc);
            var hasHistTo = TryParseDateUtcEndExclusive(historyTo, out var histToExclusive);

            if (hasHistFrom || hasHistTo)
            {
                var histQuery = _context.CandidateStatusHistories.AsQueryable();
                if (hasHistFrom)
                {
                    histQuery = histQuery.Where(h => h.ChangedAt >= histFromUtc);
                }

                if (hasHistTo)
                {
                    histQuery = histQuery.Where(h => h.ChangedAt < histToExclusive);
                }

                var idsInRange = await histQuery.Select(h => h.CandidateId).Distinct().ToListAsync();
                query = query.Where(c => idsInRange.Contains(c.Id));
            }

            var candidates = await query
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();

            var ids = candidates.Select(c => c.Id).ToList();
            var histories = await _context.CandidateStatusHistories
                .Where(h => ids.Contains(h.CandidateId))
                .ToListAsync();

            IEnumerable<CandidateStatusHistory> historiesForCounts(IEnumerable<CandidateStatusHistory> all, int candidateId)
            {
                var list = all.Where(h => h.CandidateId == candidateId);
                if (hasHistFrom)
                {
                    list = list.Where(h => h.ChangedAt >= histFromUtc);
                }

                if (hasHistTo)
                {
                    list = list.Where(h => h.ChangedAt < histToExclusive);
                }

                return list;
            }

            var rows = new List<HistoryLookupRow>();
            foreach (var c in candidates)
            {
                var slice = historiesForCounts(histories, c.Id).ToList();
                var counts = BuildHistoryCounts(slice);
                rows.Add(new HistoryLookupRow
                {
                    CandidateId = c.Id,
                    Name = c.Name,
                    Email = c.Email,
                    Phone = c.Phone,
                    CurrentStatus = c.Status,
                    InterviewsAttended = c.InterviewCount,
                    HistoryCounts = counts
                });
            }

            var model = new HistoryLookupViewModel
            {
                Search = search,
                Name = name,
                Email = email,
                Phone = phone,
                HistoryFrom = historyFrom,
                HistoryTo = historyTo,
                Rows = rows
            };

            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Archived()
        {
            var list = await _context.Candidates
                .Where(x => x.IsDeleted)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
            return View(list);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Restore(int id)
        {
            var candidate = await _context.Candidates.FirstOrDefaultAsync(x => x.Id == id);
            if (candidate == null)
            {
                return NotFound();
            }

            candidate.IsDeleted = false;
            await _context.SaveChangesAsync();
            TempData["Success"] = "Candidate restored.";
            return RedirectToAction(nameof(Details), new { id });
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new CandidateCreateViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CandidateCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var extension = Path.GetExtension(model.CvFile.FileName).ToLowerInvariant();
            var allowed = new[] { ".pdf", ".doc", ".docx" };
            if (!allowed.Contains(extension))
            {
                ModelState.AddModelError(nameof(model.CvFile), "Only PDF, DOC, and DOCX files are allowed.");
                return View(model);
            }

            var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads", "cv");
            Directory.CreateDirectory(uploadsFolder);

            var fileName = $"{Guid.NewGuid():N}{extension}";
            var physicalPath = Path.Combine(uploadsFolder, fileName);
            await using (var stream = System.IO.File.Create(physicalPath))
            {
                await model.CvFile.CopyToAsync(stream);
            }

            var candidate = new Candidate
            {
                Name = model.Name.Trim(),
                Email = model.Email.Trim().ToLowerInvariant(),
                Phone = model.Phone.Trim(),
                CVPath = $"/uploads/cv/{fileName}",
                Status = CandidateStatus.NotCalled,
                InterviewCount = 0,
                CreatedAt = DateTime.UtcNow
            };

            _context.Candidates.Add(candidate);
            await _context.SaveChangesAsync();

            _context.CandidateStatusHistories.Add(new CandidateStatusHistory
            {
                CandidateId = candidate.Id,
                Status = candidate.Status,
                ChangedAt = DateTime.UtcNow
            });
            await _context.SaveChangesAsync();

            TempData["Success"] = "Candidate uploaded successfully.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var candidate = await _context.Candidates
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

            if (candidate == null)
            {
                return NotFound();
            }

            var history = await _context.CandidateStatusHistories
                .Where(x => x.CandidateId == id)
                .OrderByDescending(x => x.ChangedAt)
                .ToListAsync();

            var model = new CandidateDetailsViewModel
            {
                Candidate = candidate,
                History = history,
                HistoryCounts = BuildHistoryCounts(history)
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(int id, CandidateStatus status)
        {
            var candidate = await _context.Candidates
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

            if (candidate == null)
            {
                return NotFound();
            }

            candidate.Status = status;
            if (status == CandidateStatus.Interviewed)
            {
                candidate.InterviewCount += 1;
            }

            _context.CandidateStatusHistories.Add(new CandidateStatusHistory
            {
                CandidateId = candidate.Id,
                Status = status,
                ChangedAt = DateTime.UtcNow
            });

            await _context.SaveChangesAsync();
            TempData["Success"] = "Status updated.";
            return RedirectToAction(nameof(Details), new { id });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var candidate = await _context.Candidates.FirstOrDefaultAsync(x => x.Id == id);
            if (candidate == null)
            {
                return NotFound();
            }

            candidate.IsDeleted = true;
            await _context.SaveChangesAsync();
            TempData["Success"] = "Candidate deleted.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> DownloadCv(int id)
        {
            var candidate = await _context.Candidates
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

            if (candidate == null || string.IsNullOrWhiteSpace(candidate.CVPath))
            {
                return NotFound();
            }

            var relativePath = candidate.CVPath.TrimStart('/').Replace("/", Path.DirectorySeparatorChar.ToString());
            var path = Path.Combine(_environment.WebRootPath, relativePath);
            if (!System.IO.File.Exists(path))
            {
                return NotFound();
            }

            return PhysicalFile(path, "application/octet-stream", Path.GetFileName(path));
        }

        private static Dictionary<CandidateStatus, int> BuildHistoryCounts(IEnumerable<CandidateStatusHistory> histories)
        {
            var dict = new Dictionary<CandidateStatus, int>();
            foreach (CandidateStatus s in Enum.GetValues(typeof(CandidateStatus)))
            {
                dict[s] = 0;
            }

            foreach (var h in histories)
            {
                dict[h.Status] = dict[h.Status] + 1;
            }

            return dict;
        }

        private static bool TryParseDateUtcStart(string value, out DateTime utcStart)
        {
            utcStart = default;
            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }

            if (!DateTime.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsed))
            {
                return false;
            }

            utcStart = DateTime.SpecifyKind(parsed.Date, DateTimeKind.Utc);
            return true;
        }

        private static bool TryParseDateUtcEndExclusive(string value, out DateTime utcEndExclusive)
        {
            utcEndExclusive = default;
            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }

            if (!DateTime.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsed))
            {
                return false;
            }

            utcEndExclusive = DateTime.SpecifyKind(parsed.Date.AddDays(1), DateTimeKind.Utc);
            return true;
        }
    }
}
