using AutoMapper;
using SistemaOro.Data.Entities;
using SistemaOro.Data.Libraries;
using SistemaOro.Data.Repositories;
using SistemaOro.Forms.Models;
using Unity;

namespace SistemaOro.Forms.Services;

public static class MapperConfig
{
    public static Mapper InitializeAutomapper()
    {
        //Provide all the Mapping Configuration
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Cliente, DtoCliente>();
            cfg.CreateMap<DtoCliente, Cliente>();
            cfg.CreateMap<DtoTiposPrecios, TipoPrecio>();
            cfg.CreateMap<TipoPrecio, DtoTiposPrecios>();
        });
        var mapper = new Mapper(config);
        return mapper;
    }
}