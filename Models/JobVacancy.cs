using ERecruitmentBE.DTO;
using ERecruitmentBE.Interfaces;

namespace ERecruitmentBE.Models
{
    public class JobVacancy : IPublicPropModel
    {
        public JobVacancy()
        {
            Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public List<Requirement> ListRequirement { get; set; }
        public DateTimeOffset? CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        public bool Deleted { get; set; }
    }
}
