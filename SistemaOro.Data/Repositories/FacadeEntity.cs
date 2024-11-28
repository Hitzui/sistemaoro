using System.Diagnostics;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using NLog;
using SistemaOro.Data.Entities;
using SistemaOro.Data.Exceptions;

namespace SistemaOro.Data.Repositories;

public abstract class FacadeEntity<TEntity>(DataContext context) : ICrudRepository<TEntity>
    where TEntity : class
{
    private readonly Logger Logger = LogManager.GetCurrentClassLogger();
    private readonly DbSet<TEntity> _set = context.Set<TEntity>();
    
    public string ErrorSms { get; protected set; } = "";

    public virtual async Task<TEntity?> GetByIdAsync(object id)
    {
        var find= await _set.FindAsync(id);
        if (find is not null) return find;
        ErrorSms = $"No existe en el cotexto actual la entidad con el codigo indicado: {id}";
        return null;

    }

    public virtual async Task<bool> AddAsync(TEntity? entity)
    {
        context.ChangeTracker.Clear();
        if (entity is null)
        {
            ErrorSms = "No existe en el cotexto actual la entidad";
            return false;
        }
        try
        {
            await _set.AddAsync(entity);
            await context.SaveChangesAsync();
            context.ChangeTracker.Clear();
            return true;
        }
        catch (Exception e)
        {
            var mensaje2=string.Empty;
            if (e.InnerException is not null)
            {
                mensaje2 = e.InnerException.Message;
            }
            ErrorSms = $"No existe en el cotexto actual la entidad. Error: {e.Message} {mensaje2}";
            Logger.Error(e, "Error en agregar FacadeEntity");
            context.ChangeTracker.Clear();
            return false;
        }
    }

    public virtual async Task<bool> UpdateAsync(TEntity? entity)
    {
        try
        {
            context.ChangeTracker.Clear();
            if (entity is null)
            {
                throw new EntityValidationException("Debe especificar la entidad a actualizar");
            }

            _set.Update(entity);
            await context.SaveChangesAsync();
            context.ChangeTracker.Clear();
            return true;
        }
        catch (Exception e)
        {
            var mensaje2=string.Empty;
            if (e.InnerException is not null)
            {
                mensaje2 = e.InnerException.Message;
            }
            ErrorSms = $"Error: {e.Message} {mensaje2}";
            context.ChangeTracker.Clear();
            Logger.Error(e, "Error en UpdateAsync FacadeEntity");
            return false;
        }
    }

    public virtual async Task<bool> DeleteAsync(object entity)
    {
        context.ChangeTracker.Clear();
        try
        {
            context.Remove(entity);
            await context.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            var mensaje = "";
            if (e.InnerException is not null)
            {
                mensaje = e.InnerException.Message;
            }
            ErrorSms = $"Error: {e.Message} {mensaje}";
            context.ChangeTracker.Clear();
            Logger.Error(e, "Error en DeleteAsync FacadeEntity");
            return false;
        }

    }

    public virtual async Task<List<TEntity>> FindAll()
    {
        await using var context2 = new DataContext();
        return await context2.Set<TEntity>().AsNoTracking().ToListAsync();
    }

    protected IQueryable<TEntity> Get(
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        string includeProperties = "",
        int? pageSize = null,
        int? pageNumber = null
    )
    {
        var query = _set.AsNoTracking();

        // Aplicar filtro, si existe
        if (filter is not null)
        {
            query = query.Where(filter);
        }

        // Aplicar orden, si existe
        if (orderBy is not null)
        {
            query = orderBy(query);
        }

        // Incluir propiedades relacionadas
        if (!string.IsNullOrWhiteSpace(includeProperties))
        {
            foreach (var property in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(property.Trim());
            }
        }

        // Aplicar paginación si ambos parámetros están especificados y son válidos
        if (pageNumber.HasValue && pageSize.HasValue && pageNumber > 0 && pageSize > 0)
        {
            query = query.Skip((pageNumber.Value - 1) * pageSize.Value).Take(pageSize.Value);
        }

        return query;
    }


    protected Task<TEntity?> FindByProperty(Expression<Func<TEntity, bool>> predicate)
    {
        return _set.AsNoTracking().SingleOrDefaultAsync(predicate);
    }

}