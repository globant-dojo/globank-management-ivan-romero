using GenericRepository.Dtos;
using GenericRepository.Models;

namespace ReportesApp.Services
{
    public interface IReportesService
    {
        List<ReporteDto> Convertir(Cliente cliente);
    }
}
