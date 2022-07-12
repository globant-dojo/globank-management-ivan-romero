namespace GenericRepository.Dtos
{
    public class ReporteDto
    {
        public DateTime Fecha { get; set; }
        public string Cliente { get; set; } = null!;
        public string NumeroCuenta { get; set; } = null!;
        public string Tipo { get; set; } = null!;
        public Decimal SaldoInicial { get; set; }
        public bool Estado { get; set; }
        public decimal Movimiento { get; set; }
        public decimal SaldoDisponible { get; set; }

    }
}
