using GenericRepository.Models;

namespace CuentasApp.Services
{
    public interface ICuentasService
    {
        bool CuentaTieneMovimientos(Cuenta cuenta);
    }

}
