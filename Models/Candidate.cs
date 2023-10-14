using ERecruitmentBE.DTO;
using ERecruitmentBE.Interfaces;

namespace ERecruitmentBE.Models
{
    public class Candidate : IPublicPropModel
    {
        public Candidate()
        {
            Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string IdJobVacancy { get; set; }
        public string NoHandphone { get; set; }
        public string Email { get; set; }
        public string? AIScreeningResult { get; set; }
        public CV_SCREENING_AI_STATUS AIScreeningStatus { get; set; }
        public int ApplicantSpecApprove { get; set; }
        public STATUS_CANDIDATE Status { get; set; }
        public DateTimeOffset? CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        public bool Deleted { get; set; }
    }
}
