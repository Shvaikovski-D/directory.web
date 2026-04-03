namespace directory.web.Application.Common.Exceptions;

public class NotFoundException : Exception
{
    public string? ResourceType { get; }

    public object? ResourceId { get; }

    public NotFoundException(string resourceName, object resourceId)
        : base($"Ресурс '{resourceName}' с идентификатором '{resourceId}' не найден.")
    {
        ResourceType = resourceName;
        ResourceId = resourceId;
    }

    public NotFoundException(string message) 
        : base(message)
    {
        ResourceType = null;
        ResourceId = null;
    }
}
