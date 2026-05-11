using CvTracker.Web.Models;

namespace CvTracker.Web.Helpers
{
    public static class StatusLabels
    {
        public static string For(CandidateStatus status)
        {
            return status switch
            {
                CandidateStatus.NotCalled => "Not called",
                CandidateStatus.Interviewed => "Interviewed",
                CandidateStatus.Shortlisted => "Shortlisted",
                CandidateStatus.Selected => "Selected",
                CandidateStatus.Called => "Called",
                _ => status.ToString()
            };
        }
    }
}
