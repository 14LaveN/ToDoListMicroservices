using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Impl;
using ToDoListMicroservices.BackgroundTasks.QuartZ.Jobs;

namespace ToDoListMicroservices.BackgroundTasks.QuartZ.Schedulers;

/// <summary>
/// Represents the user database scheduler class.
/// </summary>
public sealed class UserDbScheduler
    : AbstractScheduler<BaseDbJob>;