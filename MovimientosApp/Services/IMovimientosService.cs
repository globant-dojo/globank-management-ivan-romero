using GenericRepository.Models;

namespace MovimientosApp.Services
{
    public interface IMovimientosService
    {
        bool CuentaTieneSaldo(Movimiento movimiento, Cuenta cuenta);
        decimal CalcularNuevoSaldo(Cuenta cuenta, string tipoMovimiento, decimal valor);
    }
}
