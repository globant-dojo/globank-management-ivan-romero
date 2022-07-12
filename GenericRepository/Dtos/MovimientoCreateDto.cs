namespace GenericRepository.Dtos
{
    public class MovimientoCreateDto
    {
        public string TipoMovimiento { get; set; } = null!;
        public decimal Valor { get; set; }
        public int CuentaId { get; set; }
    }
}
