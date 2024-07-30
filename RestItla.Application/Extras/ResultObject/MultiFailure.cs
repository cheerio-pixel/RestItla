using System.Collections;

namespace RestItla.Application.Extras.ResultObject
{
    internal class MultiFailureNode
    {
        public AppError Value { get; }
        public MultiFailureNode? Next { get; set; }
        public MultiFailureNode? Prev { get; set; }

        public MultiFailureNode(AppError value)
        {
            Value = value;
            Next = null;
            Prev = null;
        }

        public MultiFailureNode(AppError value, MultiFailureNode next)
        {
            Value = value;
            Next = next;
            Next.Prev = this;
        }
    }

    public class MultiFailure<T>
    : Result<T>, IEnumerable<AppError>, IFailure
    {
        private MultiFailureNode? _head;
        private MultiFailureNode? _end;

        public override bool IsSuccess => false;
        public override bool IsFailure => true;
        public override T Value => throw new InvalidOperationException();
        public override MultiFailure<T> Errors => this;

        internal MultiFailure(AppError error)
        {
            _head = new MultiFailureNode(error);
            _end = _head;
        }

        internal MultiFailure(Failure<T> fail)
        : this(fail.Error)
        {
        }

        private MultiFailure(MultiFailureNode? head, MultiFailureNode? end)
        {
            _head = head;
            _end = end;
        }

        internal MultiFailure(IEnumerable<AppError> failures)
        {
            foreach (AppError f in failures)
            {
                Append(f);
            }
        }

        public MultiFailure<T> Clone()
        {
            return new MultiFailure<T>(this as IEnumerable<AppError>);
        }

        public MultiFailure<T> Append(AppError error)
        {
            MultiFailureNode cur = new(error);
            if (_head is null)
            {
                _head = cur;
                _end = cur;
                return this;
            }
            cur.Prev = _end;
            cur.Next = null;

            if (_end is not null)
            {
                _end.Next = cur;
            }
            _end = cur;
            return this;
        }

        public MultiFailure<T> Append(MultiFailure<T> fail)
        {
            if (fail._head is not null)
            {
                if (_head is null)
                {
                    return fail;
                }

                if (_end is null)
                {
                    _end = _head;
                }
                _end.Next = fail._head;
                fail._head.Prev = _end;
                _end = fail._end;
            }
            return this;
        }

        public override Result<T> Combine(Result<T> combinator)
        {
            return
            combinator switch
            {
                Success<T> s => s,
                Failure<T> f => Clone().Append(f.Errors),
                MultiFailure<T> mf => Append(mf),
                _ => throw new NotSupportedException(combinator.GetType().ToString())
            };
        }

        public IEnumerator<AppError> GetEnumerator()
        {
            MultiFailureNode? _current = _head;
            if (_current is not null)
            {
                do
                {
                    yield return _current.Value;
                    _current = _current.Next;
                }
                while (_current?.Next != null);
            }
        }

        public override Result<U> FlatMap<U>(Func<T, Result<U>> mapper)
        {
            return new MultiFailure<U>(_head, _end);
        }

        public override Result<U> Map<U>(Func<T, U> mapper)
        {
            return new MultiFailure<U>(_head, _end);
        }

        public override U Match<U>(Func<T, U> success, Func<MultiFailure<T>, U> failure)
        {
            return failure(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override Task<Result<U>> Map<U>(Func<T, Task<U>> mapper)
        {
            return new MultiFailure<U>(_head, _end);
        }

        public override Task<Result<U>> FlatMap<U>(Func<T, Task<Result<U>>> mapper)
        {
            return new MultiFailure<U>(_head, _end);
        }

        public override Task<Result<T>> Peek(Action<T> action)
        {
            return this;
        }

        public override Result<T> MapError(Func<AppError, AppError> errorFunc)
        {
            return new MultiFailure<T>(this.Select(errorFunc));
        }

        public override async Task<Result<T>> MapError(Func<AppError, Task<AppError>> errorFunc)
        {

            MultiFailure<T> result = new(null, null);
            foreach (AppError e in this)
            {
                result.Append(await errorFunc(e));
            }
            return result;
        }
    }
}