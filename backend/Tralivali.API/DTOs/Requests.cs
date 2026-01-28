namespace Tralivali.API.DTOs;

public class RegisterRequest
{
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string InviteCode { get; set; } = string.Empty;
    public string PublicKey { get; set; } = string.Empty;
    public string PrivateKeyEncrypted { get; set; } = string.Empty;
}

public class LoginRequest
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class CreateInviteRequest
{
    public string Email { get; set; } = string.Empty;
}

public class SendMessageRequest
{
    public string RecipientId { get; set; } = string.Empty;
    public string EncryptedContent { get; set; } = string.Empty;
    public string EncryptedKey { get; set; } = string.Empty;
}

public class AuthResponse
{
    public string Token { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
}
