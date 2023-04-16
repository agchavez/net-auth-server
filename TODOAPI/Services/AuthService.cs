using AccessData.Models;
using AccessData;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TODOAPI.Interfaces;
using TODOAPI.Models.Request;
using Microsoft.EntityFrameworkCore;
using TODOAPI.Models.Response;
using System.Security.Cryptography;
using AUTHSERVER.utils;

public class AuthService : IAuthService
{
    private readonly AccessDataContext _context;
    private readonly IConfiguration _configuration;
    private readonly IErrorService _errorService;

    public AuthService(AccessDataContext context, IConfiguration configuration, IErrorService errorService)
    {
        _context = context;
        _configuration = configuration;
        _errorService = errorService;
    }

    public async Task<(bool isError, ApplicationException? error, UserRegisterResponse? result)> RegisterAsync(RegisterUserRequest userRegistrationDto)
    {
        // Verificar si el usuario ya existe en la base de datos
        var userExist = await _context.Users.SingleOrDefaultAsync(u => u.Email == userRegistrationDto.Email);
        if (userExist != null)
        {
            var _error = _errorService.GetConflictException("El correo electrónico ya está registrado.", 1001);
            return (true, _error, null);
        }

        // Generar el hash de la contraseña
        string asswordHash = Convert.ToBase64String(GeneratePasswordHash(userRegistrationDto.Password));

        // Agregar el nuevo usuario a la base de datos
        var user = new User
        {
            Email = userRegistrationDto.Email,
            Password = asswordHash,
            Name = userRegistrationDto.Name,
            LastName = userRegistrationDto.LastName,
        };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var result = new UserRegisterResponse
        {
            Email = user.Email,
            Name = user.Name,
            LastName = user.LastName,
            Id = user.Id,
        };
        return (false, null, result);
    }

    public async Task<LoginResponse> LoginAsync(string email, string password)
    {
        var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == email);

        if (user == null)
        {
            throw new ApplicationException("El correo electrónico ya está registrado.");
        }

        // Verificar si la contraseña es correcta
        if (!VerifyPasswordHash(password, user.Password))
        {
            throw new ApplicationException("El correo electrónico ya está registrado.");
        }

        // Generar el token JWT
        string token = GenerateJwtToken(user);

        return new LoginResponse
        {
            Success = true,
            Token = token,
            Email = email,
        };
    }

    private byte[] GeneratePasswordHash(string password)
    {
        using (var sha256 = SHA256.Create())
        {
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            byte[] hashBytes = sha256.ComputeHash(passwordBytes);
            return hashBytes;
        }
    }

    private bool VerifyPasswordHash(string password, string passwordUser)
    {
        using (var sha256 = SHA256.Create())
        {
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            byte[] hashBytes = sha256.ComputeHash(passwordBytes);
            byte[] passwordHash = Convert.FromBase64String(passwordUser);
            return hashBytes.SequenceEqual( passwordHash );
        }
    }

    public string GenerateJwtToken(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role,"ADMIN"),
            new Claim(ClaimTypes.Role, "USER")
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("!SomethingSecret!"));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: "apiWithAuthBackend",
            audience: "apiWithAuthBackend",
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
