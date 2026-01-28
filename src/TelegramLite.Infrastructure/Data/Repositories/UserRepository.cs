using TelegramLite.Domain.Entities;
using TelegramLite.Infrastructure.Data.MongoDB;

namespace TelegramLite.Infrastructure.Data.Repositories;

/// <summary>
/// Repository for User entities.
/// </summary>
public interface IUserRepository : IRepository<User>
{
    /// <summary>
    /// Finds a user by email address.
    /// </summary>
    /// <param name="email">The email address.</param>
    /// <returns>The user if found, otherwise null.</returns>
    Task<User?> FindByEmailAsync(string email);
}

/// <summary>
/// MongoDB implementation of IUserRepository.
/// </summary>
public class UserRepository : MongoRepository<User>, IUserRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UserRepository"/> class.
    /// </summary>
    /// <param name="context">The MongoDB context.</param>
    public UserRepository(MongoDbContext context) : base(context.Users)
    {
    }

    /// <inheritdoc/>
    public async Task<User?> FindByEmailAsync(string email)
    {
        return await FindOneAsync(u => u.Email == email);
    }
}
