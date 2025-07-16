using System.Linq.Expressions;

namespace Framework.Repositories
{
    public interface IRepository<T>
    {
        Task<List<T>> ListAsync<U>(Expression<Func<T, bool>>? filter = null, bool tracking = false) where U : T;

        Task<T?> SingleOrDefaultAsync<U>(Expression<Func<T, bool>> filter, bool tracking = false) where U : T;

        Task<bool> AnyAsync<U>(Expression<Func<T, bool>> filter) where U : T;
    }
}
