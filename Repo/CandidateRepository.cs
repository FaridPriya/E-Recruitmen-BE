using ERecruitmentBE.Data;
using ERecruitmentBE.Models;
using Microsoft.EntityFrameworkCore;

namespace ERecruitmentBE.Repo
{
    public class CandidateRepository
    {
        private readonly AppDbContext _db;
        public CandidateRepository(AppDbContext db)
        {
            _db = db;
        }
        public async Task<Candidate> GetCandidateById(string id)
        {
            var props = await _db.Candidates.Where(a => !a.Deleted && a.Id == id).FirstOrDefaultAsync();
            return props;
        }

        public void InsertCandidate(Candidate candidate)
        {
            _db.Candidates.Add(candidate);
        }

        public void InsertCandidateSpecification(CandidateSpecification candidateSpecification)
        {
            _db.CandidateSpecifications.Add(candidateSpecification);
        }
        public void UpdateCandidate(Candidate candidate)
        {
            _db.Entry(candidate).State = EntityState.Modified;
        }

        public void DeleteCandidate(Candidate candidate)
        {
            candidate.Deleted = true;
            _db.Entry(candidate).State = EntityState.Modified;
        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
