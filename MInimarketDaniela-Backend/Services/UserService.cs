using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MInimarketDaniela_Backend.DataAccess;
using MInimarketDaniela_Backend.DTOs;
using MInimarketDaniela_Backend.Models.DataModels;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MInimarketDaniela_Backend.Services
{
    public class UserService : IUserService
    {
        private readonly MinimarketContext _context;
        private readonly IConfiguration _configuration;

        public UserService(MinimarketContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<User> RegisterAsync(RegisterUserDto userDto)
        {
            if (string.IsNullOrWhiteSpace(userDto.Email) || string.IsNullOrWhiteSpace(userDto.Username) || string.IsNullOrWhiteSpace(userDto.Name) || string.IsNullOrWhiteSpace(userDto.LastName) || string.IsNullOrWhiteSpace(userDto.Password))
            {
                throw new ArgumentException("Please fill all required fields"); 
            }

            var userExists = await _context.Users.AnyAsync(u => u.Username == userDto.Username || u.Email == userDto.Email);

            if (userExists) throw new Exception("Username or email already exists.");

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(userDto.Password);

            var user = new User
            {
                Name = userDto.Name,
                LastName = userDto.LastName,
                Email = userDto.Email,
                Username = userDto.Username,
                Password = hashedPassword,
                Role = userDto.Role,

                CreatedAt = DateTime.UtcNow,
                CreatedBy = "System",
                IsDeleted = false
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            user.Password = "*********";

            return (user);
        }

        public async Task<string> LoginAsync(LoginDto loginDto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == loginDto.Username && !u.IsDeleted);

            if (user == null) throw new KeyNotFoundException("User or password wrong.");

            if (!BCrypt.Net.BCrypt.Verify(loginDto.Password, user.Password)) throw new KeyNotFoundException("User or password wrong.");

            return GenerateToken(user);
        }

        private string GenerateToken(User user)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var key = Encoding.UTF8.GetBytes(jwtSettings.GetSection("key").Value!);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim("LastName", user.LastName),
                new Claim("Email", user.Email)
            };

            var creds = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(8),
                SigningCredentials = creds,

                Issuer = jwtSettings["Issuer"],
                Audience = jwtSettings["Audience"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescription);

            return tokenHandler.WriteToken(token);
        }
    }
}
