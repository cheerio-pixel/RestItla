namespace RestItla.Application.Extras.ResultObject
{
    internal class Success<T>
    : Result<T>, ISuccess
    {
        public override T Value { get; }
        public override bool IsSuccess => true;
        public override bool IsFailure => false;
        public override MultiFailure<T> Errors => throw new InvalidOperationException();

        public Success(T value)
        {
            Value = value;
        }

        public override Result<U> FlatMap<U>(Func<T, Result<U>> mapper)
        {
            return mapper(Value);
        }

        public override Result<U> Map<U>(Func<T, U> mapper)
        {
            return new Success<U>(mapper(Value));
        }

        public override U Match<U>(Func<T, U> success, Func<MultiFailure<T>, U> failure)
        {
            return success(Value);
        }

        public override async Task<Result<U>> Map<U>(Func<T, Task<U>> mapper)
        {
            return new Success<U>(await mapper(Value));
        }

        public override async Task<Result<U>> FlatMap<U>(Func<T, Task<Result<U>>> mapper)
        {
            return await mapper(Value);
        }

        public override Task<Result<T>> Peek(Action<T> action)
        {
            action(Value);
            return this;
        }

        public override Result<T> MapError(Func<AppError, AppError> errorFunc)
        {
            return this;
        }

        public override Task<Result<T>> MapError(Func<AppError, Task<AppError>> errorFunc)
        {
            return this;
        }
    }
}