using GenericRepository.Dtos;
using GenericRepository.Models;

namespace ReportesApp.Services
{
    public class ReportesService : IReportesService
    {
        public List<ReporteDto> ObtenerFormatoSalida(Cliente cliente)
        {
            return (from cuenta in cliente.Cuentas
                    from movimiento in cuenta.Movimientos
                    let item = new ReporteDto
                    {
                        Cliente = cliente.Nombre,
                        NumeroCuenta = cuenta.NumeroCuenta ?? "",
                        Tipo = cuenta.TipoCuenta ?? "",
                        Fecha = movimiento.Fecha,
                        SaldoInicial = movimiento.Saldo - movimiento.Valor,
                        SaldoDisponible = movimiento.Saldo,
                        Movimiento = movimiento.Valor,
                        Estado = movimiento.Estado ?? true
                    }
                    select item).ToList();
        }
    }
}
