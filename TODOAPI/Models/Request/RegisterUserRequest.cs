using AccessData.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TODOAPI.Models.Request
{
    public class RegisterUserRequest
    {

        // Correo del usuario unico
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        // Nombre del usuario
        [Required]
        [MinLength(3)]
        public string Name { get; set; }
        // Apellido del usuario
        [Required]
        [MinLength(3)]
        public string LastName { get; set; }

        [Required]
        [MinLength(3)]
        public string Password { get; set; }
    }
}
