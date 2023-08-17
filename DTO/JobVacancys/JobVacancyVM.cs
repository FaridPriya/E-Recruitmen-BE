using ERecruitmentBE.DTO.ApplicantSpecification;

namespace ERecruitmentBE.DTO.JobVacancys
{
    public class JobVacancyVM
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public List<RequirementVM> ListRequirement { get; set; }
    }
}
