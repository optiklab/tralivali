using TelegramLite.Domain.Entities;
using Xunit;

namespace TelegramLite.Tests.Domain;

/// <summary>
/// Unit tests for User entity.
/// </summary>
public class UserTests
{
    [Fact]
    public void User_Creation_ShouldInitializeWithDefaults()
    {
        // Arrange & Act
        var user = new User();

        // Assert
        Assert.NotNull(user.Id);
        Assert.NotNull(user.Email);
        Assert.NotNull(user.DisplayName);
        Assert.NotNull(user.PasswordHash);
        Assert.NotNull(user.PublicKey);
        Assert.NotNull(user.Devices);
        Assert.Empty(user.Devices);
        Assert.True(user.CreatedAt <= DateTime.UtcNow);
        Assert.Null(user.InvitedBy);
    }

    [Fact]
    public void User_SetProperties_ShouldRetainValues()
    {
        // Arrange
        var user = new User
        {
            Id = "user123",
            Email = "test@example.com",
            DisplayName = "Test User",
            PasswordHash = "hashed_password",
            PublicKey = "public_key_data",
            InvitedBy = "inviter123"
        };

        // Assert
        Assert.Equal("user123", user.Id);
        Assert.Equal("test@example.com", user.Email);
        Assert.Equal("Test User", user.DisplayName);
        Assert.Equal("hashed_password", user.PasswordHash);
        Assert.Equal("public_key_data", user.PublicKey);
        Assert.Equal("inviter123", user.InvitedBy);
    }
}
