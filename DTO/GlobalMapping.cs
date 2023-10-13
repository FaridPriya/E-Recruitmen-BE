using AutoMapper;
using ERecruitmentBE.DTO.ApplicantSpecification;
using ERecruitmentBE.DTO.Candidate;
using ERecruitmentBE.DTO.JobVacancys;

namespace ERecruitmentBE.DTO
{
    public class GlobalMapping : Profile
    {
        public GlobalMapping()
        {
            CreateMap<ApplicantSpecificationItemVM, Models.ApplicantSpecificationItem>();
            CreateMap<ApplicantSpecificationVM, Models.ApplicantSpecification>();
            CreateMap<CandidatePostDTO, Models.Candidate>();
            CreateMap<JobVacancyVM, Models.JobVacancy>();
            CreateMap<RequirementVM, Models.Requirement>();
        }
    }
}
