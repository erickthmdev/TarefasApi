using TarefasApi.Data;
using TarefasApi.Models;
using TarefasApi.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.Data;
using static TarefasApi.DTOs.AuthDtos;

namespace TarefasApi.Endpoints;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/auth").WithTags("Auth");
        group.MapPost("/register", Register);
        group.MapPost("/login", Login);
    }

    private static async Task<IResult> Register(RegisterUserRequest request, AppDbContext db, ITokenService tokenService)
    {
        if (await db.Users.AnyAsync(u => u.Email == request.Email))
            return Results.Conflict("Email já cadastrado.");

        var user = new User { Name = request.Name, Email = request.Email };
        var hasher = new PasswordHasher<User>();
        user.PasswordHash = hasher.HashPassword(user, request.Password);

        db.Users.Add(user);
        await db.SaveChangesAsync();

        var (token, expiresAt) = tokenService.GenerateToken(user);
        return Results.Created($"/auth/users/{user.Id}", new AuthResponse(token, expiresAt, user.Name, user.Email));
    }

    private static async Task<IResult> Login(LoginRequest request, AppDbContext db, ITokenService tokenService)
    {
        var user = await db.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
        if (user is null) return Results.Unauthorized();

        var hasher = new PasswordHasher<User>();
        if (hasher.VerifyHashedPassword(user, user.PasswordHash, request.Password) == PasswordVerificationResult.Failed)
            return Results.Unauthorized();

        var (token, expiresAt) = tokenService.GenerateToken(user);
        return Results.Ok(new AuthResponse(token, expiresAt, user.Name, user.Email));
    }
}