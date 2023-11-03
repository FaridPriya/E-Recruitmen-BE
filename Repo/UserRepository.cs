using Azure.Core;
using ERecruitmentBE.Data;
using ERecruitmentBE.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Security.Cryptography;
using System.Text;

namespace ERecruitmentBE.Repo
{
    public class UserRepository
    {
        private readonly AppDbContext _db;
        public UserRepository(AppDbContext db)
        {
            _db = db;
        }

        public byte[] GenerateSalt()
        {
            using (var rngCryptoServiceProvider = new RNGCryptoServiceProvider())
            {
                var salt = new byte[32];
                rngCryptoServiceProvider.GetBytes(salt);
                return salt;
            }
        }

        public byte[] GeneratePasswordHash(string password, byte[] salt)
        {
            using (var sha256 = SHA256.Create())
            {
                var passwordBytes = Encoding.UTF8.GetBytes(password);
                var saltedPassword = new byte[passwordBytes.Length + salt.Length];
                Array.Copy(passwordBytes, saltedPassword, passwordBytes.Length);
                Array.Copy(salt, 0, saltedPassword, passwordBytes.Length, salt.Length);

                return sha256.ComputeHash(saltedPassword);
            }
        }

        //public bool VerifyPassword(string password)
        //{
        //    var inputPasswordHash = GeneratePasswordHash(password, Salt);
        //    return StructuralComparisons.StructuralEqualityComparer.Equals(inputPasswordHash, PasswordHash);
        //}

        public bool CheckUsernameIsAlready(string username)
        {
            return _db.Users.Any(u => u.Username == username);
        }

        public bool IsLogged(string username, string password)
        {
            var user =  _db.Users.Where(u => u.Username == username).FirstOrDefault();
            if (user == null) return false;
            var passwordHash = GeneratePasswordHash(password, user.Salt);

            if (!StructuralComparisons.StructuralEqualityComparer.Equals(passwordHash, user.PasswordHash))
            {
                return false;
            }

            return true;
        }

        public bool IsLoggedCandidate(string username, string password)
        {
            var user = _db.Users.Where(u => u.Username == username && u.UserType == DTO.USER_TYPE.Candidate).FirstOrDefault();
            if (user == null) return false;
            var passwordHash = GeneratePasswordHash(password, user.Salt);

            if (!StructuralComparisons.StructuralEqualityComparer.Equals(passwordHash, user.PasswordHash))
            {
                return false;
            }

            return true;
        }

        public User GetUserByUsername(string username)
        {
            var user = _db.Users.Where(u => u.Username == username && u.UserType == DTO.USER_TYPE.Candidate).FirstOrDefault();
            return user;
        }

        public void InsertUser(User user)
        {
            _db.Users.Add(user);
        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
