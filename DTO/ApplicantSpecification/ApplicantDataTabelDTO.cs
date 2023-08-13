using ERecruitmentBE.Models;
using System.Linq.Expressions;

namespace ERecruitmentBE.DTO.ApplicantSpecification
{
    public class ApplicantDataTabelDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public int CountItem { get; set; }
        public List<ApplicantSpecificationItem> ListApplicantSpecificationsItem { get; set; }
        public DateTimeOffset? CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        public bool Deleted { get; set; }

        public static Expression<Func<Models.ApplicantSpecification, ApplicantDataTabelDTO>> SELECT_DATA_TABLE = x => new ApplicantDataTabelDTO
        {
            Id = x.Id,
            Name = x.Name,
            Description = x.Description,
            Deleted = x.Deleted,
        };
    }
}
