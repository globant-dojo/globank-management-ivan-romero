using GenericRepository.Models;
using Microsoft.AspNetCore.Mvc;

namespace ClientesApp.Services
{
    public interface IClientesService
    {
        bool ClienteTieneCuentas(Cliente cliente);
    }
}
