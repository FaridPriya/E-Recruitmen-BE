using ERecruitmentBE.DTO.ApplicantSpecification;
using ERecruitmentBE.Models;
using System.Linq.Expressions;

namespace ERecruitmentBE.DTO.Pretest
{
    public class PretestQuestionDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public List<PretestQuestionItem> ListPretestQuestionItem { get; set; }
        public DateTimeOffset? CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        public bool Deleted { get; set; }

        public static Expression<Func<Models.PretestQuestion, PretestQuestionDTO>> SELECT_DATA_TABLE = x => new PretestQuestionDTO
        {
            Id = x.Id,
            Name = x.Name,
            Description = x.Description,
            Deleted = x.Deleted,
            CreatedAt = x.CreatedAt,
            UpdatedAt = x.UpdatedAt,
        };
    }
}
