namespace TODOAPI.Models.Response
{
    public class LoginResponse
    {
        public bool Success { get; set; }
        public string Email { get; set; } = null!;
        public string Token { get; set; } = null!;
    }
    public class UserRegisterResponse
    {
        public Guid Id { get; set; }

        // Correo del usuario unico=
        public string? Email { get; set; }

        // Nombre del usuario
        public string Name { get; set; }
        // Apellido del usuario
        public string LastName { get; set; }
    }
}
