using ERecruitmentBE.Interfaces;

namespace ERecruitmentBE.Models
{
    public class PretestQuestion : IPublicPropModel
    {
        public PretestQuestion()
        {
            Id = Guid.NewGuid().ToString();
        }
        public string Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public ICollection<PretestQuestionItem> ListPretestQuestionItem { get; set; }
        public DateTimeOffset? CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        public bool Deleted { get; set; }
    }
}
