using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Sms.Api.Models;

namespace Sms.Api.Extensions;

public static class WebApiExtensions
{
    public static void ConfigureExceptionHandler(this WebApplication app)
    {
        ILogger<Program> logger = app.Services.GetRequiredService<ILogger<Program>>();

        app.UseExceptionHandler(appError =>
        {
            appError.Run(async context =>
            {
                context.Response.ContentType = "application/json";

                var contextFeature = context.Features.Get<IExceptionHandlerFeature>();

                if (contextFeature is not null)
                {
                    context.Response.StatusCode = contextFeature.Error switch
                    {
                        ValidationException validationException => StatusCodes.Status400BadRequest,
                        _ => StatusCodes.Status500InternalServerError
                    };

                    string message = contextFeature.Error.Message;

                    if (context.Response.StatusCode == StatusCodes.Status500InternalServerError)
                    {
                        logger.LogError(contextFeature.Error, "Something went wrong: {error}", contextFeature.Error);
                        message = "INTERNAL_SERVER_ERROR";
                    }
                    else
                    {
                        logger.LogWarning("{customError}", message);
                    }

                    var error = new ApiError(context.Response.StatusCode, message);
                    await context.Response.WriteAsync(error.ToString());
                }
            });
        });
    }
}