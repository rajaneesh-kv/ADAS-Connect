using System.Linq;
using System.Threading.Tasks;
using CvTracker.Web.Data;
using CvTracker.Web.Models;
using CvTracker.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CvTracker.Web.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var query = _context.Candidates.Where(x => !x.IsDeleted);

            var model = new DashboardViewModel
            {
                Total = await query.CountAsync(),
                Interviewed = await query.CountAsync(x => x.Status == CandidateStatus.Interviewed),
                Shortlisted = await query.CountAsync(x => x.Status == CandidateStatus.Shortlisted),
                Selected = await query.CountAsync(x => x.Status == CandidateStatus.Selected)
            };

            return View(model);
        }

        public async Task<IActionResult> Reports()
        {
            var query = _context.Candidates.Where(x => !x.IsDeleted);
            var model = new DashboardViewModel
            {
                Total = await query.CountAsync(),
                Interviewed = await query.CountAsync(x => x.Status == CandidateStatus.Interviewed),
                Shortlisted = await query.CountAsync(x => x.Status == CandidateStatus.Shortlisted),
                Selected = await query.CountAsync(x => x.Status == CandidateStatus.Selected)
            };

            return View(model);
        }
    }
}
