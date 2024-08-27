namespace DigitalHub_Task.Models.Entity
{
    public class Task
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public TaskStatus Status { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public enum TaskStatus
    {
        NotStarted,
        Started,
        Completed
    }

}
