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
[Route("[Controller]")]
public class MovimientosController : ControllerBase
{
    private readonly IRepository<Movimiento> _movimientosRepository;
    private readonly IRepository<Cuenta> _cuentaRepository;
    private readonly IMovimientosService _movimientosService;
    private readonly IMapper _mapper;

    public MovimientosController(IRepository<Movimiento> movimientosRepository, IMovimientosService movimientosService, IMapper mapper, IRepository<Cuenta> cuentaRepository)
    {
        _movimientosRepository = movimientosRepository;
        _movimientosService = movimientosService;
        _mapper = mapper;
        _cuentaRepository = cuentaRepository;
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
            return NotFound($"Movimiento con Id = {id} no existe.");

        return Ok(movimientoEnBDD);
    }

    [HttpPost]
    public async Task<ActionResult<Cliente>> Post([FromForm] MovimientoCreateDto movimientoDto)
    {
        if (movimientoDto is null)
            return BadRequest(ModelState);

        var cuentaEnBDD = await _cuentaRepository.GetByIdAsync(movimientoDto.CuentaId);

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

        return CreatedAtRoute(nameof(GetById), new { id = movimiento.Id }, movimiento);
    }

    [HttpDelete("{id:int}/)")]
    public async Task<ActionResult<Cliente>> Delete(int id)
    {
        var movimientoEnBDD = await _movimientosRepository.GetByIdAsync(id);

        if (movimientoEnBDD is null)
        {
            return NotFound($"Movimiento con Id = {id} no existe.");
        }

        _movimientosRepository.Delete(movimientoEnBDD);

        var result = await _movimientosRepository.UnitOfWork.SaveChangesAsync();
        if (result <= 0)
            return BadRequest();

        return NoContent();
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Put(int id, [FromBody] Movimiento movimiento)
    {
        if (movimiento is null)
            return BadRequest(ModelState);

        if (id != movimiento.Id)
            return BadRequest("Id no válido o no concuerdan.");

        var movimientoEnBDD = await _movimientosRepository.GetByIdAsync(id);

        if (movimientoEnBDD is null)
            return NotFound($"Cuenta con Id = {id} no existe.");

        movimientoEnBDD.Fecha = movimiento.Fecha;
        movimientoEnBDD.TipoMovimiento = movimiento.TipoMovimiento;
        movimientoEnBDD.Valor = movimiento.Valor;
        movimientoEnBDD.Saldo = movimiento.Saldo;
        movimientoEnBDD.Estado = movimiento.Estado;
        movimientoEnBDD.Cuenta.Id = movimiento.Cuenta.Id;

        _movimientosRepository.Update(movimientoEnBDD);

        var result = await _movimientosRepository.UnitOfWork.SaveChangesAsync();
        if (result <= 0)
            return BadRequest("Los cambios no han sido guardados.");

        return NoContent();
    }

    [HttpPatch("{id:int}")]
    public async Task<IActionResult> Patch(int id, [FromBody] JsonPatchDocument<Movimiento> patchDoc)
    {
        if (patchDoc is null)
            return BadRequest(ModelState);

        var existEntity = await _movimientosRepository.GetByIdAsync(id);

        if (existEntity is null)
            return NotFound($"Cuenta con Id = {id} no existe.");

        patchDoc.ApplyTo(existEntity, ModelState);

        var isValid = TryValidateModel(existEntity);

        if (!isValid)
            return BadRequest(ModelState);

        try
        {
            await _movimientosRepository.UnitOfWork.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            throw;
        }

        return NoContent();
    }
}
