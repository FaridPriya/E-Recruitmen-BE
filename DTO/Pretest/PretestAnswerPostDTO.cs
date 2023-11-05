namespace ERecruitmentBE.DTO.Pretest
{
    public class PretestAnswerPostDTO
    {
        public string PretestQuestionId { get; set; }
        public string PretestQuestionItemId { get; set; }
        public string Question { get; set; }
        public string? Answer { get; set; }
        public int Duration { get; set; }
    }
}
