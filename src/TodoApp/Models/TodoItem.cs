namespace TodoApp.Models;

public record TodoItem(int Id, string Title, bool IsDone = false);
