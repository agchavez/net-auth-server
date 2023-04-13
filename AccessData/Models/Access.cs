using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccessData.Models
{
    public class Access
    {

        // Id de usuario que es un uuid
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        // Id del usuario que es un uuid
        [Required]
        public Guid UserId { get; set; }

        // Nombre del acceso
        [Required]
        public string Name { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        // Fecha de registro default now
        public DateTime create_at { get; set; } = DateTime.Now;
    }
}
