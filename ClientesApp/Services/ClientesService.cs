using GenericRepository.Models;

namespace ClientesApp.Services
{
    public class ClientesService : IClientesService
    {
        public ClientesService()
        {
        }
        public bool ClienteTieneCuentas(Cliente cliente)
        {
            return cliente.Cuentas.Any();
        }
    }
}
