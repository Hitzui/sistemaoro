using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using SistemaOro.Data.Entities;

namespace SistemaOro.Data.Repositories;

public class GenericRepo<T>(DbContext dbContext)
    where T : class
{
    private readonly DbSet<T> _dbSet = dbContext.Set<T>();

    public IQueryable<T> Get(
        Expression<Func<T, bool>>? filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        string includeProperties = "",
        int? pageSize = null, int? pageNumber = null
    )
    {
        var query = _dbSet.AsNoTracking();

        if (filter is not null) query = query.Where(filter);

        if (orderBy is not null) query = orderBy(query);

        foreach (var property in includeProperties.Split(
                     new char[] { ','}, StringSplitOptions.RemoveEmptyEntries))
        {
            query = query.Include(property);
        }

        return (pageNumber is not null && pageSize is not null) ?
            query.Skip((pageNumber.Value - 1) * pageSize.Value).Take(pageSize.Value): query;
    }
}