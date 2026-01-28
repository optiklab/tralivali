using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System.Security.Claims;
using Tralivali.API.DTOs;
using Tralivali.Core.Models;
using Tralivali.Infrastructure.Services;

namespace Tralivali.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class InvitesController : ControllerBase
{
    private readonly MongoDbContext _mongoDb;

    public InvitesController(MongoDbContext mongoDb)
    {
        _mongoDb = mongoDb;
    }

    [HttpPost]
    public async Task<ActionResult<Invite>> CreateInvite([FromBody] CreateInviteRequest request)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var invitesCollection = _mongoDb.GetCollection<Invite>("invites");

        var invite = new Invite
        {
            InviterUserId = userId,
            Email = request.Email,
            InviteCode = Guid.NewGuid().ToString("N")[..8].ToUpper(),
            ExpiresAt = DateTime.UtcNow.AddDays(7)
        };

        await invitesCollection.InsertOneAsync(invite);

        return Ok(invite);
    }

    [HttpGet]
    public async Task<ActionResult<List<Invite>>> GetMyInvites()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var invitesCollection = _mongoDb.GetCollection<Invite>("invites");
        var invites = await invitesCollection
            .Find(i => i.InviterUserId == userId)
            .ToListAsync();

        return Ok(invites);
    }
}
