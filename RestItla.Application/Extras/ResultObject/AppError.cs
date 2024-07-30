using RestItla.Domain.Enum;

namespace RestItla.Application.Extras.ResultObject
{
    public record AppError(ErrorType Type, string Message);

    public static class AppErrorExt
    {
        public static AppError Because(this ErrorType type, string message)
        => new(type, message);
    }

    public static class AppErrors
    {
    }
}