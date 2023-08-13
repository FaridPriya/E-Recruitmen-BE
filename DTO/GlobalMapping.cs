using AutoMapper;
using ERecruitmentBE.DTO.ApplicantSpecification;

namespace ERecruitmentBE.DTO
{
    public class GlobalMapping : Profile
    {
        public GlobalMapping()
        {
            CreateMap<ApplicantSpecificationItemVM, Models.ApplicantSpecificationItem>();
            CreateMap<ApplicantSpecificationVM, Models.ApplicantSpecification>();
        }
    }
}
