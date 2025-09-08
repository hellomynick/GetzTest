namespace GetzTest.Infrastructure.Repositories;

public interface IBaseRepository<T>
    where T : class
{
    Task CreateAsync(T entity);
    T Update(T entity);
    bool Delete(T entity);
}
