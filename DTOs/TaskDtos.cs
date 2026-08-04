using TarefasApi.Enums;

namespace TarefasApi.DTOs
{
    public class TaskDtos
    {
        public record CreateTaskRequest(string Title, string? Description, DateTime? DueDate, TaskType Type);
        public record UpdateTaskRequest(string Title, string? Description, DateTime? DueDate);
        public record TaskResponse(int Id, string Title, string? Description, bool IsCompleted, DateTime? DueDate, TaskType Type);
    }
}
