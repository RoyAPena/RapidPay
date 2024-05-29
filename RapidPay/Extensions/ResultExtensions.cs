﻿using SharedKernel;

namespace RapidPay.Api.Extensions
{
    public static class ResultExtensions
    {
        public static IResult ToProblemDetails(this Result result)
        {
            if (result.IsSuccess)
            {
                throw new InvalidOperationException();
            }

            return Results.Problem(
                statusCode: GetStatusCode(result.Error.Type),
                title: GetTitle(result.Error.Type),
                extensions: new Dictionary<string, object?>
                {
                    {"errors", new[] {result.Error} }
                });

            static int GetStatusCode(ErrorType errorType) => 
                errorType switch
                {
                ErrorType.Validation => StatusCodes.Status400BadRequest,
                ErrorType.NotFound => StatusCodes.Status404NotFound,
                ErrorType.Conflict => StatusCodes.Status500InternalServerError,
                _ => StatusCodes.Status500InternalServerError
            };

            static string GetTitle(ErrorType errorType) =>
                errorType switch
                {
                    ErrorType.Validation => "Bad Request",
                    ErrorType.NotFound => "Not Found",
                    ErrorType.Conflict => "Conflict",
                    _ => "Server Error"
                };
        }
    }
}
