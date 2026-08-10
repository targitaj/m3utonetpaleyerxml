using MediaNotes.Api.Contracts;
using MediaNotes.Api.Data;
using MediaNotes.Api.Models;
using MediaNotes.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MediaNotes.Api.Controllers;

[ApiController]
[Route("api/auth")]
public sealed class AuthController(NotesDbContext db, IConfiguration configuration) : ControllerBase
{
    [HttpPost("register")]
    public async Task<ActionResult<AuthResponse>> Register(RegisterRequest request, CancellationToken cancellationToken)
    {
        var email = request.Email.Trim().ToLowerInvariant();
        if (await db.Users.AnyAsync(x => x.Email == email, cancellationToken))
            return Conflict(new { message = "Аккаунт с таким email уже существует." });

        var (hash, salt) = Passwords.Hash(request.Password);
        var user = new User { Email = email, PasswordHash = hash, PasswordSalt = salt };
        db.Users.Add(user);
        await db.SaveChangesAsync(cancellationToken);
        return Ok(CreateResponse(user));
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login(LoginRequest request, CancellationToken cancellationToken)
    {
        var email = request.Email.Trim().ToLowerInvariant();
        var user = await db.Users.SingleOrDefaultAsync(x => x.Email == email, cancellationToken);
        if (user is null || !Passwords.Verify(request.Password, user.PasswordHash, user.PasswordSalt))
            return Unauthorized(new { message = "Неверный email или пароль." });
        return Ok(CreateResponse(user));
    }

    private AuthResponse CreateResponse(User user)
    {
        var issued = new TokenService(configuration).Create(user);
        return new AuthResponse(issued.Token, user.Email, issued.ExpiresUtc);
    }
}
