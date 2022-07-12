using GenericRepository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using ReportesApp.Services;

namespace MovimimientosApp.Controllers;

[ApiController]
[Route("api/Cliente/{clienteId}/[Controller]")]
public class ReportesController : ControllerBase
{
    private readonly IReporteRepository _reportesRepository;
    private readonly IReportesService _reportesService;

    public ReportesController(IReporteRepository reportesRepository, IReportesService reportesService)
    {
        _reportesRepository = reportesRepository;
        _reportesService = reportesService;
    }

    [HttpGet]
    public async Task<IActionResult> Get(int clienteId, DateTime fechaIni, DateTime fechaFin)
    {
        var datosParaReporte = await _reportesRepository.GetReportePorFechas(clienteId, fechaIni, fechaFin);
        if (datosParaReporte is null)
            return NotFound($"Cliente con Id = {clienteId} no existe.");

        var result = _reportesService.ObtenerFormatoSalida(datosParaReporte);

        return Ok(result);
    }
}
