namespace Backend.Repositories;

/// <summary>
/// Generic repository interface providing base CRUD operations.
/// Can be extended by specific repositories for domain-specific queries.
/// </summary>
/// <typeparam name="TEntity">The entity type managed by this repository.</typeparam>
public interface IRepository<TEntity> where TEntity : class
{
    /// <summary>
    /// Retrieves an entity by its primary key.
    /// </summary>
    Task<TEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves all entities with cursor-based pagination.
    /// </summary>
    Task<IEnumerable<TEntity>> GetAllAsync(int pageSize = 50, int? cursor = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new entity.
    /// </summary>
    Task<TEntity> CreateAsync(TEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing entity.
    /// </summary>
    Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes an entity by its primary key.
    /// </summary>
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the total count of entities.
    /// </summary>
    Task<int> GetCountAsync(CancellationToken cancellationToken = default);
}
