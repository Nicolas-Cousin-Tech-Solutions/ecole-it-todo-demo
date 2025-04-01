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
);

app.MapGet("/todos/{id}", async (int id, TodoContext db) =>
    await db.TodoItems.FirstOrDefaultAsync(todo => todo.Id == id)
);

app.MapPost("/todos", async (TodoItem todo, TodoContext db) =>
{
    // Modification d'un record
    // var todo1 = new TodoItem(1, "Creer le site Blazor pour le cours", false);
    // var todo2 = todo1 with { IsDone = true};

    db.TodoItems.Add(todo);
    await db.SaveChangesAsync();
    return Results.Created($"/todos/{todo.Id}", todo);
});

app.Run();