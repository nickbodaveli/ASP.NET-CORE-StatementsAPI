namespace WebAPI.Helpers.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        void Add<T>(T entity) where T : class ;
        void Update<T>(T entity) where T : class ;
        void Remove<T>(T entity) where T : class ;
        IQueryable<T> Query<T>() where T : class ;
        void Commit();
        Task CommitAsync();
        void Attach<T>(T entity) where T : class ;
    }
}
