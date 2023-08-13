namespace ERecruitmentBE.Interfaces
{
    public interface IPublicPropModel
    {
        string Id { get; set; }

        DateTimeOffset? CreatedAt { get; set; }

        DateTimeOffset? UpdatedAt { get; set; }

        bool Deleted { get; set; }
    }
}
