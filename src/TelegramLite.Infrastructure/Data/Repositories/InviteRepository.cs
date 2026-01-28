using TelegramLite.Domain.Entities;
using TelegramLite.Infrastructure.Data.MongoDB;

namespace TelegramLite.Infrastructure.Data.Repositories;

/// <summary>
/// Repository for Invite entities.
/// </summary>
public interface IInviteRepository : IRepository<Invite>
{
    /// <summary>
    /// Finds an invite by its token.
    /// </summary>
    /// <param name="token">The invite token.</param>
    /// <returns>The invite if found, otherwise null.</returns>
    Task<Invite?> FindByTokenAsync(string token);

    /// <summary>
    /// Finds invites created by a specific user.
    /// </summary>
    /// <param name="inviterId">The inviter's user ID.</param>
    /// <returns>A list of invites created by the user.</returns>
    Task<IEnumerable<Invite>> FindByInviterIdAsync(string inviterId);
}

/// <summary>
/// MongoDB implementation of IInviteRepository.
/// </summary>
public class InviteRepository : MongoRepository<Invite>, IInviteRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InviteRepository"/> class.
    /// </summary>
    /// <param name="context">The MongoDB context.</param>
    public InviteRepository(MongoDbContext context) : base(context.Invites)
    {
    }

    /// <inheritdoc/>
    public async Task<Invite?> FindByTokenAsync(string token)
    {
        return await FindOneAsync(i => i.Token == token);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Invite>> FindByInviterIdAsync(string inviterId)
    {
        return await FindAsync(i => i.InviterId == inviterId);
    }
}
