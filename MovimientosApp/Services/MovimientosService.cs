using GenericRepository.Models;

namespace MovimientosApp.Services
{
    public class MovimientosService : IMovimientosService
    {
        public MovimientosService()
        {

        }

        public decimal CalcularNuevoSaldo(Cuenta cuenta, string tipoMovimiento, decimal valor)
        {
            if (tipoMovimiento == "D")
            {
                cuenta.SaldoInicial -= Math.Abs(valor);
            }
            else
            {
                cuenta.SaldoInicial += Math.Abs(valor);
            }

            return cuenta.SaldoInicial;
        }

        public bool CuentaTieneSaldo(Movimiento movimiento, Cuenta cuenta)
        {
            if(movimiento.TipoMovimiento == "C")
                return true;

            return Math.Abs(movimiento.Valor) <= cuenta.SaldoInicial;
        }
    }
}
