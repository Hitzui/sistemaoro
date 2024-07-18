namespace SistemaOro.Data.Repositories;

public interface ICrudRepository<TEntity>
    where TEntity : class
{
    string ErrorSms { get; }
    Task<TEntity?> GetByIdAsync(object id);
    Task<bool> AddAsync(TEntity? entity);
    Task<bool> UpdateAsync(TEntity? entity);
    Task<bool> DeleteAsync(object id);

    Task<List<TEntity>> FindAll();
}