using System.Text;
using Microsoft.AspNetCore.Mvc;
using Sms.Api.Extensions;
using Sms.Api.Models;
using Sms.Application.Extensions;
using Sms.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

builder.Services.AddControllers(options =>
{
    options.Filters.Add(new ProducesAttribute("application/json"));
    options.Filters.Add(new ProducesResponseTypeAttribute(typeof(ApiError), StatusCodes.Status400BadRequest));
    options.Filters.Add(
        new ProducesResponseTypeAttribute(typeof(ApiError), StatusCodes.Status500InternalServerError));
})
.ConfigureApiBehaviorOptions(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var keys = context.ModelState.Keys;
        StringBuilder stringBuilder = new StringBuilder();

        foreach (var key in keys)
        {
            var modelErrors = context.ModelState[key]?.Errors;
            if (modelErrors != null)
                foreach (var modelError in modelErrors)
                    stringBuilder.Append(key).Append(':').Append(modelError.ErrorMessage);
        }

        return new BadRequestObjectResult(new ApiError(StatusCodes.Status400BadRequest, stringBuilder.ToString()));
    };
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.CustomSchemaIds((type) => type.Name
        .Replace("Dto", string.Empty)
        .Replace("Command", string.Empty)
        .Replace("Query", string.Empty));
});

builder.Services
    .AddApplication()
    .AddInfrastructure();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.ConfigureExceptionHandler();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();