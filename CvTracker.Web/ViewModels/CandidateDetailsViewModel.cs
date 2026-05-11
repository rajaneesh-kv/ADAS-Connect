using System.Collections.Generic;
using CvTracker.Web.Models;

namespace CvTracker.Web.ViewModels
{
    public class CandidateDetailsViewModel
    {
        public Candidate Candidate { get; set; }

        public List<CandidateStatusHistory> History { get; set; } = new List<CandidateStatusHistory>();

        public Dictionary<CandidateStatus, int> HistoryCounts { get; set; } = new Dictionary<CandidateStatus, int>();
    }
}
