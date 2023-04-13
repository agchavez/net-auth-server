using AccessData.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TODOAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccessController : ControllerBase
    {

        private readonly AccessData.AccessDataContext _context;
        public AccessController(AccessData.AccessDataContext context)
        {
            _context = context;
        }

        // GET: api/<AccessController>
        [HttpPost]
        public async Task<ActionResult<Access>> Create(Access access)
        {
            // Agregar el nuevo acceso a la base de datos
            _context.Accesses.Add(access);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new { id = access.Id }, access);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Access>> Get(Guid id)
        {
            var access = await _context.Accesses.FindAsync(id);

            if (access == null)
            {
                return NotFound();
            }

            return access;
        }
    }
}
