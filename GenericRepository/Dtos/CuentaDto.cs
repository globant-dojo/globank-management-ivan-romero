using GenericRepository.Models;

namespace GenericRepository.Dtos
{
    public class CuentaDto
    {
        public int Id { get; set; }
        public string? NumeroCuenta { get; set; }
        public string? TipoCuenta { get; set; }
        public decimal SaldoInicial { get; set; }
        public bool? Estado { get; set; }
        public ICollection<MovimientoDto> Movimientos { get; set; }
    }
}
