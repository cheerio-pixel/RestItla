namespace RestItla.Application.Extras.ResultObject
{
    // Add supporte for linq in Result
    public static class ResultLinq
    {
        public static Result<V> SelectMany<T, U, V>(
                         this Result<T> result,
                         Func<T, Result<U>> bind,
                         Func<T, U, V> project) =>
        result.FlatMap(a => bind(a).Map(b => project(a, b)));

        public static Result<U> SelectMany<T, U>(this Result<T> result,
                                                   Func<T, Result<U>> bind)
        => result.FlatMap(bind);

        public static Result<U> Select<T, U>(this Result<T> result, Func<T, U> project)
        => result.Map(project);

    }
}