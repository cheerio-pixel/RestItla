using Microsoft.AspNetCore.Mvc;

using RestItla.Application.Extras.ResultObject;
using RestItla.Domain.Enum;

namespace RestItla.WebApi.Extension
{
    public static class ActionResultExt
    {
        public static IActionResult ToActionResult<T>(this Result<T> self)
        => self.Match(
            success: s => new OkObjectResult(s),
            failure: f => HandleError(f.First())
        );

        private static IActionResult HandleError(AppError error)
        => error.Type switch
        {
            ErrorType.NotFound => new NotFoundObjectResult(error),
            ErrorType.Unauthorized => new NotFoundObjectResult(error),
            ErrorType.InvalidCredentials => new UnauthorizedObjectResult(error),
            ErrorType.Conflict => new ConflictObjectResult(error),
            ErrorType.Unknown => new ObjectResult(error)
            {
                StatusCode = 500
            },
            _ => throw new NotImplementedException(),
        };
    }
}