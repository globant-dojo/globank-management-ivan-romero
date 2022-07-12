using AutoMapper;
using ClientesApp.Services;
using GenericRepository.Dtos;
using GenericRepository.Interfaces;
using GenericRepository.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClientesApp.Controllers;

[ApiController]
[Route("[Controller]")]
public class ClientesController : ControllerBase
{
    private readonly IRepository<Cliente> _clientesRepository;
    private readonly IClientesService _clientesService;
    private readonly IMapper _mapper;

    public ClientesController(IRepository<Cliente> clientesRepository, IClientesService clientesService, IMapper mapper)
    {
        _clientesRepository = clientesRepository;
        _clientesService = clientesService;
        _mapper = mapper;
    }

    [HttpGet]
    public ActionResult<Cliente> Get()
    {
        var results = _clientesRepository.GetAll();

        return Ok(_mapper.Map<List<ClienteDto>>(results));
    }

    [HttpGet("{id:int}", Name = "GetById")]
    public async Task<IActionResult> GetById(int id)
    {
        var clienteEnBDD = await _clientesRepository.GetByIdAsync(id);

        if (clienteEnBDD is null)
            return NotFound($"Cliente con Id = {id} no existe.");

        return Ok(_mapper.Map<ClienteDto>(clienteEnBDD));
    }

    [HttpGet("{id:int}/cuentas")]
    public async Task<IActionResult> GetClienteCuentas(int id)
    {
        var clientesCuentasEnBdd = await _clientesRepository.GetAllIncluding(a => a.Cuentas).FirstOrDefaultAsync(a => a.Id == id);

        if (clientesCuentasEnBdd is null)
            return NotFound($"Cliente con Id = {id} no existe.");

        return Ok(_mapper.Map<ClienteDto>(clientesCuentasEnBdd));
    }

    [HttpPost]
    public async Task<ActionResult<Cliente>> Post([FromForm] Cliente cliente)
    {
        if (cliente is null)
            return BadRequest(ModelState);

        _clientesRepository.Add(cliente);

        var result = await _clientesRepository.UnitOfWork.SaveChangesAsync();
        if (result <= 0)
            return BadRequest("Los cambios no han sido guardados.");

        return CreatedAtRoute(nameof(GetById), new { id = cliente.Id }, cliente);
    }

    [HttpDelete("{id:int})")]
    public async Task<ActionResult<Cliente>> Delete(int id)
    {
        var clienteYSusCuentas = await _clientesRepository.GetAllIncluding(a => a.Cuentas).FirstOrDefaultAsync(a => a.Id == id);


        if (clienteYSusCuentas is null)
        {
            return NotFound($"Cliente con Id = {id} no existe.");
        }

        var tieneCuentas = _clientesService.ClienteTieneCuentas(clienteYSusCuentas);

        if(tieneCuentas)
        {
            return NotFound($"Cliente con Id = {id} tiene cuentas.");
        }

        _clientesRepository.Delete(clienteYSusCuentas);

        var result = await _clientesRepository.UnitOfWork.SaveChangesAsync();
        if (result <= 0)
            return BadRequest();

        return NoContent();
    }

    [HttpDelete("{id:int}/Cascada)")]
    public async Task<ActionResult<Cliente>> DeleteCascada(int id)
    {
        var cliente = await _clientesRepository.GetByIdAsync(id);

        if (cliente is null)
        {
            return NotFound($"Cliente con Id = {id} no existe.");
        }

        _clientesRepository.Delete(cliente);        

        var result = await _clientesRepository.UnitOfWork.SaveChangesAsync();

        if (result <= 0)
            return BadRequest();

        return NoContent();
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Put(int id, [FromBody] Cliente client)
    {
        if (client is null)
            return BadRequest(ModelState);

        if (id != client.Id)
            return BadRequest("Id no válido o no concuerdan.");

        var clienteEnBDD = await _clientesRepository.GetByIdAsync(id);

        if (clienteEnBDD is null)
            return NotFound($"Cliente con Id = {id} no existe.");

        clienteEnBDD.Telefono = client.Telefono;
        clienteEnBDD.Nombre = client.Nombre;
        clienteEnBDD.Identificacion = client.Identificacion;
        clienteEnBDD.Direccion = client.Direccion;
        clienteEnBDD.Estado = client.Estado;
        clienteEnBDD.Contrasena = client.Contrasena;

        _clientesRepository.Update(clienteEnBDD);

        var result = await _clientesRepository.UnitOfWork.SaveChangesAsync();
        if (result <= 0)
            return BadRequest("Los cambios no han sido guardados.");

        return NoContent();
    }

    [HttpPatch("{id:int}")]
    public async Task<IActionResult> Patch(int id, [FromBody] JsonPatchDocument<Cliente> patchDoc)
    {
        if (patchDoc is null)
            return BadRequest(ModelState);

        var existEntity = await _clientesRepository.GetByIdAsync(id);
        if (existEntity is null)
            return NotFound($"Cliente con Id = {id} no existe.");

        patchDoc.ApplyTo(existEntity, ModelState);

        var isValid = TryValidateModel(existEntity);
        
        if (!isValid)
            return BadRequest(ModelState);

        try
        {
            await _clientesRepository.UnitOfWork.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            throw;
        }

        return NoContent();
    }
}
