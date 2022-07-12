using AutoMapper;
using CuentasApp.Services;
using GenericRepository.Dtos;
using GenericRepository.Interfaces;
using GenericRepository.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CuentasApp.Controllers;

[ApiController]
[Route("api/[Controller]")]
public class CuentasController : ControllerBase
{
    private readonly IRepository<Cuenta> _cuentasRepository;
    private readonly IRepository<Cliente> _clienteRepository;
    private readonly ICuentasService _cuentasService;
    private readonly IMapper _mapper;
    public CuentasController(IRepository<Cuenta> cuentasRepository, ICuentasService cuentasService, IMapper mapper, IRepository<Cliente> clienteRepository)
    {
        _cuentasRepository = cuentasRepository;
        _cuentasService = cuentasService;
        _mapper = mapper;
        _clienteRepository = clienteRepository;
    }

    [HttpGet]
    public ActionResult<Cuenta> Get()
    {
        var results = _cuentasRepository.GetAll();

        return Ok(_mapper.Map<List<CuentaDto>>(results));
    }

    [HttpGet("{id:int}", Name = "GetById")]
    public async Task<IActionResult> GetById(int id)
    {
        var cuenta = await _cuentasRepository.GetByIdAsync(id);

        if (cuenta is null)
            return BadRequest($"Cuenta con Id = {id} no existe.");

        return Ok(_mapper.Map<CuentaDto>(cuenta));
    }

    [HttpGet("{id:int}/movimientos")]
    public async Task<IActionResult> GetCuentaMovimientos(int id)
    {
        var cuentasMovimientosEnBdd = await _cuentasRepository.GetAllIncluding(a => a.Movimientos).FirstOrDefaultAsync(a => a.Id == id);

        if (cuentasMovimientosEnBdd is null)
            return BadRequest($"Cuenta con Id = {id} no existe.");

        return Ok(_mapper.Map<CuentaDto>(cuentasMovimientosEnBdd));
    }

    [HttpPost]
    public async Task<ActionResult<Cuenta>> Post([FromBody] CuentaCreateDto cuentaDto)
    {
        if (cuentaDto is null)
            return BadRequest(ModelState);

        var clienteEnBDD = await _clienteRepository.GetByIdAsync(cuentaDto.ClienteId);

        if (clienteEnBDD is null)
            return BadRequest($"Cliente con Id = {cuentaDto.ClienteId} no existe.");

        var cuenta = _mapper.Map<Cuenta>(cuentaDto);
        cuenta.Cliente = clienteEnBDD;
        _cuentasRepository.Add(cuenta);

        var result = await _cuentasRepository.UnitOfWork.SaveChangesAsync();
        if (result <= 0)
            return BadRequest("Los cambios no han sido guardados.");

        var cuentaResult = _mapper.Map<CuentaDto>(cuenta);

        return CreatedAtRoute(nameof(GetById), new { id = cuenta.Id }, cuentaResult);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<Cuenta>> Delete(int id)
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

    [HttpDelete("{id:int}/Cascada")]
    public async Task<ActionResult<Cuenta>> DeleteCascada(int id)
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
    public async Task<IActionResult> Put(int id, [FromBody] CuentaUpdateDto cuentaDto)
    {
        if (cuentaDto is null)
            return BadRequest(ModelState);

        if (id != cuentaDto.Id)
            return BadRequest("Id no válido o no concuerdan.");

        var cuentaEnBDD = await _cuentasRepository.GetByIdAsync(id);

        if (cuentaEnBDD is null)
            return BadRequest($"Cuenta con Id = {id} no existe.");

        var clienteEnBDD = await _clienteRepository.GetByIdAsync(cuentaDto.ClienteId);

        if (clienteEnBDD is null)
            return BadRequest($"Cliente con Id = {cuentaDto.ClienteId} no existe.");

        cuentaEnBDD.TipoCuenta = cuentaDto.TipoCuenta;
        cuentaEnBDD.NumeroCuenta = cuentaDto.NumeroCuenta;
        cuentaEnBDD.SaldoInicial = cuentaDto.SaldoInicial;
        cuentaEnBDD.Estado = cuentaDto.Estado;
        cuentaEnBDD.Cliente = clienteEnBDD;

        _cuentasRepository.Update(cuentaEnBDD);

        var result = await _cuentasRepository.UnitOfWork.SaveChangesAsync();
        if (result <= 0)
            return BadRequest("Los cambios no han sido guardados.");

        return NoContent();
    }
}
