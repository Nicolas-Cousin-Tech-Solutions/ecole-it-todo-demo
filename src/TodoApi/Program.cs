using Microsoft.EntityFrameworkCore;
using TodoApi.Data;
using TodoApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<TodoContext>(options =>
    options.UseInMemoryDatabase("TodoList"));

builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapGet("/", () => "SERVER IS RUNNING!")
    .WithName("GetRoot");

app.MapGet("/todos", async (TodoContext db) =>
    await db.TodoItems.ToListAsync()
)
.WithName("GetTodos")
.WithDescription("Get the todo list.");

app.MapGet("/todos/{id}", async (int id, TodoContext db) =>
    await db.TodoItems.FirstOrDefaultAsync(todo => todo.Id == id)
)
.WithName("GetTodo")
.WithDescription("Get the todo with the provided id or null.");

app.MapPut("/todos/{id}", async (int id, TodoItem todo, TodoContext db) =>
    {
        var todoToRemove = await db.TodoItems.FirstOrDefaultAsync(todo => todo.Id == id);

        if (todoToRemove is { })
        {
            db.TodoItems.Remove(todoToRemove);
            db.TodoItems.Add(todo);
            await db.SaveChangesAsync();
        }
        
        return Results.Ok(todo);
    }
)
.WithName("UpdateTodoWithTodoItem");


app.MapPut("/todos/{id}/done", async (int id, TodoContext db) =>
    {
        var todo = await db.TodoItems.FirstOrDefaultAsync(todo => todo.Id == id);

        if (todo is { IsDone: false })
        {
            db.TodoItems.Remove(todo);
            db.TodoItems.Add(todo with {IsDone = true});
            await db.SaveChangesAsync();
        }

        return Results.Ok(todo);
     }
)
.WithName("UpdateTodoItemAsDone");

app.MapPost("/todos", async (TodoItem todo, TodoContext db) =>
{
    // Modification d'un record
    // var todo1 = new TodoItem(1, "Creer le site Blazor pour le cours", false);
    // var todo2 = todo1 with { IsDone = true};

    db.TodoItems.Add(todo);
    await db.SaveChangesAsync();
    return Results.Created($"/todos/{todo.Id}", todo);
})
.WithName("CreateTodo")
.WithDescription("Create a new toto item.");

app.MapDelete("/todos/{id}", async (int id, TodoContext db) =>
{
    var todo = await db.TodoItems.FirstOrDefaultAsync(todo => todo.Id == id);

    if (todo is { })
    {
        db.TodoItems.Remove(todo);
        await db.SaveChangesAsync();
    }

    return Results.NoContent();
});

app.Run();