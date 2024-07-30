namespace RestItla.Domain.Enum
{
    public enum ErrorType
    {
        ConnectionError = -1,
        Unknown = 0,
        NotFound = 1,
        Unauthorized = 2,
        InvalidCredentials = 3,
        Conflict = 4
    }
}