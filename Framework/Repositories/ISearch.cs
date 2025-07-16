using Framework.Models;

namespace Framework.Repositories
{
    public interface ISearch<T>
    {
        FoundItems<T> Search<U>(Search<T> search) where U : T;
    }
}
