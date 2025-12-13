using PsecKit.Core.Domain.Entities;

namespace PsecKit.Core.Application.Contracts.Services
{
    public interface IPasswordHasher
    {
        string HashPassword(User user, string password);
        bool VerifyHashedPassword(User user, string hashedPassword, string providedPassword);
    }
}
