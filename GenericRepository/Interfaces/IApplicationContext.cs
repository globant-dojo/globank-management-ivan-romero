using GenericRepository.Models;
using Microsoft.EntityFrameworkCore;

namespace GenericRepository.Interfaces;

public interface IApplicationContext : IUnitOfWork
{
    public DbSet<Cliente> Clientes { get; }
    public DbSet<Cliente> Cuentas { get; }
    public DbSet<Cliente> Movimientos { get; }
}