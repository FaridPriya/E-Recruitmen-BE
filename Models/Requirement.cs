using ERecruitmentBE.Interfaces;

namespace ERecruitmentBE.Models
{
    public class Requirement : IPublicPropModel
    {
        public Requirement()
        {
            Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }
        public string JobVacancyId { get; set; }
        public string ApplicantSpecificationId { get; set; }
        public string ApplicantSpecificationName { get; set; }
        public DateTimeOffset? CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        public bool Deleted { get; set; }
    }
}
