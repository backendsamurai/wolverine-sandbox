using MongoDB.Bson;
using Wolverine.Attributes;

namespace WolverineSandbox;

public record CreateTodo(string Title);


public class TodoItem
{
    public ObjectId Id { get; set; }

    public required string Title { get; set; }

    public bool IsCompleted { get; set; }
}

public static class CreateTodoHandler
{
    public static TodoItem Handle(CreateTodo todo, TodoRepository repo)
    {
        var newTodo = new TodoItem
        {
            Title = todo.Title,
            IsCompleted = false
        };

        var addedTodo = repo.AddTodo(newTodo);

        var randNumber = Random.Shared.Next(1, 6);

        if (randNumber % 2 == 0)
        {
            throw new Exception("Unexpected error");
        }

        return addedTodo;
    }
}
