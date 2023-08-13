using ERecruitmentBE.Interfaces;

namespace ERecruitmentBE.Models
{
    public class ApplicantSpecificationItem : IPublicPropModel
    {
        public ApplicantSpecificationItem()
        {
            Id = Guid.NewGuid().ToString();
        }
        public string Id { get; set; }
        public string ApplicantSpecificationId { get; set; }
        public string Name { get; set; }
        public DateTimeOffset? CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        public bool Deleted { get; set; }
    }
}
