using AutoMapper;
using ERecruitmentBE.Data;
using ERecruitmentBE.DTO;
using ERecruitmentBE.DTO.ApplicantSpecification;
using ERecruitmentBE.Models;
using ERecruitmentBE.Repo;
using Microsoft.AspNetCore.Mvc;


namespace ERecruitmentBE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly UserRepository _userRepository;
        public AccountController(AppDbContext db, IMapper mapper)
        {
            _db = db;
            _userRepository = new UserRepository(db);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Post([FromBody] UserDTO request)
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
    }
}
