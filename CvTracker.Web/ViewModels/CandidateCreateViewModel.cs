using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace CvTracker.Web.ViewModels
{
    public class CandidateCreateViewModel
    {
        [Required]
        [MaxLength(120)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(150)]
        public string Email { get; set; }

        [Required]
        [MaxLength(20)]
        public string Phone { get; set; }

        [Required]
        public IFormFile CvFile { get; set; }
    }
}
