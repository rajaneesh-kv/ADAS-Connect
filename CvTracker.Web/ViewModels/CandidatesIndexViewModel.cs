using System;
using System.Collections.Generic;
using CvTracker.Web.Models;

namespace CvTracker.Web.ViewModels
{
    public class CandidatesIndexViewModel
    {
        public string Search { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string DateFrom { get; set; }

        public string DateTo { get; set; }

        public CandidateStatus? Status { get; set; }

        public List<Candidate> Candidates { get; set; } = new List<Candidate>();
    }
}
