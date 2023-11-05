using AutoMapper;
using ERecruitmentBE.Data;
using ERecruitmentBE.DTO.ApplicantSpecification;
using ERecruitmentBE.DTO.Pretest;
using ERecruitmentBE.Models;
using ERecruitmentBE.Repo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ERecruitmentBE.Controllers
{
    [Route("api/Pretest")]
    [ApiController]
    [Authorize]
    public class PretestController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly PretestRepository _pretestRepository;
        private readonly JobVacancyRepository _jobVacancyRepository;
        private readonly IMapper _mapper;

        public PretestController(AppDbContext db, IMapper mapper)
        {
            _db = db;
            _pretestRepository = new PretestRepository(db);
            _jobVacancyRepository = new JobVacancyRepository(db);
            _mapper = mapper;
        }

        // GET: api/<PretestController>
        [HttpGet("IsCandidateAnswer")]
        [Authorize(Roles = "Candidate")]
        public IActionResult GetPretestItem()
        {
            try
            {
                var data = _pretestRepository.IsCandidateAlreadyAnswer(User.Identity as ClaimsIdentity);
                return Ok(data);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // GET: api/<PretestController>
        [HttpGet("PretestCandidateAnswers/{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetPretestCandidateAnswers(string id)
        {
            try
            {
                var data = _pretestRepository.GetPretestAnswerCandidate(id);
                return Ok(data);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // GET: api/<PretestController>
        [HttpGet("PretestItems")]
        [Authorize(Roles = "Candidate")]
        public async Task<IActionResult> GetPretestItem(int skip)
        {
            try
            {
                var jobId = _pretestRepository.GetCandidateJobId(User.Identity as ClaimsIdentity);
                var job = await _jobVacancyRepository.GetJobVacancyOnlyById(jobId);
                if(job != null)
                {
                    var data = await _pretestRepository.GetPretestItem(job.PretestQuestionId, skip);
                    return Ok(data);
                }
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // GET: api/<PretestController>
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var data = _pretestRepository.GetPretestQuestionDataTabel().ToList();
                return Ok(data);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // GET api/<PretestController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var data = await _pretestRepository.GetPretestQuestionById(id);
            if (data == null)
            {
                return BadRequest("Data not found");
            }
            return Ok(data);
        }

        // POST api/<PretestController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PretestPostDTO pretestPostDTO)
        {
            if (!pretestPostDTO.ListPretestQuestionItem.Any())
            {
                return BadRequest("Question cannot empty");
            }

            var isDuplicatedValue = pretestPostDTO.ListPretestQuestionItem.GroupBy(a => a.Question)
                .Select(a => new { a.Key, keyCount = a.Count() })
                .Where(a => a.keyCount > 1).Any();

            if (isDuplicatedValue)
            {
                return BadRequest("Cannot input duplicated question !");
            }

            var isDataAny = _pretestRepository.IsPretestNameExist(pretestPostDTO.Name);
            if (isDataAny)
            {
                return BadRequest("Data is already exist !");
            }

            var pretest = _mapper.Map<PretestQuestion>(pretestPostDTO);
            if (pretest.ListPretestQuestionItem.Any())
            {
                var listItem = pretest.ListPretestQuestionItem.Select(a => {
                    a.PretestQuestionId = pretest.Id;
                    return a;
                }).ToList();
                pretest.ListPretestQuestionItem = listItem;
            }

            await using var trx = await _db.Database.BeginTransactionAsync();
            try
            {
                _pretestRepository.Insert(pretest);
                await _pretestRepository.SaveAsync();
                await trx.CommitAsync();
                return Ok(pretest);
            }
            catch (Exception e)
            {
                await trx.RollbackAsync();
                return BadRequest(e.Message);
            }
        }

        // POST api/<PretestController>
        [HttpPost("Pretestanswer")]
        [Authorize(Roles = "Candidate")]
        public async Task<IActionResult> PostAnswer([FromBody] PretestAnswerPostDTO pretestPostDTO)
        {
            if (pretestPostDTO.Answer == null)
            {
                return BadRequest("Answer cannot empty");
            }

            var isDataAny = _pretestRepository.IsPretestAnswerExist(User.Identity as ClaimsIdentity, pretestPostDTO.PretestQuestionId, pretestPostDTO.PretestQuestionItemId);
            if (isDataAny)
            {
                return BadRequest("Answer is already exist !");
            }

            var candidateId = _pretestRepository.GetCandidateId(User.Identity as ClaimsIdentity);
            var pretest = _mapper.Map<PretestAnswer>(pretestPostDTO);
            pretest.CandidateId = candidateId;

            await using var trx = await _db.Database.BeginTransactionAsync();
            try
            {
                _pretestRepository.InsertAnswer(pretest);
                await _pretestRepository.SaveAsync();
                await trx.CommitAsync();
                return Ok(pretest);
            }
            catch (Exception e)
            {
                await trx.RollbackAsync();
                return BadRequest(e.Message);
            }
        }

        // PUT api/<PretestController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] PretestQuestion latestPretestQuestion)
        {
            if(id != latestPretestQuestion.Id)
            {
                return BadRequest("Id not same");
            }

            if (!latestPretestQuestion.ListPretestQuestionItem.Any())
            {
                return BadRequest("Question cannot empty");
            }

            var isDuplicatedValue = latestPretestQuestion.ListPretestQuestionItem.GroupBy(a => a.Question)
                .Select(a => new { a.Key, keyCount = a.Count() })
                .Where(a => a.keyCount > 1).Any();

            if (isDuplicatedValue)
            {
                return BadRequest("Cannot input duplicated question !");
            }

            var pretest = await _pretestRepository.GetPretestQuestionById(id);
            if (pretest == null)
            {
                return BadRequest("Data is already exist !");
            }

            foreach (var item in pretest.ListPretestQuestionItem.ToList())
            {
                var latestItem = latestPretestQuestion.ListPretestQuestionItem.FirstOrDefault(a => a.Id == item.Id);
                if (latestItem == null)
                {
                    pretest.ListPretestQuestionItem.Remove(item);
                }
                else
                {
                    item.Question = latestItem.Question;
                }
            }

            var listCurrentId = pretest.ListPretestQuestionItem.Select(a => a.Id).ToList();
            foreach (var latestItem in latestPretestQuestion.ListPretestQuestionItem)
            {
                if (listCurrentId.Contains(latestItem.Id)) continue;
                latestItem.PretestQuestionId = id;
                pretest.ListPretestQuestionItem.Add(latestItem);
            }

            await using var trx = await _db.Database.BeginTransactionAsync();
            try
            {
                pretest.Name = latestPretestQuestion.Name;
                pretest.Description = latestPretestQuestion.Description;
                _pretestRepository.Update(pretest);
                await _pretestRepository.SaveAsync();
                await trx.CommitAsync();
                return Ok(pretest);
            }
            catch (Exception e)
            {
                await trx.RollbackAsync();
                return BadRequest(e.Message);
            }
        }

        // DELETE api/<PretestController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var pretest = await _pretestRepository.GetPretestQuestionById(id);
            if (pretest == null)
            {
                return BadRequest("Data is not found");
            }
            await using var trx = await _db.Database.BeginTransactionAsync();
            try
            {
                _pretestRepository.Delete(pretest);
                await _pretestRepository.SaveAsync();
                await trx.CommitAsync();
                return Ok();
            }
            catch (Exception e)
            {
                await trx.RollbackAsync();
                return BadRequest(e.Message);
            }
        }
    }
}
