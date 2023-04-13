using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccessData.Models
{
    public class User
    {

        // Id de usuario que es un uuid
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        // Correo del usuario unico
        [Required]
        public string? Email { get; set; }

        // Nombre del usuario
        [Required]
        public string Name { get; set; }
        // Apellido del usuario
        [Required]
        public string LastName { get; set; }

        [Required]
        public string?Password{ get; set; }
        public virtual ICollection<Access>? Accesses { get; set; }

    }
}
