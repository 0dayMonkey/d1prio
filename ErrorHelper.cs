using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace ApiTools
{
    public static class ErrorHelper
    {

        public static ObjectResult ResponseExcRfc7807(int statusCode, Exception exc, PathString path)
        {
            return ResponseExcRfc7807(statusCode, exc.Message, exc.InnerException == null ? exc.Message : exc.InnerException.Message, path);
        }

        public static ObjectResult ResponseExcRfc7807(int statusCode, string title, string detail, PathString path)
        {
            var problemDetails = new ProblemDetails
            {
                Status = statusCode,
                Type = "about:blank",
                Title = title,
                Detail = !string.IsNullOrWhiteSpace(detail) ? detail : title,
                Instance = path
            };

            return ResponseExcRfc7807(problemDetails);
        }

        public static ObjectResult ResponseExcRfc7807(ProblemDetails problemDetails)
        {
            return new ObjectResult(problemDetails)
            {
                ContentTypes = { "application/problem+json" },
                StatusCode = problemDetails.Status,
            };
        }

        public static ObjectResult InternalErrorProblemsDetails(int statusCode, string path)
        {
            var problemDetails = new ProblemDetails
            {
                Type = "about:blank",
                Title = "An internal error occurred",
                Detail = "An unexpected error occurred. Please contact the support or try again later",
                Instance = path,
                Status = statusCode,
            };

            return ResponseExcRfc7807(problemDetails);
        }

    }
}
