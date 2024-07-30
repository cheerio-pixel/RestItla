namespace RestItla.Application.Extras.ResultObject
{
    public abstract class Result<T>
    {
        public abstract U Match<U>(Func<T, U> success, Func<MultiFailure<T>, U> failure);
        public abstract Result<U> Map<U>(Func<T, U> mapper);
        public abstract Result<U> FlatMap<U>(Func<T, Result<U>> mapper);

        public abstract Task<Result<U>> Map<U>(Func<T, Task<U>> mapper);
        public abstract Task<Result<U>> FlatMap<U>(Func<T, Task<Result<U>>> mapper);

        // Map every error to another, possibly to change the reason.
        public abstract Result<T> MapError(Func<AppError, AppError> errorFunc);
        public abstract Task<Result<T>> MapError(Func<AppError, Task<AppError>> errorFunc);

        public abstract Task<Result<T>> Peek(Action<T> action);

        public abstract bool IsSuccess { get; }
        public abstract bool IsFailure { get; }

        public abstract T Value { get; }
        public abstract MultiFailure<T> Errors { get; }

        /// <summary>
        /// Smash together two results failure. In case of success substitute.
        /// </summary>
        public virtual Result<T> Combine(Result<T> combinator)
        {
            return combinator;
        }

        public static implicit operator Result<T>(AppError error)
        {
            return new Failure<T>(error);
        }

        public static implicit operator Result<T>(T value)
        {
            return new Success<T>(value);
        }

        public static implicit operator Task<Result<T>>(Result<T> result)
        {
            return Task.FromResult(result);
        }
    }

    public static class Result
    {
        public static Result<T> Fail<T>(AppError error)
        {
            return error;
        }

        public static Result<Unit> Fail(AppError error)
        {
            return error;
        }

        public static Result<T> Fail<T>(IEnumerable<AppError> errors)
        {
            return new MultiFailure<T>(errors);
        }

        public static Result<T> Ok<T>(T value)
        {
            return value;
        }

        public static Result<R> Apply<A, R>(this Result<Func<A, R>> self, Result<A> resultA)
        {
            return self.Match(
                success: f => resultA.Match(
                    success: a => Ok(f(a)),
                    failure: fs => Fail<R>(fs)
                ),
                failure: fs => resultA.Match(
                    success: _ => Fail<R>(fs),
                    failure: otherFs => Fail<R>(fs.Concat(otherFs))
                )
            );
        }

        public static async Task<Result<U>> Map<T, U>(this Task<Result<T>> self, Func<T, U> mapper)
        {
            return (await self).Map(mapper);
        }

        public static async Task<Result<U>> FlatMap<T, U>(this Task<Result<T>> self, Func<T, Task<Result<U>>> mapper)
        {
            return await (await self).FlatMap(mapper);
        }

        public static async Task<Result<T>> MapError<T>(this Task<Result<T>> self, Func<AppError, AppError> errorFunc)
        {
            return (await self).MapError(errorFunc);
        }

        public static async Task<Result<T>> MapError<T>(this Task<Result<T>> self, Func<AppError, Task<AppError>> errorFunc)
        {
            return await (await self).MapError(errorFunc);
        }

        public static Result<T> SwapError<T>(this Result<T> self, AppError error)
        => self.MapError(e => e.Type == error.Type ? error : e);

        public static async Task<Result<T>> SwapError<T>(this Task<Result<T>> self, AppError error)
        => (await self).SwapError(error);

        public static async Task<Result<T>> Peek<T>(this Task<Result<T>> self, Action<T> mapper)
        {
            return await (await self).Peek(mapper);
        }

        public static async Task<Result<T>> Peek<T>(this Result<T> self, Func<T, Task> mapper)
        {
            if (self.IsSuccess)
            {
                await mapper(self.Value);
            }
            return self;
        }

        public static void Match_<T>(this Result<T> self, Action<T> success, Action<MultiFailure<T>> failure)
        {
            switch (self)
            {
                case Success<T> s:
                    success(s.Value);
                    break;
                case Failure<T> f:
                    failure(new MultiFailure<T>(f));
                    break;
                case MultiFailure<T> mf:
                    failure(mf);
                    break;
                default:
                    throw new NotSupportedException(self.GetType().ToString());
            }
        }
    }

    public interface ISuccess { }
    public interface IFailure { }
}