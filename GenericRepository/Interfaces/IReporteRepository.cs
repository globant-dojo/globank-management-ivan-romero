using GenericRepository.Models;

namespace GenericRepository.Interfaces;

public interface IReporteRepository : IRepository<Cliente>, IReadRepository<Cliente>
{
    Task<Cliente> GetReportePorFechas(int clienteId, DateTime fechaIni, DateTime fechaFin);
}