using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CvTracker.Web.Models
{
    public class Candidate
    {
        public int Id { get; set; }

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
        public string CVPath { get; set; }

        [Required]
        public CandidateStatus Status { get; set; } = CandidateStatus.NotCalled;

        public int InterviewCount { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public bool IsDeleted { get; set; }

        public ICollection<CandidateStatusHistory> StatusHistory { get; set; } = new List<CandidateStatusHistory>();
    }
}
