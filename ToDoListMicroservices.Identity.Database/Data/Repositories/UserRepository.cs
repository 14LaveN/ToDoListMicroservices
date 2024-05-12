using ToDoListMicroservices.Database.Common;
using ToDoListMicroservices.Domain.Common.Core.Primitives.Maybe;
using ToDoListMicroservices.Domain.Common.ValueObjects;
using ToDoListMicroservices.Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using IUserRepository = ToDoListMicroservices.Identity.Database.Data.Interfaces.IUserRepository;

namespace ToDoListMicroservices.Identity.Database.Data.Repositories;

/// <summary>
/// Represents the user repository class.
/// </summary>
/// <param name="userDbContext">The user database context.</param>
public class UserRepository(BaseDbContext userDbContext)
    : IUserRepository
{
    /// <inheritdoc />
    public async Task<Maybe<User>> GetByIdAsync(Guid userId) =>
            await userDbContext
                .Set<User>()
                .AsNoTracking()
                .AsSplitQuery()
                .FirstOrDefaultAsync(x=>x.Id == userId) 
            ?? throw new ArgumentNullException();

    /// <inheritdoc />
    public async Task<Maybe<User>> GetByNameAsync(string name) =>
        await userDbContext
            .Set<User>()
            .FirstOrDefaultAsync(x=>x.UserName == name) 
        ?? throw new ArgumentNullException();

    /// <inheritdoc />
    public async Task<Maybe<User>> GetByEmailAsync(EmailAddress emailAddress) =>
        await userDbContext
            .Set<User>()
            .FirstOrDefaultAsync(x=>x.EmailAddress == emailAddress) 
        ?? throw new ArgumentNullException();
}