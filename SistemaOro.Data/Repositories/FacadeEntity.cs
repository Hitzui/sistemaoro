using System.Diagnostics;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using SistemaOro.Data.Entities;
using SistemaOro.Data.Exceptions;

namespace SistemaOro.Data.Repositories;

public abstract class FacadeEntity<TEntity>(DataContext context) : ICrudRepository<TEntity>
    where TEntity : class
{
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
        if (entity is null)
        {
            ErrorSms = "No existe en el cotexto actual la entidad";
            return false;
        }
        try
        {
            await _set.AddAsync(entity);
            await context.SaveChangesAsync();
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
            Console.WriteLine(ErrorSms);
            context.ChangeTracker.Clear();
            return false;
        }
    }

    public virtual async Task<bool> UpdateAsync(TEntity? entity)
    {
        try
        {
            if (entity is null)
            {
                throw new EntityValidationException("Debe especificar la entidad a actualizar");
            }
            _set.Update(entity);
            await context.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            ErrorSms = $"No existe en el cotexto actual la entidad. Error: {e.Message} {e.Source}";
            context.ChangeTracker.Clear();
            return false;
        }
    }

    public virtual async Task<bool> DeleteAsync(object id)
    {
        try
        {
            var entity = await _set.FindAsync(id);
            if (entity == null) return false;
            _set.Remove(entity);
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
            ErrorSms = $"No existe en el cotexto actual la entidad. Error: {e.Message} {mensaje}";
            context.ChangeTracker.Clear();
            return false;
        }

    }

    public virtual async Task<List<TEntity>> FindAll()
    {
        return await _set.ToListAsync();
    }

    public IQueryable<TEntity> Get(
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        string includeProperties = "",
        int? pageSize = null, int? pageNumber = null
    )
    {
        var query = _set.AsNoTracking();

        if (filter is not null) query = query.Where(filter);

        if (orderBy is not null) query = orderBy(query);

        foreach (var property in includeProperties.Split(
                     new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
        {
            query = query.Include(property);
        }

        return pageNumber is not null && pageSize is not null ? query.Skip((pageNumber.Value - 1) * pageSize.Value).Take(pageSize.Value) : query;
    }
}