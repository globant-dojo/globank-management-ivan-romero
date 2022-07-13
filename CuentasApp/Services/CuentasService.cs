using GenericRepository.Models;

namespace CuentasApp.Services
{
    public class CuentasService : ICuentasService
    {
        public bool CuentaTieneMovimientos(Cuenta cuenta)
        {
            return cuenta.Movimientos.Any();
        }
    }
}
