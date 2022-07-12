using AutoMapper;
using CuentasApp.Services;
using GenericRepository.Interfaces;
using GenericRepository.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CuentasApp.Controllers;

[ApiController]
[Route("[Controller]")]
public class CuentasController : ControllerBase
{
    private readonly IRepository<Cuenta> _cuentasRepository;
    private readonly ICuentasService _cuentasService;
    private readonly IMapper _mapper;
    public CuentasController(IRepository<Cuenta> cuentasRepository, ICuentasService cuentasService, IMapper mapper)
    {
        _cuentasRepository = cuentasRepository;
        _cuentasService = cuentasService;
        _mapper = mapper;
    }

    [HttpGet]
    public ActionResult<Cliente> Get()
    {
        var results = _cuentasRepository.GetAll();

        return Ok(results);
    }

    [HttpGet("{id:int}", Name = "GetById")]
    public async Task<IActionResult> GetById(int id)
    {
        var entity = await _cuentasRepository.GetByIdAsync(id);

        if (entity is null)
            return NotFound($"Cuenta con Id = {id} no existe.");

        return Ok(entity);
    }

    [HttpGet("{id:int}/movimientos")]
    public async Task<IActionResult> GetCuentaMovimientos(int id)
    {
        var entity = await _cuentasRepository.GetAllIncluding(a => a.Movimientos).FirstOrDefaultAsync(a => a.Id == id);

        if (entity is null)
            return NotFound($"Cuenta con Id = {id} no existe.");

        return Ok(entity);
    }

    [HttpPost]
    public async Task<ActionResult<Cuenta>> Post([FromForm] Cuenta entity)
    {
        if (entity is null)
            return BadRequest(ModelState);

        _cuentasRepository.Add(entity);

        var result = await _cuentasRepository.UnitOfWork.SaveChangesAsync();
        if (result <= 0)
            return BadRequest("Los cambios no han sido guardados.");

        return CreatedAtRoute(nameof(GetById), new { id = entity.Id }, entity);
    }

    [HttpDelete("{id:int}/)")]
    public async Task<ActionResult<Cliente>> Delete(int id)
    {
        var cuentasCliente = await _cuentasRepository.GetAllIncluding(a => a.Movimientos).FirstOrDefaultAsync(a => a.Id == id);

        if (cuentasCliente is null)
        {
            return NotFound($"Cuenta con Id = {id} no existe.");
        }

        var tieneCuentas = _cuentasService.CuentaTieneMovimientos(cuentasCliente);

        if (tieneCuentas)
        {
            return NotFound($"Cuenta con Id = {id} tiene movimientos.");
        }

        _cuentasRepository.Delete(cuentasCliente);

        var result = await _cuentasRepository.UnitOfWork.SaveChangesAsync();
        if (result <= 0)
            return BadRequest();

        return NoContent();
    }

    [HttpDelete("{id:int}/Cascada)")]
    public async Task<ActionResult<Cliente>> DeleteCascada(int id)
    {
        var cuenta = await _cuentasRepository.GetByIdAsync(id);

        if (cuenta is null)
        {
            return NotFound($"Cuenta con Id = {id} no existe.");
        }

        _cuentasRepository.Delete(cuenta);

        var result = await _cuentasRepository.UnitOfWork.SaveChangesAsync();

        if (result <= 0)
            return BadRequest();

        return NoContent();
    }
    
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Put(int id, [FromBody] Cuenta cuenta)
    {
        if (cuenta is null)
            return BadRequest(ModelState);

        if (id != cuenta.Id)
            return BadRequest("Id no válido o no concuerdan.");

        var cuentaEnBDD = await _cuentasRepository.GetByIdAsync(id);

        if (cuentaEnBDD is null)
            return NotFound($"Cuenta con Id = {id} no existe.");

        cuentaEnBDD.TipoCuenta = cuenta.TipoCuenta;
        cuentaEnBDD.NumeroCuenta = cuenta.NumeroCuenta;
        cuentaEnBDD.SaldoInicial = cuenta.SaldoInicial;
        cuentaEnBDD.Estado = cuenta.Estado;
        cuentaEnBDD.Cliente.Id = cuenta.Cliente.Id;

        _cuentasRepository.Update(cuentaEnBDD);

        var result = await _cuentasRepository.UnitOfWork.SaveChangesAsync();
        if (result <= 0)
            return BadRequest("Los cambios no han sido guardados.");

        return NoContent();
    }

    [HttpPatch("{id:int}")]
    public async Task<IActionResult> Patch(int id, [FromBody] JsonPatchDocument<Cuenta> patchDoc)
    {
        if (patchDoc is null)
            return BadRequest(ModelState);

        var existEntity = await _cuentasRepository.GetByIdAsync(id);
        
        if (existEntity is null)
            return NotFound($"Cuenta con Id = {id} no existe.");

        patchDoc.ApplyTo(existEntity, ModelState);

        var isValid = TryValidateModel(existEntity);

        if (!isValid)
            return BadRequest(ModelState);

        try
        {
            await _cuentasRepository.UnitOfWork.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            throw;
        }

        return NoContent();
    }
}
