namespace ERecruitmentBE.DTO
{
    public class Constanta
    {
    }

    public enum STATUS_CANDIDATE
    {
        InProgress = 0,
        Passed = 1,
        NotPassed = 2,
        rejected = 3
    }

    public enum CV_SCREENING_AI_STATUS
    {
        Pending = 0,
        Done = 1,
        Fail =2
    }

    public enum APPLICANT_SPECIFICATION_TYPE
    {
        Skill = 0,
        Experience = 1,
        Education = 2
    }
}
