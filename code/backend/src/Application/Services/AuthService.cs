using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;
using Volo.Abp.Security.Encryption;
using CVSystem.Application.Contracts.Services;
using CVSystem.Application.DTOs;
using CVSystem.Domain.Entities;

namespace CVSystem.Application.Services
{
    /// <summary>
    /// خدمة المصادقة
    /// </summary>
    public class AuthService : IAuthService
    {
        private readonly IRepository<User, Guid> _userRepository;
        private readonly IRepository<UserProfile, Guid> _userProfileRepository;
        private readonly IStringEncryptionService _encryptionService;
        private readonly IConfiguration _configuration;

        public AuthService(
            IRepository<User, Guid> userRepository,
            IRepository<UserProfile, Guid> userProfileRepository,
            IStringEncryptionService encryptionService,
            IConfiguration configuration)
        {
            _userRepository = userRepository;
            _userProfileRepository = userProfileRepository;
            _encryptionService = encryptionService;
            _configuration = configuration;
        }

        /// <summary>
        /// تسجيل مستخدم جديد
        /// </summary>
        public async Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto)
        {
            // التحقق من صحة البيانات
            if (!registerDto.IsValid())
            {
                throw new ArgumentException("بيانات التسجيل غير صحيحة");
            }

            // التحقق من عدم وجود مستخدم بنفس البريد الإلكتروني
            var existingUser = await _userRepository.FirstOrDefaultAsync(u => u.Email == registerDto.Email);
            if (existingUser != null)
            {
                throw new InvalidOperationException("البريد الإلكتروني مستخدم مسبقاً");
            }

            // التحقق من عدم وجود مستخدم بنفس اسم المستخدم
            var userName = string.IsNullOrWhiteSpace(registerDto.UserName) ? registerDto.Email : registerDto.UserName;
            existingUser = await _userRepository.FirstOrDefaultAsync(u => u.UserName == userName);
            if (existingUser != null)
            {
                throw new InvalidOperationException("اسم المستخدم مستخدم مسبقاً");
            }

            // إنشاء مستخدم جديد
            var user = new User(
                Guid.NewGuid(),
                userName,
                registerDto.Email,
                registerDto.FullName
            );

            // تعيين تاريخ الميلاد والجنسية إذا كانت موجودة
            if (registerDto.DateOfBirth.HasValue)
            {
                user.DateOfBirth = registerDto.DateOfBirth.Value;
            }

            if (!string.IsNullOrWhiteSpace(registerDto.Nationality))
            {
                user.Nationality = registerDto.Nationality;
            }

            // تشفير كلمة المرور
            var passwordHash = _encryptionService.Encrypt(registerDto.Password);
            user.SetPasswordHash(passwordHash);

            // حفظ المستخدم في قاعدة البيانات
            await _userRepository.InsertAsync(user, autoSave: true);

            // إنشاء ملف شخصي للمستخدم
            var userProfile = new UserProfile(user.Id);
            await _userProfileRepository.InsertAsync(userProfile, autoSave: true);

            // إنشاء توكن JWT
            var token = GenerateJwtToken(user);

            // إرجاع استجابة المصادقة
            return new AuthResponseDto(
                user.Id,
                user.FullName,
                user.Email,
                user.UserName,
                token,
                DateTime.UtcNow.AddHours(24), // صلاحية 24 ساعة
                true,
                DateTime.UtcNow
            );
        }

