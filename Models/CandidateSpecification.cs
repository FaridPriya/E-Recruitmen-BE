using ERecruitmentBE.Interfaces;

namespace ERecruitmentBE.Models
{
    public class CandidateSpecification : IPublicPropModel
    {
        public CandidateSpecification()
        {
            Id = Guid.NewGuid().ToString();
        }
        public string Id { get; set; }
        public string CandidateId { get; set; }
        public string ApplicantId { get; set; }
        public string ApplicantItemId { get; set; }
        public string ApplicantItemName { get; set; }
        public DateTimeOffset? CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        public bool Deleted { get; set; }
    }
}
