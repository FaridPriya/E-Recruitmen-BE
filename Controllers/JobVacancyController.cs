using AutoMapper;
using ERecruitmentBE.Data;
using ERecruitmentBE.DTO;
using ERecruitmentBE.DTO.ApplicantSpecification;
using ERecruitmentBE.DTO.JobVacancys;
using ERecruitmentBE.Models;
using ERecruitmentBE.Repo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ERecruitmentBE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class JobVacancyController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly JobVacancyRepository _jobVacancyRepository;
        private readonly ApplicantSpecificationRepository _applicantSpecificationRepository;
        private readonly IMapper _mapper;
        public JobVacancyController(AppDbContext db, IMapper mapper)
        {
            _db = db;
            _applicantSpecificationRepository = new ApplicantSpecificationRepository(db);
            _jobVacancyRepository = new JobVacancyRepository(db);
            _mapper = mapper;
        }

        // GET: api/<JobVacancyController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/<JobVacancyController>/GetDataTable
        [HttpPost("GetDataTable")]
        public IActionResult GetDataTable([FromBody] DataTabelParam dataTabelParam)
        {
            try
            {
                var model = _jobVacancyRepository.GetJobVacancyDataTabel();

                #region SEARCHING
                if (!string.IsNullOrWhiteSpace(dataTabelParam.Search))
                {
                    var keywords = dataTabelParam.Search.ToLower();
                    var searches = keywords.Split(' ');
                    foreach (var search in searches)
                    {
                        var dateKeyword = search.Replace('-', '/');
                        model = model.Where(x =>
                                !string.IsNullOrEmpty(x.Name) && x.Name.ToLower().Contains(search) ||
                                !string.IsNullOrEmpty(x.Description) && x.Description.ToLower().Contains(search));
                    }
                }
                #endregion

                #region PAGING
                var usersPaged = model.Skip(dataTabelParam.Start).Take(dataTabelParam.Length).ToList();
                #endregion

                var resultData = new
                {
                    draw = dataTabelParam.Draw,
                    recordsFiltered = model.Count(),
                    recordsTotal = model.Count(),
                    data = usersPaged
                };

                return Ok(resultData);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }

        // GET api/<JobVacancyController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var data = await _jobVacancyRepository.GetJobVacancyId(id);
            if (data == null)
            {
                return BadRequest("Data not found");
            }
            return Ok(data);
        }

        // POST api/<JobVacancyController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] JobVacancyVM jobVacancyVM)
        {
            var isDuplicatedValue = jobVacancyVM.ListRequirement.GroupBy(a => a.ApplicantSpecificationId)
                .Select(a => new { a.Key, keyCount = a.Count() })
                .Where(a => a.keyCount > 1).Any();

            if (isDuplicatedValue)
            {
                return BadRequest("Cannot input duplicated value !");
            }

            var isDataAny = _jobVacancyRepository.IsJobVacancyAny(jobVacancyVM.Name);
            if (isDataAny)
            {
                return BadRequest("Data is already exist !");
            }

            JobVacancy jobVacancy = new JobVacancy();
            jobVacancy = _mapper.Map<JobVacancy>(jobVacancyVM);

            if (jobVacancy.ListRequirement.Any())
            {
                var listItem = jobVacancy.ListRequirement.Select(a => {
                    a.JobVacancyId = jobVacancy.Id;
                    return a;
                }).ToList();
                jobVacancy.ListRequirement = listItem;
            }

            foreach(var item in jobVacancy.ListRequirement)
            {
                var specification = await _applicantSpecificationRepository.GetApplicantSpecificationOnlyById(item.ApplicantSpecificationId);
                if(specification == null)
                {
                    return BadRequest($"Applicant Specification with id {item.ApplicantSpecificationId} not found");
                }

                item.ApplicantSpecificationId = specification.Id;
                item.ApplicantSpecificationName = specification.Name;
            }

            await using var trx = await _db.Database.BeginTransactionAsync();
            try
            {
                _jobVacancyRepository.InsertJobVacancy(jobVacancy);
                await _jobVacancyRepository.SaveAsync();
                await trx.CommitAsync();
                return Ok(jobVacancy);
            }
            catch (Exception e)
            {
                await trx.RollbackAsync();
                return BadRequest(e.Message);
            }
        }

        // PUT api/<JobVacancyController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] JobVacancy latestJob)
        {
            if (id != latestJob.Id)
            {
                return BadRequest("Id Data is not same");
            }

            var isDuplicatedValue = latestJob.ListRequirement.GroupBy(a => a.ApplicantSpecificationId)
               .Select(a => new { a.Key, keyCount = a.Count() })
               .Where(a => a.keyCount > 1).Any();

            if (isDuplicatedValue)
            {
                return BadRequest("Cannot input duplicated value !");
            }

            var jobVacancy = await _jobVacancyRepository.GetJobVacancyId(id);
            if (jobVacancy == null)
            {
                return BadRequest("Data is not found");
            }

            foreach (var item in jobVacancy.ListRequirement.ToList())
            {
                var latestRequirement = latestJob.ListRequirement.FirstOrDefault(a => a.Id == item.Id);
                if (latestRequirement == null)
                {
                    jobVacancy.ListRequirement.Remove(item);
                }
            }

            var listCurrentRequirementId = jobVacancy.ListRequirement.Select(a => a.Id).ToList();
            foreach (var latestItem in latestJob.ListRequirement)
            {
                if (listCurrentRequirementId.Contains(latestItem.Id)) continue;
                var specification = await _applicantSpecificationRepository.GetApplicantSpecificationOnlyById(latestItem.ApplicantSpecificationId);
                if(specification == null)
                {
                    return BadRequest($"Applicant Specification with id {latestItem.ApplicantSpecificationId} not found");
                }
                latestItem.JobVacancyId = id;
                latestItem.ApplicantSpecificationId = specification.Id;
                latestItem.ApplicantSpecificationName = specification.Name;
                jobVacancy.ListRequirement.Add(latestItem);
            }

            await using var trx = await _db.Database.BeginTransactionAsync();
            try
            {
                _jobVacancyRepository.UpdateJobVacancy(jobVacancy);
                await _jobVacancyRepository.SaveAsync();
                await trx.CommitAsync();
                return Ok(jobVacancy);
            }
            catch (Exception e)
            {
                await trx.RollbackAsync();
                return BadRequest(e.Message);
            }
        }

        // DELETE api/<JobVacancyController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var data = await _jobVacancyRepository.GetJobVacancyId(id);
            if (data == null)
            {
                return BadRequest("Data is not found");
            }
            await using var trx = await _db.Database.BeginTransactionAsync();
            try
            {
                _jobVacancyRepository.DeleteJobVacancy(data);
                await _jobVacancyRepository.SaveAsync();
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
