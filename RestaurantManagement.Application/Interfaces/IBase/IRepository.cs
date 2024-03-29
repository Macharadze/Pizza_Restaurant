namespace RestaurantManagement.Application.Interfaces.IBase
{
    public interface IRepository<T> where T : class
    {
        Task<List<T>> GetAll(CancellationToken cancellationToken);
        Task<T> GetById(CancellationToken cancellationToken, string entityId);
        Task Create(CancellationToken cancellationToken, T entity);
        Task<bool> Update(CancellationToken cancellationToken, T entity, string entityId);
        Task<bool> Delete(CancellationToken cancellationToken, string entityId);
        Task<bool> Exists(CancellationToken cancellationToken, string entityId, out T existingEntity);
    }
}
