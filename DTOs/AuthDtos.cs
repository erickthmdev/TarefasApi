namespace TarefasApi.DTOs;

public class AuthDtos
{

public record RegisterUserRequest(string Name, string Email, string Password);
public record LoginUserRequest(string Email, string Password);
public record AuthResponse(string Token, DateTime ExpiresAt, string Name, string Email);

}