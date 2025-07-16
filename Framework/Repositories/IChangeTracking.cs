namespace Framework.Repositories
{
    public interface IChangeTracking<T>
    {

        public Task AddAsync(T item, ChangeContext changeContext);

        public Task UpdateAsync(T item, ChangeContext changeContext);

        public Task RemoveAsync(T item, ChangeContext changeContext);

    }

    public interface IChangeTracking : IDisposable
    {
        public Task ApplyAsync();
    }


}
