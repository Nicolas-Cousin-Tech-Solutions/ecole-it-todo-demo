using Microsoft.EntityFrameworkCore;
using TodoApi.Data;
using TodoApi.Models;

namespace TodoApi.Services;

public interface ITodoService
{
    Task<IReadOnlyList<TodoItem>> GetAll();
    TodoItem GetById(int id);
    TodoItem Update(TodoItem todoItem);
    TodoItem Update(int id);
    void Delete(int id);
}

public class TodoService(TodoContext context) : ITodoService
{
    public void Delete(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<IReadOnlyList<TodoItem>> GetAll()
        => await context.TodoItems.ToListAsync();

    public TodoItem GetById(int id)
    {
        throw new NotImplementedException();
    }

    public TodoItem Update(TodoItem todoItem)
    {
        throw new NotImplementedException();
    }

    public TodoItem Update(int id)
    {
        throw new NotImplementedException();
    }
}