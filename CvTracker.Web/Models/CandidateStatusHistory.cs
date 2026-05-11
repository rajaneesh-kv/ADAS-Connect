using System;
using System.ComponentModel.DataAnnotations;

namespace CvTracker.Web.Models
{
    public class CandidateStatusHistory
    {
        public int Id { get; set; }

        public int CandidateId { get; set; }

        [Required]
        public CandidateStatus Status { get; set; }

        public DateTime ChangedAt { get; set; } = DateTime.UtcNow;

        public Candidate Candidate { get; set; }
    }
}
