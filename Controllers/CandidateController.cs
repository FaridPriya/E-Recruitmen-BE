using ERecruitmentBE.Data;
using ERecruitmentBE.Models;
using ERecruitmentBE.Repo;
using Microsoft.AspNetCore.Mvc;

namespace ERecruitmentBE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CandidateController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly CandidateRepository _candidateRepository;
        public CandidateController(AppDbContext db)
        {
            _db = db;
            _candidateRepository = new CandidateRepository(db);
        }

        // GET: api/<CandidateController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<CandidateController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            try
            {
                var currentCandidate = await _candidateRepository.GetCandidateById(id);
                if (currentCandidate == null) throw new Exception("Candidate not Found");
                return Ok(currentCandidate);
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
            
        }

        // POST api/<CandidateController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Candidate candidate)
        {
            await using var trx = await _db.Database.BeginTransactionAsync();
            try
            {
                candidate.Status = DTO.STATUS_CANDIDATE.InProgress;
                _candidateRepository.InsertCandidate(candidate);
                await _candidateRepository.SaveAsync();
                await trx.CommitAsync();
                return Ok(candidate);
            }
            catch(Exception e)
            {
                await trx.RollbackAsync();
                return BadRequest(e.Message);
            }
        }

        // PUT api/<CandidateController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] Candidate candidate)
        {
            await using var trx = await _db.Database.BeginTransactionAsync();
            try
            {
                var currentCandidate = await _candidateRepository.GetCandidateById(id);
                if(currentCandidate == null)
                {
                    throw new Exception("Candidate not Found");
                }

                currentCandidate.Name = candidate.Name;
                currentCandidate.Email = candidate.Email;
                currentCandidate.NoHandphone = candidate.NoHandphone;
                currentCandidate.Status = candidate.Status;

                _candidateRepository.UpdateCandidate(currentCandidate);
                await _candidateRepository.SaveAsync();
                await trx.CommitAsync();
                return Ok(candidate);
            }
            catch (Exception e)
            {
                await trx.RollbackAsync();
                return BadRequest(e.Message);
            }
        }

        // DELETE api/<CandidateController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await using var trx = await _db.Database.BeginTransactionAsync();
            try
            {
                var currentCandidate = await _candidateRepository.GetCandidateById(id);
                if (currentCandidate == null)
                {
                    throw new Exception("Candidate not Found");
                }

                _candidateRepository.DeleteCandidate(currentCandidate);
                await _candidateRepository.SaveAsync();
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
