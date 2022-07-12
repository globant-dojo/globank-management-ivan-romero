namespace GenericRepository.Dtos
{
    public class ClienteCreateDto
    {
        public string Nombre { get; set; } = null!;
        public string Contrasena { get; set; } = null!;
        public string? Identificacion { get; set; }
        public string Direccion { get; set; } = null!;
        public string Telefono { get; set; } = null!;
        public bool? Estado { get; set; }
    }
}
