using AutoMapper;
using ERecruitmentBE.Data;
using ERecruitmentBE.DTO;
using ERecruitmentBE.DTO.ApplicantSpecification;
using ERecruitmentBE.Models;
using ERecruitmentBE.Repo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ERecruitmentBE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AccountController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly CandidateRepository _candidateRepository;
        private readonly UserRepository _userRepository;
        private readonly IMapper _mapper;
        public AccountController(AppDbContext db, IMapper mapper)
        {
            _db = db;
            _userRepository = new UserRepository(db);
            _candidateRepository = new CandidateRepository(db);
            _mapper = mapper;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> register([FromBody] UserDTO request)
        {
            if (request == null || string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest("Username and Password must be fill");
            }

            var isUsernameAlready = _userRepository.CheckUsernameIsAlready(request.Username);
            if (isUsernameAlready)
                return BadRequest("Username is already exist");

            var user = new User();
            user.Username = request.Username;
            user.Salt = _userRepository.GenerateSalt();
            user.PasswordHash = _userRepository.GeneratePasswordHash(request.Password, user.Salt);
            user.UserType = USER_TYPE.Admin;

            await using var trx = await _db.Database.BeginTransactionAsync();
            try
            {
                _userRepository.InsertUser(user);
                await _userRepository.SaveAsync();
                await trx.CommitAsync();
                return Ok();
            }
            catch (Exception e)
            {
                await trx.RollbackAsync();
                return BadRequest(e.Message);
            }
        }

        [HttpPost("register-candidate")]
        [AllowAnonymous]
        public async Task<IActionResult> registerCandidate([FromBody] RegisterCandidateDTO request)
        {
            if (request == null || string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest("Username and Password must be fill");
            }

            var isUsernameAlready = _userRepository.CheckUsernameIsAlready(request.Username);
            if (isUsernameAlready)
                return BadRequest("Username is already exist");

            var isCandidateExist = _candidateRepository.IsCandidateExist(request);
            if(isCandidateExist)
                return BadRequest("Candidate is already exist");

            var user = new User();
            user.Username = request.Username;
            user.Salt = _userRepository.GenerateSalt();
            user.PasswordHash = _userRepository.GeneratePasswordHash(request.Password, user.Salt);
            user.UserType = USER_TYPE.Candidate;

            await using var trx = await _db.Database.BeginTransactionAsync();
            try
            {
                _userRepository.InsertUser(user);
                await _userRepository.SaveAsync();

                Candidate candidate = new Candidate();
                candidate = _mapper.Map<Candidate>(request);
                candidate.Id = user.Id;
                candidate.Status = DTO.STATUS_CANDIDATE.InProgress;
                candidate.AIScreeningStatus = DTO.CV_SCREENING_AI_STATUS.Pending;
                _candidateRepository.InsertCandidate(candidate);
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

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> login([FromBody] UserDTO request)
        {
            if (request == null || string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest("Username and Password must be fill");
            }
            
            try
            {
                var userisAny = _userRepository.IsLogged(request.Username, request.Password);
                if (userisAny)
                {
                    // Buat klaim pengguna
                    var claims = new[]
                    {
                        new Claim(ClaimTypes.Name, request.Username),
                        new Claim(ClaimTypes.Role, USER_TYPE.Admin.ToString()),
                        // Anda dapat menambahkan klaim lain yang diperlukan
                    };

                    // Buat token JWT
                    var jwtKey = Encoding.ASCII.GetBytes("FaridGantengSekali881123");
                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(claims),
                        Expires = DateTime.UtcNow.AddDays(1),
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(jwtKey), SecurityAlgorithms.HmacSha256Signature)
                    };
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var token = tokenHandler.CreateToken(tokenDescriptor);
                    var tokenString = tokenHandler.WriteToken(token);
                    return Ok(new { Token = tokenString });
                }
                else
                {
                    return Unauthorized("Username and password is wrong");
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("login-candidate")]
        [AllowAnonymous]
        public async Task<IActionResult> loginCandidate([FromBody] UserDTO request)
        {
            if (request == null || string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest("Username and Password must be fill");
            }

            try
            {
                var userisAny = _userRepository.IsLoggedCandidate(request.Username, request.Password);
                if (userisAny)
                {
                    var user = _userRepository.GetUserByUsername(request.Username);
                    var candidate = await _candidateRepository.GetCandidateById(user.Id);
                    // Buat klaim pengguna
                    var claims = new[]
                    {
                        new Claim(ClaimTypes.Name, user.Username),
                        new Claim(ClaimTypes.Role, USER_TYPE.Candidate.ToString()),
                        new Claim("CandidateId", user.Id),
                        new Claim("JobId", candidate.IdJobVacancy)
                        // Anda dapat menambahkan klaim lain yang diperlukan
                    };

                    // Buat token JWT
                    var jwtKey = Encoding.ASCII.GetBytes("FaridGantengSekali881123");
                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(claims),
                        Expires = DateTime.UtcNow.AddDays(1),
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(jwtKey), SecurityAlgorithms.HmacSha256Signature)
                    };
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var token = tokenHandler.CreateToken(tokenDescriptor);
                    var tokenString = tokenHandler.WriteToken(token);
                    return Ok(new { Token = tokenString, CandidateName = candidate.Name });
                }
                else
                {
                    return Unauthorized("Username and password is wrong");
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
