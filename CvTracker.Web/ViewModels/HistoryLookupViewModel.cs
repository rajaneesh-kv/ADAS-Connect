using System.Collections.Generic;
using CvTracker.Web.Models;

namespace CvTracker.Web.ViewModels
{
    public class HistoryLookupViewModel
    {
        public string Search { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string HistoryFrom { get; set; }

        public string HistoryTo { get; set; }

        public List<HistoryLookupRow> Rows { get; set; } = new List<HistoryLookupRow>();
    }

    public class HistoryLookupRow
    {
        public int CandidateId { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public CandidateStatus CurrentStatus { get; set; }

        public int InterviewsAttended { get; set; }

        public Dictionary<CandidateStatus, int> HistoryCounts { get; set; } = new Dictionary<CandidateStatus, int>();
    }
}
