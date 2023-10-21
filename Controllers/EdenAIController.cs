using AutoMapper;
using ERecruitmentBE.Data;
using ERecruitmentBE.DTO.ApplicantSpecification;
using ERecruitmentBE.Models;
using ERecruitmentBE.Repo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using System.Linq;
using System.Net.Http;
using System.Text;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ERecruitmentBE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EdenAIController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly JobVacancyRepository _jobVacancyRepository;
        private readonly CandidateRepository _candidateRepository;
        private readonly ApplicantSpecificationRepository _applicantSpecificationRepository;
        public EdenAIController(AppDbContext db, IMapper mapper)
        {
            _db = db;
            _applicantSpecificationRepository = new ApplicantSpecificationRepository(db);
            _jobVacancyRepository = new JobVacancyRepository(db);
            _candidateRepository = new CandidateRepository(db);
        }

        // POST api/<EdenAIController>
        [HttpPost("ScreeningCV/{id}")]
        public async Task<IActionResult> ScreeningCV(string id, IFormFile pdfFile)
        {
            var candidate = await _candidateRepository.GetCandidateById(id);
            if (candidate == null)
                return BadRequest("candidate not found");

            var jobRequirement = await _jobVacancyRepository.GetListRequirementJobVacancy(candidate.IdJobVacancy);
            if (!jobRequirement.Any())
                return BadRequest("job vacancy not found");

            var listApplicantSpecId = jobRequirement.Select(a => a.ApplicantSpecificationId).ToList();
            var listApplicantSpec = await _applicantSpecificationRepository.GetApplicantSpecificationItemByApplicantId(listApplicantSpecId);

            if (!jobRequirement.Any())
                return BadRequest("Applicant not found");

            try
            {
                var screeningResult = new RootHireability();
                string edenResult;
                var client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
                client.DefaultRequestHeaders.Add("Authorization", "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1c2VyX2lkIjoiYmFlZmQ5YzMtYWNjZS00MTExLTg3ZDUtMmM0MDY0YWNjOWI3IiwidHlwZSI6ImFwaV90b2tlbiJ9.PAjXg4X42ioPmnTmjuIVsNcQIGo8D3CIpQzxEE34deU");
                client.BaseAddress = new Uri("https://api.edenai.run/");

                using (var formData = new MultipartFormDataContent())
                {
                    formData.Add(new StreamContent(pdfFile.OpenReadStream()), "file", pdfFile.FileName);
                    formData.Add(new StringContent("true"), "response_as_dict");
                    formData.Add(new StringContent("false"), "attributes_as_list");
                    formData.Add(new StringContent("false"), "show_original_response");
                    formData.Add(new StringContent("hireability"), "providers");

                    //var response = await client.PostAsync("v2/ocr/resume_parser", formData);
                    //response.IsSuccessStatusCode
                    if (true)
                    {
                        //edenResult = await response.Content.ReadAsStringAsync();
                        edenResult = "{\"hireability\":{\"status\":\"success\",\"extracted_data\":{\"personal_infos\":{\"name\":{\"first_name\":\"RATNA\",\"last_name\":\"AKHIDA\",\"raw_name\":\"RATNAAKHIDA\",\"middle\":null,\"title\":null,\"prefix\":null,\"sufix\":null},\"address\":{\"formatted_location\":null,\"postal_code\":null,\"region\":null,\"country\":null,\"country_code\":null,\"raw_input_location\":null,\"street\":null,\"street_number\":null,\"appartment_number\":null,\"city\":\"\"},\"self_summary\":null,\"objective\":null,\"date_of_birth\":null,\"place_of_birth\":null,\"phones\":[\"082158214780\"],\"mails\":[\"akhida.fatikaa@gmail.com\"],\"urls\":[],\"fax\":[],\"current_profession\":null,\"gender\":null,\"nationality\":\"Indonesia\",\"martial_status\":null,\"current_salary\":null},\"education\":{\"total_years_education\":null,\"entries\":[{\"title\":\"bachelors\",\"start_date\":null,\"end_date\":\"2019-02-01\",\"location\":{\"formatted_location\":null,\"postal_code\":null,\"region\":null,\"country\":null,\"country_code\":null,\"raw_input_location\":null,\"street\":null,\"street_number\":null,\"appartment_number\":null,\"city\":\"\"},\"establishment\":\"S.PsiMercuBuanaUniversityofYogyakarta\",\"description\":\"Psychology\",\"gpa\":\"\",\"accreditation\":null}]},\"work_experience\":{\"total_years_experience\":null,\"entries\":[{\"title\":\"PeopleOperationSupervisorPT\",\"start_date\":\"2022-02-01\",\"end_date\":null,\"company\":\"MerapiVisitamaIndonesia(SyncoreGroup)\",\"location\":{\"formatted_location\":null,\"postal_code\":null,\"region\":null,\"country\":null,\"country_code\":null,\"raw_input_location\":null,\"street\":null,\"street_number\":null,\"appartment_number\":null,\"city\":\"Yogyakarta\"},\"description\":\"*Manageamultitudeofadministrativetaskforend-to-endpre-onboarding,onboarding,andoffboardingforallnewhiresandemployees.Includingprovideonboardingkits.*Designrecruitmentstrategyformanpowerfulfillment.*DesignandinitiateSOPsandPoliciesrelatingtopeoplemanagementfortheOperationsteams.*CreatemonthlyreportsaboutRecruitmentReportandOperationsTeamReporttotheProjectManager.*Calculatingemployeesalaryandovertime.*Responsibleforcoordinatingandfullysupportingtheoperationalneedsoftheclient.*Responsibleforemployeeadministrative.*AssistingProjectManagerthepreparationofMoUtotheclient.\",\"industry\":\"management\"},{\"title\":\"SyncoreIndonesia\",\"start_date\":\"2020-07-01\",\"end_date\":\"2022-01-31\",\"company\":\"HRDLeaderPT\",\"location\":{\"formatted_location\":null,\"postal_code\":null,\"region\":null,\"country\":null,\"country_code\":null,\"raw_input_location\":null,\"street\":null,\"street_number\":null,\"appartment_number\":null,\"city\":\"Yogyakarta\"},\"description\":\"*Managearound100employees(headcompanyandunitbusiness)*Designingoverallrecruitingstrategyheadcompanyandunitbusiness.*Makingannualmanpowerplanningandannualturnoverreports.*AssessmentforEmployeeDevelopment.*Responsibleformakinginternaleventsforemployeeengagements.*DesigningTrainingNeedAnalysis(TNA)andorganizeinternaltrainingforemployees.*Conductmonthlyemployeeperformanceevaluations(OneonOneEvaluations,EmployeeCounseling,andetc.)FatikaRatnaAkhida-2*Responsibleforcompany'semployeeindustrialrelations.*AssistingforBusinessProcessOutsourcing(BPO)developmentsuchas:SocialMediaAwareness,TalentManagement,Payroll,Reportingtoclient,Payroll,andotherdevelopments.*Responsibleforcalculatingsalaries,overtime,BPJS.HROperationStaff,PTSyncoreIndonesia,YogyakartaNov2019-Des2020*ResponsibleforRecruitmentandSelectionandreporting.*Responsibleforemployeeadministration(database,employeecorrespondence,etc.)*Assistingthepreparationofemployeeonboardingprocess.\",\"industry\":\"informationtechnology\"},{\"title\":\"AmazaraCiptaIndonesia\",\"start_date\":\"2019-03-01\",\"end_date\":\"2019-08-01\",\"company\":\"CustomerSupportPT\",\"location\":{\"formatted_location\":null,\"postal_code\":null,\"region\":null,\"country\":null,\"country_code\":null,\"raw_input_location\":null,\"street\":null,\"street_number\":null,\"appartment_number\":null,\"city\":\"Yogyakarta\"},\"description\":\"*Handlingcustomercomplainonlineandoffline*Responsiblestockopnamereport.*Handlingcustomerchatvialivechat,marketplace(Shopee,Tokopedia),andWhatsapp.\",\"industry\":null}]},\"languages\":[{\"name\":\"EN\",\"code\":null}],\"skills\":[{\"name\":\"EnglishLanguage\",\"type\":\"unknown\"},{\"name\":\"HR\",\"type\":\"unknown\"},{\"name\":\"HumanResources\",\"type\":\"unknown\"},{\"name\":\"HumanResourcesInformationSystem\",\"type\":\"unknown\"},{\"name\":\"Management\",\"type\":\"intermediate\"},{\"name\":\"Microsoft\",\"type\":\"intermediate\"},{\"name\":\"MicrosoftExcel\",\"type\":\"intermediate\"},{\"name\":\"MicrosoftOffice\",\"type\":\"intermediate\"},{\"name\":\"MicrosoftOutlook\",\"type\":\"intermediate\"},{\"name\":\"MicrosoftPowerPoint\",\"type\":\"intermediate\"},{\"name\":\"MicrosoftWord\",\"type\":\"intermediate\"},{\"name\":\"Outsourcing\",\"type\":\"intermediate\"},{\"name\":\"Payroll\",\"type\":\"intermediate\"},{\"name\":\"ProjectManagement\",\"type\":\"beginner\"},{\"name\":\"ProjectManager\",\"type\":\"beginner\"},{\"name\":\"Recruiting\",\"type\":\"beginner\"},{\"name\":\"SocialMedia\",\"type\":\"beginner\"},{\"name\":\"Spreadsheet\",\"type\":\"unknown\"},{\"name\":\"TalentManagement\",\"type\":\"unknown\"}],\"certifications\":[],\"courses\":[],\"publications\":[],\"interests\":[]}},\"eden-ai\":{\"status\":\"success\",\"extracted_data\":{\"personal_infos\":{\"name\":{\"first_name\":\"RATNA\",\"last_name\":\"AKHIDA\",\"raw_name\":\"RATNAAKHIDA\",\"middle\":null,\"title\":null,\"prefix\":null,\"sufix\":null},\"address\":{\"formatted_location\":null,\"postal_code\":null,\"region\":null,\"country\":null,\"country_code\":null,\"raw_input_location\":null,\"street\":null,\"street_number\":null,\"appartment_number\":null,\"city\":\"\"},\"self_summary\":null,\"objective\":null,\"date_of_birth\":null,\"place_of_birth\":null,\"phones\":[\"082158214780\"],\"mails\":[\"akhida.fatikaa@gmail.com\"],\"urls\":[],\"fax\":[],\"current_profession\":null,\"gender\":null,\"nationality\":\"Indonesia\",\"martial_status\":null,\"current_salary\":null},\"education\":{\"total_years_education\":null,\"entries\":[{\"title\":\"bachelors\",\"start_date\":null,\"end_date\":\"2019-02-01\",\"location\":{\"formatted_location\":null,\"postal_code\":null,\"region\":null,\"country\":null,\"country_code\":null,\"raw_input_location\":null,\"street\":null,\"street_number\":null,\"appartment_number\":null,\"city\":\"\"},\"establishment\":\"S.PsiMercuBuanaUniversityofYogyakarta\",\"description\":\"Psychology\",\"gpa\":\"\",\"accreditation\":null}]},\"work_experience\":{\"total_years_experience\":null,\"entries\":[{\"title\":\"PeopleOperationSupervisorPT\",\"start_date\":\"2022-02-01\",\"end_date\":null,\"company\":\"MerapiVisitamaIndonesia(SyncoreGroup)\",\"location\":{\"formatted_location\":null,\"postal_code\":null,\"region\":null,\"country\":null,\"country_code\":null,\"raw_input_location\":null,\"street\":null,\"street_number\":null,\"appartment_number\":null,\"city\":\"Yogyakarta\"},\"description\":\"*Manageamultitudeofadministrativetaskforend-to-endpre-onboarding,onboarding,andoffboardingforallnewhiresandemployees.Includingprovideonboardingkits.*Designrecruitmentstrategyformanpowerfulfillment.*DesignandinitiateSOPsandPoliciesrelatingtopeoplemanagementfortheOperationsteams.*CreatemonthlyreportsaboutRecruitmentReportandOperationsTeamReporttotheProjectManager.*Calculatingemployeesalaryandovertime.*Responsibleforcoordinatingandfullysupportingtheoperationalneedsoftheclient.*Responsibleforemployeeadministrative.*AssistingProjectManagerthepreparationofMoUtotheclient.\",\"industry\":\"management\"},{\"title\":\"SyncoreIndonesia\",\"start_date\":\"2020-07-01\",\"end_date\":\"2022-01-31\",\"company\":\"HRDLeaderPT\",\"location\":{\"formatted_location\":null,\"postal_code\":null,\"region\":null,\"country\":null,\"country_code\":null,\"raw_input_location\":null,\"street\":null,\"street_number\":null,\"appartment_number\":null,\"city\":\"Yogyakarta\"},\"description\":\"*Managearound100employees(headcompanyandunitbusiness)*Designingoverallrecruitingstrategyheadcompanyandunitbusiness.*Makingannualmanpowerplanningandannualturnoverreports.*AssessmentforEmployeeDevelopment.*Responsibleformakinginternaleventsforemployeeengagements.*DesigningTrainingNeedAnalysis(TNA)andorganizeinternaltrainingforemployees.*Conductmonthlyemployeeperformanceevaluations(OneonOneEvaluations,EmployeeCounseling,andetc.)FatikaRatnaAkhida-2*Responsibleforcompany'semployeeindustrialrelations.*AssistingforBusinessProcessOutsourcing(BPO)developmentsuchas:SocialMediaAwareness,TalentManagement,Payroll,Reportingtoclient,Payroll,andotherdevelopments.*Responsibleforcalculatingsalaries,overtime,BPJS.HROperationStaff,PTSyncoreIndonesia,YogyakartaNov2019-Des2020*ResponsibleforRecruitmentandSelectionandreporting.*Responsibleforemployeeadministration(database,employeecorrespondence,etc.)*Assistingthepreparationofemployeeonboardingprocess.\",\"industry\":\"informationtechnology\"},{\"title\":\"AmazaraCiptaIndonesia\",\"start_date\":\"2019-03-01\",\"end_date\":\"2019-08-01\",\"company\":\"CustomerSupportPT\",\"location\":{\"formatted_location\":null,\"postal_code\":null,\"region\":null,\"country\":null,\"country_code\":null,\"raw_input_location\":null,\"street\":null,\"street_number\":null,\"appartment_number\":null,\"city\":\"Yogyakarta\"},\"description\":\"*Handlingcustomercomplainonlineandoffline*Responsiblestockopnamereport.*Handlingcustomerchatvialivechat,marketplace(Shopee,Tokopedia),andWhatsapp.\",\"industry\":null}]},\"languages\":[{\"name\":\"EN\",\"code\":null}],\"skills\":[{\"name\":\"EnglishLanguage\",\"type\":\"unknown\"},{\"name\":\"HR\",\"type\":\"unknown\"},{\"name\":\"HumanResources\",\"type\":\"unknown\"},{\"name\":\"HumanResourcesInformationSystem\",\"type\":\"unknown\"},{\"name\":\"Management\",\"type\":\"intermediate\"},{\"name\":\"Microsoft\",\"type\":\"intermediate\"},{\"name\":\"MicrosoftExcel\",\"type\":\"intermediate\"},{\"name\":\"MicrosoftOffice\",\"type\":\"intermediate\"},{\"name\":\"MicrosoftOutlook\",\"type\":\"intermediate\"},{\"name\":\"MicrosoftPowerPoint\",\"type\":\"intermediate\"},{\"name\":\"MicrosoftWord\",\"type\":\"intermediate\"},{\"name\":\"Outsourcing\",\"type\":\"intermediate\"},{\"name\":\"Payroll\",\"type\":\"intermediate\"},{\"name\":\"ProjectManagement\",\"type\":\"beginner\"},{\"name\":\"ProjectManager\",\"type\":\"beginner\"},{\"name\":\"Recruiting\",\"type\":\"beginner\"},{\"name\":\"SocialMedia\",\"type\":\"beginner\"},{\"name\":\"Spreadsheet\",\"type\":\"unknown\"},{\"name\":\"TalentManagement\",\"type\":\"unknown\"}],\"certifications\":[],\"courses\":[],\"publications\":[],\"interests\":[]}}}";
                        screeningResult = JsonConvert.DeserializeObject<RootHireability>(edenResult);
                        candidate.AIScreeningResult = JsonConvert.SerializeObject(screeningResult);
                        candidate.AIScreeningStatus = DTO.CV_SCREENING_AI_STATUS.Success;
                    }
                    else
                    {
                        candidate.AIScreeningStatus = DTO.CV_SCREENING_AI_STATUS.Fail;
                    }
                }

                var listCandidateSpecification = new List<CandidateSpecification>();
                if (screeningResult?.hireability.status == "success")
                {
                    foreach (var spec in listApplicantSpec)
                    {
                        if (edenResult.Contains(spec.Name))
                        {
                            var data = new CandidateSpecification()
                            {
                                ApplicantId = spec.ApplicantSpecificationId,
                                ApplicantItemId = spec.Id,
                                ApplicantItemName = spec.Name,
                                CandidateId = id
                            };
                            listCandidateSpecification.Add(data);
                        }
                    }
                }

                await using var trx = await _db.Database.BeginTransactionAsync();
                try
                {
                    foreach(var item in listCandidateSpecification)
                    {
                        _candidateRepository.InsertCandidateSpecification(item);
                    }

                    candidate.ApplicantSpecApprove = listCandidateSpecification.Count();
                    _candidateRepository.UpdateCandidate(candidate);

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
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
