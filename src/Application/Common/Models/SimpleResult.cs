namespace directory.web.Application.Common.Models;

public class SimpleResult
{
    internal SimpleResult(bool succeeded, IEnumerable<string> errors)
    {
        Succeeded = succeeded;
        Errors = errors.ToArray();
    }

    public bool Succeeded { get; init; }

    public string[] Errors { get; init; }

    public static SimpleResult Success()
    {
        return new SimpleResult(true, Array.Empty<string>());
    }

    public static SimpleResult Failure(IEnumerable<string> errors)
    {
        return new SimpleResult(false, errors);
    }
}
