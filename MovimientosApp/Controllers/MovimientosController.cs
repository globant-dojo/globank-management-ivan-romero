using AutoMapper;
using GenericRepository.Dtos;
using GenericRepository.Interfaces;
using GenericRepository.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovimientosApp.Services;

namespace MovimimientosApp.Controllers;

[ApiController]
[Route("api/[Controller]")]
public class MovimientosController : ControllerBase
{
    private readonly IRepository<Movimiento> _movimientosRepository;
    private readonly IRepository<Cuenta> _cuentasRepository;
    private readonly IMovimientosService _movimientosService;
    private readonly IMapper _mapper;

    public MovimientosController(IRepository<Movimiento> movimientosRepository, IMovimientosService movimientosService, IMapper mapper, IRepository<Cuenta> cuentasRepository)
    {
        _movimientosRepository = movimientosRepository;
        _movimientosService = movimientosService;
        _mapper = mapper;
        _cuentasRepository = cuentasRepository;
    }

    [HttpGet]
    public ActionResult<Cliente> Get()
    {
        var results = _movimientosRepository.GetAll();

        return Ok(results);
    }

    [HttpGet("{id:int}", Name = "GetById")]
    public async Task<IActionResult> GetById(int id)
    {
        var movimientoEnBDD = await _movimientosRepository.GetByIdAsync(id);

        if (movimientoEnBDD is null)
            return BadRequest($"Movimiento con Id = {id} no existe.");

        return Ok(movimientoEnBDD);
    }

    [HttpPost]
    public async Task<ActionResult<Cliente>> Post([FromBody] MovimientoCreateDto movimientoDto)
    {
        if (movimientoDto is null)
            return BadRequest(ModelState);

        var cuentaEnBDD = await _cuentasRepository.GetByIdAsync(movimientoDto.CuentaId);

        if (cuentaEnBDD is null)
            return BadRequest($"Cuenta con Id = {movimientoDto.CuentaId} no existe.");

        var movimiento = _mapper.Map<Movimiento>(movimientoDto);

        if (movimiento.TipoMovimiento is null)
            return BadRequest($"TipoMovimiento no válido.");

        var saldoNuevo = _movimientosService.CalcularNuevoSaldo(cuentaEnBDD, movimiento.TipoMovimiento, movimiento.Valor);

        if (saldoNuevo < 0)
        {
            return BadRequest($"Cuenta con Id = {movimientoDto.CuentaId} no tiene saldo suficiente.");
        }

        cuentaEnBDD.SaldoInicial = saldoNuevo;

        movimiento.Valor = movimiento.TipoMovimiento == "D" ? -Math.Abs(movimiento.Valor) : Math.Abs(movimiento.Valor);
        movimiento.Saldo = saldoNuevo;
        movimiento.Cuenta = cuentaEnBDD;

        var result2 = await _movimientosRepository.AddAsync(movimiento);

        if (result2.Id < 0)
        {            
            return BadRequest("Los cambios no han sido guardados.");
        }

        var movimientoResult = _mapper.Map<MovimientoDto>(movimiento);

        return CreatedAtRoute(nameof(GetById), new { id = movimiento.Id }, movimientoResult);
    }

    [HttpDelete("{id:int}/")]
    public async Task<ActionResult<Movimiento>> Delete(int id)
    {
        var movimientoEnBDD = await _movimientosRepository.GetByIdAsync(id);

        if (movimientoEnBDD is null)
        {
            return BadRequest($"Movimiento con Id = {id} no existe.");
        }

        _movimientosRepository.Delete(movimientoEnBDD);

        var result = await _movimientosRepository.UnitOfWork.SaveChangesAsync();
        if (result <= 0)
            return BadRequest();

        return NoContent();
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Put(int id, [FromBody] MovimientoUpdateDto movimientoDto)
    {
        if (movimientoDto is null)
            return BadRequest(ModelState);

        if (id != movimientoDto.Id)
            return BadRequest("Id no válido o no concuerdan.");

        var movimientoEnBDD = await _movimientosRepository.GetByIdAsync(movimientoDto.Id);

        if (movimientoEnBDD is null)
            return BadRequest($"Movimiento con Id = {id} no existe.");

        var cuentaEnBDD = await _cuentasRepository.GetByIdAsync(id);

        if (cuentaEnBDD is null)
            return BadRequest($"Cuenta con Id = {movimientoDto.CuentaId} no existe.");

        movimientoEnBDD.Fecha = movimientoDto.Fecha;
        movimientoEnBDD.TipoMovimiento = movimientoDto.TipoMovimiento;
        movimientoEnBDD.Valor = movimientoDto.Valor;
        movimientoEnBDD.Saldo = movimientoDto.Saldo;
        movimientoEnBDD.Estado = movimientoDto.Estado;
        movimientoEnBDD.Cuenta = cuentaEnBDD;

        _movimientosRepository.Update(movimientoEnBDD);

        //TO DO Implementar lógica para recalcular entidades

        var result = await _movimientosRepository.UnitOfWork.SaveChangesAsync();

        if (result <= 0)
            return BadRequest("Los cambios no han sido guardados.");

        return NoContent();
    }
}
