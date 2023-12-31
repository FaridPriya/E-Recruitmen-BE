﻿using System.Linq.Expressions;

namespace ERecruitmentBE.DTO.Candidate
{
    public class CandidateDTO
    {
        public CandidateDTO()
        {
            Id = Guid.NewGuid().ToString();
        }
        public string Id { get; set; }
        public string Name { get; set; }
        public string IdJobVacancy { get; set; }
        public string JobVacancyName { get; set; }
        public string NoHandphone { get; set; }
        public string Email { get; set; }
        public CV_SCREENING_AI_STATUS AIScreeningStatus { get; set; }
        public string? AIScreeningResult { get; set; }
        public int ApplicantSpecApprove { get; set; }
        public STATUS_CANDIDATE Status { get; set; }
        public DateTimeOffset? CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        public bool Deleted { get; set; }
        public string? ApplyDate { get; set; }
        public bool IsAlreadyCVUpload { get; set; }

        public List<CandidateSpecificationDTO> Skill { get; set; }
        public List<CandidateSpecificationDTO> Experience { get; set; }
        public List<CandidateSpecificationDTO> Education { get; set; }

        public static Expression<Func<Models.Candidate, CandidateDTO>> SELECT = x => new CandidateDTO
        {
            Id = x.Id,
            Name = x.Name,
            IdJobVacancy = x.IdJobVacancy,
            AIScreeningStatus = x.AIScreeningStatus,
            AIScreeningResult = x.AIScreeningResult,
            Email = x.Email,
            NoHandphone = x.NoHandphone,
            Status = x.Status,
            CreatedAt = x.CreatedAt,
            UpdatedAt = x.UpdatedAt
        };

        public static Expression<Func<Models.Candidate, CandidateDTO>> SELECT_FOR_CANDIDATE = x => new CandidateDTO
        {
            Id = x.Id,
            Name = x.Name,
            IdJobVacancy = x.IdJobVacancy,
            Status = x.Status,
            IsAlreadyCVUpload = x.AIScreeningStatus != CV_SCREENING_AI_STATUS.Pending ? true: false,
        };
    }
}
