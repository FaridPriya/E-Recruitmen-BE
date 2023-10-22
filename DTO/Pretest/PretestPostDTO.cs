namespace ERecruitmentBE.DTO.Pretest
{
    public class PretestPostDTO
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public List<PretestQuestionItemPostDTO> ListPretestQuestionItem { get; set; }
    }

    public class PretestQuestionItemPostDTO
    {
        public int IndexNo { get; set; }
        public string Question { get; set; }
    }
}
