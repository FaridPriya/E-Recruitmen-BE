namespace ERecruitmentBE.DTO
{
    public class DashboardDTO
    {
        public int ActiveJobCount { get; set; }
        public int CandidateInMonth { get; set; }
        public int CandidateInWeek { get; set; }
        public int CandidateInDay { get; set; }
        public List<ResumeJobDTO> JobDetail { get; set; }
    }

    public class ResumeJobDTO
    {
        public string JobId { get; set; }
        public string JobName { get; set; }
        public int CandidateCount { get; set; }
        public int CandidatePendingCount { get; set; }
        public int CandidateFailedCount { get; set; }
        public int CandidatePassedCount { get; set; }
        public int CandidateRejectCount { get; set; }
    }
}
