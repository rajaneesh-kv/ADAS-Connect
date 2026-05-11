using CvTracker.Web.Models;
using Microsoft.AspNetCore.Identity;

namespace CvTracker.Web.Services
{
    public class PasswordHasher : IPasswordHasher
    {
        private readonly Microsoft.AspNetCore.Identity.PasswordHasher<User> _hasher =
            new Microsoft.AspNetCore.Identity.PasswordHasher<User>();

        public string Hash(string password)
        {
            return _hasher.HashPassword(new User(), password);
        }

        public bool Verify(string hashedPassword, string providedPassword)
        {
            var result = _hasher.VerifyHashedPassword(new User(), hashedPassword, providedPassword);
            return result == PasswordVerificationResult.Success || result == PasswordVerificationResult.SuccessRehashNeeded;
        }
    }
}
