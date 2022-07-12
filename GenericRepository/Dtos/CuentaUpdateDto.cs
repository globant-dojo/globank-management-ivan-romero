namespace GenericRepository.Dtos
{
    public class CuentaUpdateDto
    {
        public int Id { get; set; }
        public string NumeroCuenta { get; set; } = null!;
        public string TipoCuenta { get; set; } = null!;
        public decimal SaldoInicial { get; set; }
        public bool? Estado { get; set; }        
        public int ClienteId { get; set; }
    }
}
