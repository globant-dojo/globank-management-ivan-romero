using AutoMapper;
using GenericRepository.Dtos;
using GenericRepository.Models;

namespace GenericRepository.Profiles
{
    public class GenericProfile : Profile
    {
        public GenericProfile()
        {
            // Source -> Target
            CreateMap<Cliente, ClienteDto>().ReverseMap();
            CreateMap<Movimiento, MovimientoCreateDto>().ReverseMap();
            CreateMap<Movimiento, MovimientoDto>().ReverseMap();
            CreateMap<Cuenta, CuentaDto>().ReverseMap();
        }
    }
}
