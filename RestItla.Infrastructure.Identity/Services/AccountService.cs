using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using RestItla.Application.DTO.User;
using RestItla.Application.Extras.ResultObject;
using RestItla.Application.Interfaces.Services;
using RestItla.Domain.Enum;
using RestItla.Domain.Settings;
using RestItla.Infrastructure.Identity.Entities;

namespace RestItla.Infrastructure.Identity.Services
{
    internal class AccountService
    : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly JwtSettings _jwtSettings;

        public AccountService(UserManager<ApplicationUser> userManager,
                              SignInManager<ApplicationUser> signInManager,
                              IOptions<JwtSettings> jwtSettings)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtSettings = jwtSettings.Value;
        }

        public async Task<Result<LoginResponseDTO>> Login(LoginDTO dto)
        {
            string normalizedEmail = _userManager.NormalizeEmail(dto.Email);
            ApplicationUser? user = await _userManager.FindByEmailAsync(normalizedEmail);

            if (user is null)
            {
                return ErrorType.InvalidCredentials.
                       Because($"Invalid credentials for {dto.Email}");
            }

            var result = await _signInManager.PasswordSignInAsync(user,
                                                                  dto.Password,
                                                                  false,
                                                                  lockoutOnFailure: false);
            if (!result.Succeeded)
            {
                return ErrorType.InvalidCredentials.
                       Because($"Invalid credentials for {dto.Email}");
            }

            user.Email ??= dto.Email;
            JwtSecurityToken jwtSecurityToken = await GenerateJWToken(user);

            IList<string> rolesList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);

            return new LoginResponseDTO()
            {
                Id = user.Id,
                Email = user.Email,
                Roles = rolesList.ToList(),
                JWTToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
            };
        }

        public async Task<Result<RegisterResponseDTO>> Register(RegisterDTO dto, Role role)
        {
            var userWithSameEmail = await _userManager.FindByEmailAsync(dto.Email);
            if (userWithSameEmail != null)
            {
                return ErrorType.Conflict.
                       Because($"Email '{dto.Email}' is already registered.");
            }

            ApplicationUser user = new()
            {
                UserName = dto.Email,
                Email = dto.Email,
                Name = dto.Name,
                Surname = dto.Surname,
            };

            IdentityResult result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
            {
                return ErrorType.Unknown
                                .Because($"An error occurred trying to register the user.");
            }

            await _userManager.AddToRoleAsync(user, role.ToString());
            Result<LoginResponseDTO> loginResponse = await Login(new LoginDTO()
            {
                Email = dto.Email,
                Password = dto.Password
            });

            return loginResponse.Map(l => new RegisterResponseDTO()
            {
                Id = l.Id,
                JWToken = l.JWTToken
            });
        }

        private async Task<JwtSecurityToken> GenerateJWToken(ApplicationUser user)
        {
            IList<Claim> userClaims = await _userManager.GetClaimsAsync(user);
            IList<string> roles = await _userManager.GetRolesAsync(user);

            List<Claim> roleClaims = new();

            foreach (string role in roles)
            {
                roleClaims.Add(new Claim("roles", role));
            }

            IEnumerable<Claim> claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email!),
                new Claim("UUID", user.Id)
            }
            .Union(userClaims)
            .Union(roleClaims);

            var symmectricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var signingCredetials = new SigningCredentials(symmectricSecurityKey, SecurityAlgorithms.HmacSha256);

            return new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
                signingCredentials: signingCredetials);
        }
    }
}