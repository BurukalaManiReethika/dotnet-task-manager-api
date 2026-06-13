using dotnet_task_manager_api.Data;
using dotnet_task_manager_api.Models;
using dotnet_task_manager_api.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace dotnet_task_manager_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly JwtTokenService _jwtTokenService;
        private readonly PasswordHasher<User> _passwordHasher = new();

        public AuthController(AppDbContext context, JwtTokenService jwtTokenService)
        {
            _context = context;
            _jwtTokenService = jwtTokenService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthResponse>> Register(RegisterRequest request)
        {
            var normalizedUsername = request.Username.Trim();
            if (string.IsNullOrWhiteSpace(normalizedUsername))
                return BadRequest("Username is required.");

            if (await _context.Users.AnyAsync(user => user.Username.ToLower() == normalizedUsername.ToLower()))
                return Conflict("Username is already registered.");

            var user = new User
            {
                Username = normalizedUsername
            };
            user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Register), _jwtTokenService.GenerateToken(user));
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponse>> Login(LoginRequest request)
        {
            var normalizedUsername = request.Username.Trim();
            if (string.IsNullOrWhiteSpace(normalizedUsername))
                return Unauthorized("Invalid username or password.");

            var user = await _context.Users.SingleOrDefaultAsync(user => user.Username.ToLower() == normalizedUsername.ToLower());

            if (user == null)
                return Unauthorized("Invalid username or password.");

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
            if (result == PasswordVerificationResult.Failed)
                return Unauthorized("Invalid username or password.");

            return Ok(_jwtTokenService.GenerateToken(user));
        }
    }
}
