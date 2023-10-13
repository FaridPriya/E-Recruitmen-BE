using AutoMapper;
using ERecruitmentBE.Data;
using ERecruitmentBE.DTO.Candidate;
using ERecruitmentBE.DTO.JobVacancys;
using ERecruitmentBE.Models;
using ERecruitmentBE.Repo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ERecruitmentBE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CandidateController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly CandidateRepository _candidateRepository;
        private readonly JobVacancyRepository _jobVacancyRepository;
        private readonly IMapper _mapper;
        public CandidateController(AppDbContext db, IMapper mapper)
        {
            _db = db;
            _candidateRepository = new CandidateRepository(db);
            _jobVacancyRepository = new JobVacancyRepository(db);
            _mapper = mapper;
        }

        // GET: api/<CandidateController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var listCandidate = await _candidateRepository.GetAllCandidate();
            var listJob = new List<JobVacancyDTO>();
            foreach(var candidate in listCandidate)
            {
                var selectedJob = listJob.FirstOrDefault(a => a.Id == candidate.IdJobVacancy);
                if(selectedJob == null)
                {
                    var job = await _jobVacancyRepository.GetJobVacancyName(candidate.IdJobVacancy);
                    if(job != null)
                    {
                        candidate.JobVacancyName = job.Name;
                        listJob.Add(job);
                    }
                }
                else
                {
                    candidate.JobVacancyName = selectedJob.Name;
                }
                
            }
            return Ok(listCandidate);
        }

        // GET api/<CandidateController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetbyId(string id)
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
        public async Task<IActionResult> Post([FromBody] CandidatePostDTO candidatePost)
        {
            await using var trx = await _db.Database.BeginTransactionAsync();
            try
            {
                Candidate candidate = new Candidate();
                candidate = _mapper.Map<Candidate>(candidatePost);

                candidate.Status = DTO.STATUS_CANDIDATE.InProgress;
                candidate.AIScreeningStatus = DTO.CV_SCREENING_AI_STATUS.Pending;
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
