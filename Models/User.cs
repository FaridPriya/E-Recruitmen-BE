using ERecruitmentBE.DTO;
using ERecruitmentBE.Interfaces;

namespace ERecruitmentBE.Models
{
    public class User : IPublicPropModel
    {
        public User()
        {
            Id = Guid.NewGuid().ToString();
        }
        public string Id { get; set; }
        public string Username { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] Salt { get; set; }
        public DateTimeOffset? CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        public bool Deleted { get; set; }
        public USER_TYPE UserType { get; set; }
    }
}
