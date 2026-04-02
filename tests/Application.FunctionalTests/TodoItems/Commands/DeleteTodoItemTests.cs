using directory.web.Application.TodoItems.Commands.CreateTodoItem;
using directory.web.Application.TodoItems.Commands.DeleteTodoItem;
using directory.web.Application.TodoLists.Commands.CreateTodoList;
using directory.web.Domain.Entities;

namespace directory.web.Application.FunctionalTests.TodoItems.Commands;

public class DeleteTodoItemTests : TestBase
{
    [Test]
    public async Task ShouldRequireValidTodoItemId()
    {
        var command = new DeleteTodoItemCommand(99);

        await Should.ThrowAsync<NotFoundException>(() => TestApp.SendAsync(command));
    }

    [Test]
    public async Task ShouldDeleteTodoItem()
    {
        var listId = await TestApp.SendAsync(new CreateTodoListCommand
        {
            Title = "New List"
        });

        var itemId = await TestApp.SendAsync(new CreateTodoItemCommand
        {
            ListId = listId,
            Title = "New Item"
        });

        await TestApp.SendAsync(new DeleteTodoItemCommand(itemId));

        var item = await TestApp.FindAsync<TodoItem>(itemId);

        item.ShouldBeNull();
    }
}
