using AutoMapper;
using ERecruitmentBE.Data;
using ERecruitmentBE.DTO;
using ERecruitmentBE.DTO.ApplicantSpecification;
using ERecruitmentBE.Models;
using ERecruitmentBE.Repo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace ERecruitmentBE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ApplicantSpecificationController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly ApplicantSpecificationRepository _applicantSpecificationRepository;
        private readonly IMapper _mapper;
        public ApplicantSpecificationController(AppDbContext db, IMapper mapper)
        {
            _db = db;
            _applicantSpecificationRepository = new ApplicantSpecificationRepository(db);
            _mapper = mapper;
        }

        // GET: api/<ApplicantSpecificationController>
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var data = _applicantSpecificationRepository.GetApplicantSpecificationDataTabel().ToList();
                return Ok(data);

            }catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // GET: api/<ApplicantSpecificationController>/GetDataTable
        [HttpPost("GetDataTable")]
        public IActionResult GetDataTable([FromBody] DataTabelParam dataTabelParam)
        {
            try
            {
                var model = _applicantSpecificationRepository.GetApplicantSpecificationDataTabel();

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

        // GET api/<ApplicantSpecificationController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var data = await _applicantSpecificationRepository.GetApplicantSpecificationById(id);
            if(data == null)
            {
                return BadRequest("Data not found");
            }
            return Ok(data);
        }

        // POST api/<ApplicantSpecificationController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ApplicantSpecificationVM applicantSpecificationVM)
        {
            if (!applicantSpecificationVM.ListApplicantSpecificationsItem.Any())
            {
                return BadRequest("Specification List cannot empty");
            }

            var isDuplicatedValue = applicantSpecificationVM.ListApplicantSpecificationsItem.GroupBy(a => a.Name)
                .Select(a => new { a.Key, keyCount = a.Count() })
                .Where(a => a.keyCount > 1).Any();

            if (isDuplicatedValue)
            {
                return BadRequest("Cannot input duplicated value !");
            }

            var isDataAny = _applicantSpecificationRepository.IsApplicantSpecificationAny(applicantSpecificationVM.Name);
            if (isDataAny)
            {
                return BadRequest("Data is already exist !");
            }

            ApplicantSpecification applicantSpecification = new ApplicantSpecification();
            applicantSpecification = _mapper.Map<ApplicantSpecification>(applicantSpecificationVM);

            if (applicantSpecification.ListApplicantSpecificationsItem.Any()){
                var listItem = applicantSpecification.ListApplicantSpecificationsItem.Select(a => {
                    a.ApplicantSpecificationId = applicantSpecification.Id;
                    return a;
                }).ToList();
                applicantSpecification.ListApplicantSpecificationsItem = listItem;
            }

            await using var trx = await _db.Database.BeginTransactionAsync();
            try
            {
                _applicantSpecificationRepository.InsertApplicantSpecification(applicantSpecification);
                await _applicantSpecificationRepository.SaveAsync();
                await trx.CommitAsync();
                return Ok(applicantSpecification);
            }
            catch (Exception e)
            {
                await trx.RollbackAsync();
                return BadRequest(e.Message);
            }
        }

        // PUT api/<ApplicantSpecificationController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] ApplicantSpecification latestApplicant)
        {
            if(id != latestApplicant.Id)
            {
                return BadRequest("Id Data is not same");
            }

            var isDuplicatedValue = latestApplicant.ListApplicantSpecificationsItem.GroupBy(a => a.Name)
                .Select(a => new { a.Key, keyCount = a.Count() })
                .Where(a => a.keyCount > 1).Any();

            if (isDuplicatedValue)
            {
                return BadRequest("Cannot input duplicated value !");
            }

            var applicantSpecification = await _applicantSpecificationRepository.GetApplicantSpecificationById(id);
            if (applicantSpecification == null)
            {
                return BadRequest("Data is not found");
            }

            foreach(var item in applicantSpecification.ListApplicantSpecificationsItem.ToList())
            {
                var latestItem = latestApplicant.ListApplicantSpecificationsItem.FirstOrDefault(a => a.Id == item.Id);
                if(latestItem == null)
                {
                    applicantSpecification.ListApplicantSpecificationsItem.Remove(item);
                }
                else
                {
                    item.Name = latestItem.Name;
                }
            }

            var listCurrentId = applicantSpecification.ListApplicantSpecificationsItem.Select(a => a.Id).ToList();
            foreach (var latestItem in latestApplicant.ListApplicantSpecificationsItem)
            {
                if (listCurrentId.Contains(latestItem.Id)) continue;
                latestItem.ApplicantSpecificationId = id;
                applicantSpecification.ListApplicantSpecificationsItem.Add(latestItem);
            }

            await using var trx = await _db.Database.BeginTransactionAsync();
            try
            {
                _applicantSpecificationRepository.UpdateApplicantSpecification(applicantSpecification);
                await _applicantSpecificationRepository.SaveAsync();
                await trx.CommitAsync();
                return Ok(applicantSpecification);
            }
            catch (Exception e)
            {
                await trx.RollbackAsync();
                return BadRequest(e.Message);
            }
        }

        // DELETE api/<ApplicantSpecificationController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var applicantSpecification = await _applicantSpecificationRepository.GetApplicantSpecificationById(id);
            if (applicantSpecification == null)
            {
                return BadRequest("Data is not found");
            }
            await using var trx = await _db.Database.BeginTransactionAsync();
            try
            {
                _applicantSpecificationRepository.DeleteApplicantSpecification(applicantSpecification);
                await _applicantSpecificationRepository.SaveAsync();
                await trx.CommitAsync();
                return Ok(applicantSpecification);
            }
            catch (Exception e)
            {
                await trx.RollbackAsync();
                return BadRequest(e.Message);
            }
        }
    }
}
