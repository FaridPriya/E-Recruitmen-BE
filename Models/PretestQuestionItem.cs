using ERecruitmentBE.Interfaces;

namespace ERecruitmentBE.Models
{
    public class PretestQuestionItem : IPublicPropModel
    {
        public PretestQuestionItem()
        {
            Id = Guid.NewGuid().ToString();
        }
        public string Id { get; set; }
        public string PretestQuestionId { get; set; }
        public int IndexNo { get; set; }
        public string Question { get; set; }
        public DateTimeOffset? CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        public bool Deleted { get; set; }
    }
}