        /// <summary>
        /// تسجيل الدخول
        /// </summary>
        public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
        {
            if (!loginDto.IsValid())
            {
                throw new ArgumentException("بيانات الدخول غير صحيحة");
            }

            // البحث عن المستخدم بالبريد الإلكتروني أو اسم المستخدم
            User user = null;
            
            if (loginDto.IsEmail())
            {
                user = await _userRepository.FirstOrDefaultAsync(u => u.Email == loginDto.GetEmail());
            }
            else
            {
                user = await _userRepository.FirstOrDefaultAsync(u => u.UserName == loginDto.GetUserName());
            }

            // التحقق من وجود المستخدم
            if (user == null)
            {
                user?.IncrementFailedLoginAttempts();
                await _userRepository.UpdateAsync(user);
                throw new UnauthorizedAccessException("بيانات الدخول غير صحيحة");
            }

            // التحقق من أن الحساب نشط
            if (!user.IsActive)
            {
                throw new UnauthorizedAccessException("الحساب غير نشط");
            }

            // التحقق من أن الحساب غير مقفل
            if (user.IsLocked())
            {
                throw new UnauthorizedAccessException("الحساب مقفل مؤقتاً. حاول مرة أخرى لاحقاً");
            }

            // التحقق من صحة كلمة المرور
            var passwordHash = _encryptionService.Encrypt(loginDto.Password);
            if (user.PasswordHash != passwordHash)
            {
                user.IncrementFailedLoginAttempts();
                
                // قفل الحساب بعد 5 محاولات فاشلة
                if (user.FailedLoginAttempts >= 5)
                {
                    user.LockAccount();
                }
                
                await _userRepository.UpdateAsync(user);
                throw new UnauthorizedAccessException("بيانات الدخول غير صحيحة");
            }

            // إعادة تعيين محاولات الدخول الفاشلة وتحديث آخر تاريخ دخول
            user.UpdateLastLogin();
            await _userRepository.UpdateAsync(user);

            // إنشاء توكن JWT
            var token = GenerateJwtToken(user);

            // إرجاع استجابة المصادقة
            return new AuthResponseDto(
                user.Id,
                user.FullName,
                user.Email,
                user.UserName,
                token,
                DateTime.UtcNow.AddHours(24), // صلاحية 24 ساعة
                true,
                DateTime.UtcNow
            );
        }

        /// <summary>
        /// تسجيل الخروج
        /// </summary>
        public async Task<bool> LogoutAsync(Guid userId)
        {
            var user = await _userRepository.FirstOrDefaultAsync(u => u.Id == userId);
            if (user != null)
            {
                // يمكن إضافة منطق إضافي هنا مثل إلغاء التوكنات
                return true;
            }
            return false;
        }

        /// <summary>
        /// تحديث التوكن
        /// </summary>
        public async Task<AuthResponseDto> RefreshTokenAsync(string refreshToken)
        {
            // TODO: تنفيذ تحديث التوكن
            throw new NotImplementedException();
        }

        /// <summary>
        /// التحقق من صحة التوكن
        /// </summary>
        public async Task<bool> ValidateTokenAsync(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Secret"]);
                
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = _configuration["Jwt:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = _configuration["Jwt:Audience"],
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// الحصول على المستخدم من التوكن
        /// </summary>
        public async Task<UserDto> GetUserFromTokenAsync(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                return null;
            }

            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(token);
                
                var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid userId))
                {
                    return null;
                }

                var user = await _userRepository.FirstOrDefaultAsync(u => u.Id == userId);
                if (user == null)
                {
                    return null;
                }

                // تحويل User إلى UserDto
                // TODO: استخدام AutoMapper
                return new UserDto
                {
                    Id = user.Id,
                    FullName = user.FullName,
                    UserName = user.UserName,
                    Email = user.Email,
                    IsActive = user.IsActive,
                    LastLoginDate = user.LastLoginDate,
                    CreationTime = user.CreationTime
                };
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// تغيير كلمة المرور
        /// </summary>
        public async Task<bool> ChangePasswordAsync(Guid userId, string currentPassword, string newPassword)
        {
            var user = await _userRepository.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                throw new ArgumentException("المستخدم غير موجود");
            }

            // التحقق من كلمة المرور الحالية
            var currentPasswordHash = _encryptionService.Encrypt(currentPassword);
            if (user.PasswordHash != currentPasswordHash)
            {
                throw new UnauthorizedAccessException("كلمة المرور الحالية غير صحيحة");
            }

            // التحقق من قوة كلمة المرور الجديدة
            if (string.IsNullOrWhiteSpace(newPassword) || newPassword.Length < 8)
            {
                throw new ArgumentException("كلمة المرور الجديدة يجب أن تكون 8 أحرف على الأقل");
            }

            // تحديث كلمة المرور
            var newPasswordHash = _encryptionService.Encrypt(newPassword);
            user.SetPasswordHash(newPasswordHash);
            
