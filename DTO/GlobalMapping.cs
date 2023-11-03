using AutoMapper;
using ERecruitmentBE.DTO.ApplicantSpecification;
using ERecruitmentBE.DTO.Candidate;
using ERecruitmentBE.DTO.JobVacancys;
using ERecruitmentBE.DTO.Pretest;

namespace ERecruitmentBE.DTO
{
    public class GlobalMapping : Profile
    {
        public GlobalMapping()
        {
            CreateMap<ApplicantSpecificationItemVM, Models.ApplicantSpecificationItem>();
            CreateMap<ApplicantSpecificationVM, Models.ApplicantSpecification>();
            CreateMap<PretestPostDTO, Models.PretestQuestion>();
            CreateMap<PretestAnswerPostDTO, Models.PretestAnswer>();
            CreateMap<PretestQuestionItemPostDTO, Models.PretestQuestionItem>();
            CreateMap<ApplicantSpecificationVM, Models.ApplicantSpecification>();
            CreateMap<CandidatePostDTO, Models.Candidate>();
            CreateMap<RegisterCandidateDTO, Models.Candidate>();
            CreateMap<JobVacancyVM, Models.JobVacancy>();
            CreateMap<RequirementVM, Models.Requirement>();
        }
    }
}
