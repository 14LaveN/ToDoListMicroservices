using System.Linq.Expressions;
using ToDoListMicroservices.Database.Common.Specifications;

namespace ToDoListMicroservices.Task.Database.Data;

/// <summary>
/// Represents the specification for determining the unprocessed personal event.
/// </summary>
public sealed class DoneTasksSpecification : Specification<Domain.Entities.TaskEntity>
{
    /// <inheritdoc />
    public override Expression<Func<Domain.Entities.TaskEntity, bool>> ToExpression() => 
        taskEntity => taskEntity.IsDone;
}