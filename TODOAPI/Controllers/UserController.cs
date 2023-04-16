using AccessData;
using AccessData.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using TODOAPI.Interfaces;
using TODOAPI.Models.Request;
using System.Security.Claims;


namespace TODOAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly AccessData.AccessDataContext _context;
        private readonly IAuthService _authService;
        public UserController(AccessData.AccessDataContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        // GET: api/<UserController>
        [HttpGet]
        public IActionResult Get()
        {
            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve
            };
            var users = _context.Users.Include(u => u.Accesses).ToList();
            return Ok(users);
        }

        // GET api/<UserController>/5
        [HttpGet("{id}")]
        public string Get(Guid id)
        {
            return "value";
        }

        // POST api/<UserController>
        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Create(User user)
        {
            var email  = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value;
            var id = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var roles = HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();
            return Ok(new
            {
                email,
                id,
                roles
            } );
            // Verificar si el usuario ya existe en la base de datos
            if (await _context.Users.AnyAsync(u => u.Email == user.Email))
            {
                return BadRequest($"El correo electrónico {user.Email} ya está registrado.");
            }

            // Agregar el nuevo usuario a la base de datos
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(user);
        }

        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUserRequest userRegistrationDto)
        {
            try
            {
                var result = await _authService.RegisterAsync(userRegistrationDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Log the exception here
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest loginRequest)
        {
            try
            {
                var result = await _authService.LoginAsync(loginRequest.Email, loginRequest.Password);
                return Ok(result);
            }
            catch (ApplicationException ex)
            {
                return Unauthorized(ex.Message);
            }
        }
    }
}
