using ERecruitmentBE.Data;
using ERecruitmentBE.DTO;
using ERecruitmentBE.DTO.Candidate;
using ERecruitmentBE.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using static Azure.Core.HttpHeader;

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

        public async Task<Candidate> GetCandidateModelByToken(ClaimsIdentity claimsIdentity)
        {
            var candidateId = claimsIdentity.Claims.FirstOrDefault(a => a.Type == "CandidateId")?.Value ?? "";
            var props = await _db.Candidates.Where(a => !a.Deleted && a.Id == candidateId).FirstOrDefaultAsync();
            return props;
        }

        public async Task<CandidateDTO> GetCandidateDTOById(string id)
        {
            var props = await _db.Candidates.Where(a => !a.Deleted && a.Id == id).Select(CandidateDTO.SELECT).FirstOrDefaultAsync();
            return props;
        }

        public async Task<CandidateDTO> GetCandidateByToken(ClaimsIdentity claimsIdentity)
        {
            var candidateId = claimsIdentity.Claims.FirstOrDefault(a => a.Type == "CandidateId")?.Value ?? "";
            var props = await _db.Candidates.Where(a => !a.Deleted && a.Id == candidateId).Select(CandidateDTO.SELECT_FOR_CANDIDATE).FirstOrDefaultAsync();
            return props;
        }
        public async Task<List<CandidateSpecification>> GetCandidateSpec(string id)
        {
            var props = await _db.CandidateSpecifications.Where(a => !a.Deleted && a.CandidateId == id).ToListAsync();
            return props;
        }

        public async Task<List<CandidateDTO>> GetAllCandidate()
        {
            var props = await _db.Candidates.Where(a => !a.Deleted)
                .Select(CandidateDTO.SELECT)
                .OrderByDescending(a=>a.CreatedAt)
                .ToListAsync();
            return props;
        }

        public bool IsCandidateExist(RegisterCandidateDTO candidateReg)
        {
            var props = _db.Candidates.Where(a => !a.Deleted && a.Email.ToLower() == candidateReg.Email.ToLower() && a.IdJobVacancy.ToLower() == candidateReg.IdJobVacancy.ToLower()
            && a.Name.ToLower() == candidateReg.Name.ToLower()).Any();
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
