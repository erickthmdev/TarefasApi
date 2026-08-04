using TarefasApi.Enums;

namespace TarefasApi.Models
{
    public class TaskItem
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsCompleted { get; set; } = false;
        public DateTime? DueDate { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public TaskType Type { get; set; }
       
        public int UserId { get; set; }
        public User User { get; set; } = null!;
    }
}
