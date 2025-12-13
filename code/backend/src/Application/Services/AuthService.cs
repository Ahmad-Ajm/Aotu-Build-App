using PsecKit.Core.Application.Contracts.Services;
using PsecKit.Core.Application.DTOs;
using PsecKit.Core.Domain.Entities;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Validation;
using Volo.Abp.Identity;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System;
using System.Extensions;

namespace PsecKit.Core.Application.Services
{
    public class AuthService : ApplicationService, IAuthService
    {
        private readonly IRepository<User, Guid> _userRepository;
        private readonly IConfiguration _configuration;
        private readonly IPasswordHasher _passwordHasher; // Assuming you have a password hasher service

        public AuthService(IRepository<User, Guid> userRepository, IConfiguration configuration, IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _passwordHasher = passwordHasher;
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto)
        {
            // Validate if email is already used
            if (await _userRepository.AnyAsync(u => u.Email == registerDto.Email))
            {
                throw new UserFriendlyException("Email already in use.");
            }

            var user = ObjectMapper.Map<RegisterDto, User>(registerDto);
            user.PasswordHash = _passwordHasher.HashPassword(user, registerDto.Password); // Hash the password
            user.UserName = registerDto.Email; // Using email as username for simplicity

            user = await _userRepository.InsertAsync(user, autoSave: true);

            var token = GenerateJwtToken(user);

            return new AuthResponseDto
            {
                Success = true,
                Message = "Registration successful",
                Data = new AuthResponseDataDto
                {
                    UserId = user.Id,
                    Email = user.Email,
                    FullName = user.FullName,
                    Token = token
                }
            };
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
        {
            var user = await _userRepository.FirstOrDefaultAsync(u => u.Email == loginDto.Email);

            if (user == null || !_passwordHasher.VerifyHashedPassword(user, user.PasswordHash, loginDto.Password))
            {
                // Increment failed login attempts and lock account if necessary (Future enhancement)
                throw new UserFriendlyException("Invalid credentials.");
            }

            var token = GenerateJwtToken(user);

            user.LastLoginDate = Clock.Now;
            await _userRepository.UpdateAsync(user);

            return new AuthResponseDto
            {
                Success = true,
                Message = "Login successful",
                Data = new AuthResponseDataDto
                {
                    UserId = user.Id,
                    Email = user.Email,
                    FullName = user.FullName,
                    Token = token
                }
            };
        }

        private string GenerateJwtToken(User user)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secret = jwtSettings["Secret"];
            var issuer = jwtSettings["Issuer"];
            var audience = jwtSettings["Audience"];
            var expirationMinutes = double.Parse(jwtSettings["ExpirationMinutes"]);

            var key = Encoding.ASCII.GetBytes(secret);
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email)
                }),
                Expires = Clock.Now.AddMinutes(expirationMinutes),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
