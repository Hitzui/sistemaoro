using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using SistemaOro.Data.Entities;

namespace SistemaOro.Data.Repositories;

public abstract class FacadeEntity<TEntity>(DbContext context) : ICrudRepository<TEntity>
    where TEntity : class
{
    private readonly DbContext _context = context ?? throw new ArgumentNullException(nameof(context));
    private readonly DbSet<TEntity> _set = context.Set<TEntity>();
    
    public string ErrorSms { get; set; } = "";

    public virtual async Task<TEntity?> GetByIdAsync(int id)
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
        await _set.AddAsync(entity);
        var result = await _context.SaveChangesAsync()>0;
        if (result)
        {
            return true;
        }
        ErrorSms = "No existe en el cotexto actual la entidad";
        return false;
    }

    public virtual async Task<bool> UpdateAsync(TEntity? entity)
    {
        if (entity is null)
        {
            ErrorSms = "No existe en el cotexto actual la entidad";
            return false;
        }
        _set.Update(entity);
        var result = await _context.SaveChangesAsync()>0;
        if (result)
        {
            return true;
        }
        ErrorSms = "No existe en el cotexto actual la entidad";
        return false;
    }

    public virtual async Task<bool> DeleteAsync(int id)
    {
        var entity = await _set.FindAsync(id);
        if (entity == null) return false;
        _set.Remove(entity);
        var result = await _context.SaveChangesAsync()>0;
        if (result)
        {
            return true;
        }
        ErrorSms = "No existe en el cotexto actual la entidad";
        return false;

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