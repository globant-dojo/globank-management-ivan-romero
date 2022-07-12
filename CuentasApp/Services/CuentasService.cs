using GenericRepository.Models;

namespace CuentasApp.Services
{
    public class CuentasService : ICuentasService
    {
        public CuentasService() 
        {

        }

        public bool CuentaTieneMovimientos(Cuenta cuenta)
        {
            return cuenta.Movimientos.Any();
        }
    }
}
