namespace GenericRepository.Dtos
{
    public class CuentaDto
    {
        public int Id { get; set; }
        public string NumeroCuenta { get; set; } = null!;
        public string TipoCuenta { get; set; } = null!;
        public decimal SaldoInicial { get; set; }
        public bool? Estado { get; set; }
        public ICollection<MovimientoDto> Movimientos { get; set; } = null!;
    }
}
