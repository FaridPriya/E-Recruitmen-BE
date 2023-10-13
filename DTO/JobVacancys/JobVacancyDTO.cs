using ERecruitmentBE.Models;
using System.Linq.Expressions;

namespace ERecruitmentBE.DTO.JobVacancys
{
    public class JobVacancyDTO
    {
        public JobVacancyDTO()
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

        public static Expression<Func<Models.JobVacancy, JobVacancyDTO>> SELECT = x => new JobVacancyDTO
        {
            Id = x.Id,
            Name = x.Name
        };
    }
}
