using Backend.Entities;

namespace Backend.Repositories;

/// <summary>
/// Repository interface for User entity operations.
/// Provides abstraction layer for data access with clean architecture principles.
/// </summary>
public interface IUserRepository
{
    /// <summary>
    /// Retrieves a user by their unique identifier.
    /// </summary>
    /// <param name="id">The user's unique identifier.</param>
    /// <param name="cancellationToken">Token to cancel the async operation.</param>
    /// <returns>The user if found; otherwise null.</returns>
    Task<User?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a user by their username.
    /// </summary>
    /// <param name="username">The username to search for.</param>
    /// <param name="cancellationToken">Token to cancel the async operation.</param>
    /// <returns>The user if found; otherwise null.</returns>
    Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a user by their email address.
    /// </summary>
    /// <param name="email">The email address to search for.</param>
    /// <param name="cancellationToken">Token to cancel the async operation.</param>
    /// <returns>The user if found; otherwise null.</returns>
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves all active users with cursor-based pagination.
    /// </summary>
    /// <param name="pageSize">Maximum number of records to return.</param>
    /// <param name="cursor">Optional cursor for pagination (last ID from previous page).</param>
    /// <param name="cancellationToken">Token to cancel the async operation.</param>
    /// <returns>A collection of users.</returns>
    Task<IEnumerable<User>> GetAllAsync(int pageSize = 50, int? cursor = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves all users by role with cursor-based pagination.
    /// </summary>
    /// <param name="roleId">The role identifier to filter by.</param>
    /// <param name="pageSize">Maximum number of records to return.</param>
    /// <param name="cursor">Optional cursor for pagination.</param>
    /// <param name="cancellationToken">Token to cancel the async operation.</param>
    /// <returns>A collection of users with the specified role.</returns>
    Task<IEnumerable<User>> GetByRoleAsync(int roleId, int pageSize = 50, int? cursor = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new user in the database.
    /// </summary>
    /// <param name="user">The user entity to create.</param>
    /// <param name="cancellationToken">Token to cancel the async operation.</param>
    /// <returns>The created user with assigned identifier.</returns>
    Task<User> CreateAsync(User user, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing user's information.
    /// </summary>
    /// <param name="user">The user entity with updated values.</param>
    /// <param name="cancellationToken">Token to cancel the async operation.</param>
    /// <returns>The updated user.</returns>
    Task<User> UpdateAsync(User user, CancellationToken cancellationToken = default);

    /// <summary>
    /// Soft deletes a user by setting IsActive to false.
    /// </summary>
    /// <param name="id">The user's unique identifier.</param>
    /// <param name="cancellationToken">Token to cancel the async operation.</param>
    /// <returns>True if successfully deleted; false if user not found.</returns>
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Permanently deletes a user from the database.
    /// </summary>
    /// <param name="id">The user's unique identifier.</param>
    /// <param name="cancellationToken">Token to cancel the async operation.</param>
    /// <returns>True if successfully deleted; false if user not found.</returns>
    Task<bool> PermanentDeleteAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if a username exists in the database.
    /// </summary>
    /// <param name="username">The username to check.</param>
    /// <param name="cancellationToken">Token to cancel the async operation.</param>
    /// <returns>True if username exists; otherwise false.</returns>
    Task<bool> UsernameExistsAsync(string username, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if an email address exists in the database.
    /// </summary>
    /// <param name="email">The email address to check.</param>
    /// <param name="cancellationToken">Token to cancel the async operation.</param>
    /// <returns>True if email exists; otherwise false.</returns>
    Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates the last login timestamp for a user.
    /// </summary>
    /// <param name="userId">The user's unique identifier.</param>
    /// <param name="cancellationToken">Token to cancel the async operation.</param>
    /// <returns>True if successfully updated; false if user not found.</returns>
    Task<bool> UpdateLastLoginAsync(int userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the total count of active users.
    /// </summary>
    /// <param name="cancellationToken">Token to cancel the async operation.</param>
    /// <returns>The total count of active users.</returns>
    Task<int> GetCountAsync(CancellationToken cancellationToken = default);
}
