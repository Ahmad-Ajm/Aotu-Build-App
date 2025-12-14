using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using CVSystem.Application.Contracts.Users;
using CVSystem.Application.Contracts.Users.Dtos;
using CVSystem.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace CVSystem.Application.Users;

public class UserAppService : IUserAppService
{
    private readonly CVSystem.EntityFrameworkCore.CVSystemDbContext _dbContext;

    public UserAppService(CVSystem.EntityFrameworkCore.CVSystemDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterUserRequestDto input)
    {
        if (input.Password != input.ConfirmPassword)
        {
            throw new ArgumentException("Passwords do not match");
        }

        var existingUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == input.Email);
        if (existingUser != null)
        {
            throw new InvalidOperationException("Email already exists");
        }

        var user = new User
        {
            Id = Guid.NewGuid(),
            FullName = input.FullName,
            Email = input.Email,
            UserName = input.Email,
            PasswordHash = HashPassword(input.Password),
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();

        // TODO: generate JWT token
        var token = Convert.ToBase64String(Guid.NewGuid().ToByteArray());

        return new AuthResponseDto
        {
            UserId = user.Id,
            Email = user.Email,
            FullName = user.FullName,
            Token = token
        };
    }

    public async Task<AuthResponseDto> LoginAsync(LoginRequestDto input)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == input.Email);
        if (user == null)
        {
            throw new InvalidOperationException("Invalid credentials");
        }

        var hashed = HashPassword(input.Password);
        if (user.PasswordHash != hashed)
        {
            throw new InvalidOperationException("Invalid credentials");
        }

        user.LastLoginDate = DateTime.UtcNow;
        await _dbContext.SaveChangesAsync();

        // TODO: generate JWT token
        var token = Convert.ToBase64String(Guid.NewGuid().ToByteArray());

        return new AuthResponseDto
        {
            UserId = user.Id,
            Email = user.Email,
            FullName = user.FullName,
            Token = token
        };
    }

    private static string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(password);
        var hash = sha256.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }
}
