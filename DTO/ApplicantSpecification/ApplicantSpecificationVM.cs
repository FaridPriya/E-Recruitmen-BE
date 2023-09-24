namespace ERecruitmentBE.DTO.ApplicantSpecification
{
    public class ApplicantSpecificationVM
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public APPLICANT_SPECIFICATION_TYPE Type { get; set; }
        public List<ApplicantSpecificationItemVM> ListApplicantSpecificationsItem { get; set; }
    }
}
