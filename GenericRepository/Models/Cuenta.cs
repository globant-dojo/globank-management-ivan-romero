namespace GenericRepository.Models
{
    public class Cuenta    {
        public Cuenta()
        {
            Movimientos = new List<Movimiento>();
        }
        public int Id { get; set; }
        public string? NumeroCuenta { get; set; }
        public string? TipoCuenta { get; set; }
        public decimal SaldoInicial { get; set; }
        public bool? Estado { get; set; }

        public virtual ICollection<Movimiento> Movimientos { get; set; } = null!;
        public virtual Cliente Cliente { get; set; } = null!;

    }
}
