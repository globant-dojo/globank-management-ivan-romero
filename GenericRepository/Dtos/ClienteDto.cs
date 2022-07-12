using GenericRepository.Models;

namespace GenericRepository.Dtos
{
    public class ClienteDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string? Genero { get; set; }
        public int? Edad { get; set; }
        public string? Identificacion { get; set; }
        public string Direccion { get; set; } = null!;
        public string Telefono { get; set; } = null!;
        //public string Contrasena { get; set; } = null!;
        public bool? Estado { get; set; }
        public ICollection<CuentaDto> Cuentas { get; set; }
    }
}
