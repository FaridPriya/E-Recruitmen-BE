using ERecruitmentBE.Data;
using ERecruitmentBE.DTO.ApplicantSpecification;
using ERecruitmentBE.Models;
using Microsoft.EntityFrameworkCore;

namespace ERecruitmentBE.Repo
{
    public class ApplicantSpecificationRepository
    {
        private readonly AppDbContext _db;
        public ApplicantSpecificationRepository(AppDbContext db)
        {
            _db = db;
        }
        public IQueryable<ApplicantDataTabelDTO> GetApplicantSpecificationDataTabel()
        {
            var props = _db.ApplicantSpecifications.Where(a => !a.Deleted)
                .Select(ApplicantDataTabelDTO.SELECT_DATA_TABLE)
                .AsQueryable();
            return props;
        }
        public async Task<ApplicantSpecification> GetApplicantSpecificationById(string id)
        {
            var props = await _db.ApplicantSpecifications.Where(a => !a.Deleted && a.Id == id)
                .Include(a=>a.ListApplicantSpecificationsItem)
                .FirstOrDefaultAsync();
            return props;
        }

        public async Task<List<ApplicantSpecification>> GetAllApplicantSpecificationWithItem()
        {
            var props = await _db.ApplicantSpecifications.Where(a => !a.Deleted)
                .Include(a => a.ListApplicantSpecificationsItem)
                .ToListAsync();
            return props;
        }

        public async Task<ApplicantSpecification> GetApplicantSpecificationOnlyById(string id)
        {
            var props = await _db.ApplicantSpecifications.Where(a => !a.Deleted && a.Id == id)
                .FirstOrDefaultAsync();
            return props;
        }

        public async Task<List<ApplicantSpecificationItem>> GetApplicantSpecificationItemByApplicantId(List<string> applicantId)
        {
            var props = await _db.ApplicantSpecificationItems.Where(a => !a.Deleted && applicantId.Contains(a.ApplicantSpecificationId))
                .ToListAsync();
            return props;
        }

        public bool IsApplicantSpecificationAny(string name)
        {
            return _db.ApplicantSpecifications.Any(a => a.Name.ToLower() == name.ToLower() && !a.Deleted);
        }

        public void InsertApplicantSpecification(ApplicantSpecification applicantSpecification)
        {
            _db.ApplicantSpecifications.Add(applicantSpecification);
        }
        public void UpdateApplicantSpecification(ApplicantSpecification applicantSpecification)
        {
            _db.Entry(applicantSpecification).State = EntityState.Modified;
        }

        public void DeleteApplicantSpecification(ApplicantSpecification applicantSpecification)
        {
            applicantSpecification.Deleted = true;
            applicantSpecification.ListApplicantSpecificationsItem.ForEach(item =>
            {
                item.Deleted = true;
            });
            _db.Entry(applicantSpecification).State = EntityState.Modified;
        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
