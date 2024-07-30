namespace RestItla.Application.Extras.ResultObject
{
    internal class Failure<T>
    : Result<T>, IFailure
    {
        public AppError Error { get; }
        public override MultiFailure<T> Errors => new(Error);

        public override bool IsSuccess => false;
        public override bool IsFailure => true;
        public override T Value => throw new InvalidOperationException();

        public Failure(AppError error)
        {
            Error = error;
        }

        public override U Match<U>(Func<T, U> success, Func<MultiFailure<T>, U> failure)
        {
            return failure(new MultiFailure<T>(this));
        }

        public override Result<U> Map<U>(Func<T, U> mapper)
        {
            return new Failure<U>(Error);
        }

        public override Result<U> FlatMap<U>(Func<T, Result<U>> mapper)
        {
            return new Failure<U>(Error);
        }

        public override Result<T> Combine(Result<T> combinator)
        {
            return new MultiFailure<T>(this).Combine(combinator);
        }

        public override Task<Result<U>> Map<U>(Func<T, Task<U>> mapper)
        {
            return new Failure<U>(Error);
        }

        public override Task<Result<U>> FlatMap<U>(Func<T, Task<Result<U>>> mapper)
        {
            return new Failure<U>(Error);
        }

        public override Task<Result<T>> Peek(Action<T> action)
        {
            return this;
        }

        public override Result<T> MapError(Func<AppError, AppError> errorFunc)
        {
            return errorFunc(Error);
        }

        public override async Task<Result<T>> MapError(Func<AppError, Task<AppError>> errorFunc)
        {
            return await errorFunc(Error);
        }
    }
}