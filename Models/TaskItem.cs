namespace TasksApi.Models
{

    public enum TodoStatus
    {
        Pending=0,
        InProgress=1,
        Completed=2
    }
    public sealed class TaskItem
    {
        public string Id { get; set; } = default!;
        public string Title { get; set; } = default!;
        public string? Description { get; set; }
        public string DueDate { get; set; } = default!;
        public TodoStatus Status { get; set; }= TodoStatus.Pending;
        public string CreatedAt { get; set; }=default!;
        public string UpdatedAt { get; set; }= default!;
    }
}