            await _userRepository.UpdateAsync(user, autoSave: true);
            return true;
        }

        /// <summary>
        /// نسيان كلمة المرور
        /// </summary>
        public async Task<bool> ForgotPasswordAsync(string email)
        {
            var user = await _userRepository.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                // لا نريد إخبار المستخدم إذا كان البريد غير موجود لأسباب أمنية
                return true;
            }

            // TODO: إرسال رابط إعادة تعيين كلمة المرور إلى البريد الإلكتروني
            // في هذه المرحلة، نرجع true فقط للإشارة إلى أن العملية بدأت
            
            return true;
        }

        /// <summary>
        /// التحقق من توكن إعادة تعيين كلمة المرور
        /// </summary>
        public async Task<bool> ValidateResetPasswordTokenAsync(string token)
        {
            // TODO: تنفيذ التحقق من توكن إعادة تعيين كلمة المرور
            throw new NotImplementedException();
        }

        /// <summary>
        /// إعادة تعيين كلمة المرور
        /// </summary>
        public async Task<bool> ResetPasswordAsync(string token, string newPassword)
        {
            // TODO: تنفيذ إعادة تعيين كلمة المرور
            throw new NotImplementedException();
        }

        /// <summary>
        /// التحقق من وجود مستخدم بالبريد الإلكتروني
        /// </summary>
        public async Task<bool> UserExistsByEmailAsync(string email)
        {
            return await _userRepository.AnyAsync(u => u.Email == email);
        }

        /// <summary>
        /// التحقق من وجود مستخدم باسم المستخدم
        /// </summary>
        public async Task<bool> UserExistsByUserNameAsync(string userName)
        {
            return await _userRepository.AnyAsync(u => u.UserName == userName);
        }

        /// <summary>
        /// الحصول على عدد محاولات الدخول الفاشلة
        /// </summary>
        public async Task<int> GetFailedLoginAttemptsAsync(string emailOrUserName)
        {
            User user = null;
            
            if (emailOrUserName.Contains("@"))
            {
                user = await _userRepository.FirstOrDefaultAsync(u => u.Email == emailOrUserName);
            }
            else
            {
                user = await _userRepository.FirstOrDefaultAsync(u => u.UserName == emailOrUserName);
            }

            return user?.FailedLoginAttempts ?? 0;
        }

        /// <summary>
        /// التحقق من أن الحساب مقفل
        /// </summary>
        public async Task<bool> IsAccountLockedAsync(string emailOrUserName)
        {
            User user = null;
            
            if (emailOrUserName.Contains("@"))
            {
                user = await _userRepository.FirstOrDefaultAsync(u => u.Email == emailOrUserName);
            }
            else
            {
                user = await _userRepository.FirstOrDefaultAsync(u => u.UserName == emailOrUserName);
            }

            return user?.IsLocked() ?? false;
        }

        /// <summary>
        /// قفل الحساب
        /// </summary>
        public async Task<bool> LockAccountAsync(string emailOrUserName, int minutes = 30)
        {
            User user = null;
            
            if (emailOrUserName.Contains("@"))
            {
                user = await _userRepository.FirstOrDefaultAsync(u => u.Email == emailOrUserName);
            }
            else
            {
                user = await _userRepository.FirstOrDefaultAsync(u => u.UserName == emailOrUserName);
            }

            if (user == null)
            {
                return false;
            }

            user.LockAccount(minutes);
            await _userRepository.UpdateAsync(user, autoSave: true);
            return true;
        }

        /// <summary>
        /// فتح قفل الحساب
        /// </summary>
        public async Task<bool> UnlockAccountAsync(string emailOrUserName)
        {
            User user = null;
            
            if (emailOrUserName.Contains("@"))
            {
                user = await _userRepository.FirstOrDefaultAsync(u => u.Email == emailOrUserName);
            }
            else
            {
                user = await _userRepository.FirstOrDefaultAsync(u => u.UserName == emailOrUserName);
            }

            if (user == null)
            {
                return false;
            }

            user.UnlockAccount();
            await _userRepository.UpdateAsync(user, autoSave: true);
            return true;
        }

        /// <summary>
        /// إنشاء توكن JWT
        /// </summary>
        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Secret"] ?? "DefaultSecretKeyForDevelopmentOnlyChangeInProduction");
            
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim("FullName", user.FullName)
                }),
                Expires = DateTime.UtcNow.AddHours(24),
                Issuer = _configuration["Jwt:Issuer"] ?? "CVSystem",
                Audience = _configuration["Jwt:Audience"] ?? "CVSystemUsers",
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key), 
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}