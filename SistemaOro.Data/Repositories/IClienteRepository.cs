﻿using System.Data;
using SistemaOro.Data.Entities;

namespace SistemaOro.Data.Repositories;

public interface IClienteRepository : ICrudRepository<Cliente>
{
    Task<string> CodCliente();
    Task<bool> Create(Cliente cliente);
    Task<List<Cliente>> FindAll();
    Task<Cliente?> FindByNombre(string nombre);
    Task<Cliente?> FindByApellido(string apellido);
    Task<Cliente?> FindByCedula(string cedula);
    Task<List<Cliente>> FilterByName(string nombre);
    Task<List<Cliente>> FilterByNameAndPagination(string nombre, int page=0);
    Task<List<Cliente>> FilterByCodigo(string codigo);
    Task<List<Cliente>> FilterByApellido(string apellido);
    IQueryable<Cliente> FetchPage(int skip, int take);
    string? ErrorSms { get; }
    Task<bool> ExisteCliente(string codcliente);
    
    Task<DataTable> FindAllClientesDataTable();

    Task<List<Cliente>> FilterByNameAndApellido(string filtro);
}