namespace WolverineSandbox;

public sealed class TodoRepository(MongoContext context) : MongoRepository<TodoItem>(context)
{
    public TodoItem AddTodo(TodoItem todo)
    {
        return Insert(todo);
    }
}
