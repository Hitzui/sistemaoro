using System.Data;
using Microsoft.EntityFrameworkCore;
using SistemaOro.Data.Configuration;
using SistemaOro.Data.Entities;
using SistemaOro.Data.Libraries;

namespace SistemaOro.Data.Repositories;

public class ClienteRepository(IParametersRepository parametersRepository,DataContext context) : IClienteRepository
{
    public async Task<string> CodCliente()
    {
        var codcaja = ConfiguracionGeneral.Caja;
        var codagencia = ConfiguracionGeneral.Agencia;
        var findParameters = await parametersRepository.RecuperarParametros();
        if (findParameters is null)
        {
            ErrorSms = "No existen los parametros en la base de datos";
            return "";
        }

        var codigo = findParameters.Codcliente + 1;
        return $"{codcaja}{codigo.ToString().PadLeft(10, '0')}";
    }

    public async Task<bool> Create(Cliente cliente)
    {
        context.Add(cliente);
        var findParameters = await parametersRepository.RecuperarParametros();
        if (findParameters is null)
        {
            ErrorSms = "No existen los parametros en la base de datos";
            return false;
        }

        findParameters.Codcliente += 1;
        var result = await context.SaveChangesAsync() > 0;
        if (result)
        {
            return true;
        }

        ErrorSms = "No fue posible crear los datos del cliente en el sistema.";
        return false;
    }

    public async Task<bool> Update(Cliente cliente)
    {
        var findCliente = await FindById(cliente.Codcliente);
        if (findCliente is null)
        {
            ErrorSms = $"No existe el cliente con el codigo {cliente.Codcliente} en la base de datos";
            return false;
        }

        context.Entry(findCliente).CurrentValues.SetValues(cliente);
        var result = await context.SaveChangesAsync() > 0;
        if (result)
        {
            return true;
        }

        ErrorSms = $"No fue posible guardar los datos del cliente con el codigo {cliente.Codcliente} en la base de datos";
        return false;
    }

    public async Task<bool> Delete(Cliente cliente)
    {
        var find = await FindById(cliente.Codcliente);
        if (find is null)
        {
            ErrorSms = $"No existe el cliente con el codigo {cliente.Codcliente} en la base de datos";
            return false;
        }

        context.Clientes.Remove(cliente);
        var result = await context.SaveChangesAsync() > 0;
        if (result)
        {
            return true;
        }

        ErrorSms = $"No fue posible eliminar los datos del cliente con el codigo {cliente.Codcliente} en la base de datos";
        return false;
    }

    public async Task<Cliente?> FindById(string codcliente)
    {
        return await context.Clientes.SingleOrDefaultAsync(cliente => cliente.Codcliente == codcliente);
    }

    public async Task<List<Cliente>> FindAll()
    {
        return await context.Clientes.OrderBy(cliente => cliente.Nombres).ToListAsync();
    }

    public async Task<Cliente?> FindByNombre(string nombre)
    {
        return await context.Clientes.FirstOrDefaultAsync(cliente => cliente.Nombres == nombre);
    }

    public async Task<Cliente?> FindByApellido(string apellido)
    {
        return await context.Clientes.FirstOrDefaultAsync(cliente => cliente.Apellidos == apellido);
    }

    public async Task<Cliente?> FindByCedula(string cedula)
    {
        return await context.Clientes.FirstOrDefaultAsync(cliente => cliente.Numcedula == cedula);
    }

    public async Task<List<Cliente>> FilterByName(string nombre)
    {
        return await context.Clientes.Where(cliente => cliente.Nombres.Contains(nombre)).ToListAsync();
    }

    public async Task<List<Cliente>> FilterByNameAndPagination(string nombre, int page = 0)
    {
        return await new GenericRepo<Cliente>(context).Get(
            filter: f => f.Nombres.Contains(nombre),
            orderBy: o => o.OrderBy(p => p.Nombres),
            pageNumber: page, pageSize: 10
        ).ToListAsync();
    }

    public async Task<List<Cliente>> FilterByCodigo(string codigo)
    {
        return await context.Clientes.Where(cliente => cliente.Codcliente.Contains(codigo)).ToListAsync();
    }

    public async Task<List<Cliente>> FilterByApellido(string apellido)
    {
        return await context.Clientes.Where(cliente => cliente.Apellidos!.Contains(apellido)).ToListAsync();
    }

    public string? ErrorSms { get; private set; }

    public async Task<bool> ExisteCliente(string codcliente)
    {
        var find = await FindById(codcliente);
        return find is null;
    }

    public async Task<DataTable> FindAllClientesDataTable()
    {
        // Paso 1: Realizar la consulta LINQ para obtener los datos
        var query = await FindAll();

        // Paso 2: Crear un objeto DataTable
        var dataTable = new DataTable();

        // Paso 3: Definir las columnas del DataTable
        foreach (var property in typeof(Cliente).GetProperties())
        {
            dataTable.Columns.Add(property.Name, property.PropertyType);
        }

        // Paso 4: Iterar sobre los resultados de la consulta LINQ y agregar cada fila al DataTable
        foreach (var item in query)
        {
            var row = dataTable.NewRow();
            foreach (var property in typeof(Cliente).GetProperties())
            {
                row[property.Name] = property.GetValue(item);
            }

            dataTable.Rows.Add(row);
        }

        return dataTable;
    }

    public async Task<List<Cliente>> FilterByNameAndApellido(string filtro)
    {
        return await context.Clientes.Where(cliente => cliente.Nombres.Contains(filtro) || cliente.Apellidos!.Contains(filtro)).ToListAsync();
    }
}