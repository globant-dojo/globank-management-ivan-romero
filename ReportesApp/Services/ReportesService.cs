using GenericRepository.Dtos;
using GenericRepository.Models;

namespace ReportesApp.Services
{
    public class ReportesService : IReportesService
    {
        public List<ReporteDto> Convertir(Cliente cliente)
        {
            List<ReporteDto> listaMovimientos =  new List<ReporteDto>();

            foreach(var cuenta in cliente.Cuentas)
            {         
                foreach (var movimiento in cuenta.Movimientos)
                {
                    var item = new ReporteDto()
                    {
                        Cliente = cliente.Nombre,
                        NumeroCuenta = cuenta.NumeroCuenta??"",
                        Tipo = cuenta.TipoCuenta??"",
                        Fecha = movimiento.Fecha,
                        SaldoInicial = movimiento.Saldo - movimiento.Valor,
                        SaldoDisponible = movimiento.Saldo,
                        Movimiento = movimiento.Valor,
                        Estado = movimiento.Estado ?? true
                    };                   

                    listaMovimientos.Add(item);
                }                
            }
            return listaMovimientos;
        }
    }
}
