using Domain.Abstractions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Domain.Shared;

public class AppError
{
    public static readonly AppError None = new(string.Empty, string.Empty);
    public static readonly AppError NullValue = new("Error.NullValue", "The specificed result value is null");

    public AppError(string code, string message)
    {
        Code = code;
        Message = message;
    }

    public string Code { get; set; }
    public string Message { get; set; }

    public static implicit operator string(AppError error) => error.Code;
}