using AutoMapper;
using ERecruitmentBE.Data;
using ERecruitmentBE.DTO.ApplicantSpecification;
using ERecruitmentBE.DTO.Pretest;
using ERecruitmentBE.Models;
using ERecruitmentBE.Repo;
using Microsoft.AspNetCore.Mvc;


namespace ERecruitmentBE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PretestController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly PretestRepository _pretestRepository;
        private readonly IMapper _mapper;

        public PretestController(AppDbContext db, IMapper mapper)
        {
            _db = db;
            _pretestRepository = new PretestRepository(db);
            _mapper = mapper;
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
