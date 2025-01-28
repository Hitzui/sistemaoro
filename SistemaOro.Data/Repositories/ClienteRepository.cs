using System.Data;
using Microsoft.EntityFrameworkCore;
using NLog;
using SistemaOro.Data.Configuration;
using SistemaOro.Data.Entities;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SistemaOro.Data.Repositories;

public class ClienteRepository(DataContext context) : FacadeEntity<Cliente>(context), IClienteRepository
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    public async Task<string> CodCliente()
    {
        var codcaja = ConfiguracionGeneral.Caja;
        var findParameters = await context.Id.SingleOrDefaultAsync();
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
        await using var tx = await context.Database.BeginTransactionAsync();
        try
        {
            context.ChangeTracker.Clear();
            var findParameters = await context.Id.SingleOrDefaultAsync();
            if (findParameters is null)
            {
                ErrorSms = "No existen los parametros en la base de datos";
                return false;
            }

            findParameters.Codcliente += 1;
            await context.AddAsync(cliente);
            await context.SaveChangesAsync();
            context.ChangeTracker.Clear();
            Logger.Info($"Se han guardado los datos del cliente {DateTime.Now}");
            await tx.CommitAsync();
            return true;
        }
        catch (Exception e)
        {
            var mensaje = "";
            if (e.InnerException is not null)
            {
                mensaje = e.InnerException.Message;
            }

            Logger.Error(e, "Ha ocurrido un error en el metodo Create");
            ErrorSms = $"No fue posible crear el cliente en la base de datos {e.Message} {mensaje}";
            await tx.RollbackAsync();
            return false;
        }
    }

    public async Task<bool> Update(Cliente cliente)
    {
        try
        {
            context.Clientes.Update(cliente);
            var result = await context.SaveChangesAsync();
            context.ChangeTracker.Clear();
            return result > 0;
        }
        catch (Exception e)
        {
            var mensaje = "";
            if (e.InnerException is not null)
            {
                mensaje = e.InnerException.Message;
            }

            ErrorSms = $"No fue posible actualizar el cliente en la base de datos {e.Message} {mensaje}";
            Logger.Error(e, "Ha ocurrido un error en el metodo Update");
            return false;
        }
    }

    public async Task<bool> Delete(Cliente cliente)
    {
        try
        {
            context.Clientes.Remove(cliente);
            var result = await context.SaveChangesAsync() > 0;
            if (result)
            {
                return true;
            }

            ErrorSms = $"No fue posible eliminar los datos del cliente con el codigo {cliente.Codcliente} en la base de datos";
            return false;
        }
        catch (Exception e)
        {
            ErrorSms = e.Message;
            Logger.Error(e, "Ha ocurrido un error en el metodo Update");
            return false;
        }
    }


    public IQueryable<Cliente> FetchPage(int skip, int take)
    {
        return context.Clientes.AsNoTracking().Skip(skip).Take(take);
    }


    public Task<Cliente?> FindById(string codcliente)
    {
        return context.Clientes
            .Include(cliente => cliente.TipoDocumento)
            .SingleOrDefaultAsync(cliente => cliente.Codcliente == codcliente);
    }

    public Task<List<Cliente>> FindAll()
    {
        return Get(orderBy: query => query.OrderBy(cliente => cliente.Nombres)).ToListAsync();
    }

    public Task<Cliente?> FindByNombre(string nombre)
    {
        return FindByProperty(cliente => cliente.Nombres == nombre);
    }

    public Task<Cliente?> FindByApellido(string apellido)
    {
        return FindByProperty(cliente => cliente.Apellidos == apellido);
    }

    public Task<Cliente?> FindByCedula(string cedula)
    {
        return FindByProperty(cliente => cliente.Numcedula == cedula);
    }

    public Task<List<Cliente>> FilterByName(string nombre)
    {
        return Get(filter: cliente => cliente.Nombres.Contains(nombre)).ToListAsync();
    }

    public Task<List<Cliente>> FilterByNameAndPagination(string nombre, int page = 0)
    {
        return Get(
            filter: f => f.Nombres.Contains(nombre),
            orderBy: o => o.OrderBy(p => p.Nombres),
            pageSize: 10,
            pageNumber: page
        ).ToListAsync();
    }

    public Task<List<Cliente>> FilterByCodigo(string codigo)
    {
        return Get(filter: cliente => cliente.Codcliente.Contains(codigo)).ToListAsync();
    }

    public Task<List<Cliente>> FilterByApellido(string apellido)
    {
        return Get(filter: cliente => cliente.Apellidos!.Contains(apellido)).ToListAsync();
    }

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
        return Get(filter: cliente => cliente.Nombres.Contains(filtro) || cliente.Apellidos!.Contains(filtro)).ToListAsync();
    }
}