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
        public void Test1()
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

        private IQueryable<Cliente> ObtenerClientesTest()
        {
            var clientes = new List<Cliente>();
            clientes.Add(
                    new Cliente()
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
    }
}