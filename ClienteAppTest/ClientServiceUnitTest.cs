using AutoMapper;
using ClientesApp.Controllers;
using ClientesApp.Services;
using GenericRepository.Dtos;
using GenericRepository.Interfaces;
using GenericRepository.Models;
using GenericRepository.Profiles;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace ClienteAppTest
{
    public class ClientServiceUnitTest
    {
        private static IMapper _mapper;
    

        public ClientServiceUnitTest()
        {
            if (_mapper == null)
            {
                var mappingConfig = new MapperConfiguration(mc =>
                {
                    mc.AddProfile(new GenericProfile());
                });
                IMapper mapper = mappingConfig.CreateMapper();
                _mapper = mapper;
            }
        }

        [Fact]
        public void ClienteController_GetAll_RetornaValor()
        {
            var mockRepository = new Mock<IRepository<Cliente>>();
            var mockServices = new Mock<IClientesService>();
          
            mockRepository.Setup(repo => repo.GetAll(true)).Returns(ObtenerClientesTest());
            var controller = new ClientesController(mockRepository.Object, mockServices.Object, _mapper);

            // Act
            var result = controller.Get().Result as OkObjectResult;
            // Assert
            var prueba = (List<ClienteDto>) result.Value;
            
            Assert.True(prueba.Any());     
        }

        [Fact]
        public void ClienteController_GetById_RetornaValor()
        {
            var mockRepository = new Mock<IRepository<Cliente>>();
            var mockServices = new Mock<IClientesService>();
            CancellationToken cancellationToken = default;
            int id = 1;
            mockRepository.Setup(repo => repo.GetByIdAsync(id, cancellationToken)).Returns(ObtenerClienteAsyncTest());
            var controller = new ClientesController(mockRepository.Object, mockServices.Object, _mapper);

            // Act
            var result = controller.GetById(id).Result as OkObjectResult;
            // Assert
            var prueba = (ClienteDto)result.Value;

            Assert.Equal(prueba.Id, id);
        }

        [Fact]
        public async Task ClienteController_GetById_RetornaBadRequest()
        {
            var mockRepository = new Mock<IRepository<Cliente>>();
            var mockServices = new Mock<IClientesService>();
            CancellationToken cancellationToken = default;
            int id = 2;
            mockRepository.Setup(repo => repo.GetByIdAsync(id, cancellationToken)).Returns(ObtenerClienteAsyncTestNull);
            var controller = new ClientesController(mockRepository.Object, mockServices.Object, _mapper);

            // Act
            var result = await controller.GetById(id);
            // Assert
            Assert.Equal(400, (result as ObjectResult)?.StatusCode);
        }

        [Fact]
        public async Task ClienteController_Post_RetornaValor()
        {
            var mockRepository = new Mock<IRepository<Cliente>>();
            var mockServices = new Mock<IClientesService>();
            CancellationToken cancellationToken = default;
            ClienteCreateDto clienteDto = new ClienteCreateDto { Contrasena = "123", Direccion = "Quito", Nombre = "Test", Telefono = "099999999" };
            var cliente = _mapper.Map<Cliente>(clienteDto);
            cliente.Id = 1;
            mockRepository.Setup(repo => repo.Add(cliente)).Returns(cliente);
            mockRepository.Setup(repo => repo.UnitOfWork.SaveChangesAsync(cancellationToken)).Returns(SaveChangesAsyncTest(1));


            var controller = new ClientesController(mockRepository.Object, mockServices.Object, _mapper);

            // Act
            var result = await controller.Post(clienteDto) ;
            // Assert
            Assert.Equal(201, (result.Result as ObjectResult)?.StatusCode);
        }

        [Fact]
        public async Task ClienteController_Post_RetornaBadRequest()
        {
            var mockRepository = new Mock<IRepository<Cliente>>();
            var mockServices = new Mock<IClientesService>();
            CancellationToken cancellationToken = default;
            ClienteCreateDto clienteDto = new ClienteCreateDto { Contrasena = "123", Direccion = "Quito", Nombre = "Test", Telefono = "099999999" };
            var cliente = _mapper.Map<Cliente>(clienteDto);
            cliente.Id = 1;
            mockRepository.Setup(repo => repo.Add(cliente)).Returns(cliente);
            mockRepository.Setup(repo => repo.UnitOfWork.SaveChangesAsync(cancellationToken)).Returns(SaveChangesAsyncTest(0));


            var controller = new ClientesController(mockRepository.Object, mockServices.Object, _mapper);

            // Act
            var result = await controller.Post(clienteDto);
            // Assert
            Assert.Equal(400, (result.Result as ObjectResult)?.StatusCode);
        }

        private Task<int> SaveChangesAsyncTest(int i)
        {
            return Task.FromResult(i);
        }

        private IQueryable<Cliente> ObtenerClientesTest()
        {
            var clientes = new List<Cliente>();
            clientes.Add(
                    new Cliente
                    {
                        Id = 1,
                        Nombre = "Carlos García",
                        Genero = "Masculino",
                        Edad = 20,
                        Identificacion = "175489123",
                        Direccion = "Quito",
                        Telefono = "0987512455",
                        Estado = true
                    });
            return clientes.AsQueryable();
        }

        private Task<Cliente> ObtenerClienteAsyncTest()
        {            
            var cliente =
                    new Cliente
                    {
                        Id = 1,
                        Nombre = "Carlos García",
                        Genero = "Masculino",
                        Edad = 20,
                        Identificacion = "175489123",
                        Direccion = "Quito",
                        Telefono = "0987512455",
                        Estado = true
                    };
            return Task.FromResult(cliente);
        }

        private Task<Cliente> ObtenerClienteAsyncTestNull()
        {
            return Task.FromResult<Cliente>(null);
        }
    }
}