using Microsoft.EntityFrameworkCore;
using TodoApi.Data;
using TodoApi.Models;
using TodoApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder
.Services
.AddDbContext<TodoContext>(
    options => options.UseInMemoryDatabase("TodoList"));

builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ITodoService, TodoService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/", () => "SERVER IS RUNNING!")
    .WithName("GetRoot");

app.MapGet("/todos", async (ITodoService service) =>
    await service.GetAll()
);

app.MapPost("/todos", async (TodoItem todoItem, TodoContext db) => {
    db.TodoItems.Add(todoItem);
    await db.SaveChangesAsync();
    return Results.Created($"/todos/{todoItem.Id}", todoItem);
});

app.MapGet("/todos/{id}", async (int id, TodoContext db) => {
    return await db.TodoItems.FindAsync(id);
});

app.MapPut("/todos/{id}", async (int id, TodoContext db) => {
    var todoItem = await db.TodoItems.FindAsync(id);

    if (todoItem is null)
    {
        return;
    }

    var updatedTodoItem = todoItem with { IsDone = true };

    db.TodoItems.Remove(todoItem);
    db.TodoItems.Add(updatedTodoItem);

    await db.SaveChangesAsync();

    return;
});

app.Run();
