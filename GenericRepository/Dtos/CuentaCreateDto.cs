namespace GenericRepository.Dtos
{
    public class CuentaCreateDto
    {
        public string? NumeroCuenta { get; set; }
        public string? TipoCuenta { get; set; }
        public decimal SaldoInicial { get; set; }
        public bool? Estado { get; set; }
        public int ClienteId { get; set; }
    }
}
