using System.Collections.Concurrent;
using TasksApi.Models;
using TodoStatus = TasksApi.Models.TodoStatus;

namespace TasksApi.Services;

public interface ITaskStore
{
    Task<IReadOnlyCollection<TaskItem>> GetAllAsync();
    Task<TaskItem?> GetAsync(Guid id);
    Task<TaskItem> AddAsync(CreateTaskDto dto);
    Task<TaskItem?> UpdateStatusAsync(Guid id, TodoStatus status);
    Task<bool> DeleteAsync(Guid id);
}

public sealed class InMemoryTaskStore : ITaskStore
{
    private static readonly ConcurrentDictionary<Guid, TaskItem> _db = new();

    public async Task<IReadOnlyCollection<TaskItem>> GetAllAsync()
        => await Task.FromResult((IReadOnlyCollection<TaskItem>)_db.Values.OrderBy(t => t.DueDate).ToList());

    public async Task<TaskItem?> GetAsync(Guid id)
        => await Task.FromResult(_db.TryGetValue(id, out var t) ? t : null);

    public async Task<TaskItem> AddAsync(CreateTaskDto dto)
    {
        var task = new TaskItem
        {
            Id = Guid.NewGuid().ToString(),
            Title = dto.Title.Trim(),
            Description = dto.Description?.Trim(),
            DueDate = dto.DueDate.ToString("o"),
            Status = TodoStatus.Pending,
            CreatedAt = DateTime.UtcNow.ToString("o")
        };
        return await Task.FromResult(task);
    }

    public Task<TaskItem?> UpdateStatusAsync(Guid id, TodoStatus status)
    {
        if (!_db.TryGetValue(id, out var t)) return Task.FromResult<TaskItem?>(null);
        t.Status = status;
        t.UpdatedAt = DateTime.UtcNow.ToString("o");
        return Task.FromResult<TaskItem?>(t);
    }

    public Task<bool> DeleteAsync(Guid id)
        => Task.FromResult(_db.TryRemove(id, out _));
}