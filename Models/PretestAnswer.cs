using ERecruitmentBE.Interfaces;

namespace ERecruitmentBE.Models
{
    public class PretestAnswer : IPublicPropModel
    {
        public PretestAnswer()
        {
            Id = Guid.NewGuid().ToString();
        }
        public string Id { get; set; }
        public string PretestQuestionId { get; set; }
        public string PretestQuestionItemId { get; set; }
        public string CandidateId { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public int Duration { get; set; }
        public DateTimeOffset? CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        public bool Deleted { get; set; }
    }
}
