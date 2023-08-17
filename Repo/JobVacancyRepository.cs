using ERecruitmentBE.Data;
using ERecruitmentBE.DTO.ApplicantSpecification;
using ERecruitmentBE.Models;
using Microsoft.EntityFrameworkCore;

namespace ERecruitmentBE.Repo
{
    public class JobVacancyRepository
    {
        private readonly AppDbContext _db;
        public JobVacancyRepository(AppDbContext db)
        {
            _db = db;
        }
        public IQueryable<JobVacancy> GetJobVacancyDataTabel()
        {
            var props = _db.JobVacancys.Where(a => !a.Deleted)
                .AsQueryable();
            return props;
        }
        public async Task<JobVacancy> GetJobVacancyId(string id)
        {
            var props = await _db.JobVacancys.Where(a => !a.Deleted && a.Id == id)
                .Include(a => a.ListRequirement)
                .FirstOrDefaultAsync();
            return props;
        }
        public bool IsJobVacancyAny(string name)
        {
            return _db.JobVacancys.Any(a => a.Name.ToLower() == name.ToLower() && !a.Deleted);
        }

        public void InsertJobVacancy(JobVacancy jobVacancy)
        {
            _db.JobVacancys.Add(jobVacancy);
        }
        public void UpdateJobVacancy(JobVacancy jobVacancy)
        {
            _db.Entry(jobVacancy).State = EntityState.Modified;
        }

        public void DeleteJobVacancy(JobVacancy jobVacancy)
        {
            _db.JobVacancys.Remove(jobVacancy);
        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
