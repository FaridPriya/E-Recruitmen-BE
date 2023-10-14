namespace ERecruitmentBE.DTO.Candidate
{
    public class CandidateSpecificationDTO
    {
        public string ApplicantId { get; set; }
        public string ApplicantName { get; set; }
        public APPLICANT_SPECIFICATION_TYPE ApplicantType { get; set; }
        public string ApplicantItemId { get; set; }
        public string ApplicantItemName { get; set; }
        public bool AiPassed { get; set; }
        public DateTimeOffset? CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        public bool Deleted { get; set; }
    }
}
