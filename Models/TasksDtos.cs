namespace TasksApi.Models
{
    public class CreateTaskDto
    {
        public required string Title { get; set; }
        public string? Description { get; set; }
        public DateTime DueDate { get; set; }
    }
    public class UpdateTaskDto
    {
        public required string Title { get; set; }
        public string? Description { get; set; }
        public DateTime DueDate { get; set; }
        public TodoStatus Status { get; set; }
    }
    public class DeleteTaskDto { }

    public class UpdateStatusDto
    {
        public TodoStatus? Status { get; set; }
    }

}
