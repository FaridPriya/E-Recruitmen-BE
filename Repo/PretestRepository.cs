using ERecruitmentBE.Data;
using ERecruitmentBE.DTO.ApplicantSpecification;
using ERecruitmentBE.DTO.Pretest;
using ERecruitmentBE.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ERecruitmentBE.Repo
{
    public class PretestRepository : IDisposable
    {
        private readonly AppDbContext _db;
        public PretestRepository(AppDbContext db)
        {
            _db = db;
        }

        public string GetCandidateId(ClaimsIdentity claimsIdentity)
        {
            var candidateId = claimsIdentity.Claims.FirstOrDefault(a => a.Type == "CandidateId")?.Value ?? "";
            return candidateId;
        }

        public string GetCandidateJobId(ClaimsIdentity claimsIdentity)
        {
            var jobId = claimsIdentity.Claims.FirstOrDefault(a => a.Type == "JobId")?.Value ?? "";
            return jobId;
        }

        public bool IsPretestNameExist(string name)
        {
            return _db.PretestQuestions.Any(a => a.Name.ToLower() == name.ToLower() && !a.Deleted);
        }

        public bool IsPretestAnswerExist(ClaimsIdentity claimsIdentity, string pretestId, string pretestitemId)
        {
            var candidateId = claimsIdentity.Claims.FirstOrDefault(a => a.Type == "CandidateId")?.Value ?? "";
            return _db.PretestAnswers.Where(a => !a.Deleted && a.CandidateId == candidateId && a.PretestQuestionId == pretestId && a.PretestQuestionItemId == pretestitemId).Any();
        }

        public List<PretestAnswer> GetPretestAnswerCandidate(string candidateId)
        {
            return _db.PretestAnswers.Where(a => !a.Deleted && a.CandidateId == candidateId)
                .OrderBy(a=>a.CreatedAt)
                .ToList();
        }

        public bool IsPretestNameExistById(string id)
        {
            return _db.PretestQuestions.Any(a => a.Id.ToLower() == id.ToLower() && !a.Deleted);
        }

        public bool IsPretestNameExist(string name, string id)
        {
            return _db.PretestQuestions.Any(a => a.Name.ToLower() == name.ToLower() && !a.Deleted && a.Id != id);
        }

        public IQueryable<PretestQuestionDTO> GetPretestQuestionDataTabel()
        {
            var props = _db.PretestQuestions.Where(a => !a.Deleted)
                .Select(PretestQuestionDTO.SELECT_DATA_TABLE)
                .AsQueryable();
            return props;
        }

        public async Task<PretestQuestion> GetPretestQuestionById(string id)
        {
            var props = await _db.PretestQuestions.Where(a => !a.Deleted && a.Id == id)
                .Include(a => a.ListPretestQuestionItem)
                .FirstOrDefaultAsync();
            return props;
        }

        public async Task<PretestQuestionItem> GetPretestItem(string id, int skip)
        {
            var props = await _db.PretestQuestionItems.Where(a => !a.Deleted && a.PretestQuestionId == id)
                .OrderBy(a=>a.IndexNo)
                .Skip(skip).Take(1).FirstOrDefaultAsync();
            return props;
        }

        public bool IsCandidateAlreadyAnswer(ClaimsIdentity claimsIdentity)
        {
            var candidateId = claimsIdentity.Claims.FirstOrDefault(a => a.Type == "CandidateId")?.Value ?? "";
            var props = _db.PretestAnswers.Where(a => !a.Deleted && a.CandidateId == candidateId)
                .Any();
            return props;
        }

        public void Insert(PretestQuestion pretestQuestion)
        {
            _db.PretestQuestions.Add(pretestQuestion);
        }

        public void InsertAnswer(PretestAnswer pretestAnswer)
        {
            _db.PretestAnswers.Add(pretestAnswer);
        }

        public void Update(PretestQuestion pretestQuestion)
        {
            _db.Entry(pretestQuestion).State = EntityState.Modified;
        }

        public void Delete(PretestQuestion pretestQuestion)
        {
            _db.PretestQuestions.Remove(pretestQuestion);
        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }

        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _db.Dispose();
                }
            }
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
