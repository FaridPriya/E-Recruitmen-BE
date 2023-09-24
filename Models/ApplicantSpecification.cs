using ERecruitmentBE.DTO;
using ERecruitmentBE.Interfaces;

namespace ERecruitmentBE.Models
{
    public class ApplicantSpecification : IPublicPropModel
    {
        public ApplicantSpecification()
        {
            Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public APPLICANT_SPECIFICATION_TYPE Type { get; set; }
        public List<ApplicantSpecificationItem> ListApplicantSpecificationsItem { get; set; }
        public DateTimeOffset? CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        public bool Deleted { get; set; }
    }
}
