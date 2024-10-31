using System.Data;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SistemaOro.Data.Configuration;
using SistemaOro.Data.Entities;
using SistemaOro.Data.Libraries;

namespace SistemaOro.Data.Repositories;

public class ClienteRepository(IParametersRepository parametersRepository, DataContext context) : IClienteRepository
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
        try
        {
            context.Clientes.Add(cliente);
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
        catch (Exception e)
        {
            Debug.WriteLine(e);
            var mensaje = "";
            if (e.InnerException is not null)
            {
                mensaje = e.InnerException.Message;
            }

            ErrorSms = $"No fue posible crear el cliente en la base de datos {e.Message} {mensaje}";
            return false;
        }
    }

    public async Task<bool> Update(Cliente cliente)
    {
        try
        {
            var findCliente = await FindById(cliente.Codcliente);
            if (findCliente is null)
            {
                ErrorSms = "No existe el cliente a actualizar los datos";
                context.ChangeTracker.Clear();
                return false;
            }

            context.Clientes.Update(cliente);
            var result = await context.SaveChangesAsync();
            context.ChangeTracker.Clear();
            return result > 0;
        }
        catch (Exception e)
        {
            Debug.WriteLine(e.Message);
            ErrorSms = e.Message;
            context.ChangeTracker.Clear();
            return false;
        }
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


    public IQueryable<Cliente> FetchPage(int skip, int take)
    {
        return context.Clientes.AsNoTracking().Skip(skip).Take(take);
    }


    public Task<Cliente?> FindById(string codcliente)
    {
        return context.Clientes.AsNoTracking()
            .Include(cliente => cliente.TipoDocumento)
            .SingleOrDefaultAsync(cliente => cliente.Codcliente == codcliente);
    }

    public Task<List<Cliente>> FindAll()
    {
        return context.Clientes.OrderBy(cliente => cliente.Nombres).ToListAsync();
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
        return await new GenericRepo<Cliente>().Get(
            f => f.Nombres.Contains(nombre),
            o => o.OrderBy(p => p.Nombres),
            "", page, 10
        ).ToListAsync();
    }

    public Task<List<Cliente>> FilterByCodigo(string codigo)
    {
        return context.Clientes.Where(cliente => cliente.Codcliente.Contains(codigo)).ToListAsync();
    }

    public Task<List<Cliente>> FilterByApellido(string apellido)
    {
        return context.Clientes.Where(cliente => cliente.Apellidos!.Contains(apellido)).ToListAsync();
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

    public Task<List<Cliente>> FilterByNameAndApellido(string filtro)
    {
        return context.Clientes.Where(cliente => cliente.Nombres.Contains(filtro) || cliente.Apellidos!.Contains(filtro)).ToListAsync();
    }
}