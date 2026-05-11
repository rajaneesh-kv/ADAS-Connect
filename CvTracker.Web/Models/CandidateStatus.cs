using System.ComponentModel.DataAnnotations;

namespace CvTracker.Web.Models
{
    /// <summary>
    /// Integer values are persisted — do not reorder existing values. New values must be appended.
    /// </summary>
    public enum CandidateStatus
    {
        [Display(Name = "Not called")]
        NotCalled = 0,

        [Display(Name = "Interviewed")]
        Interviewed = 1,

        [Display(Name = "Shortlisted")]
        Shortlisted = 2,

        [Display(Name = "Selected")]
        Selected = 3,

        [Display(Name = "Called")]
        Called = 4
    }
}
