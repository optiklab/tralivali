using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Tralivali.API.DTOs;
using Tralivali.Core.Models;
using Tralivali.Infrastructure.Configuration;
using Tralivali.Infrastructure.Services;
using BCrypt.Net;

namespace Tralivali.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly MongoDbContext _mongoDb;
    private readonly JwtSettings _jwtSettings;

    public AuthController(MongoDbContext mongoDb, IOptions<JwtSettings> jwtSettings)
    {
        _mongoDb = mongoDb;
        _jwtSettings = jwtSettings.Value;
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthResponse>> Register([FromBody] RegisterRequest request)
    {
        var invitesCollection = _mongoDb.GetCollection<Invite>("invites");
        var usersCollection = _mongoDb.GetCollection<User>("users");

        // Verify invite code
        var invite = await invitesCollection.Find(i => 
            i.InviteCode == request.InviteCode && 
            !i.IsUsed && 
            i.ExpiresAt > DateTime.UtcNow).FirstOrDefaultAsync();

        if (invite == null)
            return BadRequest(new { message = "Invalid or expired invite code" });

        // Check if user already exists
        var existingUser = await usersCollection.Find(u => 
            u.Email == request.Email || u.Username == request.Username).FirstOrDefaultAsync();

        if (existingUser != null)
            return BadRequest(new { message = "User already exists" });

        // Create new user
        var user = new User
        {
            Username = request.Username,
            Email = request.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            PublicKey = request.PublicKey,
            PrivateKeyEncrypted = request.PrivateKeyEncrypted,
            InvitedBy = new List<string> { invite.InviterUserId }
        };

        await usersCollection.InsertOneAsync(user);

        // Mark invite as used
        var update = Builders<Invite>.Update
            .Set(i => i.IsUsed, true)
            .Set(i => i.UsedAt, DateTime.UtcNow)
            .Set(i => i.UsedByUserId, user.Id);
        await invitesCollection.UpdateOneAsync(i => i.Id == invite.Id, update);

        // Update inviter's invited users list
        var userUpdate = Builders<User>.Update
            .AddToSet(u => u.InvitedUsers, user.Id);
        await usersCollection.UpdateOneAsync(u => u.Id == invite.InviterUserId, userUpdate);

        var token = GenerateJwtToken(user);

        return Ok(new AuthResponse
        {
            Token = token,
            UserId = user.Id,
            Username = user.Username
        });
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginRequest request)
    {
        var usersCollection = _mongoDb.GetCollection<User>("users");
        
        var user = await usersCollection.Find(u => u.Email == request.Email).FirstOrDefaultAsync();

        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            return Unauthorized(new { message = "Invalid credentials" });

        if (!user.IsActive)
            return Unauthorized(new { message = "Account is inactive" });

        // Update last login
        var update = Builders<User>.Update.Set(u => u.LastLoginAt, DateTime.UtcNow);
        await usersCollection.UpdateOneAsync(u => u.Id == user.Id, update);

        var token = GenerateJwtToken(user);

        return Ok(new AuthResponse
        {
            Token = token,
            UserId = user.Id,
            Username = user.Username
        });
    }

    private string GenerateJwtToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_jwtSettings.SecretKey);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email)
            }),
            Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationMinutes),
            Issuer = _jwtSettings.Issuer,
            Audience = _jwtSettings.Audience,
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
