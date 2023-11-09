using AutoMapper;
using ERecruitmentBE.Data;
using ERecruitmentBE.DTO;
using ERecruitmentBE.DTO.JobVacancys;
using ERecruitmentBE.Helper;
using ERecruitmentBE.Repo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace ERecruitmentBE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly CandidateRepository _candidateRepository;
        private readonly JobVacancyRepository _jobVacancyRepository;
        public DashboardController(AppDbContext db, IMapper mapper)
        {
            _db = db;
            _candidateRepository = new CandidateRepository(db);
            _jobVacancyRepository = new JobVacancyRepository(db);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Get()
        {
            //GET JOB ACTIVE
            var listJobActive = _jobVacancyRepository.GetActiveJobVacancy().ToList();

            //GET CANDIDATE 1 MONTH
            var startMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var endMonth = startMonth.AddMonths(1).AddDays(-1);

            var startMonthAsUtc = FilterDateHelper.GetDateFromUtc(startMonth);
            var endMonthAsUtc = FilterDateHelper.GetDateToUtc(endMonth);

            var listCandidateMounth = await _candidateRepository.GetCandidateByTime(startMonthAsUtc, endMonthAsUtc);

            //GET CANDIDATE 1 Week
            var startWeek = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Monday);
            var endWeek = startWeek.AddDays(6);

            var startWeekAsUtc = FilterDateHelper.GetDateFromUtc(startWeek);
            var endWeekAsUtc = FilterDateHelper.GetDateToUtc(endWeek);

            var listCandidateWeek = await _candidateRepository.GetCandidateByTime(startWeekAsUtc, endWeekAsUtc);

            //GET CANDIDATE 1 Day
            var startDay = DateTime.Now;
            var endDay = DateTime.Now.AddDays(1);

            var startDayAsUtc = FilterDateHelper.GetDateFromUtc(startDay);
            var endDayAsUtc = FilterDateHelper.GetDateToUtc(endDay);

            var listCandidateDay = await _candidateRepository.GetCandidateByTime(startDayAsUtc, endDayAsUtc);


            var listJobResume = new List<ResumeJobDTO>();
            foreach(var item in listJobActive)
            {
                var candidateCount = listCandidateMounth.Where(a => a.IdJobVacancy == item.Id).Count();
                var pendingCount = listCandidateMounth.Where(a => a.Status == STATUS_CANDIDATE.Pending && a.IdJobVacancy == item.Id).Count();
                var failedCount = listCandidateMounth.Where(a => a.Status == STATUS_CANDIDATE.Failed && a.IdJobVacancy == item.Id).Count();
                var passedCount = listCandidateMounth.Where(a => a.Status == STATUS_CANDIDATE.Passed && a.IdJobVacancy == item.Id).Count();

                var job = new ResumeJobDTO()
                {
                    JobId = item.Id,
                    JobName = item.Name,
                    CandidatePendingCount = pendingCount,
                    CandidateFailedCount = failedCount,
                    CandidatePassedCount = passedCount,
                    CandidateCount = candidateCount
                };

                listJobResume.Add(job);
            }

            var result = new DashboardDTO()
            {
                ActiveJobCount = listJobActive.Count(),
                CandidateInDay = listCandidateDay.Count(),
                CandidateInMonth = listCandidateMounth.Count(),
                CandidateInWeek = listCandidateWeek.Count(),
                JobDetail = listJobResume
            };


            return Ok(result);
        }
    }
}
