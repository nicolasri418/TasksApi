// using System.Text.Json.Serialization;  // ya añadido para enums como string

using Microsoft.AspNetCore.Mvc;
using TasksApi.Models;
using TasksApi.Services;

[ApiController]
[Route("api/[controller]")]
public sealed class TasksController : ControllerBase
{
    private readonly ITaskStore _store;
    public TasksController(ITaskStore store) => _store = store;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TaskItem>>> GetAll()
        => Ok(await _store.GetAllAsync());   

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<TaskItem>> GetById(Guid id)
    {
        var item = await _store.GetAsync(id);
        return item is null ? NotFound() : Ok(item);
    }

    [HttpPost]
    public async Task<ActionResult<TaskItem>> Create([FromBody] CreateTaskDto dto)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        var created = await _store.AddAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created); 
    }

    [HttpPut("{id:guid}/status")]
    public async Task<ActionResult<TaskItem>> UpdateStatus(Guid id, [FromBody] UpdateStatusDto dto)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        if (dto.Status is null) return ValidationProblem("Status is required.");
        var updated = await _store.UpdateStatusAsync(id, dto.Status.Value); 
        return updated is null ? NotFound() : Ok(updated);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var ok = await _store.DeleteAsync(id); 
        return ok ? NoContent() : NotFound();
    }
}