namespace ERecruitmentBE.DTO
{
    public class Constanta
    {
    }

    public enum STATUS_CANDIDATE
    {
        InProgress = 0,
        Passed = 1,
        Failed = 2,
        rejected = 3
    }

    public enum USER_TYPE
    {
        Admin = 0,
        Candidate = 1
    }

    public enum CV_SCREENING_AI_STATUS
    {
        Pending = 0,
        Success = 1,
        Fail =2
    }

    public enum APPLICANT_SPECIFICATION_TYPE
    {
        Skill = 0,
        Experience = 1,
        Education = 2
    }
}
