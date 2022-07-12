using GenericRepository.Models;
using GenericRepository.Interfaces;
using Microsoft.EntityFrameworkCore;
using GenericRepository.Data;

namespace GenericRepository.Repositories;

public class ReporteRepository : BaseRepository<Cliente>, IReporteRepository
{
    private readonly AppDbContext _dbContext;

    public ReporteRepository(AppDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Cliente> GetReportePorFechas(int clienteId, DateTime fechaIni, DateTime fechaFin)
    {
        return await _dbContext.Clientes.Where(con => con.Id == clienteId).Include(c => c.Cuentas).ThenInclude(cue => cue.Movimientos.Where(mov => mov.Fecha <= fechaFin && mov.Fecha >= fechaIni)).FirstOrDefaultAsync();
    }
}